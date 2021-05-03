using System;
using System.Linq;
using Consumables;
using Context;
using UnityEngine;
using Object = UnityEngine.Object;

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

            var gameObj = Object.Instantiate(blockType.BlockPrefab, container);
            switch (type)
            {
                case BlockType.Common:
                    return gameObj.GetComponent<BlockView>();
                case BlockType.TimeTravel:
                    return gameObj.GetComponent<BlockView>();
                case BlockType.SpeedBoost:
                    return gameObj.GetComponent<SpeedBlockView>();
                case BlockType.BatteringRam:
                    return gameObj.GetComponent<BlockView>();
                case BlockType.Variant:
                    return gameObj.GetComponent<VariantBlockView>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}