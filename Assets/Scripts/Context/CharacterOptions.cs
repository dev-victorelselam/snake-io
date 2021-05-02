using System.Collections.Generic;
using System.Linq;

namespace Context
{
    public class Characters
    {
        private readonly List<CharacterSettings> _charactersSettings;

        public Characters(List<CharacterSettings> characterSettings)
        {
            _charactersSettings = characterSettings;
        }

        public void AddCharacter(CharacterSettings character)
        {
            _charactersSettings.Add(character);
        }

        public void RemoveCharacter(CharacterSettings character)
        {
            if (_charactersSettings.Contains(character))
                _charactersSettings.Remove(character);
        }

        //returning a copy (ToList), avoiding anyone to mess with this array
        public List<CharacterSettings> All => _charactersSettings.ToList();
    }
}