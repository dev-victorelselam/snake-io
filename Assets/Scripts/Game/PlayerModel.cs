﻿using Context;
using UnityEngine;

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
        internal PlayerModel(int id, Color color)
        {
            Color = color;
            Id = id;
        }
        
        public Color Color { get; }
        public int Id { get; }
        
        public CharacterSettings Character { get; set; }

        public KeyCode LeftKey { get; set; }
        public KeyCode RightKey { get; set; }

        public string Username { get; set; }
        public int Score { get; set; }
    }
}