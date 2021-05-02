using System;
using System.Collections.Generic;
using Context;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class PlayerInfoSelection : MonoBehaviour
    {
        public UnityEvent<PlayerConfig> OnInfoFinished = new UnityEvent<PlayerConfig>();
        
        [SerializeField] private InputField _userName;
        [SerializeField] private Button _finishConfiguration;
        [SerializeField] private Keyboard _keyboard;
        
        private PlayerConfig _playerConfig;
        private bool _checkingButtons;
        private List<KeyCode> _keys;

        private Dictionary<KeyCode, float> _keysPressed;
        private AvailableKeys _availableKeys;

        private void Awake()
        {
            _finishConfiguration.onClick.AddListener(
                () =>
                {
                    _playerConfig.Username = _userName.text;
                    OnInfoFinished.Invoke(_playerConfig);
                });
        }

        public void Show(AvailableKeys availableKeys, PlayerConfig playerConfig)
        {
            _keys = new List<KeyCode>();
            _keysPressed = new Dictionary<KeyCode, float>();
            
            _availableKeys = availableKeys;
            _playerConfig = playerConfig;
            _checkingButtons = true;
            
            _finishConfiguration.enabled = false;
            
            _keyboard.Initialize(availableKeys.All, availableKeys.Available);
        }
        
        private void ConfirmButtonsDialog()
        {
            PopupUtility.Instance.ShowDialog($"Os botões escolhidos foram: {_keys[0]} e {_keys[1]}", 
                () =>
                {
                    AssignKeys(_keys);
                    
                    _finishConfiguration.enabled = true;
                }, 
                () =>
                {
                    _keys.Clear();
                    _keysPressed.Clear();
                    _checkingButtons = true;
                });
        }

        private void AssignKeys(List<KeyCode> keys)
        {
            _availableKeys.RemoveAvailableKey(keys[0]);
            _availableKeys.RemoveAvailableKey(keys[1]);
            
            _keyboard.DisableKey(keys[0]);
            _keyboard.DisableKey(keys[1]);
            
            _playerConfig.LeftKey = keys[0];
            _playerConfig.RightKey = keys[1];
        }

        public void Update()
        {
            if (!_checkingButtons)
                return;
            
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {

                if (Input.GetKey(keyCode))
                {
                    if (_keysPressed.ContainsKey(keyCode))
                        _keysPressed[keyCode] += Time.deltaTime;
                    else
                        _keysPressed[keyCode] = 0;
                    
                    Debug.Log($"{keyCode} pressed! with time: {_keysPressed[keyCode]}");
                }
                

                if (Input.GetKeyUp(keyCode))
                {
                    if (_keysPressed.ContainsKey(keyCode))
                        _keysPressed[keyCode] = 0;
                }
            }

            foreach (var keyValue in _keysPressed)
            {
                var key = keyValue.Key;
                var value = keyValue.Value;

                if (value >= 1)
                {
                    if (!_keys.Contains(key))
                    {
                        _keys.Add(key);
                        Debug.Log($"Adding key: {key}");
                    }
                        
                    if (_keys.Count >= 2)
                    {
                        _checkingButtons = false;
                        ConfirmButtonsDialog();
                        
                        Debug.Log($"Finish with: {_keys[0]} and {_keys[1]}");
                    }
                }
            }
        }
    }
}