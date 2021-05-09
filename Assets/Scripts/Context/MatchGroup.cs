using System.Collections.Generic;
using Game;
using GameActors;
using GameActors.Blocks.Consumables;
using UnityEngine.Events;

namespace Context
{
    public class MatchGroup
    {
        public UnityEvent OnScoreUpdate { get; } = new UnityEvent();
        public UnityEvent OnRemoveFromPlay { get; } = new UnityEvent();
        
        public int Id { get; }
        public MovementController Player { get; }
        public IAController Enemy { get; }
        public ConsumableBlock Block { get; set; }
        
        private readonly Dictionary<SnakeController, int> _score = new Dictionary<SnakeController, int>();
        
        public MatchGroup(PlayerModel playerModel, MovementController player, IAController enemy, ConsumableBlock block)
        {
            Id = playerModel.Id;
            
            Player = player;
            Enemy = enemy;
            Block = block;

            _score[player.SnakeController] = 0;
            _score[enemy.SnakeController] = 0;
        }

        public void UpdateScore(SnakeController snakeController, int value)
        {
            _score[snakeController] += value;
            OnScoreUpdate.Invoke(); //observer pattern
        }

        public int GetScore(SnakeController snakeController) => _score[snakeController];
        
        public IEnumerable<SnakeController> SnakeControllers => 
            new List<SnakeController> { Player.SnakeController, Enemy.SnakeController };

        public void PauseGroup(bool pause)
        {
            Player.SnakeController.Pause(pause);
            Enemy.SnakeController.Pause(pause);
        }
    }
}