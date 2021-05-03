using UnityEngine;

namespace Tutorial.KeyDetector
{
    public interface IKeyProvider
    {
        bool GetKey(KeyCode keyCode);
        bool GetKeyUp(KeyCode keyCode);
    }
}