using Context;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class CharacterCard : MonoBehaviour
    {
        public UnityEvent<CharacterSettings> OnCharacterSelected = new UnityEvent<CharacterSettings>();
        
        [SerializeField] private Image _image;
        [SerializeField] private Text _name;
        [SerializeField] private Image _color;
        [Space(10)] 
        [SerializeField] private Button _selectButton;
        
        public void SetModel(CharacterSettings settings)
        {
            _name.text = settings.CharacterName;
            _color.color = settings.Color;
            
            _selectButton.onClick.AddListener(() => OnCharacterSelected.Invoke(settings));
        }
    }
}