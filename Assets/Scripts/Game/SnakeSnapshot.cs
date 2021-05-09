using System.Collections.Generic;
using System.Linq;
using GameActors;

namespace Game
{
    public class SnakeSnapshot
    {
        public SnakeSnapshot(SnakeController snakeController)
        {
            BlocksSnapshot = snakeController.Blocks
                .Select(view => new BlockSnapshot(view))
                .Reverse()
                .ToList();
        }
        
        public List<BlockSnapshot> BlocksSnapshot { get; }
    }
}