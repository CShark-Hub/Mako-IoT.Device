using MakoIoT.Device.Services.Interface;
using System;

namespace MakoIoT.Device.Test.Mocks
{
    internal class DeviceStartBehaviorMock : IDeviceStartBehavior
    {
        public bool Executed { get; private set; }

        public bool DeviceStarting()
        {
            Executed = true;

            return false;
        }
    }
}
