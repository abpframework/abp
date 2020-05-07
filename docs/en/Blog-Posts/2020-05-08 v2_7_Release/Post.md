# ABP Framework v2.7.0 Has Been Released!

The **ABP Framework** & and the **ABP Commercial** v2.7 have been released. We hadn't created blog post for   the 2.4, 2.4 and 2.6 releases, so this post will also cover **what's new** with these releases and **what we've done** in the last 2 months.

## About the Release Cycle & Development

Reminding that we had started to release a new minor feature version **in every two weeks**, generally on Thursdays. Our goal is to deliver new features as soon as possible.

We've completed & merged hundreds of issues and pull requests with **1,300+ commits** in the last 7-8 weeks, only for the ABP Framework repository. Daily commit counts are constantly increasing:

![github-contribution-graph](github-contribution-graph.png)

ABP.IO Platform is rapidly growing and we are getting more and more contributions from the community.

## What's New?

In the last few releases, we've mostly focused on providing ways to extend existing modules when you use them as NuGet/NPM Packages. We've also added some useful features and some helpful guides.

### Object Extending System

The Object Extending System allows module developers to create extensible modules and allows application developers to customize and extend a module easily.

For example, you can add two extension properties to the user entity of the identity module:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdate<IdentityUser>(options =>
        {
            options.AddOrUpdateProperty<string>("SocialSecurityNumber");
            options.AddOrUpdateProperty<bool>("IsSuperUser");
        }
    );
````

It is easy to define validation rules for the properties:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUserCreateDto, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.Attributes.Add(new RequiredAttribute());
            options.Attributes.Add(
                new StringLengthAttribute(32) {
                    MinimumLength = 6 
                }
            );
        });
````

You can even write custom code to validate the property. It automatically works for the objects those are parameters of an application service, controller or a page.

While extension properties of an entity are normally stored in a single JSON formatted field in the database table, you can easily configure to store a property as a table field using the EF Core mapping:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.MapEfCore(b => b.HasMaxLength(32));
        }
    );
````

See the [Object Extensions document](https://docs.abp.io/en/abp/latest/Object-Extensions) for details about this system.

### Guide: Customizing the Existing Modules

[Customizing the Existing Modules guide](https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Guide) explains all the ways of extending/customizing a reusable module including [entities](https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities), [services](https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Overriding-Services) and the [user interface](https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Overriding-User-Interface).

### Guide: EF Core Database Migrations

TODO