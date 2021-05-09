namespace GameActors.Blocks
{
    public enum BlockType
    {
        Common,
        TimeTravel,
        SpeedBoost,
        BatteringRam,
        Variant, //this block change its type each one second, the type active when you hit, is the block you earn
        Projectile //this block fire a projectile in forward line (random direction)
                   //when picked that kill any snake that it hits
    }
}