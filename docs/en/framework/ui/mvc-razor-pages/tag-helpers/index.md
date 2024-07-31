# ABP Tag Helpers

ABP defines a set of **tag helper components** to simply the user interface development for ASP.NET Core (MVC / Razor Pages) applications.

## Bootstrap Component Wrappers

Most of the tag helpers are [Bootstrap](https://getbootstrap.com/) (v5+) wrappers. Coding bootstrap is not so easy, not so type-safe and contains too much repetitive HTML tags. ABP Tag Helpers makes it **easier** and **type safe**.

We don't aim to wrap bootstrap components 100%. Writing **native bootstrap style code** is still possible (actually, tag helpers generates native bootstrap code in the end), but we suggest to use the tag helpers wherever possible.

ABP also adds some **useful features** to the standard bootstrap components.

Here, the list of components those are wrapped by the ABP:

* [Alerts](alerts.md)
* [Badges](badges.md)
* [Blockquote](blockquote.md)
* [Borders](borders.md)
* [Breadcrumb](breadcrumbs.md)
* [Buttons](buttons.md)
* [Cards](cards.md)
* [Carousel](carousel.md)
* [Collapse](collapse.md)
* [Dropdowns](dropdowns.md)
* [Figures](figure.md)
* [Grids](grids.md)
* [List Groups](list-groups.md)
* [Modals](modals.md)
* [Navigation](navs.md)
* [Paginator](paginator.md)
* [Popovers](popovers.md)
* [Progress Bars](progress-bars.md)
* [Tables](tables.md)
* [Tabs](tabs.md)
* [Tooltips](tooltips.md)

> Until all the tag helpers are documented, you can visit https://bootstrap-taghelpers.abp.io/ to see them with live samples.

## Form Elements

**Abp Tag Helpers** add new features to standard **Asp.Net Core MVC input & select Tag Helpers** and wrap them with **Bootstrap** form controls. See [Form Elements documentation](form-elements.md) .

## Dynamic Forms

**Abp Tag helpers** offer an easy way to build complete **Bootstrap forms**. See [Dynamic Forms documentation](dynamic-forms.md).
