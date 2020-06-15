# 导航

## 介绍

`abp-nav` 是从bootstrap nav元素派生的基本标签助手.

基本用法:

````csharp
<abp-nav nav-style="Pill" align="Center">
    <abp-nav-item>
<a abp-nav-link active="true" href="#">Active</a>
    </abp-nav-item>
    <abp-nav-item>
<a abp-nav-link href="#">Longer nav link</a>
    </abp-nav-item>
    <abp-nav-item>
<a abp-nav-link href="#">link</a>
    </abp-nav-item>
    <abp-nav-item>
<a abp-nav-link disabled="true" href="#">disabled</a>
    </abp-nav-item>
</abp-nav>
````

## Demo

参阅[导航demo页面](https://bootstrap-taghelpers.abp.io/Components/Navs)查看示例.

## abp-nav Attributes

- **nav-style**:  指示包含项的位置和样式. 应为以下值之一:
  * `Default` (默认值)
  * `Vertical`
  * `Pill`
  * `PillVertical`
- **align:** 指示包含项的对齐方式:
  * `Default` (默认值)
  * `Start`
  * `Center`
  * `End`

### abp-nav-bar Attributes

- **nav-style**:  指示基本导航栏的颜色布局. 应为以下值之一:
  * `Default` (默认值)
  * `Dark`
  * `Light`
  * `Dark_Primary`
  * `Dark_Secondary`
  * `Dark_Success`
  * `Dark_Danger`
  * `Dark_Warning`
  * `Dark_Info`
  * `Dark_Dark`
  * `Dark_Link`
  * `Light_Primary`
  * `Light_Secondary`
  * `Light_Success`
  * `Light_Danger`
  * `Light_Warning`
  * `Light_Info`
  * `Light_Dark`
  * `Light_Link`
- **size:** 指示基本导航栏的大小. 应为以下值之一:
  * `Default` (默认值)
  * `Sm`
  * `Md`
  * `Lg`
  * `Xl`

### abp-nav-item Attributes

**dropdown**: 将导航项设置为下拉菜单(如果提供的话). 可以是下列值之一:

* `false` (默认值)
* `true`

示例:

````csharp
<abp-nav-bar size="Lg" navbar-style="Dark_Warning">
    <a abp-navbar-brand href="#">Navbar</a>
    <abp-navbar-toggle>
        <abp-navbar-nav>
            <abp-nav-item active="true">
                <a abp-nav-link href="#">Home <span class="sr-only">(current)</span></a>
            </abp-nav-item>
            <abp-nav-item>
                <a abp-nav-link href="#">Link</a>
            </abp-nav-item>
            <abp-nav-item dropdown="true">
                <abp-dropdown>
                    <abp-dropdown-button nav-link="true" text="Dropdown" />
                    <abp-dropdown-menu>
                        <abp-dropdown-header>Dropdown header</abp-dropdown-header>
                        <abp-dropdown-item href="#" active="true">Action</abp-dropdown-item>
                        <abp-dropdown-item href="#" disabled="true">Another disabled action</abp-dropdown-item>
                        <abp-dropdown-item href="#">Something else here</abp-dropdown-item>
                        <abp-dropdown-divider />
                        <abp-dropdown-item href="#">Separated link</abp-dropdown-item>
                    </abp-dropdown-menu>
                </abp-dropdown>
            </abp-nav-item>
            <abp-nav-item>
                <a abp-nav-link disabled="true" href="#">Disabled</a>
            </abp-nav-item>
        </abp-navbar-nav>            
        <span abp-navbar-text>
          Sample Text
        </span>
    </abp-navbar-toggle>
</abp-nav-bar>
````
