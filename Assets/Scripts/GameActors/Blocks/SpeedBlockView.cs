using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameActors.Blocks
{
    public class SpeedBlockView : BlockView
    {
        public float SpeedValue => _boostValue;
        [SerializeField] [Range(0, 20)] private float _boostValue;
        [SerializeField] private TextMeshProUGUI _textValue;
        
        protected override void Awake()
        {
            base.Awake();
            _textValue.text = $"Boost: +{_boostValue}";
            _textValue.transform.DOScale(1.15f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}