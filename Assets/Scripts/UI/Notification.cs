using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private Text _text;
        
        public async void Activate(string text)
        {
            transform.localPosition = new Vector3(1000, 0, 0);
            
            _text.text = text;
            await transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
            await Task.Delay(2000);
            await transform.DOLocalMoveX(1000, 0.5f).AsyncWaitForCompletion();
            
            Destroy(gameObject);
        }
    }
}