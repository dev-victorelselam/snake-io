using UnityEngine;

namespace Game
{
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