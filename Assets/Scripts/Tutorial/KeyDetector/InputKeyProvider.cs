using UnityEngine;

namespace Tutorial.KeyDetector
{
    public class InputKeyProvider : MonoBehaviour, IKeyProvider
    {
        public bool GetKey(KeyCode keyCode) => Input.GetKey(keyCode);

        public bool GetKeyUp(KeyCode keyCode) => Input.GetKeyUp(keyCode);
    }
}