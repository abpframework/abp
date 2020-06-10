# 面包屑

## Introduction

`ABP-breadcrumb` 是面包屑项主容器.

基本用法:

````csharp
<abp-breadcrumb>
    <abp-breadcrumb-item href="#" title="Home" />
    <abp-breadcrumb-item href="#" title="Library"/>
    <abp-breadcrumb-item title="Page"/>
</abp-breadcrumb>
````

## Demo

参阅[面包屑demo页面](https://bootstrap-taghelpers.abp.io/Components/Breadcrumbs)查看示例.

## abp-breadcrumb-item Attributes

- **title**: 设置面包屑项文本.
- **active**: 设置活动面包屑项. 如果没有其他项是活动的,默认最后一项为活动项.
- **href**: 表示 `abp-breadcrumb-item` 是否有链接. 值应该是字符串链接.