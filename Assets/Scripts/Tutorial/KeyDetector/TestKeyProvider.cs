using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Tutorial.KeyDetector
{
    public class TestKeyProvider : IKeyProvider
    {
        public UnityEvent<List<KeyCode>> OnComplete { get; } = new UnityEvent<List<KeyCode>>();
        public async void Enable()
        {
            await Task.Delay(3000);
            
            OnComplete.Invoke(new List<KeyCode> {KeyCode.A, KeyCode.D});
        }
    }
}