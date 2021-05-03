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
            var baseSpeed = context.GameSetup.SnakeBaseSpeed;
            var loadedDecaySpeed = context.GameSetup.LoadedSpeedDecay;
            var finalSpeed = baseSpeed - (snakeController.Blocks.Count * loadedDecaySpeed);
            var speedBlocks = snakeController.Blocks.Where(b => b is SpeedBlockView).Cast<SpeedBlockView>();
            
            return speedBlocks.Aggregate(finalSpeed, (current, speedBlock) => speedBlock.Apply(current));
        }

        public static string Name(this KeyCode keyCode)
        {
            var name = keyCode.ToString();
            if (name.Contains("Alpha"))
                name = name.Replace("Alpha", string.Empty);

            return name;
        }
    }

    public static class StaticValues
    {
        public static float BlockSize = 1f;
    }

    public static class GameDelay
    {
        public static float Get()
        {
            var gameSpeed = ContextProvider.Context.GameSetup.GameSpeed;
            
            var normalized = gameSpeed / 5; //normalize gameSpeed 
            var delay = 1 - normalized;
            
            return delay;
        }
    }
}