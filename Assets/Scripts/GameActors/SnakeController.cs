using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Context;
using Game;
using GameActors.Blocks;
using GameActors.Blocks.Consumables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameActors
{
    public class SnakeController : MonoBehaviour, ISpeedable
    {
        #region Events
        public UnityEvent<SnakeController> OnDie { get; } = new UnityEvent<SnakeController>();
        public UnityEvent<SnakeController, BlockType> OnPick { get; } = new UnityEvent<SnakeController, BlockType>();
        public UnityEvent<TimeTravelBlockView> OnTimeTravelPoint { get; } = new UnityEvent<TimeTravelBlockView>();
        public UnityEvent<TimeTravelBlockView> OnRewind { get; } = new UnityEvent<TimeTravelBlockView>();
        public UnityEvent<SnakeController> OnKill { get; } = new UnityEvent<SnakeController>();
        #endregion
        #region Interface
        public int Loads => Blocks.Count;
        public List<float> SpeedBlocks => Blocks
            .Where(b => b.BlockType == BlockType.SpeedBoost)
            .Cast<SpeedBlockView>().Select(s => s.SpeedValue)
            .ToList();
        #endregion

        [SerializeField] private TextMeshPro _name;
        
        private readonly List<BlockView> _blocks = new List<BlockView>();
        public int Id => _playerModel.Id;
        public BlockView Head => _blocks.First();
        public List<BlockView> Blocks => _blocks;
        

        private PlayerModel _playerModel;
        private MoveType _lastMoveType;
        private Coroutine _updateRoutine;
        private bool _paused;
        private bool _collisionEnabled;

        public void Initialize(Transform spawn, PlayerModel playerModel, string snakeName)
        {
            _playerModel = playerModel;
            _name.text = snakeName;
            _name.color = playerModel.Color;

            foreach (var block in playerModel.Character.StartBlocks)
                AddBlock(block);
            
            Respawn(spawn);
        }
        
        public void Pause(bool pause) => _paused = pause;

        public void Respawn(Transform spawnPoint)
        {
            if (_updateRoutine != null)
            {
                StopCoroutine(_updateRoutine);
                _updateRoutine = null;
            }

            var startPosition = spawnPoint.position;
            var dir = spawnPoint.eulerAngles.GetDirection() ;
            for (var i = 0; i < _blocks.Count; i++)
            {
                _blocks[i].transform.position = startPosition + (dir * Extensions.BlockSize * i);
                _blocks[i].transform.eulerAngles = spawnPoint.eulerAngles;
                _blocks[i].Collider.enabled = true;
            }
            
            _updateRoutine = StartCoroutine(UpdateCoroutine());
            _collisionEnabled = true;
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
                Head.Move(Extensions.BlockSize, _lastMoveType);
                _lastMoveType = MoveType.Forward;
            }
        }
        
        private void Update()
        {
            if (Head != null)
            {
                _name.transform.position = Head.transform.position + new Vector3(0, 2, -5);
                _name.transform.eulerAngles = Vector3.zero;
            }
        }

        private BlockView AddBlock(BlockType type)
        {
            var obj = BlockFactoring.CreateInstance(transform, type);
            return AddBlock(obj);
        }
        
        private BlockView AddBlock(BlockView obj)
        {
            obj.Collider.enabled = false;
            obj.transform.SetSiblingIndex(0);
            obj.OnContact.AddListener(CheckCollision);
            _blocks.Insert(0, obj);

            IterateBlocks(_blocks);
            
            if (obj.BlockType == BlockType.TimeTravel)
                OnTimeTravelPoint.Invoke((TimeTravelBlockView) obj);

            return obj;
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

            transform.localPosition = snapshot.TransformSnapshot.Position;
            transform.localEulerAngles = snapshot.TransformSnapshot.Rotation;

            foreach (var blockSnapshot in snapshot.BlocksSnapshot)
            {
                var block = AddBlock(blockSnapshot.BlockType);
                block.transform.localPosition = blockSnapshot.Position;
                block.transform.localEulerAngles = blockSnapshot.Rotation;
            }
        }

        private void CheckCollision(BlockView myBlock, IHittable element)
        {
            if (_paused || !_collisionEnabled)
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
                    AddBlock(consumableBlock.BlockType);
                    OnPick.Invoke(this, consumableBlock.BlockType);
                    break;
                
                case BlockView colliderBlock :
                    if (myBlock.IsHead)
                        CheckForDeath(colliderBlock);
                    else
                        OnKill.Invoke(this);
                    break;
            }
        }

        private void CheckForDeath(BlockView colliderBlock)
        {
            if (HasBlockOfType(BlockType.BatteringRam))
            {
                var batteringRam = (BatteringRamBlockView) Blocks.Last(b => b.BlockType == BlockType.BatteringRam);
                if (batteringRam.ActivateBatteringRam(this, colliderBlock))
                    return;
            }
            
            else if (HasBlockOfType(BlockType.TimeTravel))
            {
                var timeTravelBlock = Blocks.Last(b => b.BlockType == BlockType.TimeTravel);
                OnRewind.Invoke((TimeTravelBlockView) timeTravelBlock);
                return;
            }
            
            Debug.LogError($"{gameObject.name} Die by: {colliderBlock.transform.parent.name}");
            Pause(true);
            OnDie.Invoke(this);
        }

        public async void DisableCollisions(float duration)
        {
            _collisionEnabled = false;
            await Task.Delay((int) duration * 1000); //1000 ms = 1s
            _collisionEnabled = true;
        }

        private bool HasBlockOfType(BlockType blockType) => Blocks.Any(b => b.BlockType == blockType);
    }
}
