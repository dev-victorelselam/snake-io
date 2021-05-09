using Context;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public static class PlayerCreator //creator pattern to ensure exclusive ids
    {
        private static int _idCount;
        public static PlayerModel New()
        {
            _idCount++;
            return new PlayerModel(_idCount, Extensions.RandomColor());
        }
    }
    
    public class PlayerModel
    {
        public readonly UnityEvent OnUpdate = new UnityEvent();
        
        internal PlayerModel(int id, Color color)
        {
            Color = color;
            Id = id;
        }
        
        public Color Color { get; }
        public int Id { get; }
        public string Username { get; set; }
        
        public CharacterSettings Character { get; set; }
        public KeyCode LeftKey { get; set; }
        public KeyCode RightKey { get; set; }
        
    }
}