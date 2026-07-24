```json
//[doc-seo]
{
    "Description": "Learn how to install and configure the standalone ABP Blogging module, including its MVC UI, routes, permissions, files and database providers."
}
```

# Blogging Module

The Blogging module is a free and open-source application module for creating one or more blogs. It provides an MVC / Razor Pages user interface, application services, HTTP APIs and Entity Framework Core and MongoDB integrations for blogs, posts, tags, comments and member profiles.

> This page documents the standalone `Volo.Blogging` module. [CMS Kit: Blogging](cms-kit/blogging.md) is a different feature family with different entities, configuration and UI.

## How to Install

Use the ABP CLI to add the module to an existing solution:

```bash
abp add-module Volo.Blogging
```

The command adds the module packages and dependencies to the compatible projects in your solution. Apply the generated database migration after adding the module.

### The Source Code

The source code of this module is available in the [ABP repository](https://github.com/abpframework/abp/tree/dev/modules/blogging). It is licensed with [MIT](https://choosealicense.com/licenses/mit/), so you can use and customize it.

## User Interface and Content Workflow

The module provides two MVC / Razor Pages surfaces:

* The public site lists blogs, posts and popular tags, renders post content as Markdown, displays member profiles and allows authenticated users to add comments.
* The administration page manages blogs. Post creation and editing are available from the public blog UI to users with the corresponding permissions.

When the application has one blog, the blog index redirects directly to that blog. With multiple blogs, the index displays the available blogs.

Creating a post makes it available to the public list and reading APIs immediately. The standalone module does not add a draft, review or scheduled-publication state. If a post URL is already used in the same blog, the application service appends a generated suffix and returns the resulting URL.

Tags are entered with a post. The application service normalizes tag names to lowercase, removes duplicates and maintains their usage counts.

### Member Profiles

The module keeps a local `BlogUser` record for post and comment authors. It uses ABP's [user lookup and synchronization](identity/user-synchronization.md) infrastructure to obtain Identity user data. The public member page lists an active user's posts and profile information; the current user can edit the Blogging-specific fields on their own profile.

## Permissions

The administration menu is visible with the `Blogging.Blog.Management` permission. Blog operations use separate child permissions:

* `Blogging.Blog.Create`
* `Blogging.Blog.Update`
* `Blogging.Blog.Delete`
* `Blogging.Blog.ClearCache`

Post creation, update and deletion require `Blogging.Post.Create`, `Blogging.Post.Update` and `Blogging.Post.Delete`, respectively.

Any authenticated user can create a comment. A comment can be updated or deleted by its creator or by a user with `Blogging.Comment.Update` or `Blogging.Comment.Delete`.

See the [Authorization](../framework/fundamentals/authorization/index.md) documentation to learn how to grant permissions to roles and users.

## Routing

`BloggingUrlOptions.RoutePrefix` controls the public URL prefix. Its default value produces URLs under `/blog/`. The following example moves the public blog under `/articles/` and enables single-blog mode for the blog whose short name is `engineering`:

```csharp
Configure<BloggingUrlOptions>(options =>
{
    options.RoutePrefix = "articles";
    options.SingleBlogMode.Enabled = true;
    options.SingleBlogMode.BlogName = "engineering";
});
```

Single-blog mode removes the blog short-name segment from post URLs. If the application contains multiple blogs, set `SingleBlogMode.BlogName` to the short name of the blog to expose. If it contains exactly one blog, the module can select it without setting `BlogName`.

Set `RoutePrefix` to an empty string to serve the blog from `/`. In this mode the route constraint uses `IgnoredPaths` to avoid capturing other top-level application routes. The module already adds its framework endpoints, bundle folder and member route; add application-specific top-level paths when needed:

```csharp
Configure<BloggingUrlOptions>(options =>
{
    options.RoutePrefix = "";
    options.IgnoredPaths.Add("health");
});
```

## Post Images

Post images are saved through the [BLOB Storing](../framework/infrastructure/blob-storing) system in the `blogging-files` container. Configure a BLOB provider for this container as you would for any other typed container.

The upload service accepts JPEG, PNG, GIF and BMP images. The maximum file size is 5 MiB by default. `BloggingWebConsts.FileUploading.MaxFileSize` is a process-wide static value, so set it once during application startup, before the application begins accepting uploads:

```csharp
BloggingWebConsts.FileUploading.MaxFileSize = 10 * 1024 * 1024;
```

## Social Media Metadata

The post page emits Twitter card metadata. Configure the site handle with `BloggingTwitterOptions`:

```csharp
Configure<BloggingTwitterOptions>(options =>
{
    options.Site = "@myblog";
});
```

## Post List Cache

The time-ordered post list is cached per blog for one hour. Creating, updating or deleting posts through `IPostAppService` invalidates that cache within the current unit of work. The blog administration page also provides a **Clear Cache** action to users with the `Blogging.Blog.ClearCache` permission.

If custom code changes posts directly through `IPostRepository`, publish the module's `PostChangedEvent` with the affected blog ID. The built-in event handler then removes that blog's cached list as part of the current unit of work.

## Internals

### Domain Layer

The main aggregates are:

* `Blog`: A named blog identified in public URLs by its short name.
* `Post`: Markdown content, cover image, URL, tags and read count for a blog.
* `Comment`: A comment or direct reply attached to a post.
* `Tag`: A normalized tag and its usage count within a blog.
* `BlogUser`: The module-local representation of an Identity user and Blogging profile.

### Application Layer

The public application-service contracts are `IBlogAppService`, `IPostAppService`, `ICommentAppService`, `ITagAppService` and `IFileAppService`. `IBlogManagementAppService` provides blog administration operations.

The HTTP API uses the `Blogging` remote-service name for public operations and `BloggingAdmin` for administration operations. Add `BloggingHttpApiClientModule` or `BloggingAdminHttpApiClientModule` to a client application when these services run remotely.

### Database Providers

#### Common

The module uses `Blogging` as its connection string name and falls back to `Default` when that connection is not configured. See the [Connection Strings](../framework/fundamentals/connection-strings.md) documentation.

Tables and collections use the `Blg` prefix by default. Set `AbpBloggingDbProperties.DbTablePrefix` and, for providers that support schemas, `AbpBloggingDbProperties.DbSchema` before the database model is created:

```csharp
AbpBloggingDbProperties.DbTablePrefix = "MyBlog";
AbpBloggingDbProperties.DbSchema = "blogging";
```

#### Entity Framework Core

The Entity Framework Core provider uses these tables:

* `BlgUsers`
* `BlgBlogs`
* `BlgPosts`
* `BlgComments`
* `BlgTags`
* `BlgPostTags`

Call `ConfigureBlogging()` from your migration DbContext when integrating the module manually.

#### MongoDB

The MongoDB provider uses these collections:

* `BlgUsers`
* `BlgBlogs`
* `BlgPosts`
* `BlgComments`
* `BlgTags`

Post-tag links are stored with the post documents, so MongoDB does not use a separate `BlgPostTags` collection.

## See Also

* [CMS Kit: Blogging](cms-kit/blogging.md)
* [BLOB Storing](../framework/infrastructure/blob-storing)
* [User Lookup and Synchronization](identity/user-synchronization.md)
