## CmsUsers

CmsUsers Table is used to store users.

### Description

This table stores information about the users. Each record in the table represents a user and allows to manage and track the users effectively. This table is important for managing and tracking the users of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Index.md)

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [CmsBlogPosts](#cmsblogposts) | AuthorId | To match the author. |

---

## CmsBlogs

CmsBlogs table is used to store blogs.

### Description

This table stores information about the blogs. Each record in the table represents a blog and allows to manage and track the blogs effectively. This table is important for managing and tracking the blogs of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Blogging.md)

---

## CmsBlogPosts

CmsBlogPosts table is used to store blog posts.

### Description

This table stores information about the blog posts. Each record in the table represents a blog post and allows to manage and track the blog posts effectively. This table is important for managing and tracking the blog posts of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Blogging.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [CmsUsers](#cmsusers) | Id | To match the author. |

---

## CmsBlogFeatures

The CmsBlogFeatures table is used to store blog features.

### Description

This table stores information about the blog features. Each record in the table represents a blog feature and allows to manage and track the blog features effectively. This table is important for managing and tracking the blog features of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Blogging.md)

---

## CmsComments

The CmsComments table is used to store comments for CMS pages. The table is used by the CmsComments component.

### Description

This table stores information about the comments. Each record in the table represents a comment and allows to manage and track the comments effectively. This table is important for managing and tracking the comments of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Comments.md)

---

## CmsTags

CmsTags Table is used to store tags.

### Description

This table stores information about the tags. Each record in the table represents a tag and allows to manage and track the tags effectively. This table is important for managing and tracking the tags of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Tags.md)

---

## CmsEntityTags

The CmsEntityTags table is used to store the tags and their relations with entities. The table is used by the CmsTags component.

### Description

This table stores information about the tags associated with a entity. Each record in the table represents a tag associated with a entities and allows to manage and track the tags associated with a entity effectively. This table is important for managing and tracking the tags associated with a entity of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Tags.md)

---

## CmsGlobalResources

CmsGlobalResources Table is used to store global resources.

### Description

This table stores information about the global resources. Each record in the table represents a global resource and allows to manage and track the global resources effectively. This table is important for managing and tracking the global resources of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Global-Resources.md)

---

## CmsMediaDescriptors

CmsMediaDescriptors Table is used to store media descriptors.

### Description

This table stores information about the media descriptors. Each record in the table represents a media descriptor and allows to manage and track the media descriptors effectively. This table is important for managing and tracking the media descriptors of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Index.md)

---

## CmsMenuItems

CmsMenuItems Table is used to store menu items.

### Description

This table stores information about the menu items. Each record in the table represents a menu item and allows to manage and track the menu items effectively. This table is important for managing and tracking the menu items of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Menus.md)

---

## CmsPages

CmsPages Table is used to store pages.

### Description

This table stores the pages in the application. Each record in the table represents a page and allows to manage and track the pages effectively. This table can be used to store information about pages, including the page title, content, URL, creation date, and other relevant information.

### Module

[`Volo.CmsKit`](../Cms-Kit/Pages.md)

---

## CmsRatings

CmsRatings Table is used to store ratings.

### Description

This table stores information about the ratings. Each record in the table represents a rating and allows to manage and track the ratings effectively. This table is important for managing and tracking the ratings of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Ratings.md)

---

## CmsUserReactions

CmsUserReactions Table is used to store user reactions.

### Description

This table stores information about the user reactions. Each record in the table represents a user reaction and allows to manage and track the user reactions effectively. This table is important for managing and tracking the user reactions of the application.

### Module

[`Volo.CmsKit`](../Cms-Kit/Reactions.md)