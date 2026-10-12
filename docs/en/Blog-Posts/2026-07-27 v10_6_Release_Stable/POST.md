# ABP.IO Platform 10.6 Final Has Been Released!

We are glad to announce that [ABP](https://abp.io/) 10.6 stable version has been released.

## What's New With Version 10.6?

All the new features were explained in detail in the [10.6 RC Announcement Post](https://abp.io/community/announcements/abp-platform-10.6-rc-has-been-released-reoq6kzw), so there is no need to review them all again. You can check it out for more details.

Here are some of the highlights of this version:

- Background jobs now support dedicated workers, parallel execution, and successful job retention.
- API definition and generated proxies have better support for response content types, remote streams, and multipart uploads.
- Angular UI packages and templates have been upgraded to Angular 22.
- Antiforgery and OpenIddict flows include important security and reliability improvements.
- ABP Commercial adds OpenIddict access-token generation from the UI and React CRUD page generation support in ABP Suite.
- AI Management indexing is more resilient for large or memory-constrained workloads.
- The final release also includes dependency updates and stability fixes collected during the RC period.

## Getting Started with 10.6

### How to Upgrade an Existing Solution

You can upgrade your existing solutions with either ABP Studio or ABP CLI. In the following sections, both approaches are explained:

### Upgrading via ABP Studio

If you are already using the ABP Studio, you can upgrade it to the latest version. ABP Studio periodically checks for updates in the background, and when a new version of ABP Studio is available, you will be notified through a modal. Then, you can update it by confirming the opened modal. See [the documentation](https://abp.io/docs/latest/studio/installation#upgrading) for more info.

After upgrading the ABP Studio, then you can open your solution in the application, and simply click the **Upgrade ABP Packages** action button to instantly upgrade your solution:

![](https://raw.githubusercontent.com/abpframework/abp/dev/docs/en/Blog-Posts/2026-07-27%20v10_6_Release_Stable/upgrade-abp-packages.png)

### Upgrading via ABP CLI

Alternatively, you can upgrade your existing solution via ABP CLI. First, you need to install the ABP CLI or upgrade it to the latest version.

If you haven't installed it yet, you can run the following command:

```bash
dotnet tool install -g Volo.Abp.Studio.Cli
```

Or to update the existing CLI, you can run the following command:

```bash
dotnet tool update -g Volo.Abp.Studio.Cli
```

After installing/updating the ABP CLI, you can use the [`update` command](https://abp.io/docs/latest/CLI#update) to update all the ABP related NuGet and NPM packages in your solution as follows:

```bash
abp update
```

You can run this command in the root folder of your solution to update all ABP related packages.

## Migration Guides

This version includes explicitly marked migration-impacting changes for specific customization scenarios, especially custom background job stores/workers and custom AI Management document chunk repositories. The new background job runtime features are opt-in and existing applications keep the current behavior unless they enable them explicitly.

Please read the migration guide carefully, if you are upgrading from v10.5 or earlier versions: [ABP Version 10.6 Migration Guide](https://abp.io/docs/10.6/release-info/migration-guides/abp-10-6)

If you use the Angular UI, also check the dedicated [Angular 22 and ABP 10.6 Upgrade Guide](https://abp.io/docs/10.6/release-info/migration-guides/abp-10-6-angular-22).

## Community News

### New ABP Community Articles

As always, exciting articles have been contributed by the ABP community. I will highlight some of them here:

- [Introducing ABP Low-Code: Build Real ABP Apps in Minutes](https://abp.io/community/announcements/introducing-abp-lowcode-build-real-abp-apps-in-minutes-647ymozi) by [Salih Ozkara](https://abp.io/community/members/salih)
- [Building a Vendor Onboarding Workflow with ABP Low-Code](https://abp.io/community/articles/building-a-vendor-onboarding-workflow-with-abp-lowcode-1wx0ckzc) by [Salih Ozkara](https://abp.io/community/members/salih)
- [Event Recap - WeAreDevelopers World Congress 2026](https://abp.io/community/articles/event-recap-wearedevelopers-world-congress-2026-v59t8vfn) by [Irem Demirci](https://abp.io/community/members/iremdemirci)
- [Empathy in the Workplace for Software Companies](https://abp.io/community/articles/empathy-in-the-workplace-for-software-companies-wsjjw9we) by [Alper Ebicoglu](https://abp.io/community/members/alper)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP related (text or video) content](https://abp.io/community/posts/create) to the ABP Community.

## About the Next Version

The next feature version will be 10.7. You can follow the [release planning here](https://github.com/abpframework/abp/milestones). Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.
