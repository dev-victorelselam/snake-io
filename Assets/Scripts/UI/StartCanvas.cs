using System.Collections;
using Context;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class StartCanvas : MonoBehaviour, IGameUI
    {
        public UnityEvent<GameState> OnStateChanged { get; } = new UnityEvent<GameState>();
        public Transform Container => _container;
        public GameObject GameObject => gameObject;
        public GameState GameState => _gameState;

        [SerializeField] private Button _startGame;
        [SerializeField] private GameState _gameState;
        [SerializeField] private Transform _container;
        private IContext _context;

        private void Start()
        {
            _startGame.onClick.AddListener(() => _context.NavigationController.UpdateUI(GameState.Tutorial));
        }

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
    }
}