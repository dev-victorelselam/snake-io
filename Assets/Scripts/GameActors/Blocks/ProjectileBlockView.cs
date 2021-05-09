using Context;
using UnityEngine;

namespace GameActors.Blocks
{
    public class ProjectileBlockView : BlockView
    {
        [SerializeField] private Projectile _projectile;
        
        public void Shoot()
        {
            _projectile.Activate(Extensions.RandomDirection());
            ContextProvider.Context.GameController.UI.ActivatePowerUpView(BlockType.Projectile);
        }
    }
}