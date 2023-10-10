using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace MakoIoT.Device.Test.Mocks
{
    internal class MockLogger: ILogger
    {
        public void Log(LogLevel logLevel, EventId eventId, string state, Exception exception, MethodInfo format) => Logged = true;

        public bool Logged { get; private set; }

        public bool IsEnabled(LogLevel logLevel) => true;
    }
}
