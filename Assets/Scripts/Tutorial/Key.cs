using Context;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class Key : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _image;
        [SerializeField] private Text _text;

        public void SetKey(KeyCode keyCode)
        {
            _text.text = keyCode.Name();
        }

        public void Enable(bool enable) => _image.DOFade(enable ? 1 : 0.4f, 0.1f);
    }
}