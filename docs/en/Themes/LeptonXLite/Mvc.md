# LeptonX Lite Mvc

LeptonX Lite Mvc is a user interface **option** for [LeptonX Lite](https://x.leptontheme.com/) theme. 

## LeptonX Lite Mvc Features

* Built on the [Bootstrap 5](https://getbootstrap.com/) library and [ABP Framework](https://abp.io/).
* Pre-built modules typically uses the **Razor Pages** approach instead of the classic MVC pattern **(you can use MVC pattern too)**.
* Provides styles for [Datatables](https://datatables.net/).
* Responsive & mobile-compatible.
* Uses [Font Awesome](https://fontawesome.com/) icons and [Bootstrap](https://icons.getbootstrap.com/) icons.

## Customize Your LeptonX Lite Mvc Application

Abp **helps** you make **highly customizable UI**. You can easily **customize** your themes to fit your needs. **The Virtual File System** makes it possible to **manage files** that **do not physically** exist on the **file system** (disk). It's mainly used to embed **(js, css, image..)** files into assemblies and **use them like** physical files at runtime. An application (or another module) can **override** a **virtual file of a module** just like placing a file with the **same name** and **extension** into the **same folder** of the **virtual file**.

LeptonX Lite built on the [Abp Framework](https://abp.io/), so you can **easily** customize your Asp.Net Core Mvc user interface by following [Abp Mvc UI Customization](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Customization-xUser-Interface).

# LeptonX Lite Mvc Layouts

LeptonX Lite Mvc provides **layouts** for your **user interface** based [ABP Framework Theming](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Theming). You can use **layouts** to **organize your user interface**. 

The main responsibility of a theme is to **provide** the layouts. There are **three pre-defined layouts must be implemented by all the themes:**

* **Application:** The **default** layout which is used by the **main** application pages.
  
* **Account:** Mostly used by the **account module** for **login**, **register**, **forgot password**... pages.
  
* **Empty:** The **Minimal** layout that **has no layout components** at all.

**Layout names** are **constants** defined in the `LeptonXLiteTheme` class in the **Mvc** project **root**.

> The layout pages define under `Themes/LeptonXLite/Layouts` folder and you can **override it** by creating a file with the **same name** and **under** the **same folder**.  

# LeptonX Lite Mvc Components

## Brand Component

The **brand component** is a simple component that can be used to display your brand. It contains a **logo** and a **company name**.

<img src="Images/brand-component.png">

### How to override Brand Component in LeptonX Lite Mvc

* The **brand component page (.cshtml file)** defines a `Themes/LeptonXLite/Components/Brand/Default.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.  

* The **brand component (C# file)** defines `Themes/LeptonXLite/Components/Brand/MainNavbarBrandViewComponent.cs` file and you can **override it** by creating a file with the **same name** and under the **same folder**.  

## Breadcrumb Component

On websites that have a lot of pages, **breadcrumb navigation** can greatly **enhance the way users find their way** around. In terms of **usability**, breadcrumbs reduce the number of actions a website **visitor** needs to take in order to get to a **higher-level page**, and they **improve** the **findability** of **website sections** and **pages**.

<img src="Images/breadcrumb-component.png">

### How to override Breadcrumb Component in LeptonX Lite Mvc

* The **breadcrumb component page (.cshtml file)** defines `Themes/LeptonXLite/Components/Breadcrumbs/Default.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.  

* The **breadcrumb component (C# file)** defines `Themes/LeptonXLite/Components/Breadcrumbs/BreadcrumbsViewComponent.cs` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**. 

## Sidebar Menu Component

Sidebar menus have been used as **a directory for Related Pages** to a **Service** offering, **Navigation** items to a **specific service** or topic and even just as **Links** the user may be interested in.

<img src="Images/sidebar-menu-component.png">

### How to override Sidebar Menu Component in LeptonX Lite Mvc

* **Sidebar menu page (.cshtml)** defines in the `Themes/LeptonXLite/Components/Menu/Default.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**. 

* If you want to **override menu component (C#)** you can override the `Themes/LeptonXLite/Components/Menu/MainMenuViewComponent.cs` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**. 

> The **sidebar menu** renders menu items **dynamic**. The **menu item** is a **partial view** and defines in the `Themes/LeptonXLite/Components/Menu/_MenuItem.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**. 

## Page Alerts Component

Provide contextual **feedback messages** for typical user actions with the handful of **available** and **flexible** **alert messages**. Alerts are available for any length of text, as well as an **optional dismiss button**. 

<img src="Images/page-alerts-component.png">

### How to override Page Alerts Component in LeptonX Lite Mvc

* The **page alerts component page (.cshtml file)** defines `Themes/LeptonXLite/Components/PageAlerts/Default.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

* The **page alerts component (C#)** defines `Themes/LeptonXLite/Components/PageAlerts/PageAlertsViewComponent.cs` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

## Toolbar Component

Toolbar items are used to add **extra functionality to the toolbar**. The toolbar is a **horizontal bar** that **contains** a group of **toolbar items**. 

### How to override Toolbar Component in LeptonX Lite Mvc

* The **toolbar component page (.cshtml file)** defines `Themes/LeptonXLite/Components/Toolbar/Default.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

* The **toolbar component (C#)** defines `Themes/LeptonXLite/Components/Toolbar/ToolbarViewComponent.cs` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

## Toolbar Item Component

Toolbar item is a **single item** that **contains** a **link**, an **icon**, a **label** etc..

### How to override Toolbar Item Component in LeptonX Lite Mvc

* The **toolbar item component page (.cshtml file)** defines `Themes/LeptonXLite/Components/ToolbarItems/Default.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

* The **toolbar item component (C#)** defines `Themes/LeptonXLite/Components/ToolbarItems/ToolbarItemsViewComponent.cs` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

You can find the toolbar components below:

## Language Switch Component

Think about a **multi-lingual** website and the first thing that could **hit your mind** is **the language switch component**. A **navigation bar** is a **great place** to **embed a language switch**. By embedding the language switch in the navigation bar of your website, you would **make it simpler** for users to **find it** and **easily** switch the **language**  <u>**without trying to locate it across the website.**</u>

<img src="Images/language-switch-component.png">

### How to override Language Switch Component in LeptonX Lite Mvc

* The **language switch component page (.cshtml file)** defines `Themes/LeptonXLite/Components/LanguageSwitch/Default.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

* The **language switch component (C#)** defines `Themes/LeptonXLite/Components/LanguageSwitch/LanguageSwitchViewComponent.cs` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

## Mobile Language Switch Component

The **mobile** **language switch component** is used to switch the language of the website **on mobile devices**. The mobile language switch component is a **dropdown menu** that **contains all the languages** of the website.

<img src="Images/mobile-language-switch-component.png">

### How to override Mobile Language Switch Component in LeptonX Lite Mvc

* The **mobile language switch component page (.cshtml file)** defines `Themes/LeptonXLite/Components/MobileLanguageSwitch/Default.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

* The **mobile language switch component (C#)** defines `Themes/LeptonXLite/Components/MobileLanguageSwitch/MobileLanguageSwitchViewComponent.cs` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

## User Menu Component

The **User Menu** is the **menu** that **drops down** when you **click your name** or **profile picture** in the **upper right corner** of your page (**in the toolbar**). It drops down options such as **Settings**, **Logout**, etc.

<img src="Images/user-menu-component.png">

### How to override User Menu Component in LeptonX Lite Mvc

* The **user menu component page (.cshtml file)** defines `Themes/LeptonXLite/Components/UserMenu/Default.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

* The **user menu component (C#)** defines `Themes/LeptonXLite/Components/UserMenu/UserMenuViewComponent.cs` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

## Mobile User Menu Component

The **mobile user menu component** is used to display the **user menu on mobile devices**. The mobile user menu component is a **dropdown menu** that contains all the **options** of the **user menu**.

<img src="Images/mobile-user-menu-component.png">

### How to override Mobile User Menu Component in LeptonX Lite Mvc

* The **mobile user menu component page (.cshtml file)** defines `Themes/LeptonXLite/Components/MobileUserMenu/Default.cshtml` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.

* The **mobile user menu component (C#)** defines `Themes/LeptonLite/Components/MobileUserMenu/MobileUserMenuViewComponent.cs` file and you can **override it** by creating a file with the **same name** and **under** the **same folder**.