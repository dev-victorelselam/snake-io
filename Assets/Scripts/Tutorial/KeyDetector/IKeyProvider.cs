using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tutorial.KeyDetector
{
    public interface IKeyProvider
    {
        UnityEvent<List<KeyCode>> OnComplete { get; }

        void Enable();
    }
}