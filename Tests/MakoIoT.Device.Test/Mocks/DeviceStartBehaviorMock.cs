using System.Collections;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Device.Test.Mocks
{
    internal class DeviceStartBehaviorMock : IDeviceStartBehavior
    {
        private readonly bool _deviceStartingResult;
        private readonly ArrayList _executions;

        public DeviceStartBehaviorMock(int id, bool deviceStartingResult = true, ArrayList executions = null)
        {
            Id = id;

            _deviceStartingResult = deviceStartingResult;
            _executions = executions;
        }

        public bool Executed { get; private set; }

        public int Id { get; }

        public bool DeviceStarting()
        {
            if (_executions is not null)
            {
                _executions.Add(this);
            }

            Executed = true;

            return _deviceStartingResult;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceStartBehaviorMock other && Equals(other);
        }

        protected bool Equals(DeviceStartBehaviorMock other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return $"{nameof(DeviceStartBehaviorMock)}.Id: {Id}";
        }
    }
}
