using System;
using System.Linq;
using System.Threading.Tasks;
using Context;
using Game;
using GameActors;
using GameActors.Blocks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameCanvas : MonoBehaviour, IGameUI
    {
        private IContext _context;
        public Transform Container => _container;
        public GameObject GameObject => gameObject;
        public GameState GameState => _gameState;
    
        [SerializeField] private GameState _gameState;
        [SerializeField] private Transform _container;
        [Space(10)]
        [SerializeField] private VerticalLayoutGroup _textsContainer;
        [SerializeField] private Transform _notificationContainer;
        [SerializeField] private PowerUpUI[] _powerUpsViews;


        public void StartUI()
        {
            _context = ContextProvider.Context;
        }

        public void Activate()
        {
            
        }

        public void Deactivate()
        {
            
        }

        public async Task ActivatePowerUpView(BlockType blockType)
        {
            var view = _powerUpsViews.FirstOrDefault(puv => puv.BlockType == blockType);
            if (view == null)
                return;
            
            view.View.SetActive(true);
            await Task.Delay(1500);
            view.View.SetActive(false);
        }

        public void AddGroup(MatchGroup group)
        {
            var prefab = _context.GameSetup.PlayerScorePrefab;
            var player = Instantiate(prefab, _textsContainer.transform);
            var ia = Instantiate(prefab, _textsContainer.transform);
            
            player.SetSnake(group, group.Player.SnakeController);
            ia.SetSnake(group, group.Enemy.SnakeController);
        }

        public void NotifyKill(SnakeController deadSnake, SnakeController killerSnake)
        {
            var prefab = _context.GameSetup.NotificationPrefab;
            var notification = Instantiate(prefab, _notificationContainer);

            var snakeName = killerSnake == null ? string.Empty : killerSnake.Name;
            notification.Activate($"{snakeName} -> {deadSnake.Name}");
        }
    }

    [Serializable]
    internal class PowerUpUI
    {
        public BlockType BlockType;
        public GameObject View;
    }
}