using System;
using Game;
using UnityEngine;
using UnityEngine.Events;

namespace GameActors.Blocks
{
    [RequireComponent(typeof(Rigidbody))]
    public class BlockView : MonoBehaviour, IHittable
    {
        public UnityEvent<BlockView, IHittable> OnContact = new UnityEvent<BlockView, IHittable>();
        public UnityEvent<BlockView> OnBlockDisabled = new UnityEvent<BlockView>();
        
        public BlockType BlockType { get; private set; }
        private BlockView _next;
        private BlockView _previous;
        public Collider Collider { get; private set; }
        
        public bool IsTail => _next == null;
        public bool IsHead => _previous == null;
        
        private int _currentAngle;
        public int CurrentAngle
        {
            get => _currentAngle;
            set
            {
                var v = value;
                if (v > 270)
                    v = 0;
                if (v < 0)
                    v = 270;
                
                _currentAngle = v;
            }
        }

        protected virtual void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        public void Move(float speed, MoveType moveType)
        {
            var transformSnapshot = new TransformSnapshot(transform);
            
            transform.localPosition += (transform.up.normalized * speed);
            MoveDirection(moveType);
            
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

        private void MoveDirection(MoveType moveType)
        {
            switch (moveType)
            {
                case MoveType.Forward:
                    break;
                case MoveType.Right:
                    CurrentAngle -= 90;
                    transform.Rotate(new Vector3(0, 0, -90), Space.Self);
                    break;
                case MoveType.Left:
                    CurrentAngle += 90;
                    transform.Rotate(new Vector3(0, 0, 90), Space.Self);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
            }
        }

        public void DisableBlock()
        {
            OnBlockDisabled.Invoke(this);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (CheckForAdjacentBlocks(other))
            {
                Debug.Log("Contact with my block!");
                return;
            }

            var hittable = other.gameObject.GetComponent<IHittable>();
            if (hittable == null)
                return;

            OnContact.Invoke(this, hittable);
        }

        /// <summary>
        /// Avoid collisions with adjacent blocks
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private bool CheckForAdjacentBlocks(Collider other)
        {
            if (_next)
            {
                if (other.gameObject == _next.gameObject)
                    return true;
            }

            if (_previous)
            {
                if (other.gameObject == _previous.gameObject)
                    return true;
            }

            return false;
        }

        public void SetNextPart(BlockView blockView) => _next = blockView;
        public void SetPreviousPart(BlockView blockView) => _previous = blockView;
        public void SetBlockType(BlockType blockType) => BlockType = blockType;

        public virtual object SnapshotPayload() => null;
    }
}