using GameActors;
using GameActors.Blocks;
using UnityEngine;

namespace Context
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Snake/Character")]
    public class CharacterSettings : ScriptableObject
    {
        public Sprite Image;
        public string CharacterName;
        public BlockType[] StartBlocks;
    }
}