using System;
using MakoIoT.Device.Services.DependencyInjection;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Device
{
    public class DeviceBuilder : IDeviceBuilder
    {
        protected Action ConfigureDiAction;
        public ConfigureDefaultsDelegate ConfigureDefaultsAction { get; set; }

        public event EventHandler DeviceStarting;
        public event EventHandler DeviceStopped;

        public static IDeviceBuilder Create()
        {
            DI.Clear();
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
            ConfigureDefaultsAction?.Invoke((IConfigurationService)DI.Resolve(typeof(IConfigurationService)));

            var device = (IoTDevice)DI.BuildUp(typeof(IoTDevice));

            if (DeviceStarting != null)
            {
                foreach (EventHandler handler in DeviceStarting.GetInvocationList())
                {
                    DeviceStarting -= handler;
                    device.Starting += handler;
                }
            }

            if (DeviceStopped != null)
            {
                foreach (EventHandler handler in DeviceStopped.GetInvocationList())
                {
                    DeviceStopped -= handler;
                    device.Stopped += handler;
                }
            }

            return device;

        }
    }
}
