using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using GameActors;
using GameActors.Blocks;
using GameActors.Blocks.Consumables;
using UI;
using UnityEngine;

namespace Context
{
    public class MatchGroup
    {
        public MovementController Player { get; set; }
        public IAController Enemy { get; set; }

        public ConsumableBlock Block { get; set; }
    }

    public partial class GameController : MonoBehaviour
    {
        [SerializeField] private Transform _gameSpace;
        [SerializeField] private List<SpawnPoints> _spawnPoints;
        
        private GameCanvas _gameUI;
        private IContext _gameContext;
        
        private List<MatchGroup> _matchGroups = new List<MatchGroup>();

        public void StartController()
        {
            _gameContext = ContextProvider.Context;
            _gameUI = (GameCanvas) _gameContext.NavigationController.GetUIByState(GameState.Game);
        }
        
        public void StartGame()
        { }
        
        public void PauseGame()
        { }

        public void StopGame()
        {
            _gameContext.NavigationController.UpdateUI(GameState.PreGame);
        }

        public void AddPlayer(PlayerModel playerModel)
        {
            var setup = _gameContext.GameSetup;
            var spawnPoint = _spawnPoints.GetRandom();
            var fairPosition = Extensions.FindFairPosition(spawnPoint.Player, spawnPoint.Enemy);
                
            var player = SpawnPlayer(setup.SnakePrefab, playerModel, spawnPoint.Player);
            var enemy = SpawnEnemy(setup.SnakePrefab, playerModel, spawnPoint.Enemy);
            var block = SpawnBlock(setup.ConsumableBlockPrefab, fairPosition);
            
            _matchGroups.Add(new MatchGroup
            {
                Player = player,
                Enemy = enemy,
                Block = block,
            });
        }
    }
    
    public partial class GameController
    {
        private ConsumableBlock SpawnBlock(ConsumableBlock prefab, Vector3 fairPosition)
        {
            var blockType = Enum.GetValues(typeof(BlockType)).Cast<BlockType>().GetRandom();
            var block = Instantiate(prefab, _gameSpace);
            
            block.transform.position = fairPosition;
            block.Initialize(blockType);
            
            return block;
        }

        private MovementController SpawnPlayer(GameObject snakePrefab, PlayerModel playerModel, Transform spawnPoint)
        {
            var player = InstantiateSnake(snakePrefab, spawnPoint)
                .AddComponent<MovementController>();
            player.SetConfig(playerModel);

            return player;
        }

        private IAController SpawnEnemy(GameObject snakePrefab, PlayerModel playerModel, Transform spawnPoint)
        {
            var enemy = InstantiateSnake(snakePrefab, spawnPoint).AddComponent<IAController>();
            enemy.SetConfig(playerModel);

            return enemy;
        }

        private GameObject InstantiateSnake(GameObject snakePrefab, Transform spawnPoint)
        {
            var obj = Instantiate(snakePrefab, _gameSpace);
            obj.transform.position = spawnPoint.position;
            return obj;
        }
        
        [Serializable]
        class SpawnPoints
        {
            public Transform Player;
            public Transform Enemy;
        }
    }
}