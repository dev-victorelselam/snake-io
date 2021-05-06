using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game;
using GameActors;
using GameActors.Blocks;
using GameActors.Blocks.Consumables;
using Tutorial.KeyDetector;
using UI;
using UnityEngine;

namespace Context
{
    public partial class GameController : MonoBehaviour
    {
        [SerializeField] private Transform _gameSpace;
        [SerializeField] private List<SpawnPoints> _spawnPoints;
        [SerializeField] private KeyDetector _keyDetector;
        
        private GameCanvas _gameUI;
        private IContext _gameContext;
        
        private readonly List<MatchGroup> _matchGroups = new List<MatchGroup>();

        public void StartController()
        {
            _gameContext = ContextProvider.Context;
            _gameUI = (GameCanvas) _gameContext.NavigationController.GetUIByState(GameState.Game);
        }

        public void StartGame()
        {
            _keyDetector.StartListen();
            _keyDetector.OnComplete.AddListener(OnNewPlayer);
        }

        private void OnNewPlayer(List<KeyCode> playerKeys)
        {
            //identify new player
            _gameContext.NavigationController.UpdateUI(GameState.Tutorial);
        }

        public void PauseGame(bool pause)
        {
            if (pause)
            {
                
            }
        }

        public void StopGame()
        {
            _gameContext.NavigationController.UpdateUI(GameState.PreGame);
        }

        public void AddPlayer(PlayerModel playerModel)
        {
            _gameUI.AddPlayer(playerModel);
            var setup = _gameContext.GameSetup;
            var spawnPoint = _spawnPoints.GetRandom();
            var fairPosition = Extensions.FindFairPosition(spawnPoint.Player.position, spawnPoint.Enemy.position);
                
            var player = SpawnPlayer(setup.SnakePrefab, playerModel, spawnPoint.Player);
            var enemy = SpawnEnemy(setup.SnakePrefab, playerModel, spawnPoint.Enemy);
            var block = SpawnBlock(setup.ConsumableBlockPrefab, fairPosition);
            
            _matchGroups.Add(new MatchGroup
            {
                PlayerModel = playerModel,
                
                Player = player,
                Enemy = enemy,
                Block = block,
            });
        }

        private async void RespawnGroup(MatchGroup group)
        {
            await Task.Delay(1000); //wait 1 second to respawn
            
            var spawnPoint = _spawnPoints.GetRandom();
            group.Player.SnakeController.Respawn(spawnPoint.Player);
            group.Enemy.SnakeController.Respawn(spawnPoint.Enemy);
            
            var fairPosition = Extensions.FindFairPosition(spawnPoint.Player.position, spawnPoint.Enemy.position);
            if (group.Block)
                Destroy(group.Block.gameObject);
            var block = SpawnBlock(_gameContext.GameSetup.ConsumableBlockPrefab, fairPosition);

            group.Block = block;
            group.PauseGroup(false);
        }
        
        private void Kill(SnakeController snake)
        {
            var group = MatchGroupById(snake.Id);
            
            if (snake == group.Player.SnakeController)
                group.PlayerModel.Score += _gameContext.GameSetup.BlockScore;
        }

        private async void Die(SnakeController snake)
        {
            var group = MatchGroupById(snake.Id);
            group.PauseGroup(true);
            
            var blocks = snake.Blocks.ToList();
            blocks.Reverse(); //get from older to newer
            var timeTravelBlock = blocks.FirstOrDefault(b => b.BlockType == BlockType.TimeTravel);
            if (timeTravelBlock != null && timeTravelBlock is TimeTravelBlockView t)
            {
                await _gameUI.ActivatePowerUp(BlockType.TimeTravel);
                Rewind(t.Retrieve());
                
                group.PauseGroup(false);
                return;
            }

            if (snake == group.Player.SnakeController) //when die, remove score
                group.PlayerModel.Score -= 1;
            
            RespawnGroup(group);
        }

        private void Rewind(Dictionary<SnakeController, SnakeSnapshot> retrieve)
        {
            //if a snake wasn't present in the moment of the snapshot
            //it'll be ignored by design
            foreach (var snapshot in retrieve)
                snapshot.Key.ApplySnapshot(snapshot.Value);
        }

        private void Pick(SnakeController snake, BlockType blockType)
        {
            var group = MatchGroupById(snake.Id);
            group.PauseGroup(true);

            var block = BlockFactoring.CreateInstance(snake.transform, blockType);
            block.transform.localPosition = new Vector3(3000, 0, 0); //throw away to not appear in screen
            
            if (blockType == BlockType.TimeTravel)
                CreateSnapShot((TimeTravelBlockView) block);
                
            snake.AddBlock(block);
            if (snake == group.Player.SnakeController) //when pick a block, add score
                group.PlayerModel.Score += _gameContext.GameSetup.BlockScore;
            
            RespawnGroup(group);
        }

        private void CreateSnapShot(TimeTravelBlockView blockView)
        {
            var allSnakes = _matchGroups.SelectMany(mg => mg.SnakeControllers);
            blockView.AddSnapshot(allSnakes);
        }

        private MatchGroup MatchGroupById(int id) => _matchGroups.First(mg => mg.Id == id);
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

            player.SnakeController.OnPick.AddListener(Pick);
            player.SnakeController.OnDie.AddListener(Die);
            player.SnakeController.OnKill.AddListener(Kill);
            return player;
        }

        private IAController SpawnEnemy(GameObject snakePrefab, PlayerModel playerModel, Transform spawnPoint)
        {
            var enemy = InstantiateSnake(snakePrefab, spawnPoint).AddComponent<IAController>();
            enemy.SetConfig(playerModel);

            enemy.SnakeController.OnPick.AddListener(Pick);
            enemy.SnakeController.OnDie.AddListener(Die);
            enemy.SnakeController.OnKill.AddListener(Kill);
            return enemy;
        }

        private GameObject InstantiateSnake(GameObject snakePrefab, Transform spawnPoint)
        {
            var obj = Instantiate(snakePrefab, _gameSpace);
            obj.transform.position = spawnPoint.position;
            obj.transform.eulerAngles = spawnPoint.eulerAngles;
            return obj;
        }
        
        [Serializable]
        private class SpawnPoints
        {
            public Transform Player;
            public Transform Enemy;
        }
    }
}