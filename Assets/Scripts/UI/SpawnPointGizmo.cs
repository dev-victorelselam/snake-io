using UnityEngine;

public class SpawnPointGizmo : MonoBehaviour
{
    [SerializeField] private float _radius = 3;
    [SerializeField] private float _lineDistance = 6;
    [SerializeField] private Color _color = Color.red;
    
    public void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
        //useful to know the direction of the spawn
        Gizmos.DrawLine(transform.position, transform.position + transform.up * _lineDistance);
    }
}
