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
            _ = new GameContext(_navigationController, _gameController, _popupUtility, _gameSetup);
            _gameController.StartController();
            _navigationController.StartController();
        }
    }
}
