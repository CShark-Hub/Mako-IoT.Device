using System;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Device
{
    public class IoTDevice : IDevice
    {
        public IServiceProvider ServiceProvider { get; }

        public event EventHandler Starting;
        public event EventHandler Stopped;

        public IoTDevice(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Start()
        {
            if (IsRegistered(ServiceProvider, typeof(IDeviceStartBehavior)))
            {
                var behavior = (IDeviceStartBehavior)ServiceProvider.GetService(typeof(IDeviceStartBehavior));
                if (!behavior.DeviceStarting())
                {
                    return;
                }
            }
            Starting?.Invoke(this, EventArgs.Empty);
        }

        // TODO: Tests
        public static bool IsRegistered(IServiceProvider serviceProvider, Type serviceType)
        {
            var service = serviceProvider.GetService(serviceType);
            if (service == null)
            {
                return false;
            }

            return true;
        }

        public void Stop()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }
    }
}
