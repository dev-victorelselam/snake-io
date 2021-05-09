﻿using System.Collections.Generic;
using System.Linq;
using Context;
using UI;
using UnityEngine;

namespace Tutorial
{
    public class Keyboard : MonoBehaviour
    {
        [SerializeField] private Key[] _numbersKeys;
        [SerializeField] private Key[] _letters1;
        [SerializeField] private Key[] _letters2;
        [SerializeField] private Key[] _letters3;
        private List<Key> _allKeysView;

        public void Initialize()
        {
            var allKeys = ContextProvider.Context.AvailableKeys.All;
            
            _allKeysView = _numbersKeys.ToList();
            _allKeysView.AddRange(_letters1);
            _allKeysView.AddRange(_letters2);
            _allKeysView.AddRange(_letters3);

            if (_allKeysView.Count != allKeys.Count)
            {
                Debug.LogError("Check Available Keys!");
                return;
            }

            UpdateKeys();
        }

        public void UpdateKeys()
        {
            var allKeys = ContextProvider.Context.AvailableKeys.All;
            var availableKeys = ContextProvider.Context.AvailableKeys.Available;
            
            for (var i = 0; i < _allKeysView.Count; i++)
            {
                _allKeysView[i].SetKey(allKeys[i]);
                _allKeysView[i].Enable(availableKeys.Contains(allKeys[i]));
            }
        }
    }
}