# MailKit 集成

[MailKit](http://www.mimekit.net/) 是一个用于 .net 的跨平台、流行的开源邮件客户端库.ABP 框架提供了一个集成包来使用 MailKit 作为[邮件发送系统](Emailing.md)的收发组件.

## 安装

建议使用 [ABP CLI](CLI.md) 安装包.在项目文件夹（.csproj 文件）中打开命令行窗口并键入以下命令：

````bash
abp add-package Volo.Abp.MailKit
````

如果执行失败,你首先需要安装 ABP CLI.有关其他安装选项,请参阅 [包描述页面](https://abp.io/package-detail/Volo.Abp.MailKit).

## 发送电子邮件

### IEmailSender

[注入](Dependency-Injection.md) 标准的 `IEmailSender` 到任何服务并使用 `SendAsync` 方法发送电子邮件.详见 [邮件发送文档](Emailing.md).

> `IEmailSender` 是建议的发送邮件的方式,即使你使用MailKit,因为它使你的代码独立.

### IMailKitSmtpEmailSender

`BuildClientAsync()` 方法扩展了`IEmailSender`.此方法可用于获取可用于执行 MailKit 特定操作的`MailKit.Net.Smtp.SmtpClient`对象.

## 配置

MailKit 集成包使用电子邮件发送系统相同配置选项.请参阅[电子邮件发送文档](Emailing.md) 进行配置.

除了标准设置之外,这个包还定义了 `AbpMailKitOptions` 作为一个简单的 [选项](Options.md) 类.此类仅定义一个选项：

* **SecureSocketOption**:用于设置“SecureSocketOptions” . Default：`null`（使用默认值）

**示例: 使用 *SecureSocketOptions.SslOnConnect***

````csharp
Configure<AbpMailKitOptions>(options =>
{
    options.SecureSocketOption = SecureSocketOptions.SslOnConnect;
});
````

请参阅 [MailKit 文档](http://www.mimekit.net/) 了解更多信息.

## 也可以看看

* [电子邮件发送系统](Emailing.md)
