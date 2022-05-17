# ABP Tag Helpers

ABP Framework defines a set of **tag helper components** to simply the user interface development for ASP.NET Core (MVC / Razor Pages) applications.

## Bootstrap Component Wrappers

Most of the tag helpers are [Bootstrap](https://getbootstrap.com/) (v5+) wrappers. Coding bootstrap is not so easy, not so type-safe and contains too much repetitive HTML tags. ABP Tag Helpers makes it **easier** and **type safe**.

We don't aim to wrap bootstrap components 100%. Writing **native bootstrap style code** is still possible (actually, tag helpers generates native bootstrap code in the end), but we suggest to use the tag helpers wherever possible.

ABP Framework also adds some **useful features** to the standard bootstrap components.

Here, the list of components those are wrapped by the ABP Framework:

* [Alerts](Alerts.md)
* [Badges](Badges.md)
* [Blockquote](Blockquote.md)
* [Borders](Borders.md)
* [Breadcrumb](Breadcrumbs.md)
* [Buttons](Buttons.md)
* [Cards](Cards.md)
* [Carousel](Carousel.md)
* [Collapse](Collapse.md)
* [Dropdowns](Dropdowns.md)
* [Figures](Figure.md)
* [Grids](Grids.md)
* [List Groups](List-Groups.md)
* [Modals](Modals.md)
* [Navigation](Navs.md)
* [Paginator](Paginator.md)
* [Popovers](Popovers.md)
* [Progress Bars](Progress-Bars.md)
* [Tables](Tables.md)
* [Tabs](Tabs.md)
* [Tooltips](Tooltips.md)

> Until all the tag helpers are documented, you can visit https://bootstrap-taghelpers.abp.io/ to see them with live samples.

## Form Elements

**Abp Tag Helpers** add new features to standard **Asp.Net Core MVC input & select Tag Helpers** and wrap them with **Bootstrap** form controls. See [Form Elements documentation](Form-elements.md) .

## Dynamic Forms

**Abp Tag helpers** offer an easy way to build complete **Bootstrap forms**. See [Dynamic Forms documentation](Dynamic-Forms.md).
