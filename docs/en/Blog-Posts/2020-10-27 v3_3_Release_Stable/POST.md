# ABP Framework & ABP Commercial 3.3 Final Have Been Released

ABP Framework & ABP Commercial 3.3.0 have been released today.

Since all the new features are already explained in details with the [3.3 RC Announcement Post](https://blog.abp.io/abp/ABP-Framework-ABP-Commercial-v3.3-RC-Have-Been-Released), I will not repeat all the details again. Please read [the RC post](https://blog.abp.io/abp/ABP-Framework-ABP-Commercial-v3.3-RC-Have-Been-Released) for **new feature and changes** you may need to do for your solution while upgrading to the version 3.3.

## Creating New Solutions

You can create a new solution with the ABP Framework version 3.3 by either using the `abp new` command or using the **direct download** tab on the [get started page](https://abp.io/get-started).

> See the [getting started document](https://docs.abp.io/en/abp/latest/Getting-Started) for details.

## How to Upgrade an Existing Solution

### Install/Update the ABP CLI

First of all, install the ABP CLI or upgrade to the latest version.

If you haven't installed yet:

````bash
dotnet tool install -g Volo.Abp.Cli
````

To update an existing installation:

```bash
dotnet tool update -g Volo.Abp.Cli
```

### ABP UPDATE Command

[ABP CLI](https://docs.abp.io/en/abp/latest/CLI) provides a handy command to update all the ABP related NuGet and NPM packages in your solution with a single command:

````bash
abp update
````

Run this command in the root folder of your solution. After the update command, check [the RC blog post](https://blog.abp.io/abp/ABP-Framework-ABP-Commercial-v3.3-RC-Have-Been-Released) to learn if you need to make any changes in your solution.

> You may want to see the new [upgrading document](https://docs.abp.io/en/abp/latest/Upgrading).

## About the Next Version: 4.0

The next version will be 4.0 and it will be mostly related to completing the Blazor UI features and upgrading the ABP Framework & ecosystem to the .NET 5.0.

The goal is to complete the version 4.0 with a stable Blazor UI with the fundamental features implemented and publish it just after the Microsoft lunches .NET 5 in this November. The planned 4.0 preview release date is November 11th.