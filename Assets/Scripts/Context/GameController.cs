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
            
            _keyDetector.OnComplete.RemoveAllListeners();
            _keyDetector.OnComplete.AddListener(NewPlayer);
            PauseGame(false);
        }

        private void NewPlayer(List<KeyCode> playerKeys)
        {
            //identify new player
            PauseGame(true);
            _gameContext.NavigationController.UpdateUI(GameState.Tutorial);
        }

        public void PauseGame(bool pause) => _matchGroups.ForEach(mg => mg.PauseGroup(pause));

        public void AddPlayer(PlayerModel playerModel)
        {
            var setup = _gameContext.GameSetup;
            var spawnPoint = spawnPointsList.Spawns.GetRandom().Points;

            var player = SpawnPlayer(setup.SnakePrefab, playerModel, spawnPoint[0]);
            var enemy = SpawnEnemy(setup.SnakePrefab, playerModel, spawnPoint[1]);
            var block = SpawnBlock(setup.ConsumableBlockPrefab, player.transform, enemy.transform);

            var group = new MatchGroup(playerModel, player, enemy, block);

            _matchGroups.Add(group);
            enemy.SetGroup(group);
            _gameUI.AddGroup(group);
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

        private void Die(SnakeController deadSnake, SnakeController killerSnake)
        {
            var deadSnakeGroup = MatchGroupById(deadSnake.Id);
            deadSnakeGroup.PauseGroup(true);
            deadSnakeGroup.UpdateScore(deadSnake, -_gameContext.GameSetup.KillScore);
            
            var killerSnakeGroup = MatchGroupById(killerSnake.Id);
            killerSnakeGroup.UpdateScore(killerSnake, _gameContext.GameSetup.KillScore);

            _gameUI.NotifyKill(deadSnake, killerSnake);

            RespawnGroup(deadSnakeGroup);
        }
        
        private void Pick(SnakeController snake)
        {
            var group = MatchGroupById(snake.Id);
            group.PauseGroup(true);
            group.UpdateScore(snake, _gameContext.GameSetup.BlockScore); //when pick a block, add score
            
            RespawnGroup(group);
        }

        private void Rewind(TimeTravelBlockView timeTravelBlockView)
        {
            //if a snake wasn't present in the moment of the snapshot
            //it'll be ignored by design
            foreach (var snapshot in timeTravelBlockView.Retrieve())
                snapshot.Key.ApplySnapshot(snapshot.Value);
        }

        private void RemoveFromPlay(SnakeController snake)
        {
            var group = MatchGroupById(snake.Id);
            _matchGroups.Remove(group);
            
            _gameContext.AvailableKeys.AddAvailableKey(group.Player.PlayerModel.LeftKey);
            _gameContext.AvailableKeys.AddAvailableKey(group.Player.PlayerModel.RightKey);
            
            Destroy(group.Player.gameObject);
            Destroy(group.Enemy.gameObject);
            Destroy(group.Block);

            group.OnRemoveFromPlay.Invoke();
            
            if (_matchGroups.IsNullOrEmpty())
                _gameContext.NavigationController.UpdateUI(GameState.Tutorial);
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
            player.SnakeController.OnTimeTravelPoint.AddListener(CreateSnapShot);
            player.SnakeController.OnRewind.AddListener(Rewind);
            player.SnakeController.OnRemoveFromGame.AddListener(RemoveFromPlay);
            
            player.SetConfig(spawnPoint, playerModel);
            return player;
        }

        private IAController SpawnEnemy(GameObject snakePrefab, PlayerModel playerModel, SpawnPoint spawnPoint)
        {
            var enemy = InstantiateSnake(snakePrefab, spawnPoint).AddComponent<IAController>();

            enemy.SnakeController.OnPick.AddListener(Pick);
            enemy.SnakeController.OnDie.AddListener(Die);
            enemy.SnakeController.OnTimeTravelPoint.AddListener(CreateSnapShot);
            enemy.SnakeController.OnRewind.AddListener(Rewind);
            enemy.SnakeController.OnRemoveFromGame.AddListener(RemoveFromPlay);
            
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