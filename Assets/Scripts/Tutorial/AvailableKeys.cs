using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tutorial
{
    public class AvailableKeys
    {
        private readonly List<KeyCode> _allKeys;
        private readonly List<KeyCode> _availableKeys;
        private readonly List<KeyCode> _unavailableKeys;
        public AvailableKeys(List<KeyCode> availableKeys)
        {
            _allKeys = availableKeys.ToList();
            _availableKeys = availableKeys.ToList();
            _unavailableKeys = new List<KeyCode>();
        }

        public List<KeyCode> All => _allKeys.ToList();
        public List<KeyCode> Available => _availableKeys.ToList();
        public List<KeyCode> Unavailable => _unavailableKeys.ToList();

        /// <summary>
        /// Remove available Key from List
        /// </summary>
        /// <param name="key"></param>
        /// <returns>return false if operation fail, true if success</returns>
        public bool RemoveAvailableKey(KeyCode key)
        {
            if (!_availableKeys.Contains(key))
                return false;
            _availableKeys.Remove(key);
            _unavailableKeys.Add(key);
            return true;
        }

        /// <summary>
        /// Add available Key from List
        /// </summary>
        /// <param name="key"></param>
        /// <returns>return false if operation fail, true if success</returns>
        public bool AddAvailableKey(KeyCode key)
        {
            if (_availableKeys.Contains(key))
                return false;
            _availableKeys.Add(key);
            _unavailableKeys.Remove(key);
            return true;
        }
    }
}