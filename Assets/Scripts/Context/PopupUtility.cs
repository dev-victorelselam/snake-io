using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Context
{
    public class PopupUtility : MonoBehaviour
    {
        public static PopupUtility Instance;
        
        [SerializeField] private Transform _fade;
        [SerializeField] private Transform _popup;
        [SerializeField] private Text _title;
        [Space(10)]
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        private Action _confirmAction;
        private Action _cancelAction;

        private void Awake()
        {
            //simplified version of singleton
             if (Instance == null)
                 Instance = this;
            
             _confirmButton.onClick.AddListener(() =>
             {
                 _confirmAction?.Invoke();
                 Hide();
             });
            
             _cancelButton.onClick.AddListener(() =>
             {
                 _cancelAction?.Invoke();
                 Hide();
             });
             
             Hide();
        }
        
        public void ShowDialog(string text, Action confirmAction, Action cancelAction)
        {
            _confirmAction = confirmAction;
            _cancelAction = cancelAction;
            _title.text = text;
            
            _fade.gameObject.SetActive(true);
            _popup.gameObject.SetActive(true);
            
            _popup.transform.localScale = Vector3.zero;
            _popup.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }

        public void Hide(bool animate = true)
        {
            _fade.gameObject.SetActive(false);
            _popup.DOScale(0, animate ? 0.3f : 0f).SetEase(Ease.InBack).
                OnComplete(() =>  _popup.gameObject.SetActive(true));
        }
    }
}