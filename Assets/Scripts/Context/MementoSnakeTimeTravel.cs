using System.Collections.Generic;
using System.Linq;
using Game;
using GameActors;

namespace Context
{
    /// <summary>
    /// Memento pattern to hold snakes info when the time travel block was collected
    /// </summary>
    public static class MementoSnakeTimeTravel
    {
        //we identify the time travel by the controller snake that asked for it.
        private static readonly Dictionary<SnakeController, Dictionary<SnakeController, SnakeSnapshot>> _snakesSnapshot 
            = new Dictionary<SnakeController, Dictionary<SnakeController, SnakeSnapshot>>();

        public static void Create(SnakeController snakeController, List<MatchGroup> groups)
        {
            var newSnapshot = new Dictionary<SnakeController, SnakeSnapshot>();
            foreach (var matchGroup in groups)
            {
                var player = matchGroup.Player.SnakeController;
                newSnapshot.Add(player, player.GetSnapshot());
                
                var enemy = matchGroup.Enemy.SnakeController;
                newSnapshot.Add(enemy, enemy.GetSnapshot());
            }
            
            _snakesSnapshot.Add(snakeController, newSnapshot);
        }

        public static void Retrieve(SnakeController key, List<MatchGroup> groups)
        {
            
        }
    }
}