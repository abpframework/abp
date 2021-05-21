# CMS Kit: Pages

CMS Kit Page system allows you to create dynamic pages by specifying URLs, which is the fundamental feature of a CMS.

## The User Interface

### Menu items

CMS Kit module admin side adds the following items to the main menu, under the *CMS* menu item:

* **Pages**: Page management page.

`CmsKitAdminMenus` class has the constants for the menu item names.

### Pages

#### Page Management

Pages page is used to manage dynamic pages in the system.

![pages-page](D:\Github\abp\docs\en\images\cmskit-module-pages-page.png)

You can create or edit an existing page on this page.

![pages-edit](D:\Github\abp\docs\en\images\cmskit-module-pages-edit.png)

When you create a page, you can access the created page via `/pages/{slug}` URL.