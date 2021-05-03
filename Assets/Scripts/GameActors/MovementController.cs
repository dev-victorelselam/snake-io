using Context;
using Game;
using UI;
using UnityEngine;

namespace GameActors
{
    public class MovementController : MonoBehaviour
    {
        private SnakeController _snakeController;
        private PlayerModel _playerModel;

        public int Id => _playerModel.Id;

        public void SetConfig(PlayerModel playerModel)
        {
            _playerModel = playerModel;
            _snakeController = GetComponent<SnakeController>();
            _snakeController.Initialize(playerModel);
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
