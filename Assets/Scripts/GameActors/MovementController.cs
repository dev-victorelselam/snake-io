using Game;
using UnityEngine;

namespace GameActors
{
    public class MovementController : MonoBehaviour
    {
        private SnakeController _snakeController;
        private PlayerModel _playerModel;

        public SnakeController SnakeController
        {
            get
            {
                if (!_snakeController)
                    _snakeController = GetComponent<SnakeController>();
                return _snakeController;
            }
        }

        public void SetConfig(SpawnPoint spawn, PlayerModel playerModel)
        {
            gameObject.name = $"PlayerSnake: {playerModel.Username}";
            _playerModel = playerModel;
            SnakeController.Initialize(spawn, playerModel, playerModel.Username);
        }

        public void Update()
        {
            if (Input.GetKeyDown(_playerModel.RightKey))
                SnakeController.MoveRight();
            else if (Input.GetKeyDown(_playerModel.LeftKey))
                SnakeController.MoveLeft();
        }
    }
}
