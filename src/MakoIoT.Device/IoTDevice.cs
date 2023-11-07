using System;
using MakoIoT.Device.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MakoIoT.Device
{
    public class IoTDevice : IDevice
    {
        private readonly ILogger _logger;

        public IServiceProvider ServiceProvider { get; }

        public event DeviceStartingDelegate Starting;
        public event DeviceStoppedDelegate Stopped;

        public IoTDevice(IServiceProvider serviceProvider, ILogger logger)
        {
            ServiceProvider = serviceProvider;

            _logger = logger;
        }

        public void Start()
        {
            if (IsRegistered(ServiceProvider, typeof(IDeviceStartBehavior)))
            {
                var behaviors = ServiceProvider.GetServices(typeof(IDeviceStartBehavior));
                foreach (IDeviceStartBehavior behavior in behaviors)
                {
                    if (!behavior.DeviceStarting())
                    {
                        _logger.LogError($"{behavior.GetType().Name} returned false. Bailing!");

                        return;
                    }
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
