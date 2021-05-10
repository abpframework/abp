# Rating System

CMS kit provides a **rating** system to to add ratings feature to any kind of resource like blog posts, comments, etc. This section describes the details of the rating system. 

## Feature

CMS kit uses the [global feature](https://docs.abp.io/en/abp/latest/Global-Features) system for all implemented features. Commercial startup templates come with all the CMS kit related features are enabled by default, if you create the solution with the public website option. If you're installing the CMS kit manually or want to enable the rating feature individually, open the `GlobalFeatureConfigurator` class in the `Domain.Shared` project and place the following code to the `Configure ` method.

```csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.Ratings.Enable();
});
```

# Options

## CmsKitRatingOptions

You can use the rating system to to add ratings to any kind of resource, like blog posts, comments, etc. The rating system provides a mechanism to group ratings by entity types. For example, if you want to use the rating system for products, you need to define an entity type named `Product` and then add ratings under the defined entity type.

`CmsKitRatingOptions` can be configured in the domain layer, in the `ConfigureServices` method of your [module](https://docs.abp.io/en/abp/latest/Module-Development-Basics). Example:

```csharp
Configure<CmsKitRatingOptions>(options =>
{
    options.EntityTypes.Add(new RatingEntityTypeDefinition("Product"));
});
```

> If you're using the blog feature, the ABP framework defines an entity type for the blog feature automatically. You can easily override or remove the predefined entity types in `Configure` method like shown above.

`CmsKitRatingOptions` properties:

- `EntityTypes`: List of defined entity types(`RatingEntityTypeDefinition`) in the rating system.

`RatingEntityTypeDefinition` properties:

- `EntityType`: Name of the entity type.

# Internals

## Domain Layer

#### Aggregates

This module follows the [Entity Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Entities) guide.

##### Rating

A rating represents a given rating from a user.

- `Rating` (aggregate root): Represents a given rating in the system.

#### Repositories

This module follows the [Repository Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories) guide.

Following custom repositories are defined for this feature:

- `IRatingRepository`

#### Domain services

This module follows the [Domain Services Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services) guide.

##### Reaction Manager

`RatingManager` is used to perform some operations for the `Rating` aggregate root.

### Application layer

#### Application services

- `RatingPublicAppService` (implements `IRatingPublicAppService`): Implements the use cases of rating system.

### Database providers

#### Common

##### Table / collection prefix & schema

All tables/collections use the `Cms` prefix by default. Set static properties on the `CmsKitDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection string

This module uses `CmsKit` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string.

See the [connection strings](https://docs.abp.io/en/abp/latest/Connection-Strings) documentation for details.

#### Entity Framework Core

##### Tables

- CmsRatings

#### MongoDB

##### Collections

- **CmsRatings**

### MVC UI

The ratings system provides a rating widget to allow users send ratings to resources in public websites. You can simply place the widget on a page like below. 

```csharp
@await Component.InvokeAsync(typeof(RatingViewComponent), new
{
  entityType = "entityType",
  entityId = "entityId"
})
```
You need to specify entity type and entity ID parameters. Entity type represents the group name you specified while defining the reactions in the domain module. Entity ID represents the unique identifier for the resource that users give ratings to, such as blog post ID or product ID.

For more information about widgets see [widgets](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Widgets) documentation.