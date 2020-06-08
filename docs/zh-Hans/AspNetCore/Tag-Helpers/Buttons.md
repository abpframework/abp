# 按钮

ABP框架定义了Tag Helper用于简单的创建bootstrap按钮.

`<abp-button>`

## 属性

`<abp-button>` 有7个不同的属性.

* [`button-type`](#button-type)
* [`size`](#size)
* [`busy-text`](#busy-text)
* [`text`](#text)
* [`icon`](#icon)
* [`disabled`](#disabled)
* [`icon-type`](#icon-type)

### `button-type`

`button-type` 是一个可选参数. 它的默认值是 `Default`.

`<abp-button button-type="Primary">Button</abp-button>`

你可以为按钮选择以下按钮类型:

* `Default`
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

`size` 是一个可选参数. 它的默认值是 `Default`.

`<abp-button size="Default">Button</abp-button>`

你可以为按钮选择以下size类型:

* `Default`
* `Small`
* `Medium`
* `Large`
* `Block`
* `Block_Small`
* `Block_Medium`
* `Block_Large`

### `busy-text`

`busy-text` 是一个字符串类型参数. 当按钮繁忙时设置该文本.

### `text`

`text` 是一个字符串类型参数,显示在按钮上.

### `icon`

`icon` 是一个字符串类型参数. 它的值取决于[`icon-type`](#`icon-type`). 默认情况下,我们对图标使用[Font Awesome](https://fontawesome.com/). 要使用它,你需要将 `icon` 参数设置为图标名称. 

##### 示例

[fa-address-card](https://fontawesome.com/icons/address-card): ![fa-address-card](fa-address-card.png "Address Card")

`<abp-button icon="address-card" text="Address" />`

> 不要忘记: 你不需要写前缀,如果你没有更改 `icon-type` ,它会为[Font Awesome](https://fontawesome.com/)图标自动添加 `fa` 前缀.

### `disabled`

`disabled` 是一个布尔类型参数. 如果你设值为 `true`, 按钮会被禁用.

### `icon-type`

`icon-type` 是一个可选参数.它的默认值是 `FontAwesome`. 你可以创建自己的图标类型提供程序并更改它.

你可以为按钮选择以下图标类型:

* `FontAwesome`
* `Other`
