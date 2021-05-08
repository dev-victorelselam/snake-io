using System.Linq;
using Context;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tutorial
{
    public class CharacterCard : MonoBehaviour
    {
        public UnityEvent<CharacterSettings> OnCharacterSelected = new UnityEvent<CharacterSettings>();
        
        [SerializeField] private Image _image;
        [SerializeField] private Text _name;
        [SerializeField] private Text _blocks;
        [Space(10)] 
        [SerializeField] private Button _selectButton;
        
        public void SetModel(CharacterSettings settings)
        {
            _image.sprite = settings.Image;
            _name.text = settings.CharacterName;
            _selectButton.onClick.AddListener(() => OnCharacterSelected.Invoke(settings));

            _blocks.text = settings.StartBlocks.Aggregate(string.Empty, 
                (current, block) => current + $"- {block} \n");
        }
    }
}