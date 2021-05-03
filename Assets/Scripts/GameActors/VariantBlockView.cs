using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameActors.Blocks;
using UnityEngine;

namespace GameActors
{
    public class VariantBlockView : BlockView
    {
        [SerializeField] private float _timeToChange;
        private BlockView _block;

        private void Start()
        {
            var allBlocks = Enum.GetValues(typeof(BlockType)).Cast<BlockType>();
            StartCoroutine(StartChanging(allBlocks));
        }

        private IEnumerator StartChanging(IEnumerable<BlockType> blockTypes)
        {
            var types = blockTypes as BlockType[] ?? blockTypes.ToArray();
            foreach (var blockType in types)
            {
                if (_block)
                    Destroy(_block.gameObject);
                
                _block = BlockFactoring.CreateInstance(transform, blockType);
                Destroy(_block.Collider);
                SetBlockType(blockType);
                yield return new WaitForSeconds(_timeToChange);
            }

            //we want this to repeat when all blocks were used (inverting sequence)
            StartCoroutine(StartChanging(types.Reverse()));
        }
    }
}