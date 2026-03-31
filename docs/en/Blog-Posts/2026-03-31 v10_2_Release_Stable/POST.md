# ABP.IO Platform 10.2 Final Has Been Released!

We are glad to announce that [ABP](https://abp.io/) 10.2 stable version has been released.

## What's New With Version 10.2?

All the new features were explained in detail in the [10.2 RC Announcement Post](https://abp.io/community/announcements/announcing-abp-10-2-release-candidate-05zatjfq), so there is no need to review them again. You can check it out for more details.

## Getting Started with 10.2

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

There are a few breaking changes in this version that may affect your application. Please read the migration guide carefully, if you are upgrading from v10.1 or earlier versions: [ABP Version 10.2 Migration Guide](https://abp.io/docs/10.2/release-info/migration-guides/abp-10-2)

## Community News

### New ABP Community Articles

As always, exciting articles have been contributed by the ABP community. I will highlight some of them here:

- [Liming Ma](https://abp.io/community/members/maliming) has published 6 new posts:
   - [Dynamic Events in ABP](https://abp.io/community/articles/dynamic-events-in-abp-dukq95m1)
   - [Dynamic Background Jobs and Workers in ABP](https://abp.io/community/articles/dynamic-background-jobs-and-workers-in-abp-wfdkdsq9)
   - [Shared User Accounts in ABP Multi-Tenancy](https://abp.io/community/articles/shared-user-accounts-in-abp-multitenancy-mf3bkg79)
   - [Secure Client Authentication with private_key_jwt in ABP 10.3](https://abp.io/community/articles/secure-client-authentication-with-privatekeyjwt-in-abp-b2rf18bc)
   - [Operation Rate Limiting in ABP Framework](https://abp.io/community/articles/operation-rate-limiting-in-abp-framework-f4jtd6sn)
   - [Resource-Based Authorization in ABP Framework](https://abp.io/community/articles/resourcebased-authorization-in-abp-framework-choku1sn)
- [One Endpoint, Many AI Clients: Turning ABP Workspaces into OpenAI-Compatible Models](https://abp.io/community/articles/turning-abp-workspaces-into-openai-compatible-endpoints-u3ls1gp4) by [Engincan Veske](https://abp.io/community/members/EngincanV)
- [Automatically Validate Your Documentation: How We Built a Tutorial Validator](https://abp.io/community/articles/automatically-validate-your-documentation-m3ozgkhv) by [Mansur Besleney](https://abp.io/community/members/mansur.besleney)
- [Automate Localhost Access for Expo: A Guide to Dynamic Cloudflare Tunnels & Dev Builds](https://abp.io/community/articles/automate-localhost-access-for-expo-a-guide-to-dynamic-7cblqtj3) by [Sumeyye Kurtulus](https://abp.io/community/members/sumeyye.kurtulus)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP related (text or video) content](https://abp.io/community/posts/create) to the ABP Community.

## About the Next Version

The next feature version will be 10.3. You can follow the [release planning here](https://github.com/abpframework/abp/milestones). Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.
