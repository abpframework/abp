# CMS Kit Module (Pro)

> You must have an ABP Team or a higher license to use this module.

This module extends the [open-source CMS Kit module](../cms-kit) and adds additional CMS (Content Management System) capabilities to your application.

> **This module is currently available for MVC / Razor Pages and Blazor UIs**.

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
* [**URL forwarding**](URL-forwarding.md) It allows the creation of URLs that point to other pages or external websites.
* [**Poll**](poll.md) It allows to create simple polls for your visitors.
* [**Page Feedback**](page-feedback.md) It allows users to send feedback for your pages.
* [**Faq**](faq.md) system to create dynamic FAQ.

Click on a feature to understand and learn how to use it. See [the module description page](https://abp.io/modules/Volo.CmsKit.Pro) for an overview of the module features.

## How to Install

### New Solutions

CMS Kit Pro is pre-installed in [the startup templates](../../solution-templates) if you create the solution with the **public website** option. If you are using ABP CLI, you should specify the the `--with-public-website` option as shown below:

```bash
abp new Acme.BookStore --with-public-website
```

### Existing Solutions

If you want to add the CMS kit to your existing solution, you can use the ABP CLI `add-module` command:

```bash
abp add-module Volo.CmsKit.Pro
```
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

## Entity Extensions

[Module entity extension](../../framework/architecture/modularity/extending/module-entity-extensions.md) system is a **high-level** extension system that allows you to **define new properties** for existing entities of the dependent modules. It automatically **adds properties to the entity**, **database**, **HTTP API and user interface** in a single point.

To extend entities of the CMS Kit Pro module, open your `YourProjectNameModuleExtensionConfigurator` class inside of your `DomainShared` project and change the `ConfigureExtraProperties` method like shown below.

```csharp
public static void ConfigureExtraProperties()
{
    OneTimeRunner.Run(() =>
    {
        ObjectExtensionManager.Instance.Modules()
            .ConfigureCmsKitPro(cmsKitPro =>
            {
                cmsKitPro.ConfigurePoll(plan => // extend the Poll entity
                {
                    plan.AddOrUpdateProperty<string>( //property type: string
                      "PollDescription", //property name
                      property => {
                        //validation rules
                        property.Attributes.Add(new RequiredAttribute()); //adds required attribute to the defined property

                        //...other configurations for this property
                      }
                    );
                }); 

                cmsKitPro.ConfigureNewsletterRecord(newsletterRecord => // extend the NewsletterRecord entity
                {
                    newsletterRecord.AddOrUpdateProperty<string>( //property type: string
                      "NewsletterRecordDescription", //property name
                      property => {
                        //validation rules
                        property.Attributes.Add(new RequiredAttribute()); //adds required attribute to the defined property
                        property.Attributes.Add(
                          new StringLengthAttribute(MyConsts.MaximumDescriptionLength) {
                            MinimumLength = MyConsts.MinimumDescriptionLength
                          }
                        );

                        //...other configurations for this property
                      }
                    );
                });     
            });
    });
}
```
 
* `ConfigureCmsKitPro` method is used to configure the entities of the CMS Kit Pro module.

* `cmsKit.ConfigurePoll(...)` is used to configure the **Poll** entity of the CMS Kit Pro module. You can add or update the extra properties of the **Poll** entity. 

* `cmsKit.ConfigureNewsletterRecord(...)` is used to configure the **NewsletterRecord** entity of the CMS Kit Pro module. You can add or update the extra properties of the **NewsletterRecord** entity. 

* You can also set some validation rules for the property that you defined. In the above sample, `RequiredAttribute` and `StringLengthAttribute` were added for the property named **"NewsletterRecord"**. 

* When you define the new property, it will automatically add to **Entity**, **HTTP API** and **UI** for you. 
  * Once you define a property, it appears in the create and update forms of the related entity. 
  * New properties also appear in the data table on the related page.
