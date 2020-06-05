# 标签页

## 介绍

`abp-tab` 是从引导标签元素派生的基本标签导航内容容器.

基本用法:

````xml
<abp-tabs>
    <abp-tab title="Home">
             Content_Home
    </abp-tab>
    <abp-tab-link title="Link" href="#" />
    <abp-tab title="profile">
            Content_Profile
    </abp-tab>
    <abp-tab-dropdown title="Contact" name="ContactDropdown">
        <abp-tab title="Contact 1" parent-dropdown-name="ContactDropdown">
            Content_1_Content
        </abp-tab>
        <abp-tab title="Contact 2" parent-dropdown-name="ContactDropdown">
            Content_2_Content
        </abp-tab>
    </abp-tab-dropdown>
</abp-tabs>
````

## Demo

参阅[标签页demo页面](https://bootstrap-taghelpers.abp.io/Components/Tabs)查看示例.

## abp-tab Attributes

- **title**: 设置标签页菜单文字.
- **name:** 设置生成元素的"id"属性. 默认值为Guid. 除非使用Jquery更改或修改了选项卡,否则不需要.
- **active**: 设置active标签页.

示例:

````xml
<abp-tabs name="TabId">
    <abp-tab name="nav-home" title="Home">
        Content_Home
    </abp-tab>   
    <abp-tab name="nav-profile" active="true" title="profile">
        Content_Profile
    </abp-tab>
    <abp-tab name="nav-contact" title="Contact">
        Content_Contact
    </abp-tab>
</abp-tabs>
````

### Pills

示例:

````xml
<abp-tabs tab-style="Pill">
    <abp-tab title="Home">
         Content_Home
    </abp-tab>
    <abp-tab title="profile">
         Content_Profile
    </abp-tab>
    <abp-tab title="Contact">
         Content_Contact
    </abp-tab>
</abp-tabs>
````

### Vertical

**vertical-header-size**: 设置标签标题的列宽.

示例:

````xml
<abp-tabs tab-style="PillVertical" vertical-header-size="_2" >
    <abp-tab active="true" title="Home">
        Content_Home
    </abp-tab>   
    <abp-tab title="profile">
        Content_Profile
    </abp-tab>
    <abp-tab title="Contact">
        Content_Contact
    </abp-tab>
</abp-tabs>
````
