using System.Collections.Generic;
using System.Threading.Tasks;
using Context;
using UnityEngine;

namespace _Tests.KeyDetector
{
    public class KeyDetectorTest : AssertMonoBehaviour
    {
        [SerializeField] private NavigationController _navigationController;
        [SerializeField] private GameController _gameController;
        [SerializeField] private PopupUtility _popupUtility;

        private List<KeyCode> _keysDetected;
    
        public async void Awake()
        {
            Setup("[HintTest]", _navigationController, _gameController, _popupUtility, GameSetup);

            var keyDetector = gameObject.AddComponent<Tutorial.KeyDetector>();
            keyDetector.OnComplete.AddListener(KeyDetected);
            keyDetector.Enable();

            while (_keysDetected == null)
                await Task.Delay(50);
        
            Assert(() => _keysDetected[0])
                .ShouldBe(KeyCode.A).Because($"Key Provider specified that {KeyCode.A} is the first key").Run();
        
            Assert(() => _keysDetected[1])
                .ShouldBe(KeyCode.D).Because($"Key Provider specified that {KeyCode.D} is the first key").Run();
        
            Finish();
        }

        private void KeyDetected(List<KeyCode> keys)
        {
            _keysDetected = keys;
        }
    }
}
