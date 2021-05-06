using System.Collections.Generic;
using Game;
using GameActors;
using GameActors.Blocks.Consumables;

namespace Context
{
    public class MatchGroup
    {
        public int Id => PlayerModel.Id;
        public PlayerModel PlayerModel { get; set; }
        public MovementController Player { get; set; }
        public IAController Enemy { get; set; }

        public ConsumableBlock Block { get; set; }
        
        public List<SnakeController> SnakeControllers => new List<SnakeController>
        {
            Player.SnakeController,
            Enemy.SnakeController
        };

        public void PauseGroup(bool pause)
        {
            Player.SnakeController.Pause(pause);
            Enemy.SnakeController.Pause(pause);
        }
    }
}