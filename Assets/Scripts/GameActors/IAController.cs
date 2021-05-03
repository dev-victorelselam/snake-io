using UI;
using UnityEngine;

namespace GameActors
{

    public class IAController : MonoBehaviour
    {
        private PlayerConfig _playerConfig;
        private SnakeController _snakeController;

        public void SetConfig(PlayerConfig playerConfig)
        {
            _snakeController = GetComponent<SnakeController>();
            _snakeController.Initialize(playerConfig);
        }

        public void Update()
        {
            
        }
    }
}
