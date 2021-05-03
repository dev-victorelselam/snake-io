using Game;
using UI;
using UnityEngine;

namespace GameActors
{

    public class IAController : MonoBehaviour
    {
        private PlayerModel _playerModel;
        private SnakeController _snakeController;

        public void SetConfig(PlayerModel playerModel)
        {
            _snakeController = GetComponent<SnakeController>();
            _snakeController.Initialize(playerModel);
        }

        public void Update()
        {
            
        }
    }
}
