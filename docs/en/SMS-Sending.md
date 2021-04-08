# SMS Sending

The ABP Framework provides an abstraction to sending SMS. Having such an abstraction has some benefits;

- You can **easily integrate** to your favorite SMS sender with a few lines of configuration.
- You can then **easily change** your SMS sender without changing your application code.
- If you want to create **reusable application modules**, you don't need to make assumption about how the SMS are sent.

## Installation

It is suggested to use the [ABP CLI](CLI.md) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.Sms
```

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.Sms](https://www.nuget.org/packages/Volo.Abp.Sms) NuGet package to your project:

```
Install-Package Volo.Abp.Sms
```

2. Add the `AbpSmsModule` to the dependency list of your module:

```csharp
[DependsOn(
    //...other dependencies
    typeof(AbpSmsModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
```

## Sending SMS

[Inject](Dependency-Injection.md) the `ISmsSender` into any service and use the `SendAsync` method to send a SMS.

**Example**

```csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Sms;

namespace MyProject
{
    public class MyService : ITransientDependency
    {
        private readonly ISmsSender _smsSender;

        public MyService(ISmsSender smsSender)
        {
            _smsSender = smsSender;
        }

        public async Task DoItAsync()
        {
            await _smsSender.SendAsync(
                "+012345678901",        // target phone number
                "This is test sms..."   // SMS text
            );
        }
    }
}
```

The given `SendAsync` method in the example is an extension method to send sms with primitive paremeters, it basicly creates `SmsMessage` object and pass it. You can also use the default `SendAsync` method which requires `SmsMessage` object.

> `ISmsSender` is the suggested way to send SMS, since it makes your code provider independent.

## SmsMessage

In addition to use primitive parameters, you can pass a `SmsMessage` object to the `SendAsync` method.

- `PhoneNumber` (string): Target phone number to send sms.
- `Text` (string): SMS Text
- `Properties` (Dictionary<string, string>): string key-value pair to handle different usages by senders.

## NullSmsSender

`NullSmsSender` is a built-in class that implements the `ISmsSender`, but writes sms contents to the [standard log system](Logging.md), rathen than actually sending the SMSs.

This class can be useful especially in development time where you generally don't want to send real sms.
`NullSmsSender` will try to register itself automatically if there is no other registrated sms sender.

## See also

- [Twilio SMS Sender](https://docs.abp.io/en/commercial/latest/modules/twilio-sms) with [ABP Commercial](https://commercial.abp.io/).
