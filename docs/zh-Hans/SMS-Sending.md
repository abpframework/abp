# 短信发送

ABP 框架提供一个用于发送短信的抽象, 有如下优点:

- 在不改变应用程序代码的情况下, 你可以**非常容易地切换**短信发送提供者(提供商).
- 如果你想创建可重用的应用程序模块, 则不需要假设短信是如何发送的.

## 安装

建议你使用[ABP CLI](CLI.md)来安装这个包.

### 使用Abp CLI

在项目所在目录(.csproj 文件所在目录)打开命令行工具, 输入如下命令:

```bash
abp add-package Volo.Abp.Sms
```

### 手动安装

如果你想要手动安装；

1. 添加 [Volo.Abp.Sms](https://www.nuget.org/packages/Volo.Abp.Sms) NuGet 包到你的项目中:

```
Install-Package Volo.Abp.Sms
```

2. 在你的模块的依赖列表中添加对模块`AbpSmsModule`的依赖:

```csharp
[DependsOn(
    //...其它依赖
    typeof(AbpSmsModule) //添加新模块的依赖
    )]
public class YourModule : AbpModule
{
}
```

## 发送短信

[注入](Dependency-Injection.md) `ISmsSender`,  并使用`SendAsync`方法来发送短信.

**例子:**

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
                "+012345678901",        // 目标手机号
                "This is test sms..."   // 消息内容
            );
        }
    }
}
```

示例中给定的`SendAsync`方法是一个扩展方法, 用于发送带有基本参数的短信.此外, 你也可以传入一个有如下属性的`SmsMessage`对象:

- `PhoneNumber` (`string`):目标手机号
- `Text` (`string`):短信消息内容
- `Properties` (`Dictionary<string, string>`):用于传入自定义参数的键值对

## NullSmsSender

`NullSmsSender`是`ISmsSender`的一个默认实现.它写入内容到[日志](Logging.md)中, 而不是真正地发送短信.

开发时, 你不想真正地发送短信时, 这个类是非常有用的.**然而, 若你想真实发送短信, 你需要在你的应用程序代码中实现`ISmsSender`接口.**

## 实现ISmsSender接口

通过创建一个实现`ISmsSender`接口的类, 你可以很容易创建你自己的短信发送实现, 如下所示:

```csharp
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Sms;
using Volo.Abp.DependencyInjection;

namespace AbpDemo
{
    public class MyCustomSmsSender : ISmsSender, ITransientDependency
    {
        public async Task SendAsync(SmsMessage smsMessage)
        {
            // 发送短信
        }
    }
}
```

## More

[ABP Commercial](https://commercial.abp.io/)提供Twilio的集成包, 用于使用[Twilio service](https://docs.abp.io/en/commercial/latest/modules/twilio-sms)来发送短信.
