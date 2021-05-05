using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Context;
using Game;
using GameActors.Blocks;
using GameActors.Blocks.Consumables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameActors
{
    public interface ISpeedable
    {
        int Loads { get; }
        List<float> SpeedBlocks { get; }
    }
    
    public class SnakeController : MonoBehaviour, ISpeedable
    {
        public UnityEvent<SnakeController> OnDie = new UnityEvent<SnakeController>();
        public UnityEvent<SnakeController, BlockType> OnPick = new UnityEvent<SnakeController, BlockType>();
        public UnityEvent<SnakeController> OnKill = new UnityEvent<SnakeController>();

        #region Interface
        public int Loads => Blocks.Count;
        public List<float> SpeedBlocks => Blocks
            .Where(b => b.BlockType == BlockType.SpeedBoost)
            .Cast<SpeedBlockView>().Select(s => s.SpeedValue)
            .ToList();
        #endregion

        public Transform BodyContainer => _bodyContainer;
        [SerializeField] private Transform _bodyContainer;
        [SerializeField] private TextMeshPro _name;
        
        private readonly List<BlockView> _blocks = new List<BlockView>();
        public int Id => _playerModel.Id;
        private BlockView Head => _blocks.First();
        public List<BlockView> Blocks => _blocks;
        
        private PlayerModel _playerModel;
        private MoveType _lastMoveType;
        private Coroutine _updateRoutine;
        private bool _paused;

        public void Initialize(PlayerModel playerModel, string snakeName)
        {
            _playerModel = playerModel;
            _name.text = snakeName;
            _name.color = playerModel.Color;

            foreach (var block in playerModel.Character.StartBlocks)
                AddBlock(block);
            
            Respawn(transform);
        }

        public void Pause(bool pause) => _paused = pause;

        public void Respawn(Transform position)
        {
            transform.position = position.position;
            
            if (_updateRoutine != null)
            {
                StopCoroutine(_updateRoutine);
                _updateRoutine = null;
            }

            for (var i = 0; i < _blocks.Count; i++)
            {
                var newPosition = Vector3.zero;
                newPosition.y -= 1.1f * i;
                _blocks[i].transform.localPosition = newPosition;
            }
                
            _updateRoutine = StartCoroutine(UpdateCoroutine());
            Pause(false);
        }

        public void MoveRight() => _lastMoveType = MoveType.Right;
        public void MoveLeft() => _lastMoveType = MoveType.Left;

        /// <summary>
        /// Custom controlled Update, to ensure the "grid" feeling
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateCoroutine()
        {
            //this should live until the object is destroyed
            while (true)
            {
                while (_paused)
                    yield return new WaitForSeconds(0.1f);
                    
                yield return new WaitForSeconds(this.Speed()); 
                Head.Move(1.1f, _lastMoveType);
                _lastMoveType = MoveType.Forward;
            }
        }

        private BlockView AddBlock(BlockType type)
        {
            var obj = BlockFactoring.CreateInstance(_bodyContainer, type);
            AddBlock(obj);
            return obj;
        }
        
        public void AddBlock(BlockView obj)
        {
            obj.transform.SetSiblingIndex(0);
            obj.OnContact.AddListener(CheckContact);
            _blocks.Insert(0, obj);
            
            _name.transform.SetParent(obj.transform);
            _name.transform.localPosition = Vector3.up * 2;
            _name.transform.localEulerAngles = Vector3.zero;

            IterateBlocks(_blocks);
        }

        private void IterateBlocks(IReadOnlyList<BlockView> blocks)
        {
            for (var i = 0; i < blocks.Count; i++)
            {
                BlockView currentBlock = blocks[i];
                BlockView nextBlock = null;
                if (i + 1 <= blocks.Count - 1)
                {
                    nextBlock = blocks[i + 1];
                    nextBlock.SetPreviousPart(currentBlock);
                }
                
                currentBlock.SetNextPart(nextBlock);
            }
        }

        public void ApplySnapshot(SnakeSnapshot snapshot)
        {
            _blocks.ForEach(b => Destroy(b.gameObject));
            _blocks.Clear();

            foreach (var blockSnapshot in snapshot.BlocksSnapshot)
            {
                var block = AddBlock(blockSnapshot.BlockType);
                block.transform.localPosition = blockSnapshot.Position;
                block.transform.eulerAngles = blockSnapshot.Rotation;
            }
        }
        
        private void CheckContact(BlockView myBlock, IHittable element)
        {
            if (_paused)
                return;

            switch (element)
            {
                case Wall _:
                    if (myBlock.IsHead) //avoid death in spawn where blocks are in vertical 
                    {
                        Pause(true);
                        OnDie.Invoke(this);
                    }
                    break;
                
                case ConsumableBlock consumableBlock:
                    Pause(true);
                    OnPick.Invoke(this, consumableBlock.BlockType);
                    break;
                
                case BlockView _ :
                    if (myBlock.IsHead)
                    {
                        Pause(true);
                        OnDie.Invoke(this);
                    }
                    else
                    {
                        OnKill.Invoke(this);
                    }
                    break;
            }
        }
    }
}
