using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tutorial.KeyDetector
{
    public class KeyDetector : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<List<KeyCode>> OnComplete = new UnityEvent<List<KeyCode>>();

        public void StartListen()
        {
            var keyProvider = KeyProviderFactoring.CreateInstance(gameObject);

            keyProvider.OnComplete.AddListener(OnComplete.Invoke);
            keyProvider.Enable();
        }
    }
}