using System.Linq;
using Context;
using UnityEngine;

namespace GameActors
{
    public static class BlockFactoring
    {
        public static BlockView CreateInstance(Transform container, BlockType type)
        {
            var blocks = ContextProvider.Context.GameSetup.Blocks;
            var blockType = blocks.FirstOrDefault(b => b.BlockType == type);
            if (blockType == null)
            {
                Debug.LogError($"Missing block of type {type}");
                return null;
            }
            
            var obj = Object.Instantiate(blockType.BlockPrefab, container).GetComponent<BlockView>();
            obj.BlockType = type;
            return obj;
        }
    }
}