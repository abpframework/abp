# Rating System

CMS kit provides a **rating** system to to add ratings feature to any kind of resource like blog posts, comments, etc. Here how the rating component looks like on a sample page:

![ratings](../../images/cmskit-module-ratings.png)

## Options

The rating system provides a mechanism to group ratings by entity types. For example, if you want to use the rating system for products, you need to define an entity type named `Product` and then add ratings under the defined entity type.

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

## MVC UI

The ratings system provides a rating widget to allow users send ratings to resources in public websites. You can simply place the widget on a page like below. 

```csharp
@await Component.InvokeAsync(typeof(RatingViewComponent), new
{
  entityType = "Product",
  entityId = "entityId"
})
```

`entityType` was explained in the previous section. `entityId` should be the unique id of the product, in this example. If you have a Product entity, you can use its Id here.

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
