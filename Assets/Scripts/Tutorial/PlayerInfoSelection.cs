using System.Collections.Generic;
using Context;
using Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tutorial
{
    public class PlayerInfoSelection : MonoBehaviour
    {
        public UnityEvent<PlayerModel> OnInfoFinished = new UnityEvent<PlayerModel>();
        
        [SerializeField] private InputField _userName;
        [SerializeField] private Button _finishConfiguration;
        [SerializeField] private Keyboard _keyboard;
        [SerializeField] private KeyDetector.KeyDetector _keyDetector;
        
        private PlayerModel _playerModel;

        private void Awake()
        {
            _finishConfiguration.onClick.AddListener(
                () =>
                {
                    _playerModel.Username = _userName.text;
                    OnInfoFinished.Invoke(_playerModel);
                    _playerModel = null;
                });
        }

        public void Show(PlayerModel playerModel)
        {
            _playerModel = playerModel;
            _finishConfiguration.enabled = false;
            
            _keyboard.Initialize();
            _keyDetector.StartListen();
            _keyDetector.OnComplete.AddListener(ConfirmButtonsDialog);
        }
        
        private void ConfirmButtonsDialog(List<KeyCode> keys)
        {
            PopupUtility.Instance.ShowDialog($"Os botões escolhidos foram: {keys[0].Name()} e {keys[1].Name()}", 
                () => AssignKeys(keys), 
                () => _keyDetector.StartListen());
        }

        private void AssignKeys(IReadOnlyList<KeyCode> keys)
        {
            var availableKeys = ContextProvider.Context.AvailableKeys;
            availableKeys.RemoveAvailableKey(keys[0]);
            availableKeys.RemoveAvailableKey(keys[1]);

            _keyboard.UpdateKeys();

            _playerModel.LeftKey = keys[0];
            _playerModel.RightKey = keys[1];
            
            _finishConfiguration.enabled = true;
        }
    }
}