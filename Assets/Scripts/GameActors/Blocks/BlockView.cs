using System;
using Game;
using UnityEngine;
using UnityEngine.Events;

namespace GameActors.Blocks
{
    [RequireComponent(typeof(Rigidbody))]
    public class BlockView : MonoBehaviour, IHittable
    {
        private const int TimeToEnableCollision = 1;
        public UnityEvent<BlockView, IHittable> OnContact = new UnityEvent<BlockView, IHittable>();
        
        public BlockType BlockType { get; private set; }
        private BlockView _next;
        private BlockView _previous;
        public Collider Collider { get; private set; }
        
        private bool IsTail => _next == null;
        public bool IsHead => _previous == null;

        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        public void Move(float speed, MoveType moveType)
        {
            var transformSnapshot = new TransformSnapshot(transform);
            MoveDirection(moveType);
            var end = transform.localPosition + (transform.up.normalized * speed);
            transform.localPosition = end;
            
            if (!IsTail)
                _next.Move(transformSnapshot);
        }
        
        public void Move(TransformSnapshot transformSnapshot)
        {
            var newTransformSnapshot = new TransformSnapshot(transform);

            transform.localPosition = transformSnapshot.Position;
            transform.localEulerAngles = transformSnapshot.Rotation;
            
            if (!IsTail)
                _next.Move(newTransformSnapshot);
        }

        public void MoveDirection(MoveType moveType)
        {
            switch (moveType)
            {
                case MoveType.Forward:
                    break;
                case MoveType.Right:
                    transform.
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 90);
                    break;
                case MoveType.Left:
                    transform.localEulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 90);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_next)
            {
                if (other.gameObject == _next.gameObject)
                    return;
            }

            if (_previous)
            {
                if (other.gameObject == _previous.gameObject)
                    return;
            }

            var hittable = other.gameObject.GetComponent<IHittable>();
            if (hittable == null)
                return;

            OnContact.Invoke(this, hittable);
        }

        public void SetNextPart(BlockView blockView) => _next = blockView;
        public void SetPreviousPart(BlockView blockView) => _previous = blockView;
        public void SetBlockType(BlockType blockType) => BlockType = blockType;
    }
}