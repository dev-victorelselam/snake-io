using GameActors;
using UnityEngine;

namespace Context
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Snake/Character")]
    public class CharacterSettings : ScriptableObject
    {
        public string CharacterName;
        public Color Color;
        public BlockType[] StartBlocks;
    }
}