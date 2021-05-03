using System;
using Game;
using UnityEngine;
using UnityEngine.Events;

namespace GameActors.Blocks
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class BlockView : MonoBehaviour, IHittable
    {
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
            if (!IsTail)
                _next.Move(new TransformSnapshot(this));
            
            MoveDirection(moveType);
            var end = transform.position + (transform.up.normalized * speed);
            transform.position = end;
        }
        
        public void Move(TransformSnapshot transformSnapshot)
        {
            if (!IsTail)
                _next.Move(new TransformSnapshot(this));
            
            transform.position = transformSnapshot.Position;
            transform.eulerAngles = transformSnapshot.Rotation;
        }

        public void MoveDirection(MoveType moveType)
        {
            switch (moveType)
            {
                case MoveType.Forward:
                    break;
                case MoveType.Right:
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 90);
                    break;
                case MoveType.Left:
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 90);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
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