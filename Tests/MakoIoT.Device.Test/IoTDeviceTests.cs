using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Test.Mocks;
using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;
using System;
using System.Collections;

namespace MakoIoT.Device.Test
{
    [TestClass]
    public class IoTDeviceTests
    {
        [TestMethod]
        public void IsRegistered_should_return_true_if_object_is_registered()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), typeof(DeviceStartBehaviorMock));
            var service = serviceCollection.BuildServiceProvider();

            var result = IoTDevice.IsRegistered(service, typeof(IDeviceStartBehavior));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsRegistered_should_return_false_if_object_is_not_registered()
        {
            var serviceCollection = new ServiceCollection();
            var service = serviceCollection.BuildServiceProvider();

            var result = IoTDevice.IsRegistered(service, typeof(IDeviceStartBehavior));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Start_should_execute_multiple_IDeviceStartBehavior_in_the_order_they_were_registered()
        {
            // Arrange
            var executions = new ArrayList();
            var expected = new ArrayList();
            var random = new Random();

            for (var i = 0; i < 10; i++)
            {
                expected.Add(new DeviceStartBehaviorMock(random.Next(), true, executions));
            }

            var serviceCollection = new ServiceCollection();
            foreach (var deviceStartBehavior in expected)
            {
                serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), deviceStartBehavior);
            }

            var sut = new IoTDevice(serviceCollection.BuildServiceProvider());

            // Act
            sut.Start();

            // Assert
            Assert.AreEqual(expected.Count, executions.Count);

            for (var i = 0; i < executions.Count; i++)
            {
                var expect = (DeviceStartBehaviorMock) expected[i];
                var actual = (DeviceStartBehaviorMock) executions[i];

                Assert.IsTrue(actual.Executed);
                Assert.AreEqual(expect, actual);
            }
        }

        [TestMethod]
        public void Start_should_execute_single_IDeviceStartBehavior()
        {
            // Arrange
            var deviceStartBehavior = new DeviceStartBehaviorMock(1);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), deviceStartBehavior);

            var sut = new IoTDevice(serviceCollection.BuildServiceProvider());

            // Act
            sut.Start();

            // Assert
            Assert.IsTrue(deviceStartBehavior.Executed);
        }

        [TestMethod]
        public void Start_should_throw_if_IDeviceStartBehavior_DeviceStarting_returns_false()
        {
            // Arrange
            var executions = new ArrayList();
            var deviceStartBehavior1 = new DeviceStartBehaviorMock(1, true, executions);
            var deviceStartBehavior2 = new DeviceStartBehaviorMock(2, false, executions);
            var deviceStartBehavior3 = new DeviceStartBehaviorMock(2, true, executions);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), deviceStartBehavior1);
            serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), deviceStartBehavior2);
            serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), deviceStartBehavior3);

            var sut = new IoTDevice(serviceCollection.BuildServiceProvider());

            // Act
            Assert.ThrowsException(typeof(InvalidOperationException), (() => sut.Start()));

            // Assert
            Assert.IsTrue(deviceStartBehavior1.Executed);
            Assert.IsTrue(deviceStartBehavior2.Executed);
            Assert.IsFalse(deviceStartBehavior3.Executed);
            Assert.IsTrue(executions.Count == 2);
            Assert.AreEqual(deviceStartBehavior1, executions[0]);
            Assert.AreEqual(deviceStartBehavior2, executions[1]);
        }

        /// <summary>
        /// This is a helper test to ensure that the <see cref="ServiceCollection"/> returns services in the order they were registered
        /// </summary>
        [TestMethod]
        public void ServiceProvider_order_should_be_deterministic()
        {
            // Arrange
            var random = new Random();
            var expected = new ArrayList();
            
            for (var i = 0; i < 10; i++)
            {
                expected.Add(new DeviceStartBehaviorMock(random.Next()));
            }

            var serviceCollection = new ServiceCollection();
            foreach (var deviceStartBehavior in expected)
            {
                serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), deviceStartBehavior);
            }

            var sut = serviceCollection.BuildServiceProvider();

            // Act
            var results = sut.GetServices(typeof(IDeviceStartBehavior));

            // Assert
            Assert.AreEqual(expected.Count, results.Length);

            for (var i = 0; i < results.Length; i++)
            {
                var expect = expected[i];
                var actual = results[i];

                Assert.AreEqual(expect, actual);
            }
        }
    }
}
