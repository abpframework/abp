# 下拉菜单

## 介绍

`abp-dropdown` 是下拉菜单的主要容器.

基本用法:

````xml
<abp-dropdown>
    <abp-dropdown-button text="Dropdown button" />
    <abp-dropdown-menu>
        <abp-dropdown-item href="#">Action</abp-dropdown-item>
        <abp-dropdown-item href="#">Another action</abp-dropdown-item>
        <abp-dropdown-item href="#">Something else here</abp-dropdown-item>
    </abp-dropdown-menu>
</abp-dropdown>
````

## Demo

参阅 [下拉菜单demo页面](https://bootstrap-taghelpers.abp.io/Components/Dropdowns)查看示例.

## Attributes

### direction

指定下拉菜单的方向. 应为以下值之一:

* `Down` (默认值)
* `Up`
* `Right`
* `Left`

### dropdown-style

指定 `abp-dropdown-button` 是否具有用于拆分的拆分图标. 应为以下值之一:

* `Single` (默认值)
* `Split`

## Menu items

`abp-dropdown-menu` 下拉菜单项的主要容器.

基本用法:

````xml
<abp-dropdown>
    <abp-dropdown-button button-type="Secondary" text="Dropdown"/>
    <abp-dropdown-menu>
        <abp-dropdown-header>Dropdown Header</abp-dropdown-header>
        <abp-dropdown-item href="#">Action</abp-dropdown-item>
        <abp-dropdown-item active="true" href="#">Active action</abp-dropdown-item>
        <abp-dropdown-item disabled="true" href="#">Disabled action</abp-dropdown-item>
        <abp-dropdown-divider/>
        <abp-dropdown-item-text>Dropdown Item Text</abp-dropdown-item-text>
        <abp-dropdown-item href="#">Something else here</abp-dropdown-item>
    </abp-dropdown-menu>
</abp-dropdown>
````

## Attributes

### align

指定 `abp-dropdown-menu` 在哪个方向对齐. 应为以下值之一:

* `Left` (默认值)
* `Right`

### Additional content

`abp-dropdown-menu` 也可以包含其他HTML元素,例如标题,段落,分隔符或form元素.

示例:

````xml
<abp-dropdown >
    <abp-dropdown-button button-type="Secondary" text="Dropdown With Form"/>
    <abp-dropdown-menu>
        <form class="px-4 py-3">
            <abp-input asp-for="EmailAddress"></abp-input>
            <abp-input asp-for="Password"></abp-input>
            <abp-input asp-for="RememberMe"></abp-input>
            <abp-button button-type="Primary" text="Sign In" type="submit" />
        </form>
        <abp-dropdown-divider></abp-dropdown-divider>
        <abp-dropdown-item href="#">New around here? Sign up</abp-dropdown-item>
        <abp-dropdown-item href="#">Forgot password?</abp-dropdown-item>
    </abp-dropdown-menu>
</abp-dropdown>
````
