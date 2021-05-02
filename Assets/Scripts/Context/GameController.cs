using System.Collections.Generic;
using GameActors;
using UI;
using UnityEngine;

namespace Context
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Transform _gameSpace;
        
        private GameCanvas _canvas;
        private IContext _gameContext;
        
        private List<SnakeElement> _allSnakes = new List<SnakeElement>();

        public void StartController()
        {
            _gameContext = ContextProvider.Context;
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
            SpawnEnemy(_gameContext.GameSetup.SnakePrefab);
        }

        private void SpawnPlayer(GameObject snakePrefab, PlayerConfig playerConfig)
        {
            var player = InstantiateSnake(snakePrefab);
            player.gameObject
                .AddComponent<MovementController>()
                .SetPlayerConfig(playerConfig);
            
            _allSnakes.Add(new SnakeElement(player));
        }

        private void SpawnEnemy(GameObject snakePrefab)
        {
            var enemy = InstantiateSnake(snakePrefab);
            enemy.gameObject.AddComponent<IAController>();
            
            _allSnakes.Add(new SnakeElement(enemy));
        }

        private SnakeController InstantiateSnake(GameObject snakePrefab) 
            => Instantiate(snakePrefab, _gameSpace).GetComponent<SnakeController>();
    }
}