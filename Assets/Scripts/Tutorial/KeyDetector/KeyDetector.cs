﻿using System;
using System.Collections.Generic;
using Context;
using UnityEngine;
using UnityEngine.Events;

namespace Tutorial.KeyDetector
{
    public class KeyDetector : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<List<KeyCode>> OnComplete = new UnityEvent<List<KeyCode>>();

        private bool _enabled;
        private List<KeyCode> _keys;
        private Dictionary<KeyCode, float> _keysPressed;
        private AvailableKeys _availableKeys;
        
        private IKeyProvider _keyProvider;

        public void StartListen()
        {
            _keys = new List<KeyCode>();
            _keysPressed = new Dictionary<KeyCode, float>();
            _availableKeys = ContextProvider.Context.AvailableKeys;
            
            _keyProvider = KeyProviderFactoring.CreateInstance(gameObject);

            _enabled = true;
        }

        private void Disable() => _enabled = false;
        
        public void Update()
        {
            if (!_enabled)
                return;

            var keys = _availableKeys.Available;
            foreach (KeyCode keyCode in keys)
            {
                if (_keyProvider.GetKey(keyCode))
                {
                    if (_keysPressed.ContainsKey(keyCode))
                        _keysPressed[keyCode] += Time.deltaTime;
                    else
                        _keysPressed[keyCode] = 0;
                }

                if (_keyProvider.GetKeyUp(keyCode))
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
                    if (!_keys.Contains(key) && _availableKeys.Available.Contains(key))
                        _keys.Add(key);
                        
                    if (_keys.Count >= 2)
                    {
                        Debug.Log($"Finish with: {_keys[0]} and {_keys[1]}");
                        
                        OnComplete.Invoke(_keys);
                        Disable();
                    }
                }
            }
        }
    }
}