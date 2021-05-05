using System.Collections.Generic;
using System.Linq;
using GameActors.Blocks;
using UnityEngine;

namespace Game
{
    //kind of memento pattern
    public class SnakeSnapshot
    {
        public SnakeSnapshot(List<BlockView> blocks)
        {
            BlocksSnapshot = blocks.Select((view => new BlockSnapshot(view))).ToArray();
        }
        
        public BlockSnapshot[] BlocksSnapshot { get; }
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