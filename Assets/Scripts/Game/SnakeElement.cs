using System.Collections.Generic;
using GameActors;

namespace Context
{
    public class SnakeElement
    {
        public SnakeController SnakeController;
        public List<SnakeSnapshot> Snapshots = new List<SnakeSnapshot>();

        public SnakeElement(SnakeController snakeController)
        {
            SnakeController = snakeController;
        }
        
        public void CreateSnapshot()
        {
            var snapshot = new SnakeSnapshot();
            Snapshots.Add(snapshot);
        }
    }
}