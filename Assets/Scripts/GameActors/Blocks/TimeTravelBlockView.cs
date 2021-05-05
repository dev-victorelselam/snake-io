using System.Collections.Generic;
using Context;
using Game;

namespace GameActors.Blocks
{
    public class TimeTravelBlockView : BlockView
    {
        private readonly Dictionary<SnakeController, SnakeSnapshot> _snakeSnapshots = 
            new Dictionary<SnakeController, SnakeSnapshot>();
        
        public void AddSnapshot(IEnumerable<SnakeController> snakeControllers)
        {
            foreach (var snakeController in snakeControllers)
                _snakeSnapshots.Add(snakeController, snakeController.GetSnapshot());
        }

        public Dictionary<SnakeController, SnakeSnapshot> Retrieve() => _snakeSnapshots;
    }
}