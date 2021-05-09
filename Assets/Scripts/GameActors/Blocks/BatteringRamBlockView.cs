using System.Linq;

namespace GameActors.Blocks
{
    public class BatteringRamBlockView : BlockView
    {
        public bool ActivateBatteringRam(SnakeController mySnake, BlockView contactBlock)
        {
            //if we hit on ourself, skip battering ram
            if (mySnake.Blocks.Any(b => b == contactBlock))
                return false;
            
            mySnake.DisableCollisions(2);
            
            contactBlock.DisableBlock();
            DisableBlock();

            return true;
        }
    }
}