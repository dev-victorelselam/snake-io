using Game;
using UnityEngine;

namespace GameActors
{
    public class MovementController : MonoBehaviour
    {
        private SnakeController _snakeController;
        private PlayerModel _playerModel;
        
        public SnakeController SnakeController => _snakeController;

        public void SetConfig(PlayerModel playerModel)
        {
            gameObject.name = $"PlayerSnake: {playerModel.Username}";
            _playerModel = playerModel;
            _snakeController = GetComponent<SnakeController>();
            _snakeController.Initialize(playerModel, playerModel.Username);
        }

        public void Update()
        {
            if (Input.GetKeyDown(_playerModel.RightKey))
                _snakeController.MoveRight();
            else if (Input.GetKeyDown(_playerModel.LeftKey))
                _snakeController.MoveLeft();
        }
    }
}
