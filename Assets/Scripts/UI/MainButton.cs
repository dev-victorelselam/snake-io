using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainButton : MonoBehaviour
    {
        [SerializeField] private Image _buttonShine;

        private void Start()
        {
            _buttonShine.DOFade(0.2f, 1f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
