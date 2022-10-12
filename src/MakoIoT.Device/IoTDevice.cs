using System;
using MakoIoT.Device.Services.DependencyInjection;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Device
{
    public class IoTDevice : IDevice
    {
        public event EventHandler Starting;
        public event EventHandler Stopped;

        public void Start()
        {
            if (DI.IsRegistered(typeof(IDeviceStartBehavior)))
            {
                var behavior = (IDeviceStartBehavior)DI.Resolve(typeof(IDeviceStartBehavior));
                if (!behavior.DeviceStarting())
                    return;
                behavior = null;
            }
            Starting?.Invoke(this, EventArgs.Empty);
        }

        public void Stop()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }
    }
}
