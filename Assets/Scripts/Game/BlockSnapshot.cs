using GameActors.Blocks;
using UnityEngine;

namespace Game
{
    public class BlockSnapshot
    {
        public BlockSnapshot(BlockView blockView)
        {
            Position = blockView.transform.localPosition;
            Rotation = blockView.transform.localEulerAngles;

            BlockType = blockView.BlockType;
            Payload = blockView.SnapshotPayload();
        }
        
        public Vector3 Position { get; }
        public Vector3 Rotation { get; }
        
        public BlockType BlockType { get; }
        
        //this can be used to store anything from future block features
        //for now we only use to store time travel
        public object Payload { get; }
    }
}