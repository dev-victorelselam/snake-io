using System;
using Game;
using UnityEngine;

namespace Context
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField] private GameSetup _gameSetup;
        [Space(10)]
        [SerializeField] private NavigationController _navigationController;
        [SerializeField] private GameController _gameController;
        [SerializeField] private PopupUtility _popupUtility;
        
        private void Start()
        {
            new GameContext(_navigationController, _gameController, _popupUtility, _gameSetup);
            _gameController.StartController();
            _navigationController.StartController();
            
            _navigationController.OnStateChanged.AddListener(UpdateState);
        }

        private void UpdateState(GameState state)
        {
            switch (state)
            {
                case GameState.None:
                    break;
                case GameState.PreGame:
                    _gameController.StopGame();
                    break;
                case GameState.Tutorial:
                    _gameController.PauseGame();
                    break;
                case GameState.Game:
                    _gameController.StartGame();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
