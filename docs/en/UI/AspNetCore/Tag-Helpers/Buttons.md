# Buttons

## Introduction

`abp-button` is the main element to create buttons.

Basic usage:

````xml
<abp-button button-type="Primary">Click Me</abp-button>
````

## Demo

See the [buttons demo page](https://bootstrap-taghelpers.abp.io/Components/Buttons) to see it in action.

## Attributes

### button-type

A value indicates the main style/type of the button. Should be one of the following values:

* `Default` (default value)
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

### size

A value indicates the size of the button. Should be one of the following values:

* `Default` (default value)
* `Small`
* `Medium`
* `Large`
* `Block`
* `Block_Small`
* `Block_Medium`
* `Block_Large`

### busy-text

A text that is shown when the button is busy.

### text

The text of the button. This is a shortcut if you simply want to set a text to the button. Example:

````xml
<abp-button button-type="Primary" text="Click Me" />
````

In this case, you can use a self-closing tag to make it shorter.

### icon

Used to set an icon for the button. It works with the [Font Awesome](https://fontawesome.com/) icon classes by default. Example:

````xml
<abp-button icon="address-card" text="Address" />
````

##### icon-type

If you don't want to use font-awesome, you have two options:

1. Set `icon-type` to `Other` and write the CSS class of the font icon you're using.
2. If you don't use a font icon use the opening and closing tags manually and write any code inside the tags.

### disabled

Set `true` to make the button initially disabled.