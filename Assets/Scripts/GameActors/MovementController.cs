using Context;
using UI;
using UnityEngine;

namespace GameActors
{
    public class MovementController : MonoBehaviour
    {
        private SnakeController _snakeController;
        private PlayerConfig _playerConfig;

        public void SetPlayerConfig(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;
            _snakeController = GetComponent<SnakeController>();
            _snakeController.Initialize(playerConfig);
        }

        public void Update()
        {
            if (Input.GetKeyDown(_playerConfig.RightKey))
                _snakeController.MoveRight();
            else if (Input.GetKeyDown(_playerConfig.LeftKey))
                _snakeController.MoveLeft();
        }
    }
}
