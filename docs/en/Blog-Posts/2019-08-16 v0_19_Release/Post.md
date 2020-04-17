# ABP v0.19 Release With New Angular UI

ABP v0.19 has been released with [90 issues](https://github.com/abpframework/abp/milestone/17?closed=1) resolved and [650+ commits](https://github.com/abpframework/abp/compare/0.18.1...0.19.0) pushed.

## New Features

### Angular UI

Finally, ABP has a **SPA UI** option with the latest [Angular](https://angular.io/) framework. Angular integration was not simply creating a startup template.

* Created a base infrastructure to handle ABP's modularity, theming and some other features. This infrastructure has been deployed as [NPM packages](https://github.com/abpframework/abp/tree/dev/npm/ng-packs/packages).
* Created Angular UI packages for the modules like account, identity and tenant-management.
* Created a minimal startup template that authenticates using IdentityServer and uses the ASP.NET Core backend. This template uses the packages mentioned above.
* Worked on the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) and the [download page](https://abp.io/get-started) to be able to generate projects with the new UI option.
* Created a [tutorial](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=NG) to jump start with the new UI option.

We've created the template, document and infrastructure based on the latest Angular tools and trends:

* Uses [NgBootstrap](https://ng-bootstrap.github.io/) as the UI component library. You can use your favorite library, but pre-built modules work with these libraries.
* Uses [NGXS](https://ngxs.gitbook.io/ngxs/) as the state management library.

Angular was the first SPA UI option, but it is not the last. After v1.0 release, we will start to work on a second UI option. Not decided yet, but candidates are Blazor, React and Vue.js. Waiting your feedback. You can thumb up using the following issues:

* [Blazor](https://github.com/abpframework/abp/issues/394)
* [Vue.js](https://github.com/abpframework/abp/issues/1168)
* [React](https://github.com/abpframework/abp/issues/1638)

### Widget System

[Widget system](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Widgets) allows to **define and reuse** widgets for ASP.NET Core MVC applications. Widgets may have their own script and style resources and dependencies to 3rd-party libraries which are managed by the ABP framework.

### Others

We've solved many bugs and worked on existing features based on the community feedback. See the [v0.19 milestone](https://github.com/abpframework/abp/milestone/17?closed=1) for all the closed issues.

## Road Map

We had decided to wait for **ASP.NET Core 3.0** final release. Microsoft has announced to released it at [.NET Conf](https://www.dotnetconf.net/), between 23-25 September.

We have planned to finalize our work and move to ASP.NET Core 3.0 (with preview or RC) before its release. Once Microsoft releases it, we will immediately start to upgrade and test with the final release.

So, you can expect ABP **v1.0** to be released in the **first half of the October**. We are very excited and working hard on it.

You can follow the progress from [the GitHub milestones](https://github.com/abpframework/abp/milestones).

We will not add major features until v1.0.