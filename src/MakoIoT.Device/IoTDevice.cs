using System;
using MakoIoT.Device.Services.Interface;
using nanoFramework.DependencyInjection;

namespace MakoIoT.Device
{
    public class IoTDevice : IDevice
    {
        public IServiceProvider ServiceProvider { get; }

        public event DeviceStartingDelegate Starting;
        public event DeviceStoppedDelegate Stopped;

        public IoTDevice(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Start()
        {
            if (IsRegistered(ServiceProvider, typeof(IDeviceStartBehavior)))
            {
                var behaviors = ServiceProvider.GetServices(typeof(IDeviceStartBehavior));
                if (behaviors.Length > 1)
                {
                    throw new InvalidOperationException($"More than one {nameof(IDeviceStartBehavior)} is registered");
                }

                var behavior = (IDeviceStartBehavior) behaviors[0];
                if (!behavior.DeviceStarting())
                {
                    return;
                }
            }

            Starting?.Invoke(this);
        }

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
            Stopped?.Invoke(this);
        }
    }
}
