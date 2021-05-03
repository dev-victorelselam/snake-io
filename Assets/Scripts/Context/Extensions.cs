using System.Collections;
using System.Linq;
using GameActors;
using UnityEngine;

namespace Context
{
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

        public static float Speed(this SnakeController snakeController)
        {
            var context = ContextProvider.Context;
            var baseSpeed = context.GameSetup.BaseSpeed;
            var loadedDecaySpeed = context.GameSetup.LoadedSpeedDecay;
            var finalSpeed = baseSpeed - (snakeController.Blocks.Count * loadedDecaySpeed);
            var speedBlocks = snakeController.Blocks
                .Where(b => b is SpeedBlockView)
                .Cast<SpeedBlockView>();
            
            //speed need to be in inverse proportion, because more speed = less time
            var result = finalSpeed;
            foreach (var block in speedBlocks) 
                result = block.Apply(result);
            result = 1 / result;
            return result;
        }

        public static string Name(this KeyCode keyCode)
        {
            var name = keyCode.ToString();
            if (name.Contains("Alpha"))
                name = name.Replace("Alpha", string.Empty);

            return name;
        }
    }
}