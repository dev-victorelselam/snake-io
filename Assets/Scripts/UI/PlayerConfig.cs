using Context;
using UnityEngine;

namespace UI
{
    public class PlayerConfig
    {
        public PlayerConfig()
        {
            Color = Extensions.RandomColor();
        }
        
        public CharacterSettings Character;

        public KeyCode LeftKey;
        public KeyCode RightKey;

        public string Username;
        public int Score;

        public Color Color;
    }
}