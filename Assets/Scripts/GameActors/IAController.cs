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
        
        private SnakeController _snakeController;
        public SnakeController SnakeController
        {
            get
            {
                if (!_snakeController)
                    _snakeController = GetComponent<SnakeController>();
                return _snakeController;
            }
        }
        
        private MatchGroup _group;
        private ConsumableBlock Target => _group.Block;
        private BlockView Head => SnakeController.Head;

        private IHittable _rightRaycast;
        private IHittable _leftRaycast;
        private IHittable _forwardRaycast;
        private IAStates _state;

        public void SetConfig(SpawnPoint spawn, PlayerModel playerModel)
        {
            gameObject.name = $"IASnake: {playerModel.Username}";
            SnakeController.Initialize(spawn, playerModel, $"IA-{playerModel.Username}");
            SnakeController.gameObject.SetActive(false);
        }

        public void Update()
        {
            _rightRaycast = GetRaycastInfo(Head.transform.right);
            _leftRaycast = GetRaycastInfo(-Head.transform.right);
            _forwardRaycast = GetRaycastInfo(Head.transform.up);

            var block = Target.transform;
            var distanceFromBlock = block.position - Head.transform.position;

            //check if the object is not a consumable block
            if (_forwardRaycast != null && _forwardRaycast as ConsumableBlock == null)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Flee()
        {
            //don't flee from consumable block
            if (_forwardRaycast is ConsumableBlock block)
                return;

            if (_forwardRaycast is Wall)
            {
                if (_leftRaycast == null)
                    SnakeController.MoveLeft();
                else if (_rightRaycast == null)
                    SnakeController.MoveRight();
            }
            else if (_forwardRaycast is BlockView)
            {
                if (_leftRaycast == null) 
                    SnakeController.MoveLeft();
                else if (_rightRaycast == null)
                    SnakeController.MoveRight();
                
                //give preference to die by wall, so don't give points to players
                else if (_leftRaycast is Wall) 
                    SnakeController.MoveLeft();
                else if (_rightRaycast is Wall)
                    SnakeController.MoveRight();
            }
        }

        private void ChaseBlock(Vector3 distanceFromBlock)
        {
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
                        if (_rightRaycast == null) //move to her right if possible
                            SnakeController.MoveRight();
                    }, down:() =>
                    {
                        if (_leftRaycast == null) //move to her left if possible
                            SnakeController.MoveLeft();
                    }, right:() =>
                    {
                        
                    }, left:() =>
                    {
                        if (_leftRaycast == null)
                            SnakeController.MoveLeft();
                        else if (_rightRaycast == null)
                            SnakeController.MoveRight(); 
                    });
            }
            else //block is on our Left
            {
                ActionByDirection(
                    up:() =>
                    {
                        if (_leftRaycast == null) //move to her right if possible
                            SnakeController.MoveLeft();
                    }, down:() =>
                    {
                        if (_rightRaycast == null) //move to her left if possible
                            SnakeController.MoveRight();
                    }, right:() =>
                    {
                        if (_leftRaycast == null)
                            SnakeController.MoveLeft();
                        else if (_rightRaycast == null)
                            SnakeController.MoveRight();
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
                        if (_leftRaycast == null)
                            SnakeController.MoveLeft();
                        else if (_rightRaycast == null)
                            SnakeController.MoveRight();
                    }, right:() =>
                    {
                        if (_leftRaycast == null)
                            SnakeController.MoveLeft();
                    }, left:() =>
                    {
                        if (_rightRaycast == null)
                            SnakeController.MoveRight();
                    });
            }
            else //block is on our Down
            {
                ActionByDirection(
                    up:() =>
                    {
                        if (_leftRaycast == null)
                            SnakeController.MoveLeft();
                        else if (_rightRaycast == null)
                            SnakeController.MoveRight();
                    }, down:() =>
                    {
                        
                    }, right:() =>
                    {
                        if (_rightRaycast == null)
                            SnakeController.MoveRight();
                    }, left:() =>
                    {
                        if (_leftRaycast == null)
                            SnakeController.MoveLeft();
                    });
            }
        }

        private void ActionByDirection(Action up, Action down, Action right, Action left)
        {
            Debug.Log($"IA Angle: {Head.CurrentAngle}");
            
            if (Math.Abs(Head.CurrentAngle - Direction.Up) < 0.1f)
                up?.Invoke();
            else if (Math.Abs(Head.CurrentAngle - Direction.Down) < 0.1f)
                down?.Invoke();
            else if (Math.Abs(Head.CurrentAngle - Direction.Right) < 0.1f)
                right?.Invoke();
            else if (Math.Abs(Head.CurrentAngle - Direction.Left) < 0.1f) 
                left?.Invoke();
        }

        private IHittable GetRaycastInfo(Vector3 direction)
        {
            if (!SnakeController.Head)
                return null;

            var origin = Head.transform.position;
            
            Debug.DrawRay(origin, transform.TransformDirection(direction) * _checkingDistance, Color.red);
            if (Physics.Raycast(origin, transform.TransformDirection(direction), out var hit, _checkingDistance))
                return hit.collider.gameObject.GetComponent<IHittable>();

            return null;
        }

        /// <summary>
        /// By setting group, we always have track of current target
        /// </summary>
        /// <param name="group"></param>
        public void SetGroup(MatchGroup group) => _group = group;
    }
    
    internal enum IAStates
    {
        ChaseBlock,
        FleeFromHittable,
    }
}
