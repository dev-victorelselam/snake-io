using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Context;
using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;

namespace GameActors
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private Transform _bodyContainer;
        [SerializeField] private TextMeshPro _name;
        
        private readonly List<BlockView> _blocks = new List<BlockView>();
        private IContext _gameContext;
        private MoveType? _lastMoveType;
        private Coroutine _updateRoutine;

        private bool _paused;
        
        private float Speed
        {
            get
            {
                var baseSpeed = _gameContext.GameSetup.BaseSpeed;
                var loadedDecaySpeed = _gameContext.GameSetup.LoadedSpeedDecay;
                var finalSpeed = baseSpeed - (_blocks.Count * loadedDecaySpeed);
                var speedBlocks = _blocks.Where(b => b is SpeedBlockView).Cast<SpeedBlockView>();
            
                return speedBlocks.Aggregate(finalSpeed, (current, speedBlock) => speedBlock.Apply(current));
            }
        }

        public void Initialize(PlayerConfig playerConfig)
        {
            _gameContext = ContextProvider.Context;
            _name.text = playerConfig.Username;
            _name.color = playerConfig.Character.Color;

            foreach (var block in playerConfig.Character.StartBlocks)
                AddBlock(block);

            for (var i = 0; i < _blocks.Count; i++)
                _blocks[i].transform.DOLocalMoveY(-(StaticValues.BlockSize * i), 0);
            
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
                    yield return new WaitForSeconds(StaticValues.UpdateTime);
                    
                yield return new WaitForSeconds(StaticValues.UpdateTime);
                _blocks.First().Move(Speed, StaticValues.UpdateTime, _lastMoveType);
                _lastMoveType = null;
            }
        }

        public void AddBlock(BlockType type)
        {
            _paused = true;
            
            var obj = BlockFactoring.CreateInstance(_bodyContainer, type, _gameContext.GameSetup.Blocks);
            obj.Initialize(this);
            var firstBlock = _blocks.FirstOrDefault();
            if (firstBlock != null)
            {
                obj.transform.position = firstBlock.transform.position;
                obj.transform.eulerAngles = firstBlock.transform.eulerAngles;
            }

            obj.transform.SetSiblingIndex(0);
            _blocks.Insert(0, obj);

            IterateBlocks(_blocks);

            _paused = false;
        }

        private void IterateBlocks(List<BlockView> blocks)
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
    }
}
