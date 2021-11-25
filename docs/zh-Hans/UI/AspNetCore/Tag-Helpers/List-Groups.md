# 列表组

## 介绍

`abp-list-group` 是列表组内容的主要容器.

基本用法:

````xml
<abp-list-group>
    <abp-list-group-item>Cras justo odio</abp-list-group-item>
    <abp-list-group-item>Dapibus ac facilisis in</abp-list-group-item>
    <abp-list-group-item>Morbi leo risus</abp-list-group-item>
    <abp-list-group-item>Vestibulum at eros</abp-list-group-item>
</abp-list-group>
````

## Demo

参阅[列表组demo页面](https://bootstrap-taghelpers.abp.io/Components/ListGroup)查看示例.

## Attributes

### flush

指定 `abp-list-group` 项可删除某些边框和圆角,以在父容器中无边框呈现列表组项. 应为以下值之一:

* `false` (默认值)
* `true`

### active

指定 `abp-list-group-item` 是否处于active. 应为以下值之一:

* `false` (默认值)
* `true`

### disabled

指定 `abp-list-group-item` 是否被禁用. 应为以下值之一:

* `false` (默认值)
* `true`

### href

指定 `abp-list-group-item` 是否含有链接. 应该是一个字符串链接值.

### type

指定 `abp-list-group-item` 具有状态背景和颜色的样式类. 应为以下值之一:

* `Default` (默认值)
* `Primary`
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`
* `Link`

### Additional content

`abp-list-group-item` 还可以包含其他HTML元素(例如span).

示例:

````xml
<abp-list-group>
    <abp-list-group-item>Cras justo odio <span abp-badge-pill="Primary">14</span></abp-list-group-item>
    <abp-list-group-item>Dapibus ac facilisis in <span abp-badge-pill="Primary">2</span></abp-list-group-item>
    <abp-list-group-item>Morbi leo risus <span abp-badge-pill="Primary">1</span></abp-list-group-item>
</abp-list-group>
````
