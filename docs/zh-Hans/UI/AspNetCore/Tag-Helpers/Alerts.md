# Alerts

## 介绍

`abp-alert` 是创建alert的主要元素.

基本用法:

````xml
<abp-alert alert-type="Primary">
    A simple primary alert—check it out!
</abp-alert>
````

## Demo

参阅[alerts demo页面](https://bootstrap-taghelpers.abp.io/Components/Alerts)查看示例.

## Attributes

### alert-type

值做为alert的Type,应为以下值之一:

* `Default` (默认值)
* `Primary`
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`

示例:

````xml
<abp-alert alert-type="Warning">
    A simple warning  alert—check it out!
</abp-alert>
````

### alert-link

alert的链接.

示例:

````xml
<abp-alert alert-type="Danger">
    A simple danger alert with <a abp-alert-link href="#">an example link</a>. Give it a click if you like.
</abp-alert>
````

### dismissible

使alert可被忽略:

示例:

````xml
<abp-alert alert-type="Warning" dismissible="true">
    Holy guacamole! You should check in on some of those fields below.
</abp-alert>
````

### Additional content

`abp-alert`还可以包含其他HTML元素,例如标题,段落和分隔符.

示例:

````xml
<abp-alert alert-type="Success">
    <h4>Well done!</h4>
    <p>Aww yeah, you successfully read this important alert message. This example text is going to run a bit longer so that you can see how spacing within an alert works with this kind of content.</p>
    <hr>
    <p class="mb-0">Whenever you need to, be sure to use margin utilities to keep things nice and tidy.</p>
</abp-alert>
````
