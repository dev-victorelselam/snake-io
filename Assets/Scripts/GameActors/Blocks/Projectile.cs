using DG.Tweening;
using UnityEngine;

namespace GameActors.Blocks
{
    public class Projectile : MonoBehaviour, IHittable
    {
        [SerializeField] private float _distance = 150;
        [SerializeField] private float _speed = 75;

        public void Activate(Vector3 direction)
        {
            gameObject.SetActive(true);
            transform.SetParent(null);
            
            var destiny = direction * _distance;
            var time =  _distance / _speed; //t = d/v
            transform.DOMove(destiny, time).SetEase(Ease.OutExpo)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}