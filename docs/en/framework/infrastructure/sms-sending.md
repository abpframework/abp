```json
//[doc-seo]
{
    "Description": "Learn how to easily integrate SMS sending in your ABP applications with flexible options for changing SMS providers and creating reusable modules."
}
```

# SMS Sending

The ABP provides an abstraction to sending SMS. Having such an abstraction has some benefits;

- You can then **easily change** your SMS sender without changing your application code.
- If you want to create **reusable application modules**, you don't need to make assumption about how the SMS are sent.

## Installation

It is suggested to use the [ABP CLI](../../cli) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.Sms
```

> If you haven't done it yet, you first need to install the [ABP CLI](../../cli). For other installation options, see [the package description page](https://abp.io/package-detail/Volo.Abp.Sms).

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.Sms](https://www.nuget.org/packages/Volo.Abp.Sms) NuGet package to your project:

```
dotnet add package Volo.Abp.Sms
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

[Inject](../fundamentals/dependency-injection.md) the `ISmsSender` into any service and use the `SendAsync` method to send a SMS.

**Example:**

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
                "This is test sms..."   // message text
            );
        }
    }
}
```

The given `SendAsync` method in the example is an extension method to send an SMS with primitive parameters. In addition, you can pass an `SmsMessage` object which has the following properties:

- `PhoneNumber` (`string`): Target phone number
- `Text` (`string`): Message text
- `Properties` (`IDictionary<string, object>`): Key-value pairs to pass custom arguments

## NullSmsSender

`NullSmsSender` is the default implementation of `ISmsSender`. It writes SMS content to the [standard logger](../fundamentals/logging.md), rather than actually sending the SMS.

This class can be useful especially in development time where you generally don't want to send real SMS. To send real SMS, install one of the pre-built providers below or implement `ISmsSender` in your application code.

## Implementing the ISmsSender

You can easily create your SMS sending implementation by creating a class that implements the `ISmsSender` interface, as shown below:

```csharp
using System.Threading.Tasks;
using Volo.Abp.Sms;
using Volo.Abp.DependencyInjection;

namespace AbpDemo
{
    public class MyCustomSmsSender : ISmsSender, ITransientDependency
    {
        public Task SendAsync(SmsMessage smsMessage)
        {
            // Send sms
            return Task.CompletedTask;
        }
    }
}
```

## Pre-Built Providers

Adding a provider module registers its sender as the `ISmsSender` implementation in place of the default `NullSmsSender`.

### Aliyun

Install the Aliyun provider package:

```bash
abp add-package Volo.Abp.Sms.Aliyun
```

For manual installation, add the `Volo.Abp.Sms.Aliyun` package and declare a dependency on `AbpSmsAliyunModule`.

Configure the provider in the `AbpAliyunSms` section:

```json
{
  "AbpAliyunSms": {
    "AccessKeyId": "your-access-key-id",
    "AccessKeySecret": "your-access-key-secret",
    "EndPoint": "your-endpoint"
  }
}
```

Aliyun sends template-based messages. Set `SmsMessage.Text` to the template parameter JSON and use the `SignName` and `TemplateCode` properties:

```csharp
var message = new SmsMessage(
    "+012345678901",
    "{\"code\":\"123456\"}"
);

message.Properties["SignName"] = "MySign";
message.Properties["TemplateCode"] = "SMS_123456789";

await _smsSender.SendAsync(message);
```

### Tencent Cloud

Install the Tencent Cloud provider package:

```bash
abp add-package Volo.Abp.Sms.TencentCloud
```

For manual installation, add the `Volo.Abp.Sms.TencentCloud` package and declare a dependency on `AbpSmsTencentCloudModule`.

Configure the provider in the `AbpTencentCloudSms` section:

```json
{
  "AbpTencentCloudSms": {
    "SmsSdkAppId": "your-sdk-app-id",
    "SecretId": "your-secret-id",
    "SecretKey": "your-secret-key",
    "Endpoint": "sms.tencentcloudapi.com",
    "Region": "ap-guangzhou"
  }
}
```

`Endpoint` defaults to `sms.tencentcloudapi.com` and `Region` defaults to `ap-guangzhou`.

Set the sign and template identifiers through `TencentCloudSmsProperties`. The provider splits `SmsMessage.Text` by commas and sends the resulting values as template parameters:

```csharp
var message = new SmsMessage(
    "+012345678901",
    "123456,5"
);

message.Properties[TencentCloudSmsProperties.SignName] = "MySign";
message.Properties[TencentCloudSmsProperties.TemplateId] = "123456";

await _smsSender.SendAsync(message);
```

## More

[ABP](https://abp.io/) provides Twilio integration package to send SMS over [Twilio service](https://abp.io/docs/latest/modules/twilio-sms).
