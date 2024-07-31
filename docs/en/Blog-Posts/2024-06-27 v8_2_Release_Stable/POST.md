# ABP.IO Platform 8.2 Final Has Been Released!

[ABP Framework](https://abp.io/) and [ABP Commercial](https://commercial.abp.io/) 8.2 versions have been released today.

## What's New With Version 8.2?

All the new features were explained in detail in the [8.2 RC Announcement Post](https://blog.abp.io/abp/announcing-abp-8-2-release-candidate), so there is no need to review them again. You can check it out for more details. 

## Getting Started with 8.2

### Creating New Solutions

You can create a new solution with the ABP Framework version 8.2 by either using the `abp new` command or generating the CLI command on the [get started page](https://abp.io/get-started).

> See the [getting started document](https://docs.abp.io/en/abp/latest/Getting-Started) for more.

### How to Upgrade an Existing Solution

#### Install/Update the ABP CLI

First, install the ABP CLI or upgrade it to the latest version.

If you haven't installed it yet:

```bash
dotnet tool install -g Volo.Abp.Cli
```

To update the existing CLI:

```bash
dotnet tool update -g Volo.Abp.Cli
```

#### Upgrading Existing Solutions with the ABP Update Command

[ABP CLI](https://docs.abp.io/en/abp/latest/CLI) provides a handy command to update all the ABP related NuGet and NPM packages in your solution with a single command:

```bash
abp update
```

Run this command in the root folder of your solution.

## Migration Guides

There are a few breaking changes in this version that may affect your application.
Please see the following migration documents, if you are upgrading from v8.x or earlier:

* [ABP Framework 8.x to 8.2 Migration Guide](https://docs.abp.io/en/abp/8.2/Migration-Guides/Abp-8_2)
* [ABP Commercial 8.x to 8.2 Migration Guide](https://docs.abp.io/en/commercial/8.2/migration-guides/v8_2)

## Community News

### New ABP Community Posts

As always, exciting articles have been contributed by the ABP community. I will highlight some of them here:

* [How to use Angular Material with Form Validation on ABP](https://community.abp.io/posts/how-to-use-angular-material-with-form-validation-on-abp-jtheajj3) by [Mahmut Gündoğdu](https://x.com/mahmutgundogdu)
* [Tunnel your local host address to a public URL with ngrok](https://community.abp.io/posts/tunnel-your-local-host-address-to-a-public-url-with-ngrok-4cywnocj) by [Bart Van Hoey](https://github.com/bartvanhoey)
* [Antiforgery Token Validation When Angular and HTTP API Runs on the Same Server](https://community.abp.io/posts/antiforgery-token-validation-when-angular-and-http-api-runs-on-the-same-server-mzf5ppdq) by [dignite](https://x.com/dignite_adu)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP-related (text or video) content](https://community.abp.io/articles/submit) to the ABP Community.

## About the Next Version

The next feature version will be 8.3. You can follow the [release planning here](https://github.com/abpframework/abp/milestones). Please [submit an issue](https://github.com/abpframework/abp/issues/new) if you have any problems with this version.
