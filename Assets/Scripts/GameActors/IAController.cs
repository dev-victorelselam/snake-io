using System;
using Context;
using Game;
using GameActors.Blocks;
using GameActors.Blocks.Consumables;
using UnityEngine;

namespace GameActors
{
    public class IAController : MonoBehaviour
    {
        [SerializeField] private float _checkingDistance = 5;
        
        private PlayerModel _playerModel;
        private SnakeController _snakeController;
        
        private MatchGroup _group;
        private SnakeController Player => _group.Player.SnakeController;
        private ConsumableBlock Target => _group.Block;
        public SnakeController SnakeController => _snakeController;
        public Transform Head => _snakeController.Head.transform;
        
        
        private IHittable _rightCast;
        private IHittable _leftCast;
        private IHittable _forwardCast;
        private IAStates _state;

        public void SetConfig(Transform spawn, PlayerModel playerModel)
        {
            gameObject.name = $"IASnake: {playerModel.Username}";
            _snakeController = GetComponent<SnakeController>();
            _snakeController.Initialize(spawn, playerModel, $"IA-{playerModel.Username}");
        }

        public void Update()
        {
            _rightCast = GetRaycastInfo(Head.transform.right);
            _leftCast = GetRaycastInfo(-Head.transform.right);
            _forwardCast = GetRaycastInfo(Head.transform.up);

            var block = Target.transform;
            var distanceFromBlock = block.position - Head.transform.position;

            //check if the object is not a consumable block
            if (_forwardCast != null && _forwardCast as ConsumableBlock == null)
                _state = IAStates.FleeFromHittable;
            else
                _state = IAStates.ChaseBlock;

            switch (_state)
            {
                case IAStates.ChaseBlock:
                    ChaseBlock(distanceFromBlock);
                    break;
                case IAStates.FleeFromHittable:
                    Flee();
                    break;
                case IAStates.ChasePlayer:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Flee()
        {
            //don't flee from consumable block
            if (_forwardCast is ConsumableBlock block)
                return;
            
            if (_forwardCast is Wall wall)
            {
                if (_leftCast == null)
                    _snakeController.MoveLeft();
                else if (_rightCast == null)
                    _snakeController.MoveRight();
            }
            else if (_forwardCast is BlockView blockView)
            {
                if (_leftCast == null || _leftCast is Wall) //give preference to die by wall, so don't give points to players
                    _snakeController.MoveLeft();
                else if (_rightCast == null || _rightCast is Wall)
                    _snakeController.MoveRight();
            }
        }

        private void ChaseBlock(Vector3 distanceFromBlock)
        {
            //the priority should be the smallest length to the target
            if (Mathf.Abs(distanceFromBlock.x) > Mathf.Abs(distanceFromBlock.y))
                AdjustXPosition(distanceFromBlock);
            else
                AdjustYPosition(distanceFromBlock);
        }

        private void AdjustXPosition(Vector3 distanceFromBlock)
        {
            if (distanceFromBlock.x > 0) //block is on our Right
            {
                ActionByDirection(
                    up:() =>
                    {
                        if (_rightCast == null) //move to her right if possible
                            _snakeController.MoveRight();
                    }, down:() =>
                    {
                        if (_leftCast == null) //move to her left if possible
                            _snakeController.MoveLeft();
                    }, right:() =>
                    {
                        
                    }, left:() =>
                    {
                        if (_leftCast == null)
                            _snakeController.MoveLeft();
                        else if (_rightCast == null)
                            _snakeController.MoveRight(); 
                    });
            }
            else //block is on our Left
            {
                ActionByDirection(
                    up:() =>
                    {
                        if (_leftCast == null) //move to her right if possible
                            _snakeController.MoveLeft();
                    }, down:() =>
                    {
                        if (_rightCast == null) //move to her left if possible
                            _snakeController.MoveRight();
                    }, right:() =>
                    {
                        if (_leftCast == null)
                            _snakeController.MoveLeft();
                        else if (_rightCast == null)
                            _snakeController.MoveRight();
                    }, left:() =>
                    {
                        
                    });
            }
        }

        private void AdjustYPosition(Vector3 distanceFromBlock)
        {
            if (distanceFromBlock.y > 0) //block is on our Up
            {
                ActionByDirection(
                    up:() =>
                    {
                    }, down:() =>
                    {
                        if (_leftCast == null)
                            _snakeController.MoveLeft();
                        else if (_rightCast == null)
                            _snakeController.MoveRight();
                    }, right:() =>
                    {
                        if (_leftCast == null)
                            _snakeController.MoveLeft();
                    }, left:() =>
                    {
                        if (_rightCast == null)
                            _snakeController.MoveRight();
                    });
            }
            else //block is on our Down
            {
                ActionByDirection(
                    up:() =>
                    {
                        if (_leftCast == null) //move to her right if possible
                            _snakeController.MoveLeft();
                    }, down:() =>
                    {
                        if (_rightCast == null) //move to her left if possible
                            _snakeController.MoveRight();
                    }, right:() =>
                    {
                        if (_leftCast == null)
                            _snakeController.MoveLeft();
                        else if(_rightCast == null)
                            _snakeController.MoveRight();
                    }, left:() =>
                    {
                        
                    });
            }
        }

        private void ActionByDirection(Action up, Action down, Action right, Action left)
        {
            Debug.Log($"IA Angle: {Head.eulerAngles.z}");
            
            if (Math.Abs(Head.eulerAngles.z - Direction.Up) < 0.1f)
                up?.Invoke();
            else if (Math.Abs(Head.eulerAngles.z - Direction.Down) < 0.1f)
                down?.Invoke();
            else if (Math.Abs(Head.eulerAngles.z - Direction.Right) < 0.1f)
                right?.Invoke();
            else if (Math.Abs(Head.eulerAngles.z - Direction.Left) < 0.1f) 
                left?.Invoke();
        }

        private IHittable GetRaycastInfo(Vector3 direction)
        {
            if (!_snakeController.Head)
                return null;

            var origin = Head.transform.position;
            
            Debug.DrawRay(origin, transform.TransformDirection(direction) * _checkingDistance, Color.red);
            if (Physics.Raycast(origin, transform.TransformDirection(direction), out var hit, _checkingDistance))
                return hit.collider.gameObject.GetComponent<IHittable>();

            return null;
        }

        public void SetGroup(MatchGroup group) => _group = group;
    }
    
    internal enum IAStates
    {
        ChaseBlock,
        FleeFromHittable,
        ChasePlayer,
    }
}
