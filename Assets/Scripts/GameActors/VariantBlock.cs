using System;
using System.Collections;
using GameActors;
using UnityEngine;

namespace Consumables
{
    public class VariantBlock : BlockView
    {
        [SerializeField] private float _timeToChange;
        private BlockView _block;

        private void Start()
        {
            var allBlocks = Enum.GetValues(typeof(BlockType));
            StartCoroutine(StartChanging(allBlocks));
        }

        private IEnumerator StartChanging(Array blockTypes)
        {
            foreach (BlockType blockType in blockTypes)
            {
                if (_block)
                    Destroy(_block.gameObject);
                
                _block = BlockFactoring.CreateInstance(transform, blockType);
                BlockType = blockType;
                yield return new WaitForSeconds(_timeToChange);
            }

            //we want this to repeat when all blocks were used
            StartCoroutine(StartChanging(blockTypes));
        }
    }
}