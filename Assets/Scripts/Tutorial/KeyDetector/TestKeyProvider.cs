using UnityEngine;

namespace Tutorial.KeyDetector
{
    public class TestKeyProvider : IKeyProvider
    {
        private const KeyCode SecondKey = KeyCode.A;
        private const KeyCode FirstKey = KeyCode.D;
        
        public bool GetKey(KeyCode keyCode) => keyCode == FirstKey || keyCode == SecondKey;
        public bool GetKeyUp(KeyCode keyCode) => keyCode != FirstKey && keyCode != SecondKey;
    }
}