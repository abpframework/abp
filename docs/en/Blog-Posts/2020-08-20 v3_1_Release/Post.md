# ABP Framework v3.1 RC.1 Has Been Released

Today, we are releasing the **ABP Framework version 3.1 Release Candidate 1** (RC.1). The development cycle for this version was ~7 weeks. It was the longest development cycle for a feature version release ever. There were two main reasons of this long development cycle;

* We've switched to **4-weeks** release cycle (was discussed in [this issue](https://github.com/abpframework/abp/issues/4692)).
* We've [re-written](https://github.com/abpframework/abp/issues/4881) the Angular service proxy generation system using the Angular schematics to make it more stable. There were some problems with the previous implementation.

This long development cycle brings a lot of new features, improvements and bug fixes. I will highlight the fundamental features and changes in this blog post.

## About the Preview/Stable Version Cycle

As mentioned above, it is planned to release a new stable feature version (like 3.1, 3.2, 3.3...) in every 4-weeks.

In addition, we are starting to deploy **Release Candidate (RC)** versions 2-weeks before the stable versions for every feature releases. You can think these releases as "**preview**" version.

Today, we've released `3.1.0-rc.1` as the first RC/Preview version. We may release more RC versions if it is needed until the stable version.

The stable `3.1.0` version will be released on September 3, 2020. Next RC version, `3.2.0-rc.1` was planned for September 17, 2020 (2 weeks after the stable 3.1 version and 2 weeks before the stable 3.2 version).

> We **won't add new features** to a version after publishing the RC/Preview version. We only will make **bug fixes** until the stable version. The new features being developed in this period will be available in the next version.

### About the Nightly Builds

Don't confuse RC/Preview versions and nightly builds. When we say RC or preview, we are mentioning the preview system explained above.

We will continue to publish **nightly builds** for all the [ABP Framework packages](https://abp.io/packages). You can refer to [this document](https://docs.abp.io/en/abp/latest/Nightly-Builds) to learn how to use the nightly packages.

## Get Started with the RC Versions

Please try the preview versions and provide feedback to us to release more stable versions. Please open an  issue on the [GitHub repository](https://github.com/abpframework/abp/issues/new) if you find a bug.

### New Solutions

The [ABP.IO](https://abp.io/) platform and the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) are compatible with the RC system. You can select the "preview" option on the [download page](https://abp.io/get-started) or use the "**--preview**" parameter with the ABP CLI [new](https://docs.abp.io/en/abp/latest/CLI?_ga=2.106435654.411298747.1597771169-1910388957.1594128976#new) command:

````bash
abp new Acme.BookStore --preview
````

This command will create a new project with the latest RC/Preview version. Whenever the stable version is released, you can switch to the stable version for your solution using the `abp --switch-to-stable` command in the root folder of your solution.

### Existing Solutions

If you already have a solution and want to use/test the latest RC/Preview version, use the `abp --switch-to-preview` command in the root folder of your solution. You can return back to the latest stable using the `abp --switch-to-stable ` command later.

## What's New with the ABP Framework v3.1

### Angular Service Proxies

TODO

### Global Feature System

TODO

### New Account Module Features

TODO

### New Identity Module Features

TODO

## What's New with the ABP Commercial v3.1

### New Account Module Features

TODO

### New Identity Module Features

TODO

### Others

TODO