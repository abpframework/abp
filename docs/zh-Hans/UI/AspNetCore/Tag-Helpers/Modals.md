# 模态框

## 介绍

`abp-modal` 是创建模态框的主要元素.

基本用法:

````xml
<abp-button button-type="Primary" data-toggle="modal" data-target="#myModal">Launch modal</abp-button>

<abp-modal centered="true" scrollable="true" size="Large" id="myModal">
   <abp-modal-header title="Modal title"></abp-modal-header>
   <abp-modal-body>
       Woohoo, you're reading this text in a modal!
   </abp-modal-body>
   <abp-modal-footer buttons="Close"></abp-modal-footer>
</abp-modal>
````

## Demo

参阅[模态框demo页面](https://bootstrap-taghelpers.abp.io/Components/Modals)查看示例.

## Attributes

### centered

指定模态框的位置. 应为以下值之一:

* `false` (默认值)
* `true`

### Scrollable

指定模态框滚动. 应为以下值之一:

* `false` (默认值)
* `true`

### size

指定模态框的大小. 应为以下值之一:

* `Default` (默认值)
* `Small`
* `Large`
* `ExtraLarge`

### static

指定模态框是否是静态的. 应为以下值之一:

* `false` (默认值)
* `true`

### Additional content

`abp-modal-footer` 可以有多个带有对齐选项的按钮.

添加 `@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal` 到你的页面.

示例:

````xml
<abp-button button-type="Primary" data-toggle="modal" data-target="#myModal">Launch modal</abp-button>

<abp-modal centered="true" size="Large" id="myModal" static="true">
    <abp-modal-header title="Modal title"></abp-modal-header>
    <abp-modal-body>
        Woohoo, you're reading this text in a modal!
    </abp-modal-body>
    <abp-modal-footer buttons="@(AbpModalButtons.Save|AbpModalButtons.Close)" button-alignment="Between"></abp-modal-footer>
</abp-modal>
````

### button-alignment

指定模态页脚按钮的位置. 应为以下值之一:

* `Default` (默认值)
* `Start`
* `Center`
* `Around`
* `Between`
* `End`
