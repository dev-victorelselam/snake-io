using System.Collections.Generic;
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
        private List<Key> _allKeysList;

        public void Initialize()
        {
            var allKeys = ContextProvider.Context.AvailableKeys.All;
            
            _allKeysList = _numbersKeys.ToList();
            _allKeysList.AddRange(_letters1);
            _allKeysList.AddRange(_letters2);
            _allKeysList.AddRange(_letters3);

            if (_allKeysList.Count != allKeys.Count)
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
            
            for (var i = 0; i < _allKeysList.Count; i++)
            {
                _allKeysList[i].KeyCode = allKeys[i];
                _allKeysList[i].Enable(availableKeys.Contains(allKeys[i]));
            }
        }
    }
}