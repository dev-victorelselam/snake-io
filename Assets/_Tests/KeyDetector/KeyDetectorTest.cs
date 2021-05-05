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
            Context = new TestContext(_navigationController, _gameController, _popupUtility, GameSetup);
            Setup("[HintTest]");

            var keyDetector = gameObject.AddComponent<Tutorial.KeyDetector.KeyDetector>();
            keyDetector.OnComplete.AddListener(KeysDetected);
            keyDetector.StartListen();

            while (_keysDetected == null)
                await Task.Delay(50);
        
            Assert(() => _keysDetected[0])
                .ShouldBe(KeyCode.A).Because($"Test Key Provider specified that {KeyCode.A} is the first key").Run();
        
            Assert(() => _keysDetected[1])
                .ShouldBe(KeyCode.D).Because($"Test Key Provider specified that {KeyCode.D} is the first key").Run();
        
            Finish();
        }

        private void KeysDetected(List<KeyCode> keys)
        {
            _keysDetected = keys;
        }
    }
}
