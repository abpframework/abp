# CMS Kit: Tag Management

CMS Kit provides a **tag** system to tag any kind of resources, like a blog post.

## Enabling the Tag Management Feature

By default, CMS Kit features are disabled. Therefore, you need to enable the features you want, before starting to use it. You can use the [Global Feature](../../framework/infrastructure/global-features.md) system to enable/disable CMS Kit features on development time. Alternatively, you can use the ABP's [Feature System](../../framework/infrastructure/features.md) to disable a CMS Kit feature on runtime.

> Check the ["How to Install" section of the CMS Kit Module documentation](index.md#how-to-install) to see how to enable/disable CMS Kit features on development time.

## Options

The tag system provides a mechanism to group tags by entity types. For example, if you want to use the tag system for blog posts and products, you need to define two entity types named `BlogPosts` and `Product` and add tags under these entity types.

`CmsKitTagOptions` can be configured in the domain layer, in the `ConfigureServices` method of your [module](../../framework/architecture/modularity/basics.md) class.

**Example: Adding tagging support for products**

```csharp
Configure<CmsKitTagOptions>(options =>
{
    options.EntityTypes.Add(new TagEntityTypeDefiniton("Product"));
});
```

> If you're using the [Blogging Feature](./blogging.md), the ABP defines an entity type for the blog feature automatically.

`CmsKitTagOptions` properties:

- `EntityTypes`: List of defined entity types(`TagEntityTypeDefiniton`) in the tag system. 

`TagEntityTypeDefiniton` properties:

- `EntityType`: Name of the entity type.
- `DisplayName`: The display name of the entity type. You can use a user-friendly display name to show entity type definition on the admin website.
- `CreatePolicies`: List of policy/permission names allowing users to create tags under the entity type.
- `UpdatePolicies`: List of policy/permission names allowing users to update tags under the entity type.
- `DeletePolicies`: List of policy/permission names allowing users to delete tags under the entity type.

## The Tag Widget

The tag system provides a tag [widget](../../framework/ui/mvc-razor-pages/widgets.md) to display associated tags of a resource that was configured for tagging. You can simply place the widget on a page like the one below:

```csharp
@await Component.InvokeAsync(typeof(TagViewComponent), new
{
  entityType = "Product",
  entityId = "...",
  urlFormat = "/products?tagId={TagId}&tagName={TagName}"
})
```

`entityType` was explained in the previous section. In this example, the `entityId` should be the unique id of the product. If you have a `Product` entity, you can use its Id here. `urlFormat` is the string format of the URL which will be generated for each tag. You can use the `{TagId}` and `{TagName}` placeholders to populate the URL. For example, the above URL format will populate URLs like `/products?tagId=1&tagName=tag1`.

## The Popular Tags Widget

The tag system provides a popular tags [widget](../../framework/ui/mvc-razor-pages/widgets.md) to display popular tags of a resource that was configured for tagging. You can simply place the widget on a page as below:

```csharp
@await Component.InvokeAsync(typeof(PopularTagsViewComponent), new
{
  entityType = "Product",
  urlFormat = "/products?tagId={TagId}&tagName={TagName}",
  maxCount = 10
})
```

`entityType` was explained in the previous section. `urlFormat` was explained in the previous section. `maxCount` is the maximum number of tags to be displayed.

## User Interface

### Menu Items

The following menu items are added by the tagging feature to the admin application:

* **Tags**: Opens the tag management page.

### Pages

#### Tag Management

This page can be used to create, edit and delete tags for the entity types.

![tags-page](../../images/cmskit-module-tags-page.png)

You can create or edit an existing tag on this page.

![tag-edit](../../images/cmskit-module-tag-edit.png)

## Internals

### Domain Layer

#### Aggregates

This module follows the [Entity Best Practices & Conventions](../../framework/architecture/best-practices/entities.md) guide.

##### Tag

A tag represents a tag under the entity type.

- `Tag` (aggregate root): Represents a tag in the system.

##### EntityTag

An entity tag represents a connection between the tag and the tagged entity.

- `EntityTag`(entity): Represents a connection between the tag and the tagged entity.

#### Repositories

This module follows the [Repository Best Practices & Conventions](../../framework/architecture/best-practices/repositories.md) guide.

The following custom repositories are defined for this feature:

- `ITagRepository`
- `IEntityTagRepository`

#### Domain services

This module follows the [Domain Services Best Practices & Conventions](../../framework/architecture/best-practices/domain-services.md) guide.

##### Tag Manager

`TagManager` performs some operations for the `Tag` aggregate root.

##### Entity Tag Manager

`EntityTagManager` performs some operations for the `EntityTag` entity.

### Application layer

#### Application services

- `TagAdminAppService` (implements `ITagAdminAppService`).
- `EntityTagAdminAppService` (implements `IEntityTagAdminAppService`).
- `TagAppService` (implements `ITagAppService`).

### Database providers

#### Common

##### Table / Collection prefix & schema

All tables/collections use the `Cms` prefix by default. Set static properties on the `CmsKitDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection string

This module uses `CmsKit` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string.

See the [connection strings](../../framework/fundamentals/connection-strings.md) documentation for details.

#### Entity Framework Core

##### Tables

- CmsTags
- CmsEntityTags

#### MongoDB

##### Collections

- **CmsTags**
- **CmsEntityTags**
