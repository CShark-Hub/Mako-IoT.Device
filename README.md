#  Mako-IoT.Device
This is the composition framework of MAKO-IoT. These components provide backbone for your solution.

## Projects structure
Create two [nanoFramework projects](https://docs.nanoframework.net/content/getting-started-guides/index.html):

**_MyProject.Device_** (nanoFramework Class Library)

Implement your logic here. You will be able to unit test this project (without hardware!). Place everything your software needs to do here, abstracting hardware-specific operations with interfaces. For example, if you want to blink a LED - create _IBlinker_ interface here with _void Blink(bool isOn);_ method.

**_MyProject.Device.App_** (nanoFramework Application)

This is the entry point to your software. Use _DeviceBuilder_ to link all components together (MAKO IoT and your code) in the _Main()_ method of _Program_ class. Also, implement your hardware-specific logic here. For example, concrete blinker class for the interface above: _LedBlinker : IBlinker_. Link the interface with the class in _ConfigureDI_ section of _DeviceBuilder_.

```c#
public class Program
{
    public static void Main()
    {
        DeviceBuilder.Create()
            .ConfigureDI(services =>
            {
                //dependency injection registrations go here
            })
            //add other MAKO-IoT components & configuration here
            
            .Build()
            .Start();

        Thread.Sleep(Timeout.InfiniteTimeSpan);
    }
}
```

See [Blink sample](https://github.com/CShark-Hub/Mako-IoT.Device.Samples/tree/main/Blink)

## Device events
You can attach handlers to DeviceStarting and DeviceStopped events:
```c#
var builder = DeviceBuilder.Create();

builder.DeviceStarting += device =>
{
    //do something on start
};

builder.DeviceStopped += device =>
{
    //do something on stop
};

builder.Build().Start();
```

## Device start behavior
With _IDeviceStartBehavior_ you can intercept device startup. For example, based on certain condition either continue normal operation go into configuration mode.
```c#
public class MyDeviceStartBehavior : IDeviceStartBehavior
{
    public bool DeviceStarting()
    {
      if (normalOperationMode)
        return true; //continue normal device startup
      
      return false; //don't continue device startup
    }
}

public class Program
{
    public static void Main()
    {
        DeviceBuilder.Create()
            .ConfigureDI(services =>
            {
                services.AddSingleton(typeof(IDeviceStartBehavior), typeof(MyDeviceStartBehavior));
            }).Build().Start();

        Thread.Sleep(Timeout.InfiniteTimeSpan);
    }
}
```
