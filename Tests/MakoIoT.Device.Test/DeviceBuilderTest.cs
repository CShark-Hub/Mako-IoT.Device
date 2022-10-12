using nanoFramework.TestFramework;

namespace MakoIoT.Device.Test
{
    [TestClass]
    public class DeviceBuilderTest
    {
        [TestMethod]
        public void Build_should_register_Starting_event_on_Device()
        {
            bool startingCalled = false, stoppedCalled = false;


            var builder = DeviceBuilder.Create();

            builder.DeviceStarting += (sender, args) => { startingCalled = true; };
            builder.DeviceStopped += (sender, args) => { stoppedCalled = true; };

            var device = builder.Build();

            device.Start();

            Assert.True(startingCalled);
            Assert.False(stoppedCalled);
            
        }

        [TestMethod]
        public void Build_should_register_Stopped_event_on_Device()
        {
            bool startingCalled = false, stoppedCalled = false;


            var builder = DeviceBuilder.Create();

            builder.DeviceStarting += (sender, args) => { startingCalled = true; };
            builder.DeviceStopped += (sender, args) => { stoppedCalled = true; };

            var device = builder.Build();

            device.Stop();

            Assert.False(startingCalled);
            Assert.True(stoppedCalled);

        }

    }
}
