using System.Threading.Tasks;
using Context;
using Game;
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

        public async Task ActivatePowerUp(BlockType blockType)
        {
            
        }

        public void AddPlayer(PlayerModel playerModel)
        {
            var prefab = _context.GameSetup.PlayerScorePrefab;
            var text = Instantiate(prefab, _textsContainer.transform);
            
            text.text = playerModel.GetScore();
            text.color = playerModel.Color;
            
            playerModel.OnUpdate.AddListener(() => text.text = playerModel.GetScore());
        }
    }
}