## BlgBlogs

BlgBlogs Table is used to store blogs.

### Description

This table stores information about the blogs. Each record in the table represents a blog and allows to manage and track the blogs effectively. This table is important for managing and tracking the blogs of the application.

### Module

`Volo.Blogging`

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`BlgPosts`](#blgposts) | BlogId | To match the post with the blog. |

---

## BlgPosts

BlgPosts Table is used to store posts.

### Description

This table stores information about the posts. Each record in the table represents a post and allows to manage and track the posts effectively. This table is important for managing and tracking the posts of the application.

### Module

`Volo.Blogging`

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`BlgBlogs`](#blgblogs) | Id | To match the post with the blog. |

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`BlgComments`](#blgcomments) | PostId | To match the comment with the post. |
| [`BlgPostTags`](#blgposttags) | PostId | To match the post tag with the post. |

---

## BlgComments

BlgComments Table is used to store comments.

### Description

This table stores information about the comments. Each record in the table represents a comment and allows to manage and track the comments effectively. This table is important for managing and tracking the comments of the application.

### Module

`Volo.Blogging`

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`BlgPosts`](#blgposts) | Id | To match the comment with the post. |
| [`BlgComments`](#blgcomments) | Id | To match the comment with the parent comment. |

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`BlgComments`](#blgcomments) | RepliedCommentId | To match the comment with the parent comment. |

---

## BlgTags

BlgTags Table is used to store tags.

### Description

This table stores information about the tags. Each record in the table represents a tag and allows to manage and track the tags effectively. This table is important for managing and tracking the tags of the application.

### Module

`Volo.Blogging`

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`BlgPostTags`](#blgposttags) | TagId | To match the post tag with the tag. |

---

## BlgPostTags

BlgPostTags Table is used to store post tags.

### Description

This table stores information about the post tags. Each record in the table represents a post tag and allows to manage and track the post tags effectively. This table is important for managing and tracking the post tags of the application.

### Module

`Volo.Blogging`

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`BlgTags`](#blgtags) | Id | To match the post tag with the tag. |
| [`BlgPosts`](#blgposts) | Id | To match the post tag with the post. |

---

## BlgUsers

BlgUsers Table is used to store users.

### Description

This table stores information about the users. Each record in the table represents a user and allows to manage and track the users effectively. This table is important for managing and tracking the users of the application.

### Module

`Volo.Blogging`