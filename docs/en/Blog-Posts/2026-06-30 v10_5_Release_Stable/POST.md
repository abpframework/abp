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

- [Fahri Gedik](https://abp.io/community/members/fahrigedik) has published 2 new articles:
    - [New Look for ABP React Native: NativeWind, Modernization & Two Sample Apps](https://abp.io/community/articles/new-abp-modern-react-native-template-rxjiyrpb)
    - [The Antidote to Vibe Architecting: ABP Studio AI Agent](https://abp.io/community/articles/the-antidote-to-vibe-architecting-abp-studio-ai-agent-mpdeh3gr)
- [Template In, Product Out: Building Hanova with the ABP AI Agent](https://abp.io/community/articles/template-in-product-out-building-hanova-with-the-abp-ai-hcntpk3j) by [Sumeyye Kurtulus](https://abp.io/community/members/sumeyye.kurtulus)
- [Empowering AI Agents with ABP Framework: A Comprehensive Skill Collection](https://abp.io/community/articles/abp-framework-ai-agent-skills-qccn87tu) by [Burak Demir](https://abp.io/community/members/burakdemir)
- [Google Pomelli: How to Market Your App Without Being a Designer](https://abp.io/community/articles/google-pomelli-how-to-market-your-app-1hu48pda) by [Engincan Veske](https://abp.io/community/members/EngincanV)
- [DevDays 2026 Conf From a Speaker's View](https://abp.io/community/articles/devdays-2026-conference-from-a-speakers-view-39d007hs) by [Alper Ebicoglu](https://abp.io/community/members/alper)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP related (text or video) content](https://abp.io/community/posts/create) to the ABP Community.

## About the Next Version

The next feature version will be 10.6. You can follow the [release planning here](https://github.com/abpframework/abp/milestones). Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.
