using UnityEngine;

namespace GameActors.Blocks.Consumables
{
    public class ConsumableBlock : MonoBehaviour, IHittable
    {
        public BlockType BlockType => _block.BlockType;
        private BlockView _block;

        public void Initialize(BlockType blockType)
        {
            _block = BlockFactoring.CreateInstance(transform, blockType);
            //this blocks shouldn't detect collisions
            Destroy(_block.Collider);
        }
    }
}