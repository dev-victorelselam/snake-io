using UnityEngine;

namespace GameActors.Blocks.Consumables
{
    public class ConsumableBlock : MonoBehaviour
    {
        public BlockType BlockType => _block.BlockType;
        private BlockView _block;

        public void Initialize(BlockType blockType)
        {
            _block = BlockFactoring.CreateInstance(transform, BlockType);
            _block.OnContact.AddListener(CheckContact);
        }

        private void CheckContact(IHittable element)
        {
            
        }
    }
}