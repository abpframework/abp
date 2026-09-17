# ABP.IO Platform 10.5 Final Has Been Released!

We are glad to announce that [ABP](https://abp.io/) 10.5 stable version has been released.

## What's New With Version 10.5?

All the new features were explained in detail in the [10.5 RC Announcement Post](https://abp.io/community/announcements/announcing-abp-10-5-release-candidate-k6oxdfle), so there is no need to review them again. You can check it out for more details.

## Getting Started with 10.5

### How to Upgrade an Existing Solution

You can upgrade your existing solutions with either ABP Studio or ABP CLI. In the following sections, both approaches are explained:

### Upgrading via ABP Studio

If you are already using the ABP Studio, you can upgrade it to the latest version. ABP Studio periodically checks for updates in the background, and when a new version of ABP Studio is available, you will be notified through a modal. Then, you can update it by confirming the opened modal. See [the documentation](https://abp.io/docs/latest/studio/installation#upgrading) for more info.

After upgrading the ABP Studio, then you can open your solution in the application, and simply click the **Upgrade ABP Packages** action button to instantly upgrade your solution:

![](upgrade-abp-packages.png)

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

There are no explicitly marked breaking changes in this version. However, there are still some important migration notes for specific scenarios. Please read the migration guide carefully, if you are upgrading from v10.4 or earlier versions: [ABP Version 10.5 Migration Guide](https://abp.io/docs/10.5/release-info/migration-guides/abp-10-5)

## Community News

### New ABP Community Articles

As always, exciting articles have been contributed by the ABP community. I will highlight some of them here:

- [Sumeyye Kurtulus](https://abp.io/community/members/sumeyye.kurtulus) has published 2 new articles:
    - [Angular 22 State Management: Signals, SignalStore, or NgRx?](https://abp.io/community/articles/angular-22-state-management-signals-signalstore-or-ngrx-yq8zg0nw)
    - [Customizing the ABP Framework: A Developer's Guide to LeptonX Theme Overrides in Angular and the Transition to React UI](https://abp.io/community/articles/customizing-the-abp-framework-a-developers-guide-to-nklweri3)
- [Working with Dapr Workflows in the ABP Framework](https://abp.io/community/articles/working-with-dapr-workflows-in-the-abp-framework-6476or18) by [Engincan Veske](https://abp.io/community/members/EngincanV)
- [Alper Ebicoglu](https://abp.io/community/members/alper) has published 2 new articles:
    - [My Speaker's View of CONVEX Summit 2026](https://abp.io/community/articles/my-speakers-view-of-convex-summit-2026-ai-net-conference-3uk6ln1l)
    - [AI Isn't Replacing Developers - It's Changing What Good Developers Spend Time On](https://abp.io/community/articles/ai-isnt-replacing-developers-its-changing-what-good-2016q6ng)
- [Deep Dive on ABP AI Agent: The Complete Series](https://abp.io/community/articles/deep-dive-on-abp-ai-agent-the-complete-series-f7jute7n) by [Berkan Sasmaz](https://abp.io/community/members/berkansasmaz)
    - We have created a deep-dive series for ABP Studio's AI Coding Agent. You can read this series to learn the main features of the AI Coding Agent and how it can help you while developing ABP-based solutions.

Thanks to the ABP Community for all the content they have published. You can also [post your ABP related (text or video) content](https://abp.io/community/posts/create) to the ABP Community.

## About the Next Version

The next feature version will be 10.6. You can follow the [release planning here](https://github.com/abpframework/abp/milestones). Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.
