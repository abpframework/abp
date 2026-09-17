```json
//[doc-seo]
{
    "Description": "Enhance your application with the CMS Kit Module (Pro) for advanced content management, including dynamic pages, blogging, and user interactions."
}
```

# CMS Kit Module (Pro)

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

This module extends the [open-source CMS Kit module](../cms-kit) and adds additional CMS (Content Management System) capabilities to your application.

The administration UI is available for MVC / Razor Pages, Angular and Blazor (Blazorise and MudBlazor). The public website widgets documented in the feature pages are MVC / Razor Pages components.

The following features are provided by the open-source CMS Kit module:

- [**Page**](../cms-kit/pages.md) management system to manage dynamic pages with dynamic URLs.
- [**Blogging**](../cms-kit/blogging.md) system to create and publish blog posts with multiple blog support.
- [**Tagging**](../cms-kit/tags.md) system to tag any kind of resource, like a blog post.
- [**Comment**](../cms-kit/comments.md) system to add comments feature to any kind of resource, like a blog post or a product review page.
- [**Reaction**](../cms-kit/reactions.md) system to add reactions (smileys) feature to any kind of resource, like a blog post or a comment.
- [**Rating**](../cms-kit/ratings.md) system to add a rating feature to any kind of resource.
- [**Menu**](../cms-kit/menus.md) system to manage public menus dynamically.
- [**Global resources**](../cms-kit/global-resources.md) system to add global styles and scripts dynamically.
- [**Dynamic widget**](../cms-kit/dynamic-widget.md) system to create dynamic widgets for page and blog posts.
- [**Marked Item**](../cms-kit/marked-items.md) system to flag favorites or important items with icons or emojis.

The following features are provided by the CMS Kit Pro version:

* [**Newsletter**](newsletter.md) It allows users to subscribe to newsletters.
* [**Contact form**](contact-form.md) It allows users to write messages to you.
* [**URL forwarding**](url-forwarding.md) It allows the creation of URLs that point to other pages or external websites.
* [**Poll**](poll.md) Allows you to create simple polls for your visitors.
* [**Page Feedback**](page-feedback.md) It allows users to send feedback for your pages.
* [**FAQ**](faq.md) system to create dynamic FAQs.

Click on a feature to understand and learn how to use it. See [the module description page](https://abp.io/modules/Volo.CmsKit.Pro) for an overview of the module features.

## How to Install

### New Solutions

CMS Kit Pro is pre-installed in [the startup templates](../../solution-templates) if you create the solution with the **public website** option. If you are using ABP CLI, specify the `--with-public-website` option as shown below:

```bash
abp new Acme.BookStore --with-public-website
```

### Existing Solutions

If you want to add CMS Kit Pro to your existing solution, you can use the ABP CLI `add-module` command:

```bash
abp add-module Volo.CmsKit.Pro
```

> **Important**: CMS Kit Pro requires both the Pro packages **and** the base CMS Kit packages. The `add-module` command handles this automatically, but if you're adding packages manually, you need both:
> - `Volo.CmsKit.*` packages (base CMS Kit)
> - `Volo.CmsKit.Pro.*` packages (Pro features)

Open the `GlobalFeatureConfigurator` class in the `Domain.Shared` project and place the following code to the `Configure` method to enable all open-source and commercial features in the CMS Kit module.

```csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.EnableAll();
});

GlobalFeatureManager.Instance.Modules.CmsKitPro(cmsKitPro =>
{
    cmsKitPro.EnableAll();
});
```

Alternatively, you can enable features individually, like `cmsKit.Comments.Enable();`.

> If you are using Entity Framework Core, remember to add a new migration and update your database.

### Angular Administration UI

The Angular package publishes the administration routes and their menu configuration in separate entry points. Register the configuration provider in your application configuration:

```typescript
import { ApplicationConfig } from '@angular/core';
import { provideCmsKitAdminConfig } from '@abp/ng.cms-kit/admin/config';
import { provideCmsKitProAdminConfig } from '@volo/abp.ng.cms-kit-pro/admin/config';

export const appConfig: ApplicationConfig = {
  providers: [provideCmsKitAdminConfig(), provideCmsKitProAdminConfig()],
};
```

Then combine the open-source and Pro administration routes under the `cms` path:

```typescript
import { Routes } from '@angular/router';

export const appRoutes: Routes = [
  {
    path: 'cms',
    loadChildren: () =>
      Promise.all([
        import('@volo/abp.ng.cms-kit-pro/admin').then(cmsKitPro =>
          cmsKitPro.createRoutes(),
        ),
        import('@abp/ng.cms-kit/admin').then(cmsKit => cmsKit.createRoutes()),
      ]).then(([cmsKitProRoutes, cmsKitRoutes]) => [
        ...cmsKitProRoutes,
        ...cmsKitRoutes,
      ]),
  },
];
```

## Entity Extensions

The [module entity extension](../../framework/architecture/modularity/extending/module-entity-extensions.md) system allows you to define new properties for supported entities of a dependent module from a single configuration point.

To extend entities of the CMS Kit Pro module, open your `YourProjectNameModuleExtensionConfigurator` class in the `Domain.Shared` project and change the `ConfigureExtraProperties` method as shown below.

```csharp
public static void ConfigureExtraProperties()
{
    OneTimeRunner.Run(() =>
    {
        ObjectExtensionManager.Instance.Modules()
            .ConfigureCmsKitPro(cmsKitPro =>
            {
                cmsKitPro.ConfigurePoll(poll =>
                {
                    poll.AddOrUpdateProperty<string>(
                        "PollDescription",
                        property =>
                        {
                            property.Attributes.Add(new RequiredAttribute());
                        }
                    );
                });

                cmsKitPro.ConfigureNewsletterRecord(newsletterRecord =>
                {
                    newsletterRecord.AddOrUpdateProperty<string>(
                        "NewsletterRecordDescription",
                        property =>
                        {
                            property.Attributes.Add(new RequiredAttribute());
                            property.Attributes.Add(
                                new StringLengthAttribute(MyConsts.MaximumDescriptionLength)
                                {
                                    MinimumLength = MyConsts.MinimumDescriptionLength
                                }
                            );
                        }
                    );
                });
            });
    });
}
```

* `ConfigureCmsKitPro` method is used to configure the entities of the CMS Kit Pro module.

* `cmsKitPro.ConfigurePoll(...)` is used to configure the **Poll** entity of the CMS Kit Pro module. You can add or update the extra properties of the **Poll** entity.

* `cmsKitPro.ConfigureNewsletterRecord(...)` is used to configure the **NewsletterRecord** entity of the CMS Kit Pro module. You can add or update the extra properties of the **NewsletterRecord** entity.

* You can also set validation rules for the properties you define. In the example above, `RequiredAttribute` and `StringLengthAttribute` are added to the **NewsletterRecordDescription** property.

* Extra properties are added to the entity and HTTP API. UI integration is entity- and UI-specific: Poll has create and update forms, while Newsletter records are read-only and expose their extra properties through the application DTOs.
