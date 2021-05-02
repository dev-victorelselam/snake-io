using GameActors;
using UnityEngine;

namespace Context
{
    //kind of memento pattern
    public class SnakeSnapshot
    {
        public TransformSnapshot[] BlocksSnapshot;
    }

    public class TransformSnapshot
    {
        public TransformSnapshot(BlockView transform)
        {
            if (transform == null)
            {
                Position = Vector3.zero;
                Rotation = Vector3.zero;
            }
            else
            {
                Position = transform.transform.position;
                Rotation = transform.transform.eulerAngles;
            }
        }
        
        public TransformSnapshot(Transform transform)
        {
            Position = transform.position;
            Rotation = transform.eulerAngles;
        }
        
        public Vector3 Position;
        public Vector3 Rotation;
    }
}