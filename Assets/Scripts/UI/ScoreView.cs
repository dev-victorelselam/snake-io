using Context;
using GameActors;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private Text _text;
        
        private MatchGroup _group;
        private SnakeController _snake;

        public void SetSnake(MatchGroup group, SnakeController snake)
        {
            _snake = snake;
            _group = group;
            group.OnScoreUpdate.AddListener(UpdateView);
            group.OnRemoveFromPlay.AddListener(() => Destroy(gameObject));
            UpdateView();
        }

        private void UpdateView() => _text.text = $"{_snake.Name}: {_group.GetScore(_snake)}";
    }
}