using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class PowerUpAnimation : MonoBehaviour
    {
        public async void OnEnable()
        {
            transform.localPosition = new Vector3(-1600, 0, 0);
            await transform.DOLocalMoveX(0, 1f).SetEase(Ease.OutCirc).AsyncWaitForCompletion();
        }
    }
}