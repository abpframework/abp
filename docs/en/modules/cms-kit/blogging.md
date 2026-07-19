```json
//[doc-seo]
{
    "Description": "Discover how to enable and manage the blogging feature in the CMS Kit, including UI elements for blogs and blog posts."
}
```

# CMS Kit: Blogging

The blogging feature provides the necessary UI to manage and render blogs and blog posts.

## Enabling the Blogging Feature

By default, CMS Kit features are disabled. Therefore, you need to enable the features you want, before starting to use it. You can use the [Global Feature](../../framework/infrastructure/global-features.md) system to enable/disable CMS Kit features on development time. Alternatively, you can use the ABP's [Feature System](../../framework/infrastructure/features.md) to disable a CMS Kit feature on runtime.

> Check the ["How to Install" section of the CMS Kit Module documentation](index.md#how-to-install) to see how to enable/disable CMS Kit features on development time.

> The built-in MVC blog routes are registered by the Pages global feature. Enable both `Blogs` and `Pages` when you use the built-in public blog pages.

## User Interface

### Menu Items

The following menu items are added by the blogging feature to the admin application:

* **Blogs**: Blog management page.
* **Blog Posts**: Blog post management page.

## Pages

### Blogs

Blogs page is used to create and manage blogs in your system. 

![blogs-page](../../images/cmskit-module-blogs-page.png)

A screenshot from the new blog creation modal:

![blogs-edit](../../images/cmskit-module-blogs-edit.png)

**Slug** is the URL part of the blog. For this example, the root URL of the blog becomes `your-domain.com/blogs/technical-blog/`.

- You can change the default route prefix by using the `CmsBlogsWebConsts.BlogsRoutePrefix` property. For example, if you set it to `foo`, the root URL of the blog becomes `your-domain.com/foo/technical-blog/`.

    ```csharp
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        CmsBlogsWebConsts.BlogsRoutePrefix = "foo";
    }
    ```

#### Blog Features

The blogging feature uses other CMS Kit features. A newly created blog enables comments, reactions, ratings, tags, marked items, the quick navigation bar and XSS prevention by default when the related global features are available. You can enable or disable these features for each blog by clicking the features action.

![blogs-feature-action](../../images/cmskit-module-blogs-feature-action.png)

You can select/deselect the desired features for blog posts. 

![features-dialog](../../images/cmskit-module-features-dialog-2.png)

##### Quick Navigation Bar In Blog Post
If you enable **Quick navigation bar in blog posts**, the public blog post page builds a scroll index from the post headings.

![scroll-index](../../images/cmskit-module-features-scroll-index.png)

### Blog Post Management

When you create blogs, you can manage blog posts on this page.

![blog-posts-page](../../images/cmskit-module-blog-posts-page.png)

You can create and edit an existing blog post on this page. If you enable specific features such as tags, you can set tags for the blog post on this page.

![blog-post-edit](../../images/cmskit-module-blog-post-edit.png)

Blog posts have three statuses: `Draft`, `WaitingForReview` and `Published`. Creating, updating, deleting and publishing posts use separate permissions. The public blog list requests only published posts and can filter them by author, tag or the current user's marked items.

The built-in renderer allows HTML in blog post Markdown. The per-blog **Prevent XSS** feature controls whether the Markdown renderer sanitizes that HTML and is enabled for newly created blogs. Keep it enabled when post authors are not trusted to submit arbitrary HTML.

## Internals

### Domain Layer

#### Aggregates

This module follows the [Entity Best Practices & Conventions](../../framework/architecture/best-practices/entities.md) guide.

- `Blog` _(aggregate root)_: Presents blogs of application.
- `BlogPost`_(aggregate root)_: Presents blog posts in blogs.
- `BlogFeature`:_(aggregate root)_: Presents blog features enabled/disabled state. Such as reactions, ratings, comments, etc.

#### Repositories

This module follows the [Repository Best Practices & Conventions](../../framework/architecture/domain-driven-design/repositories.md) guide. The following repositories are defined for this feature:

- `IBlogRepository`
- `IBlogPostRepository`
- `IBlogFeatureRepository`

#### Domain services

This module follows the [Domain Services Best Practices & Conventions](../../framework/architecture/domain-driven-design/domain-services.md) guide.

- `BlogManager`
- `BlogPostManager`
- `BlogFeatureManager`

### Application layer

#### Application Services

##### Common

- `BlogFeatureAppService` _(implements `IBlogFeatureAppService`)_

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

## Entity Extensions

Check the ["Entity Extensions" section of the CMS Kit Module documentation](index.md#entity-extensions) to see how to extend entities of the Blogging Feature of the CMS Kit module.
