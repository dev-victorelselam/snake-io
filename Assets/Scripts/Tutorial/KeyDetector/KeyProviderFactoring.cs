using System;
using Context;
using UnityEngine;
using Environment = Context.Environment;

namespace Tutorial.KeyDetector
{
    public static class KeyProviderFactoring
    {
        public static IKeyProvider CreateInstance(GameObject gameObject)
        {
            switch (ContextProvider.Context.Environment)
            {
                case Environment.Game:
                    return gameObject.AddComponent<InputKeyProvider>();
                case Environment.Test:
                    return new TestKeyProvider();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}