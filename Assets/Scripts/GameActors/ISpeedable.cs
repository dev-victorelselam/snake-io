using System.Collections.Generic;

namespace GameActors
{
    public interface ISpeedable
    {
        int Loads { get; }
        List<float> SpeedBlocks { get; }
    }
}