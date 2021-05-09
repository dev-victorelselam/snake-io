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
        [SerializeField] private SpawnPointsList spawnPointsList;
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
            var spawnPoint = spawnPointsList.Spawns.GetRandom().Points;

            var player = SpawnPlayer(setup.SnakePrefab, playerModel, spawnPoint[0]);
            var enemy = SpawnEnemy(setup.SnakePrefab, playerModel, spawnPoint[1]);
            var block = SpawnBlock(setup.ConsumableBlockPrefab, player.transform, enemy.transform);

            var group = new MatchGroup(playerModel, player, enemy, block);

            _matchGroups.Add(group);
            enemy.SetGroup(group);
        }

        private async void RespawnGroup(MatchGroup group)
        {
            await Task.Delay(1000); //wait 1 second to respawn

            var spawnPoint = spawnPointsList.Spawns.GetRandom().Points;
            group.Player.SnakeController.Respawn(spawnPoint[0]);
            group.Enemy.SnakeController.Respawn(spawnPoint[1]);
            
            if (group.Block)
                Destroy(group.Block.gameObject);
            
            var block = SpawnBlock(_gameContext.GameSetup.ConsumableBlockPrefab, 
                group.Player.transform, group.Enemy.transform);

            group.Block = block;
            group.PauseGroup(false);
        }
        
        private void Kill(SnakeController snake)
        {
            var group = MatchGroupById(snake.Id);
            
            if (snake == group.Player.SnakeController)
                group.PlayerModel.Score += _gameContext.GameSetup.BlockScore;
        }

        private void Die(SnakeController snake)
        {
            var group = MatchGroupById(snake.Id);
            group.PauseGroup(true);

            if (snake == group.Player.SnakeController) //when die, remove score
                group.PlayerModel.Score -= 1;
            
            RespawnGroup(group);
        }

        private void Rewind(TimeTravelBlockView timeTravelBlockView)
        {
            //if a snake wasn't present in the moment of the snapshot
            //it'll be ignored by design
            foreach (var snapshot in timeTravelBlockView.Retrieve())
                snapshot.Key.ApplySnapshot(snapshot.Value);
        }

        private void Pick(SnakeController snake)
        {
            var group = MatchGroupById(snake.Id);
            group.PauseGroup(true);
            
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
        private ConsumableBlock SpawnBlock(ConsumableBlock prefab, params Transform[] transforms)
        {
            var blockType = Enum.GetValues(typeof(BlockType)).Cast<BlockType>().GetRandom();
            var block = Instantiate(prefab, _gameSpace);
            
            block.transform.position = FairPosition(transforms);
            block.Initialize(blockType);
            
            return block;
        }

        private MovementController SpawnPlayer(GameObject snakePrefab, PlayerModel playerModel, SpawnPoint spawnPoint)
        {
            var player = InstantiateSnake(snakePrefab, spawnPoint)
                .AddComponent<MovementController>();

            player.SnakeController.OnPick.AddListener(Pick);
            player.SnakeController.OnDie.AddListener(Die);
            player.SnakeController.OnKill.AddListener(Kill);
            player.SnakeController.OnTimeTravelPoint.AddListener(CreateSnapShot);
            player.SnakeController.OnRewind.AddListener(Rewind);
            
            player.SetConfig(spawnPoint, playerModel);
            return player;
        }

        private IAController SpawnEnemy(GameObject snakePrefab, PlayerModel playerModel, SpawnPoint spawnPoint)
        {
            var enemy = InstantiateSnake(snakePrefab, spawnPoint).AddComponent<IAController>();

            enemy.SnakeController.OnPick.AddListener(Pick);
            enemy.SnakeController.OnDie.AddListener(Die);
            enemy.SnakeController.OnKill.AddListener(Kill);
            enemy.SnakeController.OnTimeTravelPoint.AddListener(CreateSnapShot);
            enemy.SnakeController.OnRewind.AddListener(Rewind);
            
            enemy.SetConfig(spawnPoint, playerModel);
            return enemy;
        }

        private GameObject InstantiateSnake(GameObject snakePrefab, SpawnPoint spawnPoint)
        {
            var obj = Instantiate(snakePrefab, _gameSpace);
            obj.transform.position = Vector3.zero;
            obj.transform.eulerAngles = Vector3.zero;
            return obj;
        }

        private Vector3 FairPosition(params Transform[] transforms) =>
            Extensions.FindFairPosition(transforms.Select(t => t.position).ToArray());
    }
}