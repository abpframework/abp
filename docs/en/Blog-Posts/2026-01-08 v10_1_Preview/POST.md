# ABP Platform 10.1 RC Has Been Released

We are happy to release [ABP](https://abp.io) version **10.1 RC** (Release Candidate). This blog post introduces the new features and important changes in this new version.

Try this version and provide feedback for a more stable version of ABP v10.1! Thanks to you in advance.

## Get Started with the 10.1 RC

You can check the [Get Started page](https://abp.io/get-started) to see how to get started with ABP. You can either download [ABP Studio](https://abp.io/get-started#abp-studio-tab) (**recommended**, if you prefer a user-friendly GUI application - desktop application) or use the [ABP CLI](https://abp.io/docs/latest/cli).

By default, ABP Studio uses stable versions to create solutions. Therefore, if you want to create a solution with a preview version, first you need to create a solution and then switch your solution to the preview version from the ABP Studio UI:

![studio-switch-to-preview.png](studio-switch-to-preview.png)

## Migration Guide

There are a few breaking changes in this version that may affect your application. Please read the migration guide carefully, if you are upgrading from v10.0 or earlier: [ABP Version 10.1 Migration Guide](https://abp.io/docs/10.1/release-info/migration-guides/abp-10-1).

## What's New with ABP v10.1?

In this section, I will introduce some major features released in this version.
Here is a brief list of titles explained in the next sections:

- Resource-Based Authorization
- Introducing the TickerQ Background Worker Provider
- Angular UI: Improving Authentication Token Handling
- Angular Version Upgrade to v21
- File Management Module: Public File Sharing Support
- Payment Module: Public Page Implementation for Blazor & Angular UIs
- AI Management Module: Blazor & Angular UIs
- Identity PRO Module: Password History Support
- Account PRO Module: Introducing WebAuthn Passkeys

### Resource-Based Authorization

ABP v10.1 introduces **Resource-Based Authorization**, a powerful feature that enables fine-grained access control based on specific resource instances. This enhancement addresses a long-requested feature ([#236](https://github.com/abpframework/abp/issues/236)) that allows you to implement authorization logic that depends on the resource being accessed, not just static roles or permissions.

**What is Resource-Based Authorization?**

Unlike traditional permission-based authorization where you check if a user has a general permission (like "CanEditDocuments"), resource-based authorization allows you to make authorization decisions based on the specific resource instance. For example:

- Allow users to edit only their own blog posts
- Grant access to documents based on ownership or sharing settings
- Implement complex authorization rules that depend on resource properties

![](ai-management-demo.gif)

#### How It Works?

**1. Define resource permissions (`AddResourcePermission`)**:

```csharp
public class MyPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        //other permissions...

        context.AddResourcePermission(
            name: BookManagementPermissions.Manage.Resources.Consume,
            resourceName: BookManagementPermissions.Manage.Resources.Name,
            managementPermissionName: BookManagementPermissions.Manage.ManagePermissions,
            L("LocalizedPermissionDisplayName")
        );
    }
}
```

**2. Use `IResourcePermissionChecker.IsGrantedAsync` in your code to perform the resource permission check**:

```csharp
protected IResourcePermissionChecker ResourcePermissionChecker { get; }

public async Task MyService()
{
    if(await ResourcePermissionChecker.IsGrantedAsync(
        BookManagementPermissions.Manage.Resources.Consume, 
        BookManagementPermissions.Manage.Resources.Name,
        workspaceConfiguration.WorkspaceId!.Value.ToString()))
        {
            return;
        }

        //...
}
```

**3. Use the relevant `ResourcePermissionManagementModel` in your UI:**

> The following code block demonstrates its usage in the Blazor UI, but the same component is also implemented for MVC & Angular UIs (however, component name might be different, please refer to the documentation before using the component).

```xml
<ResourcePermissionManagementModal @ref="PermissionManagementModal" />

@code {
    ResourcePermissionManagementModal PermissionManagementModal { get; set; } = null!;

    private Task OpenResourcePermissionModel()
    {
        await PermissionManagementModal.OpenAsync(
            resourceName: BookManagementPermissions.Manage.Resources.Name, 
            resourceKey: entity.Id.ToString(), 
            resourceDisplayName: entity.Name
        );
    }
}
```

This feature integrates perfectly with ABP's existing authorization infrastructure and provides a standard way to implement complex, context-aware authorization scenarios in your applications.

### Introducing the TickerQ Background Worker Provider

ABP v10.1 now includes **[TickerQ](https://tickerq.net/)** as a new background job and background worker provider option. TickerQ is a fast, reflection-free background task scheduler for .NET — built with source generators, EF Core integration, cron + time-based execution, and a real-time dashboard. It offers reliable job execution with built-in retry mechanisms, persistent job storage, and efficient resource usage.

To use TickerQ in your ABP-based solution, refer to the following documentation:

- [TickerQ Background Job Integration](https://abp.io/docs/10.1/framework/infrastructure/background-jobs/tickerq)
- [TickerQ Background Worker Integration](https://abp.io/docs/10.1/framework/infrastructure/background-workers/tickerq)

### Angular UI: Improving Authentication Token Handling

ABP v10.1 brings significant improvements to **Angular authentication token handling**, making token refresh more reliable and providing better error handling for expired or invalid tokens.

#### What's Improved?

Prior to this version, access tokens issued by the auth-server were stored in localStorage, making them vulnerable to XSS attacks. We've made the following enhancements to improve safety and reduce security risks:

- Store sensitive tokens in memory
- Use web-workers for state sharing between tabs

These enhancements are automatically available in new Angular projects and can be applied to existing projects by updating ABP packages.

> See [#23930](https://github.com/abpframework/abp/issues/23930) for more details.

### Angular Version Upgrade to v21

ABP v10.1 **upgrades Angular to version 21**, bringing the latest improvements and features from the Angular ecosystem to your ABP applications. We've upgraded the relevant core Angular packages and 3rd party packages such as **angular-oauth2-oidc** and **ng-bootstrap**. We will also update the ABP Studio templates along with the stable v10.1 release.

> See [#24384](https://github.com/abpframework/abp/issues/24384) for the complete change list.

### File Management Module: Public File Sharing Support

_This is a **PRO** feature available for ABP Commercial customers._

The **File Management Module** now supports **public file sharing** via shareable links, similar to popular cloud storage services like Google Drive or Dropbox. This feature enables you to generate public URLs for files that can be accessed without authentication.

![](file-sharing.gif)

**Example Share URL:**

```text
https://abp.io/api/file-management/file-descriptor/share?shareToken=CfDJ8AK%2BOEpCD...
```

**Configuration:**

You can configure the public share domain through options:

```csharp
Configure<FileManagementWebOptions>(options =>
{
    options.FileDownloadRootUrl = "https://files.yourdomain.com";
});
```

This feature is available for all supported UI types (MVC, Angular, Blazor) and integrates seamlessly with the existing [File Management Module](https://abp.io/docs/latest/modules/file-management).

### Payment Module: Public Page Implementation for Blazor & Angular UIs

The **Payment Module** now includes **public page implementations for Angular and Blazor UIs**, completing UI coverage across all ABP-supported frameworks. Previously, public payment pages (payment gateway selection, pre-payment, and post-payment pages) were only available for MVC/Razor Pages UI. With this version, both admin and public pages are now available for MVC, Angular, and Blazor UIs.

The public payment pages seamlessly integrate with ABP's [Payment Module](https://abp.io/docs/latest/modules/payment) and support all configured payment gateways. The documentation will be updated soon with detailed integration guides and examples at [abp.io/docs/latest/modules/payment](https://abp.io/docs/latest/modules/payment).

### AI Management Module: Blazor & Angular UIs

With this version, Angular and Blazor UIs for the [AI Management module](https://abp.io/docs/latest/modules/ai-management) have been implemented, completing the cross-platform support for this powerful AI integration module.

![AI Management Workspaces](ai-management-workspaces.png)

The AI Management Module builds on top of [ABP's AI Infrastructure](https://abp.io/docs/latest/framework/infrastructure/artificial-intelligence) and provides:

- **Multi-Provider Support**: Integrate with OpenAI, Google Gemini, Anthropic Claude, and more from a unified API
- **Workspace-Based Organization**: Organize AI capabilities into separate workspaces for different use cases
- **Built-In Chat Interface**: Ready-to-use chat UI for conversational AI
- **Chat Widget**: Drop-in chat widget component for customer support or AI assistance
- **Resource-Based Permissions**: Control access to specific AI workspaces for users, roles, or clients

Learn more about the AI Management Module in the [announcement post](https://abp.io/community/announcements/introducing-the-ai-management-module-nz9404a9) and [official documentation](https://abp.io/docs/latest/modules/ai-management).

### Identity PRO Module: Password History Support

The [**Identity PRO Module**](https://abp.io/docs/latest/modules/identity-pro) now includes **Password History** support, preventing users from reusing previous passwords. This security feature helps enforce stronger password policies and meet compliance requirements for your organization.

Administrators can enable password reuse prevention by toggling the related setting on the _Administration -> Settings -> Identity Management_ page:

![Password History Settings](password-history-settings.png)

When changing a password, the system checks the specified number of previous passwords and displays an error message if the new password matches any of them:

![](set-password-error-modal.png)

![](reset-password-error-modal.png)

### Account PRO Module: Introducing WebAuthn Passkeys

ABP v10.1 introduces **Passkey authentication**, enabling passwordless sign-in using modern biometric authentication methods. Built on the **WebAuthn standard (FIDO2)**, this feature allows users to authenticate using Face ID, Touch ID, Windows Hello, Android biometrics, security keys, or other platform authenticators.

**What are Passkeys?**

Passkeys are a modern, phishing-resistant authentication method that replaces traditional passwords:

- **Passwordless**: No passwords to remember, type, or manage
- **Secure**: Uses public/private key cryptography stored on the user's device
- **Convenient**: Sign in with a fingerprint, face scan, or device PIN
- **Cross-Platform**: Can sync across devices depending on platform support (Apple, Google, Microsoft)

**How It Works:**

**1. Enable or disable the WebAuthn passkeys feature in the _Settings -> Account -> Passkeys_ page:**

![Passkey Setting](passkey-setting.png)

**2. Add your passkeys in the _Account/Manage_ page:**

![My Passkeys](my-passkey.png)

![Passkey registration](passkey-registration.png)

**3. Use the _Passkey login_ option for passwordless authentication the next time you log in:**

![Passkey Login](passkey-login.png)

> For more information, refer to the [Web Authentication API (WebAuthn) passkeys](https://abp.io/docs/10.1/modules/account/passkey) documentation.

## Community News

### Special Offer: Level Up Your ABP Skills with 33% Off Live Trainings!

![ABP Live Training Discount](./live-training-discount.png)

We're excited to announce a special limited-time offer for developers looking to master the ABP Platform! Get **33% OFF** on all ABP live training sessions and accelerate your learning journey with hands-on guidance from ABP experts.

**Why Join ABP Live Trainings?**

Our live training sessions provide an immersive learning experience where you can:

- **Learn from the Experts**: Get direct instruction from ABP team members and experienced trainers who know the platform inside and out.
- **Hands-On Practice**: Work through real-world scenarios and build actual applications during the sessions.
- **Interactive Q&A**: Ask questions in real-time and get immediate answers to your specific challenges.
- **Comprehensive Coverage**: From fundamentals to advanced topics, our trainings cover everything you need to build production-ready applications with ABP.
- **Certificate of Completion**: Receive a certificate upon completing the training to showcase your ABP expertise.

Don't miss this opportunity to invest in your skills and career. Whether you're new to ABP or looking to advance your expertise, our live trainings provide the structured learning path you need to succeed.

> 👉 [Learn more and claim your discount here](https://abp.io/community/announcements/improve-your-abp-skills-with-33-off-live-trainings-hjnw57xu)

### Introducing the ABP Referral Program

![ABP.IO Referral Program](./referral-program.png)

We're thrilled to announce the launch of the **ABP.IO Referral Program**, a new way for our community members to earn rewards while helping others discover the ABP Platform!

**How It Works:**

ABP's Referral Program is simple and rewarding:

1. **Get Your Unique Referral Link**: Sign up for the program and receive your personalized referral link.
2. **Share with Your Network**: Share your link with colleagues, friends, and fellow developers who could benefit from ABP.
3. **Earn Rewards**: When someone purchases an ABP Commercial license through your referral link, **you earn 5% commission**!

By joining the referral program, you're not just earning rewards and also you're helping other developers discover a platform that can significantly improve their productivity and project success.

> 👉 [Join the ABP.IO Referral Program](https://abp.io/community/announcements/introducing-abp.io-referral-program-b59obhe7)

### Announcing AI Management Module

We are excited to announce the [AI Management Module](https://abp.io/docs/10.0/modules/ai-management), a powerful new module to the ABP Platform that makes managing AI capabilities in your applications easier than ever!

![ABP - AI Management Module Workspaces](ai-management-workspaces.png)

**What is the AI Management Module?**

Built on top of the [ABP Framework's AI infrastructure](https://abp.io/docs/latest/framework/infrastructure/artificial-intelligence), the **AI Management Module** allows you to manage AI workspaces dynamically without touching your code. Whether you're building a customer support chatbot, adding AI-powered search, or creating intelligent automation workflows, this module provides everything you need to manage AI integrations through a user-friendly interface.

**Key Features:**

- **Multi-Provider Support**: Allows integrating with multiple AI providers including OpenAI, Google Gemini, Anthropic Claude, and more from a single unified API.
- **Buit-In Chat Interface**
- **Ready to Use Chat Widget**
- and more... (RAG & MCP supports are on the way!)

👉 [Read the announcement post for more...](https://abp.io/community/announcements/introducing-the-ai-management-module-nz9404a9)

### We Were At .NET Conf China 2025!

![.NET Conf China 2025](./dotnet-conf-china-2025.png)

The ABP team participated in **.NET Conf China 2025** in Shanghai, celebrating the release of .NET 10 (LTS) and the achievements of the .NET community in China. 

**Event Highlights:**

The conference brought together hundereds of developers and featured Scott Hanselman's opening keynote announcing .NET 10's availability, focused on four pillars: AI, cloud-native, cross-platform, and performance. The event covered three main themes: performance improvements, AI integration, and cross-platform development, with in-depth sessions on topics ranging from Avalonia and Blazor to AI agents and enterprise adoption.

**ABP's Participation:**

At the ABP booth, we showcased our developer platform with live demonstrations of modular architecture, multi-tenancy support, and built-in authentication systems. We hosted interactive raffles with prizes including ABP stickers, the _Mastering ABP Framework_ book, and Bluetooth headphones. The booth was a hub for sharing experiences, impromptu code walkthroughs, and meaningful conversations with Chinese developers about ABP's future.

> 👉 [Read the full event recap](https://abp.io/community/announcements/.net-conf-china-2025-fz03gfge)

### Community Talks 2025.10: AI-Powered .NET Apps with ABP & Microsoft Agent Framework

![ABP Community Talks - AI-Powered .NET Apps](./community-talk-2025-10-ai.png)

In our latest ABP Community Talks session, we dove deep into the world of **Artificial Intelligence** and its integration with the ABP Framework. This session explored Microsoft's cutting-edge AI libraries: **Extensions AI**, **Semantic Kernel**, and the **Microsoft Agent Framework**.

**What We Covered:**

We introduced the new **AI Management Module**, discussing its current status and roadmap. The session included practical demonstrations on building intelligent applications with the Microsoft Agent Framework within ABP projects, showing how these technologies empower developers to create AI-powered .NET applications.

> 👉 [Missed the live session? Click here to watch the full session](https://www.youtube.com/live/tEcd2H6yXQk)

### New ABP Community Articles

There are exciting articles contributed by the ABP community as always. I will highlight some of them here:

- [Salih Özkara](https://github.com/salihozkara) has published 3 new articles:
    - [Building Dynamic XML Sitemaps with ABP Framework](https://abp.io/community/articles/building-dynamic-xml-sitemaps-with-abp-framework-n3q6schd)
    - [Implement Automatic Method-Level Caching in ABP Framework](https://abp.io/community/articles/implement-automatic-methodlevel-caching-in-abp-framework-4uzd3wx8)
    - [Building Production-Ready LLM Applications with .NET: A Practical Guide](https://abp.io/community/articles/building-production-ready-llm-applications-with-net-ya7qemfa)
- [Adnan Ali](https://abp.io/community/members/adnanaldaim) has published 2 new articles:
    - [Integrating AI into ABP.IO Applications: The Complete Guide to Volo.Abp.AI and AI Management Module](https://abp.io/community/articles/integrating-ai-into-abp.io-applications-the-complete-guide-jc9fbjq0)
    - [How ABP.IO Framework Cuts Your MVP Development Time by 60%](https://abp.io/community/articles/how-abp.io-framework-cuts-your-mvp-development-time-by-60-8l7m3ugj)
- [My First Look and Experience with Google AntiGravity](https://abp.io/community/articles/my-first-look-and-experience-with-google-antigravity-0hr4sjtf) by [Alper Ebiçoğlu](https://twitter.com/alperebicoglu)
- [TOON vs JSON for LLM Prompts in ABP: Token-Efficient Structured Context](https://abp.io/community/articles/toon-vs-json-b4rn2avd) by [Suhaib Mousa](https://abp.io/community/members/suhaib-mousa)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP-related (text or video) content](https://abp.io/community/posts/create) to the ABP Community.

## Conclusion

This version comes with some new features and a lot of enhancements to the existing features. You can see the [Road Map](https://abp.io/docs/10.1/release-info/road-map) documentation to learn about the release schedule and planned features for the next releases. Please try ABP v10.1 RC and provide feedback to help us release a more stable version.

Thanks for being a part of this community!