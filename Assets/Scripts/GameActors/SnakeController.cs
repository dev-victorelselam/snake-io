using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Context;
using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameActors
{
    public class SnakeController : MonoBehaviour
    {
        public UnityEvent OnDie = new UnityEvent();
        public UnityEvent OnPick = new UnityEvent();
        public UnityEvent OnKill = new UnityEvent();
        
        [SerializeField] private Transform _bodyContainer;
        [SerializeField] private TextMeshPro _name;
        
        private readonly List<BlockView> _blocks = new List<BlockView>();
        private BlockView Head => _blocks.First();
        public List<BlockView> Blocks => _blocks;
        
        private PlayerConfig _playerConfig;
        private IContext _gameContext;
        private MoveType _lastMoveType;
        private Coroutine _updateRoutine;
        private bool _paused;

        public void Initialize(PlayerConfig playerConfig)
        {
            _gameContext = ContextProvider.Context;
            _playerConfig = playerConfig;
            _name.text = playerConfig.Username;
            _name.color = playerConfig.Color;

            foreach (var block in playerConfig.Character.StartBlocks)
                AddBlock(block);
            
            Respawn();
        }

        public void Respawn()
        {
            if (_updateRoutine != null)
            {
                StopCoroutine(_updateRoutine);
                _updateRoutine = null;
            }

            for (var i = 0; i < _blocks.Count; i++)
            {
                var newPosition = Vector3.zero;
                if (i - 1 >= 0)
                {
                    var oldBlock = _blocks[i - 1];
                    newPosition = oldBlock.transform.position;
                    newPosition.y -= oldBlock.Collider.bounds.size.y / 2;
                }

                _blocks[i].transform.position = newPosition;
            }
                
            _updateRoutine = StartCoroutine(UpdateCoroutine());
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
                    
                yield return new WaitForSeconds(GameDelay.Get());
                Head.Move(this.Speed(), _lastMoveType);
                _lastMoveType = MoveType.Forward;
            }
        }

        public void AddBlock(BlockType type)
        {
            _paused = true;
            
            var obj = BlockFactoring.CreateInstance(_bodyContainer, type);
            obj.transform.SetSiblingIndex(0);
            obj.OnContact.AddListener(CheckContact);
            _blocks.Insert(0, obj);
            _name.transform.SetParent(obj.transform);

            IterateBlocks(_blocks);

            _paused = false;
        }

        private void IterateBlocks(IReadOnlyList<BlockView> blocks)
        {
            for (var i = 0; i < blocks.Count; i++)
            {
                BlockView currentBlock = blocks[i];
                BlockView nextBlock = null;
                if (i + 1 <= blocks.Count - 1)
                    nextBlock = blocks[i + 1];
                
                currentBlock.SetNextPart(nextBlock);
            }
        }
        
        private void CheckContact(IHittable element)
        {
            if (element.Type == typeof(Wall))
            {
                if (CheckForDeath())
                    OnDie.Invoke();
            }
            else if (element.Type == typeof(ConsumableBlock))
            {
                var block = (ConsumableBlock) element;
            }
        }

        private bool CheckForDeath()
        {
            return true;
        }
    }
}
