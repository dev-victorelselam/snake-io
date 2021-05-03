using Context;
using Game;
using UI;
using UnityEngine;

namespace Tutorial
{
    public class TutorialCanvas : MonoBehaviour, IGameUI
    {
        public Transform Container => _container;
        public GameObject GameObject => gameObject;
        public GameState GameState => _gameState;
        [SerializeField] private GameState _gameState;
        [SerializeField] private Transform _container;
        [Space(10)]
        
        [SerializeField] private CharacterSelection _characterSelection;
        [SerializeField] private PlayerInfoSelection _playerInfoSelection;

        private IContext _context;
        
        public void StartUI()
        {
            _context = ContextProvider.Context;
            _characterSelection.OnCharacterSelected.AddListener(PlayerInfoSelection);
            _playerInfoSelection.OnInfoFinished.AddListener(StartGame);
        }

        public void Activate()
        {
            CharacterSelection();
        }

        private void CharacterSelection()
        {
            _characterSelection.Show(_context.Characters.All, new PlayerConfig());
            ChangeStep(1);
        }
        
        private void PlayerInfoSelection(PlayerConfig playerConfig)
        {
            _playerInfoSelection.Show(playerConfig);
            ChangeStep(2);
        }

        private void StartGame(PlayerConfig playerConfig)
        {
            _context.GameController.AddPlayer(playerConfig);
            _context.NavigationController.UpdateUI(GameState.Game);
        }

        public void Deactivate()
        {
            
        }

        private void ChangeStep(int step)
        {
            switch (step)
            {
                case 1:
                    _characterSelection.gameObject.SetActive(true);
                    _playerInfoSelection.gameObject.SetActive(false);
                    break;
                case 2:
                    _characterSelection.gameObject.SetActive(false);
                    _playerInfoSelection.gameObject.SetActive(true);
                    break;
            }
        }
    }
}