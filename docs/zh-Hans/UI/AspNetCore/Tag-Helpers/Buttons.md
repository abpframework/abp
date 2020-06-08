# 按钮

## 介绍

`abp-button` 是创建按钮的主要元素.

基本用法:

````xml
<abp-button button-type="Primary">Click Me</abp-button>
````

## Demo

参阅[按钮Demo页面](https://bootstrap-taghelpers.abp.io/Components/Buttons)查看示例.

## Attributes

### `button-type`

指定按钮的主样式/类型. 应为以下值之一:

* `Default` (默认值)
* `Primary`
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`
* `Outline_Primary`
* `Outline_Secondary`
* `Outline_Success`
* `Outline_Danger`
* `Outline_Warning`
* `Outline_Info`
* `Outline_Light`
* `Outline_Dark`
* `Link`

### `size`

指定按钮的大小. 应为以下值之一:

* `Default`
* `Small`
* `Medium`
* `Large`
* `Block`
* `Block_Small`
* `Block_Medium`
* `Block_Large`

### `busy-text`

当按钮busy时显示的文本.

### `text`

按钮的文本. 如果你只想为为按钮设置文本,这是一种快捷方式. 例:

````xml
<abp-button button-type="Primary" text="Click Me" />
````

在这个示例中,你可以使用 self-closing 标签将其缩短.

### `icon`

设置按钮的图标.  默认情况下它使用[Font Awesome](https://fontawesome.com/)图标库. 例:

````xml
<abp-button icon="address-card" text="Address" />
````

### `icon-type`

如果你不想使用font-awesome,你有两个选项:

1. 设置 `icon-type` 为 `Other`,并为你的按钮编写图标样式.
2. 如果你不使用图标,请手动使用opening和closing标签,并在标签内写任何代码.

### `disabled`

设置为 `true` 禁用按钮.