using System;
using System.Collections.Generic;
using System.Linq;
using Context;
using UnityEngine;
using UnityEngine.UI;

namespace _Tests
{
    public class AssertMonoBehaviour : MonoBehaviour
    {
        public GameSetup GameSetup;
        [SerializeField] private Text _testName;
        [Space(10)]
        [SerializeField] private GameObject _assertPrefab;
        [SerializeField] private ScrollRect _assertsScroll;
    
        [SerializeField] private Image _successIndicator;
        [SerializeField] private Image _failIndicator;
    
        private readonly List<AssertPrefab> _asserts = new List<AssertPrefab>();
        protected IContext Context;

        protected void Setup(string testName)
        {
            _testName.text = testName;
        
            if (_asserts.Any())
            {
                _asserts.ForEach(a => Destroy(a.gameObject));
                _asserts.Clear();
            }
        
            _successIndicator.gameObject.SetActive(false);
            _failIndicator.gameObject.SetActive(false);
        }
    
        protected AssertObject<object> Assert(Func<object> method)
        {
            var prefab = Instantiate(_assertPrefab, _assertsScroll.content).GetComponent<AssertPrefab>();
            prefab.transform.SetSiblingIndex(_asserts.Count);
            _asserts.Add(prefab);
            return prefab.Assert.And(method);
        }

        protected void Finish()
        {
            if (_asserts.All(a => a.Assert.EndResult))
                _successIndicator.gameObject.SetActive(true);
            else
                _failIndicator.gameObject.SetActive(true);
        }
    }
}