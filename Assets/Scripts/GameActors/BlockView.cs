using System;
using DG.Tweening;
using UnityEngine;

namespace GameActors
{
    [RequireComponent(typeof(Rigidbody))]
    public class BlockView : MonoBehaviour
    {
        [HideInInspector] public BlockType BlockType;
        private Rigidbody _rigidbody;
        private SnakeController _snakeController;
        private BlockView _next;
        private MoveType? _lastMoveType;

        private bool HasNextBlock => _next != null;

        public void Initialize(SnakeController snakeController)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _snakeController = snakeController;
        }

        public void Move(float speed, float time, MoveType? moveType = null)
        {
            var end = transform.position + (transform.up.normalized * speed);
            transform.DOMove(end, time);
            
            if (HasNextBlock)
                _next.Move(speed, time, _lastMoveType);
            
            if (moveType != null)
                MoveDirection((MoveType) moveType);
            _lastMoveType = moveType;
        }

        public void MoveDirection(MoveType moveType)
        {
            switch (moveType)
            {
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

        public void SetNextPart(BlockView blockView)
        {
            _next = blockView;
        }
    }
}