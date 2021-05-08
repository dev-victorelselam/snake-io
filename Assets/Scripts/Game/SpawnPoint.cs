using Context;
using UnityEngine;

namespace Game
{
    public class SpawnPoint : MonoBehaviour
    {
        public enum SpawnDirection
        {
            Up,
            Right,
            Down,
            Left
        }
    
        [SerializeField] private float _radius = 3;
        [SerializeField] private float _lineDistance = 6;
        [SerializeField] private Color _color = Color.red;
        [SerializeField] private SpawnDirection _direction = SpawnDirection.Up;

        public SpawnDirection Direction => _direction;
    
        public void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
            //useful to know the direction of the spawn
            Gizmos.DrawLine(transform.position, transform.position + transform.up * _lineDistance);
        }

        public void OnValidate()
        {
            //adjust direction by enum in inspector
            transform.eulerAngles = new Vector3(0, 0, _direction.GetAngle());
        }
    }
}
