using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using GameActors;
using GameActors.Blocks;
using UnityEngine;

namespace Context
{
    public static class Angle
    {
        public const int Up = 0;
        public const int Right = 270;
        public const int Down = 180;
        public const int Left = 90;
    }

    public static class Direction
    {
        public static Vector3 Up = new Vector3(0, 1, 0);
        public static Vector3 Right = new Vector3(1, 0, 0);
        public static Vector3 Down = new Vector3(0, -1, 0);
        public static Vector3 Left = new Vector3(-1, 0, 0);
    }

    public static class StaticValues
    {
        public static float BlockDisplacement = 2.1f;
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
            //however, center is boring and always give the same gameplay, so we add a small random offset
            const float minOffset = -12f;
            const float maxOffset = 12f;
            var offSet = new Vector3(Random.Range(minOffset, maxOffset), Random.Range(minOffset, maxOffset), 0);

            var center = new Vector3(0, 0, 0);
            foreach (var t in positions)
                center += t;
            center /= positions.Length;

            return center + offSet;
        }

        public static SnakeSnapshot GetSnapshot(this SnakeController snakeController) 
            => new SnakeSnapshot(snakeController);

        public static Vector3 GetInverseDirection(this SpawnPoint.SpawnDirection dir)
        {
            switch (dir)
            {
                case SpawnPoint.SpawnDirection.Up:
                    return Direction.Up * -1;
                case SpawnPoint.SpawnDirection.Down:
                    return Direction.Down * -1;
                case SpawnPoint.SpawnDirection.Left:
                    return Direction.Left * -1;
                case SpawnPoint.SpawnDirection.Right:
                    return Direction.Right * -1;
                default:
                    return Direction.Up * -1;
            }
        }
        
        public static int GetAngle(this SpawnPoint.SpawnDirection dir)
        {
            switch (dir)
            {
                case SpawnPoint.SpawnDirection.Up:
                    return Angle.Up;
                case SpawnPoint.SpawnDirection.Down:
                    return Angle.Down;
                case SpawnPoint.SpawnDirection.Left:
                    return Angle.Left;
                case SpawnPoint.SpawnDirection.Right:
                    return Angle.Right;
                default:
                    return Angle.Up;
            }
        }

        public static Vector3 RandomDirection()
        {
            var random = Random.Range(0f, 1f);
            if (random <= 0.25)
                return Direction.Up;
            if (random <= 0.5)
                return Direction.Down;
            if (random <= 0.75)
                return Direction.Left;
            
            return Direction.Right;
        }

        public static void ApplySnapshot(this SnakeController snakeController, SnakeSnapshot snapshot)
        {
            snakeController.Blocks.ForEach(b => Object.Destroy(b.gameObject));
            snakeController.Blocks.Clear();
            
            snakeController.DisableCollisions(0.5f);

            foreach (var blockSnapshot in snapshot.BlocksSnapshot)
            {
                var block = snakeController.AddBlock(blockSnapshot.BlockType);
                if (block is TimeTravelBlockView timeTravelBlockView)
                {
                    var timeTravel = (Dictionary<SnakeController, SnakeSnapshot>) blockSnapshot.Payload;
                    timeTravelBlockView.SetSnapshot(timeTravel);
                }

                block.CurrentAngle = blockSnapshot.CurrentAngle;
                block.transform.localPosition = blockSnapshot.Position;
                block.transform.localEulerAngles = blockSnapshot.Rotation;
                block.Collider.enabled = true;
            }
        }
        
        public static Bounds Bounds(this Camera camera)
        {
            var x = camera.transform.position.x;
            var y = camera.transform.position.y;
            var size = camera.orthographicSize * 2;
            
            var width = size * Screen.width / Screen.height;
            var height = size;
 
            return new Bounds(new Vector3(x, y, 0), new Vector3(width, height, 0));
        }
    }

    public static class KeyCodeValue
    {
        public static List<KeyCode> OrganizeKeys(List<KeyCode> keys)
        {
            var dictio = new Dictionary<KeyCode, int>();
            var availableKeys = ContextProvider.Context.GameSetup.AvailableKeys;
            for (var i = 0; i < availableKeys.Length; i++)
                dictio[availableKeys[i]] = i;

            return keys.OrderBy(k => dictio[k]).ToList();
        }
    }
}