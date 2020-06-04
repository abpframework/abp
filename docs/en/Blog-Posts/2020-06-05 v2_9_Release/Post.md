# ABP Framework v2.9 Has Been Released

The **ABP Framework** & and the **ABP Commercial** version 2.9 have been released, which is the last version before 3.0! This post will cover **what's new** with these this release and what you can expect from the version 3.0.

## What's New with the ABP Framework 2.9?

You can see all the changes on the [GitHub release notes](https://github.com/abpframework/abp/releases/tag/2.9.0). This post will only cover the important features/changes.

### Pre-Compiling Razor Pages

Pre-built pages (for [the application modules](https://docs.abp.io/en/abp/latest/Modules/Index)) and view components were compiling on runtime until this version. Now, they are pre-compiled and we've measured that the application startup time (for the MVC UI) has been 50% reduced. In other words, it has **two-times faster** than the previous version. Not only for the application startup, the speed change also effects when you visit a page for the first time.

You do nothing to get the benefit of the new system. [Overriding UI pages/components](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Customization-User-Interface) are working just like before.

### New Blob Storing Package

We've