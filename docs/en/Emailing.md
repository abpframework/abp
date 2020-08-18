# Emailing

ABP Framework provides various services, settings and integrations for email sending;

* Provides `IEmailSender` service that is used to send emails.
* Defines [settings](Settings.md) to configure email sending.
* Integrates to the [background job system](Background-Jobs.md) to send emails via background jobs.
* Provides [MailKit](https://github.com/jstedfast/MailKit) integration package.

## Installation

> This package is already installed if you are using the [application startup template](Startup-Templates/Application.md).

It is suggested to use the [ABP CLI](CLI.md) to install this package. Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.Emailing
````

If you haven't done it yet, you first need to install the ABP CLI. For other installation options, see [the package description page](https://abp.io/package-detail/Volo.Abp.Emailing).

## Sending Emails

### IEmailSender

[Inject](Dependency-Injection.md) the `IEmailSender` into any service and use the `SendAsync` method to send emails.

**Example**

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace MyProject
{
    public class MyService : ITransientDependency
    {
        private readonly IEmailSender _emailSender;

        public MyService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task DoItAsync()
        {
            await _emailSender.SendAsync(
                "target@domain.com",     // target email address
                "Email subject",         // subject
                "This is email body..."  // email body
            );
        }
    }
}
````

`SendAsync` method has overloads to supply more parameters like;

* **from**: You can set this as the first argument to set a sender email address. If not provided, the default sender address is used (see the email settings below).
* **isBodyHtml**: Indicates whether the email body may contain HTML tags. **Default: true**.

#### MailMessage

In addition to primitive parameters, you can pass a **standard `MailMessage` object** ([see](https://docs.microsoft.com/en-us/dotnet/api/system.net.mail.mailmessage)) to the `SendAsync` method to set more options, like adding attachments.

### ISmtpEmailSender

Sending emails is implemented by the standard `SmtpClient` class ([see](https://docs.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient)) by default. The implementation class is the `SmtpEmailSender`. This class also expose the `ISmtpEmailSender` service (in addition to the `IEmailSender`).

Most of the time you want to directly use the `ISmtpEmailSender` to make your code provider independent. However, if you want to create an `SmtpClient` easily with the same email settings, you can inject the `ISmtpEmailSender` and use its `BuildClientAsync` method to obtain a `SmtpClient` object and send the email yourself.

## Queueing Emails / Background Jobs

`IEmailSender` has a `QueueAsync` method that can be used to add emails to the background job queue to send them in a background thread. In this way, you don't take time of the user by waiting to send the email. `QueueAsync` method gets the same arguments with the `SendAsync` method.

Queueing emails tolerates errors since the background job system has re-try mechanism to overcome temporary network/server problems. 

See the [background jobs document](Background-Jobs.md) for more about the background job system.

## Email Settings

Email sending uses the [setting system](Settings.md) to define settings and get the values of these settings on the runtime. `Volo.Abp.Emailing.EmailSettingNames` defines constants for the setting names, just listed below:

* **Abp.Mailing.DefaultFromAddress**: Used as the sender's email address when you don't specify a sender when sending emails (just like in the example above).
* **Abp.Mailing.DefaultFromDisplayName**: Used as the sender's display name when you don't specify a sender when sending emails (just like in the example above).
* **Abp.Mailing.Smtp.Host**: The IP/Domain of the SMTP server (default: 127.0.0.1).
* **Abp.Mailing.Smtp.Port**: The Port of the SMTP server (default: 25).
* **Abp.Mailing.Smtp.UserName**: Username, if the SMTP server requires authentication.
* **Abp.Mailing.Smtp.Password**: Password, if the SMTP server requires authentication.
* **Abp.Mailing.Smtp.Domain**: Domain for the username, if the SMTP server requires authentication.
* **Abp.Mailing.Smtp.EnableSsl**: A value that indicates if the SMTP server uses SSL or not ("true" or "false". Default: "false").
* **Abp.Mailing.Smtp.UseDefaultCredentials**: If true, uses default credentials instead of the provided username and password ("true" or "false". Default: "true").

The easiest way to define these settings it to add them to the `appsettings.json` file. The [application startup template](Startup-Templates/Application.md) already has these settings in the `appsettings.json`:

````json
"Settings": {
  "Abp.Mailing.Smtp.Host": "127.0.0.1",
  "Abp.Mailing.Smtp.Port": "25",
  "Abp.Mailing.Smtp.UserName": "",
  "Abp.Mailing.Smtp.Password": "",
  "Abp.Mailing.Smtp.Domain": "",
  "Abp.Mailing.Smtp.EnableSsl": "false",
  "Abp.Mailing.Smtp.UseDefaultCredentials": "true",
  "Abp.Mailing.DefaultFromAddress": "noreply@abp.io",
  "Abp.Mailing.DefaultFromDisplayName": "ABP application"
}
````

See the [setting system document](Settings.md) to understand the setting system better.

## MailKit Integration

TODO

## NullEmailSender

TODO