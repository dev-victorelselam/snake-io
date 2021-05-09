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
        [SerializeField] private GameObject _eyes;
        
        private readonly List<BlockView> _blocks = new List<BlockView>();
        public int Id => _playerModel.Id;
        public BlockView Head => _blocks.FirstOrDefault();
        public List<BlockView> Blocks => _blocks;

        private PlayerModel _playerModel;
        private MoveType _lastMoveType;
        private Coroutine _updateRoutine;
        private bool _paused;
        private bool _collisionEnabled;

        public void Initialize(SpawnPoint spawn, PlayerModel playerModel, string snakeName)
        {
            _playerModel = playerModel;
            _name.text = snakeName;
            _name.color = playerModel.Color;

            foreach (var block in playerModel.Character.StartBlocks)
                AddBlock(block);
            
            Respawn(spawn);
        }
        
        public void Pause(bool pause) => _paused = pause;

        public void Respawn(SpawnPoint spawnPoint)
        {
            if (_updateRoutine != null)
            {
                StopCoroutine(_updateRoutine);
                _updateRoutine = null;
            }

            var startPosition = spawnPoint.transform.position;
            var dir = spawnPoint.Direction.GetInverseVector();
            var angle = spawnPoint.Direction.GetAngle();
            for (var i = 0; i < _blocks.Count; i++)
            {
                _blocks[i].transform.position = startPosition + (dir * StaticValues.BlockSize * i);
                _blocks[i].transform.eulerAngles = new Vector3(0, 0, angle);
                _blocks[i].CurrentAngle = angle;
                
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
                Head.Move(StaticValues.BlockSize, _lastMoveType);
                _lastMoveType = MoveType.Forward;
            }
        }
        
        private void Update()
        {
            if (Head)
            {
                _name.transform.localPosition = Head.transform.localPosition + new Vector3(0, 2, -5);
                _name.transform.eulerAngles = Vector3.zero;
            }

            if (_eyes)
            {
                _eyes.transform.localPosition = Head.transform.localPosition + new Vector3(0, 0.5f, -5);
                _eyes.transform.eulerAngles = Head.transform.eulerAngles;
            }
        }

        public BlockView AddBlock(BlockType type)
        {
            var obj = BlockFactoring.CreateInstance(transform, type);
            return AddBlock(obj);
        }
        
        private BlockView AddBlock(BlockView obj)
        {
            obj.transform.SetSiblingIndex(0);
            obj.OnContact.AddListener(CheckCollision);
            obj.OnBlockDisabled.AddListener(DisableBlock);
            _blocks.Insert(0, obj);

            IterateBlocks(_blocks);
            
            if (obj.BlockType == BlockType.TimeTravel)
                OnTimeTravelPoint.Invoke((TimeTravelBlockView) obj);

            return obj;
        }

        private void DisableBlock(BlockView block)
        {
            var snapshot = new TransformSnapshot(block.transform);
            var index = _blocks.IndexOf(block);
            if (!block.IsTail)
                _blocks[index + 1].Move(snapshot);

            _blocks.Remove(block);
            Destroy(block.gameObject);

            if (_blocks.IsNullOrEmpty())
            {
                //todo: notify game controller that this player is out
            }
            
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

        private void CheckCollision(BlockView myBlock, IHittable element)
        {
            if (_paused)
                return;

            switch (element)
            {
                case Wall _:
                    if (myBlock.IsHead)
                        CheckForDeath();
                    break;
                case ConsumableBlock consumableBlock:
                    Pause(true);
                    AddBlock(consumableBlock.BlockType);
                    OnPick.Invoke(this, consumableBlock.BlockType);
                    break;
                
                case BlockView colliderBlock :
                    if (!_collisionEnabled)
                        return;
                    
                    if (myBlock.IsHead)
                        CheckForDeath(colliderBlock);
                    else
                        OnKill.Invoke(this);
                    break;
            }
        }

        private void CheckForDeath(BlockView colliderBlock = null)
        {
            if (colliderBlock != null)
            {
                if (HasBlockOfType(BlockType.BatteringRam))
                {
                    var batteringRam = (BatteringRamBlockView) Blocks.Last(b => b.BlockType == BlockType.BatteringRam);
                    if (batteringRam.ActivateBatteringRam(this, colliderBlock))
                        return;
                }
                Debug.Log($"{gameObject.name} Die by: {colliderBlock.transform.parent.name}");
            }

            if (HasBlockOfType(BlockType.TimeTravel))
            {
                //priority is from newer to older, avoiding player to lose lots of blocks
                var timeTravelBlock = Blocks.First(b => b.BlockType == BlockType.TimeTravel);
                OnRewind.Invoke((TimeTravelBlockView) timeTravelBlock);
                
                return;
            }

            Pause(true);
            OnDie.Invoke(this);
        }

        public async void DisableCollisions(float duration)
        {
            _collisionEnabled = false;
            await Task.Delay((int) duration * 1000); //1000 ms = 1s
            _collisionEnabled = true;
        }

        private bool HasBlockOfType(BlockType blockType) => Blocks
            .Any(b => b.BlockType == blockType && b.gameObject.activeInHierarchy);
    }
}