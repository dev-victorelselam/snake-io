using System.Collections.Generic;
using Context;
using Game;

namespace GameActors.Blocks
{
    public class TimeTravelBlockView : BlockView
    {
        private Dictionary<SnakeController, SnakeSnapshot> _snakeSnapshots = 
            new Dictionary<SnakeController, SnakeSnapshot>();
        
        public void AddSnapshot(IEnumerable<SnakeController> snakeControllers)
        {
            foreach (var snakeController in snakeControllers)
                _snakeSnapshots.Add(snakeController, snakeController.GetSnapshot());
        }

        public Dictionary<SnakeController, SnakeSnapshot> Retrieve()
        {
            if (_snakeSnapshots.IsNullOrEmpty())
                return _snakeSnapshots;
            
            ContextProvider.Context.GameController.UI.ActivatePowerUpView(BlockType.TimeTravel);
            return _snakeSnapshots;
        }

        public override object SnapshotPayload() => _snakeSnapshots;
        public void SetSnapshot(Dictionary<SnakeController, SnakeSnapshot> snapshots) 
            => _snakeSnapshots = snapshots;
    }
}