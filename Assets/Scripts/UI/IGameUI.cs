using Context;
using UnityEngine;

namespace UI
{
    public interface IGameUI
    {
        Transform Container { get; }
        GameObject GameObject { get; }
        GameState GameState { get; }
        
        void StartUI();
        void Activate();
        void Deactivate();
    }
}