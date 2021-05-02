using Context;
using UnityEngine;

namespace UI
{
    public class GameCanvas : MonoBehaviour, IGameUI
    {
        private IContext _context;
        public Transform Container => _container;
        public GameObject GameObject => gameObject;
        public GameState GameState => _gameState;
    
        [SerializeField] private GameState _gameState;
        [SerializeField] private Transform _container;
       

        public void StartUI()
        {
            _context = ContextProvider.Context;
        }

        public void Activate()
        {
            
        }

        public void Deactivate()
        {
            
        }

        public void DisplayOptions()
        {
            throw new System.NotImplementedException();
        }

        public void Tutorial()
        {
            
        }
    }
}