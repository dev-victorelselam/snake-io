using Context;
using UnityEngine;

namespace GameActors.Blocks
{
    public class ProjectileBlockView : BlockView
    {
        [SerializeField] private Projectile _projectile;
        
        private void Shoot()
        {
            _projectile.Activate(Extensions.RandomDirection());
            ContextProvider.Context.GameController.UI.ActivatePowerUpView(BlockType.Projectile);
        }

        public override void OnPick()
        {
            Shoot();
        }
    }
}