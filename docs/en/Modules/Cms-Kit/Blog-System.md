# CMS Kit: Blogging

The blogging feature provides the necessary UI to manage and render blogs and blog posts.

## Internals

Menu Items

The following menu items are added by the blogging feature to the admin application:

* **Blogs**: Blog management page.
* **Blog Posts**: Blog post management page.

### Domain Layer

#### Aggregates

This module follows the [Entity Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Entities) guide.

- `Blog` _(aggregate root)_: Presents blogs of application.
- `BlogPost`_(aggregate root)_: Presents blog posts in blogs.
- `BlogFeature`:_(aggregate root)_: Presents blog features enabled/disabled state. Such as reactions, ratings, comments, etc.

#### Repositories

This module follows the [Repository Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories) guide. The following repositories are defined for this feature:

- `IBlogRepository`
- `IBlogPostRepository`
- `IBlogFeatureRepository`

#### Domain services

This module follows the [Domain Services Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services) guide.

- `BlogManager`: Includes some operations for `Blog` aggregate root to keep data consistency.
- `BlogPostManager`: Includes some operations for `BlogPost` aggregate root such as creating & updating.
- `BlogFeatureManager`: Includes some operations for managing blog features.

### Application layer

#### Application Services

##### Common

- `BlogFeatureAppService` _(implements I`BlogFeatureAppService`)_

##### Admin

- `BlogAdminAppService` _(implements `IBlogAdminAppService`)_
- `BlogFeatureAdminAppService` _(implements `IBlogFeatureAdminAppService`)_
- `BlogPostAdminAppService` _(implements `IBlogPostAdminAppService`)_

##### Public

- `BlogPostPublicAppService` _(implements `IBlogPostPublicAppService`)_

### Database providers

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