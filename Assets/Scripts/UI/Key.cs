using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Key : MonoBehaviour
    {
        public KeyCode KeyCode;
        [SerializeField] private CanvasGroup _image;
        [SerializeField] private Text _text;

        private void Awake()
        {
            var text = KeyCode.ToString();
            if (text.Contains("Alpha"))
                text = text.Replace("Alpha", string.Empty);
            _text.text = text;
        }

        public void Enable(bool enable) => _image.DOFade(enable ? 1 : 0.4f, 0.1f);
    }
}