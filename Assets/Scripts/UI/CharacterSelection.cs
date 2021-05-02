using System.Collections.Generic;
using Context;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class CharacterSelection : MonoBehaviour
    {
        public UnityEvent<PlayerConfig> OnCharacterSelected = new UnityEvent<PlayerConfig>();
        
        [SerializeField] private ScrollRect _charactersScroll;
        [SerializeField] private Button _nextCard;
        [SerializeField] private Button _previousCard;
        
        [Space(10)]
        [SerializeField] private HorizontalLayoutGroup _cardsContainer;
        [SerializeField] private CharacterCard _characterCardPrefab;
        
        private float ScrollStep => 1 / ((float) _cards.Count - 1);
        private readonly List<CharacterCard> _cards = new List<CharacterCard>();
        private PlayerConfig _playerConfig;

        private void Awake()
        {
            _nextCard.onClick.AddListener(NextCard);
            _previousCard.onClick.AddListener(PreviousCard);
        }
        
        public void Show(List<CharacterSettings> characterSettings, PlayerConfig playerConfig)
        {
            //optionally, we could destroy all cards and rebuild.
            //if we had a feature of creating new characters in runtime for example
            if (!_cards.IsNullOrEmpty())
                return;
            foreach (var characterSetting in characterSettings)
            {
                var obj = Instantiate(_characterCardPrefab, _cardsContainer.transform);
                _cards.Add(obj);
                
                obj.SetModel(characterSetting);
                obj.OnCharacterSelected.AddListener(CharacterSelected);
            }

            _playerConfig = playerConfig;
        }

        private void CharacterSelected(CharacterSettings character)
        {
            _playerConfig.Character = character;
            OnCharacterSelected.Invoke(_playerConfig);
        }

        private void NextCard()
        {
            var position = _charactersScroll.horizontalNormalizedPosition;
            var finalValue = Mathf.Clamp01(position + ScrollStep);
            
            _charactersScroll.DOHorizontalNormalizedPos(finalValue, 0.2f).SetEase(Ease.OutBack);
        }
        
        private void PreviousCard()
        {
            var position = _charactersScroll.horizontalNormalizedPosition;
            var finalValue = Mathf.Clamp01(position - ScrollStep);
            
            _charactersScroll.DOHorizontalNormalizedPos(finalValue, 0.2f).SetEase(Ease.OutBack);
        }
    }
}