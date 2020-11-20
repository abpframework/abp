# ABP Framework 3.1 Final Has Been Released

It is exciting for us to announce that we've released the ABP Framework & ABP Commercial 3.1 today.

Since all the new features are already explained in details with the [3.1 RC Announcement Post](https://blog.abp.io/abp/ABP-Framework-v3.1-RC-Has-Been-Released), I will not repeat all the details here. Please read [the RC post](https://blog.abp.io/abp/ABP-Framework-v3.1-RC-Has-Been-Released) for **new feature and changes** you may need to do for your solution while upgrading to the version 3.1.

## Creating New Solutions

You can create a new solution with the ABP Framework version 3.1 by either using the `abp new` command or using the **direct download** tab on the [get started page](https://abp.io/get-started).

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

After the update command, check [the RC blog post](https://blog.abp.io/abp/ABP-Framework-v3.1-RC-Has-Been-Released) to learn if you need to make any changes in your solution.

> You may want to see the new [upgrading document](https://docs.abp.io/en/abp/latest/Upgrading).

## About the version 3.2

The planned schedule for the version 3.2 is like that;

* **September 17, 2020**: 3.2.0-rc.1 (release candidate)
* **October 1, 2020**: 3.2.0 final (stable)

You can check [the GitHub milestone](https://github.com/abpframework/abp/milestone/39) to see the features/issues we are working on.

## ABP Community & Articles

We had lunched the [ABP Community web site](https://community.abp.io/) a few weeks before. The core ABP team and the ABP community have started to create content for the community.

Here, the last three articles from the ABP Community:

* [ABP Suite: How to Add the User Entity as a Navigation Property of Another Entity](https://community.abp.io/articles/abp-suite-how-to-add-the-user-entity-as-a-navigation-property-of-another-entity-furp75ex) by [@ebicoglu](https://github.com/ebicoglu)
* [Reuse ABP vNext Modules to Quickly Implement Application Features](https://community.abp.io/articles/reuse-abp-vnext-modules-to-quickly-implement-application-features-tdtmwd9w) by [@gdlcf88](https://github.com/gdlcf88)
* [Using DevExtreme Components With the ABP Framework](https://community.abp.io/articles/using-devextreme-components-with-the-abp-framework-zb8z7yqv) by [@cotur](https://github.com/cotur).

We are looking for your contributions; You can [submit your article](https://community.abp.io/articles/submit)! We will promote your article to the community.