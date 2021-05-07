using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using GameActors;
using UnityEngine;

namespace Context
{
    public static class Direction
    {
        public const int Right = 270;
        public const int Left = 90;
        public const int Up = 0;
        public const int Down = 180;
    }
    
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this ICollection collection) =>
            collection == null || collection.Count == 0;

        public static Color RandomColor()
        {
            var r = Random.Range(0, 1f);
            var g = Random.Range(0, 1f);
            var b = Random.Range(0, 1f);
            return new Color(r, g, b, 1);
        }

        public static T GetRandom<T>(this IEnumerable<T> list)
        {
            var enumerable = list as T[] ?? list.ToArray();
            var random = Random.Range(0, enumerable.Length);
            return enumerable[random];
        }

        public static float Speed(this ISpeedable speedable)
        {
            var context = ContextProvider.Context;
            var baseSpeed = context.GameSetup.BaseSpeed;
            var loadedDecaySpeed = context.GameSetup.LoadedSpeedDecay;
            
            //speed need to be in inverse proportion, because more speed = less time
            //so x = 1 / y
            return 1 / (baseSpeed - (speedable.Loads * loadedDecaySpeed) + speedable.SpeedBlocks.Sum());
        }

        public static string Name(this KeyCode keyCode)
        {
            var name = keyCode.ToString();
            if (name.Contains("Alpha"))
                name = name.Replace("Alpha", string.Empty);

            return name;
        }

        public static Vector3 FindFairPosition(Vector3[] positions)
        {
            //usually the best way to find a fair position is on center of all positions
            var center = new Vector3(0, 0, 0);
            foreach (var t in positions)
                center += t;
            return center / positions.Length;
        }

        public static SnakeSnapshot GetSnapshot(this SnakeController snakeController)
        {
            return new SnakeSnapshot(snakeController);
        }
        
        public static Vector3 GetDirection(this Vector3 eulerAngles)
        {
            switch (eulerAngles.z)
            {
                case Direction.Up:
                    return new Vector3(0, -1, 0);
                case Direction.Down:
                    return new Vector3(0, 1, 0);
                case Direction.Left:
                    return new Vector3(1, 0, 0);
                case Direction.Right:
                    return new Vector3(-1, 0, 0);
                default:
                    return new Vector3(0, -1, 0);
            }
        }

        public static float BlockSize = 1.1f;
    }
}