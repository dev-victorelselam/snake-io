using System.Collections.Generic;
using Context;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tutorial
{
    public class PlayerInfoSelection : MonoBehaviour
    {
        public UnityEvent<PlayerConfig> OnInfoFinished = new UnityEvent<PlayerConfig>();
        
        [SerializeField] private InputField _userName;
        [SerializeField] private Button _finishConfiguration;
        [SerializeField] private Keyboard _keyboard;
        [SerializeField] private KeyDetector _keyDetector;
        
        private PlayerConfig _playerConfig;

        private void Awake()
        {
            _finishConfiguration.onClick.AddListener(
                () =>
                {
                    _playerConfig.Username = _userName.text;
                    OnInfoFinished.Invoke(_playerConfig);
                });
        }

        public void Show(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;
            _finishConfiguration.enabled = false;
            
            _keyboard.Initialize();
            _keyDetector.Enable();
            _keyDetector.OnComplete.AddListener(ConfirmButtonsDialog);
        }
        
        private void ConfirmButtonsDialog(List<KeyCode> keys)
        {
            PopupUtility.Instance.ShowDialog($"Os botões escolhidos foram: {keys[0]} e {keys[1]}", 
                () => AssignKeys(keys), 
                () => _keyDetector.Enable());
        }

        private void AssignKeys(IReadOnlyList<KeyCode> keys)
        {
            var availableKeys = ContextProvider.Context.AvailableKeys;
            availableKeys.RemoveAvailableKey(keys[0]);
            availableKeys.RemoveAvailableKey(keys[1]);

            _keyboard.UpdateKeys();

            _playerConfig.LeftKey = keys[0];
            _playerConfig.RightKey = keys[1];
            
            _finishConfiguration.enabled = true;
        }
    }
}