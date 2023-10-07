using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Test.Mocks;
using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;
using System;

namespace MakoIoT.Device.Test
{
    [TestClass]
    public class IoTDeviceTests
    {
        [TestMethod]
        public void IsRegisted_Should_ReturnTrueIfObjectIsRegistered()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), typeof(DeviceStartBehaviorMock));
            var service = serviceCollection.BuildServiceProvider();

            var result = IoTDevice.IsRegistered(service, typeof(IDeviceStartBehavior));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsRegisted_Should_ReturnFalseIfObjectIsNotRegistered()
        {
            var serviceCollection = new ServiceCollection();
            var service = serviceCollection.BuildServiceProvider();

            var result = IoTDevice.IsRegistered(service, typeof(IDeviceStartBehavior));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Start_should_execute_registered_IDeviceStartBehavior()
        {
            // Arrange
            var deviceStartBehavior = new DeviceStartBehaviorMock();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), deviceStartBehavior);

            var sut = new IoTDevice(serviceCollection.BuildServiceProvider());

            // Act
            sut.Start();

            // Assert
            Assert.IsTrue(deviceStartBehavior.Executed);
        }

        [TestMethod]
        public void Start_should_throw_if_more_than_one_IDeviceStartBehavior_is_registered()
        {
            // Arrange
            var deviceStartBehavior1 = new DeviceStartBehaviorMock();
            var deviceStartBehavior2 = new DeviceStartBehaviorMock();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), deviceStartBehavior1);
            serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), deviceStartBehavior2);

            var sut = new IoTDevice(serviceCollection.BuildServiceProvider());

            // Act
            Assert.ThrowsException(typeof(InvalidOperationException), (() => sut.Start()));

            // Assert
            Assert.IsFalse(deviceStartBehavior1.Executed);
            Assert.IsFalse(deviceStartBehavior2.Executed);
        }
    }
}
