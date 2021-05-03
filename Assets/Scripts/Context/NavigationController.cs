using System.Collections;
using System.Linq;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Context
{
    public class NavigationController : MonoBehaviour
    {
        public UnityEvent<GameState> OnStateChanged = new UnityEvent<GameState>();
        
        private IGameUI[] _gameUis;
        private GameState _currentState;

        private void Awake()
        {
            //because unity don't support interfaces serialization, we have to find a way to work with it
            _gameUis = GetComponentsInChildren<IGameUI>(true);
        }
        
        public void StartController()
        {
            foreach (var gameUi in _gameUis)
            {
                gameUi.Container.DOLocalMoveX(-1000, 0.3f).SetEase(Ease.InBack);
                gameUi.StartUI();
            }

            UpdateUI(GameState.PreGame);
        }

        public void UpdateUI(GameState state)
        {
            var enterUI = GetUIByState(state);
            var outUI = GetUIByState(_currentState);
        
            if (_currentState == GameState.None)
                StartCoroutine(ChangeScreenAnimation(enterUI, null));
            else
                StartCoroutine(ChangeScreenAnimation(enterUI, outUI));
        
            _currentState = state;
            if (outUI != null)
                outUI.Deactivate();
            enterUI.Activate();
            
            OnStateChanged.Invoke(_currentState);
        }

        private IEnumerator ChangeScreenAnimation(IGameUI enterScreen, IGameUI outScreen)
        {
            enterScreen.GameObject.SetActive(true);
            enterScreen.Container.DOLocalMoveX(-3000, 0f);

            if (outScreen != null)//can't use null propagation, Unity References bypass them
            {
                yield return outScreen.Container.DOLocalMoveX(-3000, 0.3f).SetEase(Ease.InBack).WaitForCompletion();
                outScreen.GameObject.SetActive(false);
            } 
            
            enterScreen.Container.DOLocalMoveX(0, 0.3f).SetEase(Ease.OutBack);
        }

        public IGameUI GetUIByState(GameState state) => _gameUis.FirstOrDefault(g => g.GameState == state);
    }
}