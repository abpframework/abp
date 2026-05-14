# ABP.IO Platform 10.4 Final Has Been Released!

We are glad to announce that [ABP](https://abp.io/) 10.4 stable version has been released.

## What's New With Version 10.4?

All the new features were explained in detail in the [10.4 RC Announcement Post](https://abp.io/community/announcements/announcing-abp-10-4-release-candidate-7ukyudm0), so there is no need to review them again. You can check it out for more details.

## Getting Started with 10.4

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

There are no explicitly marked breaking changes in this version. However, there are still some important migration notes for specific scenarios. Please read the migration guide carefully, if you are upgrading from v10.3 or earlier versions: [ABP Version 10.4 Migration Guide](https://abp.io/docs/10.4/release-info/migration-guides/abp-10-4)

## Community News

### Highlights from the ABP Community

There have been some important announcements for the ABP community recently. Here are two highlights you may want to check out:

#### React UI for ABP Framework Is Finally Here

![React UI for ABP Framework Is Finally Here](https://abp.io/api/posts/cover-picture-source/3a2114d0-5518-38a8-d9b3-ab5100b587a4?v=20260508112328)

React support has been one of the most requested topics in the ABP community, and it is now available as a first-class UI option in the modern template system. You can read the announcement here: [React UI for ABP Framework Is Finally Here](https://abp.io/community/announcements/react-ui-for-abp-framework-is-finally-here-7rfmgb2v).

#### Introducing ABP Studio AI Agent

![Introducing ABP Studio AI Agent](https://abp.io/api/posts/cover-picture-source/3a212ebc-06c1-e10f-f83c-a90079f988c1?v=20260508112328)

ABP Studio now introduces a deeply integrated AI coding agent that understands ABP solutions, modules, layers, run profiles, and runtime feedback. You can read the announcement here: [Introducing ABP Studio AI Agent](https://abp.io/community/announcements/introducing-abp-studio-ai-agent-o1ni0toc).

### New ABP Community Articles

As always, exciting articles have been contributed by the ABP community. I will highlight some of them here:

- [ABP in the AI Era: Surviving, Evolving, and Staying Relevant](https://abp.io/community/articles/abp-in-the-ai-era-surviving-evolving-and-staying-relevant-6gyfjfpe) by [Engincan Veske](https://abp.io/community/members/EngincanV)
- [Stop Sprinkling [RequiresFeature] Everywhere — A Centralized Feature Gate for ABP.IO](https://abp.io/community/articles/stop-sprinkling-requiresfeature-everywhere-a-centralized-7znie818) by [Mohammad AlMohammad AlMahmoud](https://abp.io/community/members/Mohammad97Dev)
- [Top AI Coding Models in 2026: Which One Should Developers Actually Use?](https://abp.io/community/articles/top-ai-coding-models-in-2026-which-one-should-developers-use-rivh8x15) by [Alper Ebiçoğlu](https://abp.io/community/members/alper)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP related (text or video) content](https://abp.io/community/posts/create) to the ABP Community.

## About the Next Version

The next feature version will be 10.5. You can follow the [release planning here](https://github.com/abpframework/abp/milestones). Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.
