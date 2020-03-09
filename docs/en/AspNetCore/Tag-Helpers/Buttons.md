# Buttons

ABP framework has a special Tag Helper to create bootstrap button easily.

`<abp-button>`

## Attributes

`<abp-button>` has 7 different attribute.

* [`button-type`](#button-type)
* [`size`](#size)
* [`busy-text`](#busy-text)
* [`text`](#text)
* [`icon`](#icon)
* [`disabled`](#disabled)
* [`icon-type`](#icon-type)


### `button-type`

`button-type` is a selectable parameter. It's default value is `Default`.

`<abp-button button-type="Primary">Button</abp-button>`

You can choose one of the button type listed below.

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

`size` is a selectable parameter. It's default value is `Default`.

`<abp-button size="Default">Button</abp-button>`

You can choose one of the size type listed below.

* `Default`
* `Small`
* `Medium`
* `Large`
* `Block`
* `Block_Small`
* `Block_Medium`
* `Block_Large`

### `busy-text`

`busy-text` is a string parameter. IT shows the text while the button is busy.

### `text`

`text` is a string parameter that displaying on button.

### `icon`

`icon` is a string parameter. It is depending to [`icon-type`](#`icon-type`). For default, we use [Font Awesome](https://fontawesome.com/) for icons. To use it, you need to set `icon` parameter as a icon name. 

##### Example

[fa-address-card](https://fontawesome.com/icons/address-card): ![fa-address-card](fa-address-card.png "Address Card")

`<abp-button icon="address-card" text="Address" />`

> Don't forget: You dont need to write prefix! It will add automatically "fa" prefix for [Font Awesome](https://fontawesome.com/) icons while you did not change `icon-type`.

### `disabled`

`disabled` is a boolean parameter. If you set it `true`, your button will be disabled.

### `icon-type`

`icon-type` is a selectable parameter. It's default value is `FontAwesome`. You can create your own icon type provider and change it.

You can choose one of the size type listed below.

* `FontAwesome`
* `Other`