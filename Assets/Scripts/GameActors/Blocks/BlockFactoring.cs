using System;
using System.Linq;
using Context;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameActors.Blocks
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

            //found that unity don't handle polymorphism, so we have to specify types here
            var gameObj = Object.Instantiate(blockType.BlockPrefab, container);
            gameObj.GetComponent<BlockView>().SetBlockType(type);
            switch (type)
            {
                case BlockType.Common:
                    return gameObj.GetComponent<BlockView>();
                case BlockType.TimeTravel:
                    return gameObj.GetComponent<TimeTravelBlockView>();
                case BlockType.SpeedBoost:
                    return gameObj.GetComponent<SpeedBlockView>();
                case BlockType.BatteringRam:
                    return gameObj.GetComponent<BatteringRamBlockView>();
                case BlockType.Projectile:
                    return gameObj.GetComponent<ProjectileBlockView>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
