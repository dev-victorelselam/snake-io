using Game;
using UnityEngine;

namespace GameActors
{

    public class IAController : MonoBehaviour
    {
        private PlayerModel _playerModel;
        private SnakeController _snakeController;

        public SnakeController SnakeController => _snakeController;

        public void SetConfig(PlayerModel playerModel)
        {
            gameObject.name = $"IASnake: {playerModel.Username}";
            _snakeController = GetComponent<SnakeController>();
            _snakeController.Initialize(playerModel, $"IA-{playerModel.Username}");
        }

        public void Update()
        {
            
        }
    }
}
