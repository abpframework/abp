# Upgrading existing solution with Pro modules

ABP Studio provides a way for users, who already started the development before purchasing a license, to auto-upgrade their solutions with Pro modules.

![upgrade-to-pro](D:\github\abp\docs\en\studio\images\upgrade-to-pro.png)

## Changes in the solution

### Module changes

This command will remove the following modules from your solution:

- Volo.Abp.Account
- Volo.Abp.Identity
- Volo.Abp.TenantManagement
- Volo.Abp.LeptonXLiteTheme

And install the following modules to your solution:

- Volo.Abp.Account.Pro
- Volo.Abp.AuditLogging.Pro
- Volo.Abp.Identity.Pro
- Volo.Abp.OpenIddict.Pro
- Volo.Saas
- Volo.Abp.LanguageManagement
- Volo.Abp.TextTemplateManagement
- Volo.FileManagement
- Volo.Chat
- Volo.Abp.Gdpr
- Volo.Abp.LeptonXTheme

### Other changes

The command will also do the following changes in your solution:

- It will add the commercial NuGet source to `NuGet.config` file.
- It will create or update `appsettings.secrets.json` files to place the license key needed for Pro module usage.
- It will update the database. (And it will create a new migration if the solution uses EntityFramework Core)
- It will run `install-libs` & `bundle` commands at the end.

## Things to pay attention before using

- The command covers the most common scenarios, but there is still a possiblity that it can mess up something in your solution. Therefore, we strongly recommend a source code control system (like GitHub) to track what is changed in your solution and revert if needed.
- The command will not remove your custom codes that may be related with the removed modules listed above. So there may be build errors if you referenced one those modules. You can clear them manually.
