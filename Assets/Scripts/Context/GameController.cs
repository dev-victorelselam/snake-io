using System.Collections.Generic;
using System.Linq;
using Game;
using GameActors;
using UI;
using UnityEngine;

namespace Context
{
    public class MatchGroup
    {
        public SnakeController Player;
        public SnakeController Enemy;
        
    }
    
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Transform _gameSpace;
        
        private GameCanvas _gameUI;
        private IContext _gameContext;
        
        private List<SnakeElement> _allSnakes = new List<SnakeElement>();

        public void StartController()
        {
            _gameContext = ContextProvider.Context;
            _gameUI = (GameCanvas) _gameContext.NavigationController.GetUIByState(GameState.Game);
        }
        
        public void StartGame()
        {
            
        }
        
        public void PauseGame()
        {
            
        }

        public void StopGame()
        {
            _gameContext.NavigationController.UpdateUI(GameState.PreGame);
        }

        public void AddPlayer(PlayerConfig playerConfig)
        {
            SpawnPlayer(_gameContext.GameSetup.SnakePrefab, playerConfig);
            SpawnEnemy(_gameContext.GameSetup.SnakePrefab, playerConfig);
        }

        private SnakeController SpawnPlayer(GameObject snakePrefab, PlayerConfig playerConfig)
        {
            var player = InstantiateSnake(snakePrefab);
            player.gameObject
                .AddComponent<MovementController>()
                .SetConfig(playerConfig);
            
            _allSnakes.Add(new SnakeElement(player));

            return player;
        }

        private SnakeController SpawnEnemy(GameObject snakePrefab, PlayerConfig playerConfig)
        {
            playerConfig.Character = ContextProvider.Context.GameSetup.Characters.First();
            var enemy = InstantiateSnake(snakePrefab);
            enemy.gameObject
                .AddComponent<IAController>()
                .SetConfig(playerConfig);
            
            _allSnakes.Add(new SnakeElement(enemy));

            return enemy;
        }

        private SnakeController InstantiateSnake(GameObject snakePrefab) 
            => Instantiate(snakePrefab, _gameSpace).GetComponent<SnakeController>();
    }
}