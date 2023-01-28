using System;
using MakoIoT.Device.Services.Interface;
using nanoFramework.DependencyInjection;

namespace MakoIoT.Device
{
    public class DeviceBuilder : IDeviceBuilder
    {
        protected Action ConfigureDiAction;
        public ConfigureDefaultsDelegate ConfigureDefaultsAction { get; set; }

        public IServiceCollection Services { get; }

        public event DeviceStartingDelegate DeviceStarting;
        public event DeviceStoppedDelegate DeviceStopped;

        internal DeviceBuilder()
        {
            Services = new ServiceCollection();
        }

        public static IDeviceBuilder Create()
        {
            return new DeviceBuilder();
        }

        public virtual IDeviceBuilder ConfigureDI(Action configureDiAction)
        {
            ConfigureDiAction = configureDiAction;
            return this;
        }

        public virtual IDevice Build()
        {
            ConfigureDiAction?.Invoke();

            var serviceProvider = (IServiceProvider)Services.BuildServiceProvider();

            if (ConfigureDefaultsAction != null)
            {
                var configurationService = (IConfigurationService)serviceProvider.GetService(typeof(IConfigurationService));
                ConfigureDefaultsAction.Invoke(configurationService);
            }

            var device = (IDevice)ActivatorUtilities.CreateInstance(serviceProvider, typeof(IoTDevice));

            if (DeviceStarting != null)
            {
                foreach (DeviceStartingDelegate handler in DeviceStarting.GetInvocationList())
                {
                    DeviceStarting -= handler;
                    device.Starting += handler;
                }
            }

            if (DeviceStopped != null)
            {
                foreach (DeviceStoppedDelegate handler in DeviceStopped.GetInvocationList())
                {
                    DeviceStopped -= handler;
                    device.Stopped += handler;
                }
            }

            return device;
        }
    }
}
