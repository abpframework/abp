# Tag Management

CMS kit provides a **tag** system to tag any kind of resources, like blog posts. This section describes the details of the tag system. 

## Feature

CMS kit uses the [global feature](https://docs.abp.io/en/abp/latest/Global-Features) system for all implemented features. Commercial startup templates come with all the CMS kit related features are enabled by default, if you create the solution with public website option. If you're installing the CMS kit manually or want to enable the tag management feature individually, open the `GlobalFeatureConfigurator` class in the `Domain.Shared` project and place the following code to the `Configure ` method.

```csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.Tags.Enable();
});
```

# Options

## CmsKitTagOptions

You can use the tag system to tag any kind of resources, like blog posts, products, etc. The tag system provides a mechanism to group tags by entity types. For example, if you want to use the tag system for blog posts and products, you need to define two entity types named `BlogPosts` and `Product` and add tags under these entity types.

`CmsKitTagOptions` can be configured in the domain layer, in the `ConfigureServices` method of your [module](https://docs.abp.io/en/abp/latest/Module-Development-Basics). Example:

```csharp
Configure<CmsKitTagOptions>(options =>
{
    options.EntityTypes.Add(new TagEntityTypeDefiniton("Product"));
});
```

> If you're using the blog feature, the ABP framework defines an entity type for the blog feature automatically. You can easily override or remove the predefined entity types in `Configure` method like shown above.

`CmsKitTagOptions` properties:

- `EntityTypes`: List of defined entity types(`TagEntityTypeDefiniton`) in the tag system. 

`TagEntityTypeDefiniton` properties:

- `EntityType`: Name of the entity type.
- `DisplayName`: Display name of the entity type. You can use a user friendly display name to show entity type definition on the admin website.
- `CreatePolicies`: List of policy names allowing users to create tags under the entity type.
- `UpdatePolicies`: List of policy names allowing users to update tags under the entity type.
- `DeletePolicies`: List of policy names allowing users to delete tags under the entity type.

# Internals

## Domain Layer

#### Aggregates

This module follows the [Entity Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Entities) guide.

##### Tag

A tag represents a tag under the entity type.

- `Tag` (aggregate root): Represents a tag in the system.

##### EntityTag

An entity tag represents a connection between the tag and the tagged entity.

- `EntityTag`(entity): Represents a connection between the tag and the tagged entity.

#### Repositories

This module follows the [Repository Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories) guide.

Following custom repositories are defined for this feature:

- `ITagRepository`
- `IEntityTagRepository`

#### Domain services

This module follows the [Domain Services Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services) guide.

##### Tag Manager

`TagManager` is used to perform some operations for the `Tag` aggregate root.

##### Entity Tag Manager

`EntityTagManager` is used to perform some operations for the `EntityTag` entity.

### Application layer

#### Application services

- `TagAdminAppService` (implements `ITagAdminAppService`): Implements the use cases of tag management.
- `EntityTagAdminAppService` (implements `IEntityTagAdminAppService`): Implements the use cases of entity tag relationship.
- `TagAppService` (implements `ITagAppService`): Implements the use cases of entity tag relationship for public websites.

### Database providers

#### Common

##### Table / collection prefix & schema

All tables/collections use the `Cms` prefix by default. Set static properties on the `CmsKitDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection string

This module uses `CmsKit` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string.

See the [connection strings](https://docs.abp.io/en/abp/latest/Connection-Strings) documentation for details.

#### Entity Framework Core

##### Tables

- CmsTags
- CmsEntityTags
  - CmsTags

#### MongoDB

##### Collections

- **CmsTags**
- **CmsEntityTags**

### MVC UI

The tag system provides a tag widget to display associated tags of the resource on public websites. You can simply place the widget on a page like below. 

```csharp
@await Component.InvokeAsync(typeof(TagViewComponent), new
{
  entityType = "entityType",
  entityId = "entityId"
})
```

For more information about widgets see [widgets](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Widgets) documentation.