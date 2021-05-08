using System;
using Context;
using UnityEngine;

namespace GameActors
{
    public enum WallSide
    {
        Up,
        Down,
        Right,
        Left
    }
    
    
    [ExecuteInEditMode]
    public class Wall : MonoBehaviour, IHittable
    {
        [SerializeField] private WallSide _wallSide;

        //this was the best programmatic solution I've found for walls in this game
        private void Update()
        {
            var cameraBounds = Camera.main.Bounds();
           
            switch (_wallSide)
            {
                case WallSide.Up:
                    transform.position = new Vector3(0, cameraBounds.max.y, 0);
                    break;
                case WallSide.Down:
                    transform.position = new Vector3(0, cameraBounds.min.y, 0);
                    break;
                case WallSide.Right:
                    transform.position = new Vector3(cameraBounds.max.x, 0, 0);
                    break;
                case WallSide.Left:
                    transform.position = new Vector3(cameraBounds.min.x, 0, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}