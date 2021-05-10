# Blog System

CMS kit provides a **blog** system to add blogs & posts to a public website. This section describes the details of the blog system.

## Feature

CMS kit uses the [global feature](https://docs.abp.io/en/abp/latest/Global-Features) system for all implemented features. Commercial startup templates come with all the CMS Kit-related features are enabled by default, if you create the solution with the public website option. If you're installing the CMS Kit manually or want to enable the blog feature individually, open the `GlobalFeatureConfigurator` class in the `Domain.Shared` project and place the following code to the `Configure `method.

```csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
	cmsKit.Blogs.Enable();
});
```

# Internals

## Domain Layer

#### Aggregates

This module follows the [Entity Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Entities) guide.

- `Blog` _(aggregate root)_: Presents blogs of application.
- `BlogPost`_(aggregate root)_: Presents blog posts in blogs.
- `BlogFeature`:_(aggregate root)_: Presents blog features enabled/disabled state. Such as reactions, ratings, comments, etc.

#### Repositories

This module follows the [Repository Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories) guide.

Following custom repositories are defined for this feature:

- `IBlogRepository`
- `IBlogPostRepository`
- `IBlogFeatureRepository`

#### Domain services

This module follows the [Domain Services Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services) guide.

- `BlogManager`: Includes some operations for `Blog` aggreate root to keep data consistency.
  - `CreateAsync`: Creates a new `Blog` entity and makes sure slug is used one time.
  - `UpdateAsync`: Updates existing `Blog` entity and makes sure slug is used one time.
- `BlogPostManager`: Includes some operations for `BlogPost` aggreagate root such as creating & updating.
  - `CreateAsync`: Creates a new BlogPost and makes sure slug is used only one time.
  - `SetSlugUrlAsync`: Sets `UrlSlug` property of BlogPost entity and makes sure slug isn't duplicated in same blog.
- `BlogFeatureManager`: Includes some operations for managing blog features.
  - `SetAsync`: Sets a feature enabled/disabled for a Blog.
  - `SetDefaultsAsync`: Resets all feature configuration for a Blog.

### Application layer

#### Application Services

##### Admin

- `BlogAdminAppService` _(implements `IBlogAdminAppService`)_: Implements use cases of blog management.
- `BlogFeatureAdminAppService` _(implements `IBlogFeatureAdminAppService`)_: Implements use cases of blog feature management.
- `BlogPostAdminAppService` _(implements `IBlogPostAdminAppService`)_: Implements use cases of blog post management.

##### Common

- `BlogFeatureAppService` _(implements I`BlogFeatureAppService`)_: Implements feature checking operations.

##### Public

- `BlogPostPublicAppService` _(implements `IBlogPostPublicAppService`)_: Implements use cases of blog posts management on public websites.

### Database providers

#### Common

##### Table / collection prefix & schema

All tables/collections use the `Cms` prefix by default. Set static properties on the `CmsKitDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection string

This module uses `CmsKit` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string.

See the [connection strings](https://docs.abp.io/en/abp/latest/Connection-Strings) documentation for details.

#### Entity Framework Core

##### Tables

- CmsBlogs
- CmsBlogPosts
- CmsBlogFeatures

#### MongoDB

##### Collections

- CmsBlogs
- CmsBlogPosts
- CmsBlogFeatures
