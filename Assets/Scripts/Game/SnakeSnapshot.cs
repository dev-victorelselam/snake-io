using System.Collections.Generic;
using System.Linq;
using GameActors;
using GameActors.Blocks;
using UnityEngine;

namespace Game
{
    public class SnakeSnapshot
    {
        public SnakeSnapshot(SnakeController snakeController)
        {
            BlocksSnapshot = snakeController.Blocks.Select(view => new BlockSnapshot(view)).Reverse().ToArray();
            TransformSnapshot = new TransformSnapshot(snakeController.transform);
        }
        
        public BlockSnapshot[] BlocksSnapshot { get; }
        public TransformSnapshot TransformSnapshot { get; }
    }

    public class BlockSnapshot
    {
        public BlockSnapshot(BlockView blockView)
        {
            Position = blockView.transform.localPosition;
            Rotation = blockView.transform.localEulerAngles;

            BlockType = blockView.BlockType;
        }
        
        public Vector3 Position { get; }
        public Vector3 Rotation { get; }
        
        public BlockType BlockType { get; }
    }

    public class TransformSnapshot
    {
        public TransformSnapshot(Transform transform)
        {
            Position = transform.localPosition;
            Rotation = transform.localEulerAngles;
        }

        public Vector3 Position;
        public Vector3 Rotation;
    }
}