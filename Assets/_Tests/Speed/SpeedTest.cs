using System.Collections.Generic;
using Context;
using GameActors;

namespace _Tests.Speed
{
    internal class SpeedableMock : ISpeedable
    {
        public SpeedableMock(int loads, List<float> speeds)
        {
            Loads = loads;
            SpeedBlocks = speeds;
        }
        
        public int Loads { get; }
        public List<float> SpeedBlocks { get; }
    }

    public class SpeedTest : AssertMonoBehaviour
    {
        public void Awake()
        {
            var context = new TestContext(null, null, null, GameSetup);
            //prepare
            Setup("[SpeedTest]");

            //execute
            var speedMock1 = new SpeedableMock(3, new List<float> {1, 2}).Speed();
            var speedMock2 = new SpeedableMock(4, new List<float> {1, 2}).Speed();
        
            //assert
            Assert(() => speedMock1 > speedMock2)
                .ShouldBe(true).Because($"Speed Mock1 has 1 less load").Run();
            
            var speedMock3 = new SpeedableMock(2, new List<float> {2, 2, 2}).Speed();
            var speedMock4 = new SpeedableMock(2, new List<float> {2, 2}).Speed();
        
            Assert(() => speedMock3 > speedMock4)
                .ShouldBe(true).Because($"Speed Mock3 has 1 more speed block").Run();
            
            var speedMock5 = new SpeedableMock(3, new List<float> {1, 1, 1}).Speed();
            var speedMock6 = new SpeedableMock(2, new List<float> {1, 1}).Speed();
        
            Assert(() => speedMock5 > speedMock6)
                .ShouldBe(true).Because($"Speed Mock5 has 1 more speed block, and it should compensate 1 more load").Run();
        
            Finish();
        }
    }
}
