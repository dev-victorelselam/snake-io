using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class Keyboard : MonoBehaviour
    {
        [SerializeField] private Key[] _numbersKeys;
        [SerializeField] private Key[] _letters1;
        [SerializeField] private Key[] _letters2;
        [SerializeField] private Key[] _letters3;
        private List<Key> _allKeysList;

        public void Initialize(List<KeyCode> allKeys, List<KeyCode> availableKeys)
        {
            _allKeysList = _numbersKeys.ToList();
            _allKeysList.AddRange(_letters1);
            _allKeysList.AddRange(_letters2);
            _allKeysList.AddRange(_letters3);

            if (_allKeysList.Count != allKeys.Count)
            {
                Debug.LogError("Check Available Keys!");
                return;
            }

            for (var i = 0; i < _allKeysList.Count; i++)
            {
                _allKeysList[i].KeyCode = allKeys[i];
                _allKeysList[i].Enable(availableKeys.Contains(allKeys[i]));
            }
        }

        public void EnableKey(KeyCode keyCode)
        {
            var key =  KeyByCode(keyCode);
            key.Enable(true);
        }

        public void DisableKey(KeyCode keyCode)
        {
            var key = KeyByCode(keyCode);
            key.Enable(false);
        }
        
        private Key KeyByCode(KeyCode keyCode) => _allKeysList.First(k => k.KeyCode == keyCode);
    }
}