using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameActors.Blocks.Consumables
{
    public class ConsumableBlock : MonoBehaviour, IHittable
    {
        [SerializeField] private float _timeToChange = 1;
        public BlockType BlockType => _block.BlockType;
        private BlockView _block;

        public void Initialize(BlockType blockType)
        {
            if (blockType == BlockType.Variant)
            {
                var allBlocks = Enum.GetValues(typeof(BlockType)).Cast<BlockType>().ToList();
                allBlocks.Remove(BlockType.Variant);
                StartCoroutine(StartChanging(allBlocks));
            }
            else
            {
                _block = BlockFactoring.CreateInstance(transform, blockType);
                //this blocks shouldn't detect collisions
                Destroy(_block.Collider);
            }
        }

        private IEnumerator StartChanging(List<BlockType> blockTypes)
        {
            foreach (var blockType in blockTypes)
            {
                if (_block)
                    Destroy(_block.gameObject);
                
                _block = BlockFactoring.CreateInstance(transform, blockType);
                Destroy(_block.Collider);
                    
                yield return new WaitForSeconds(_timeToChange);
            }

            //we want this to repeat when all blocks were used (inverting sequence)
            blockTypes.Reverse();
            StartCoroutine(StartChanging(blockTypes));
        }
    }
}