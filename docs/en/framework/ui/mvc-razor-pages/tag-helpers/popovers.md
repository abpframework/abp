# Popovers

## Introduction

`abp-popover` is the abp tag for popover messages.

Basic usage:

````xml
<abp-button abp-popover="Hi, i'm popover content!">
    Popover Default
</abp-button>
````



## Demo

See the [popovers demo page](https://bootstrap-taghelpers.abp.io/Components/Popovers) to see it in action.

## Attributes

### disabled

A value indicates if the element should be disabled for interaction. If this value is set to `true`, `dismissable` attribute will be ignored. Should be one of the following values:

* `false` (default value)
* `true`

### dismissable

A value indicates to dismiss the popovers on the user's next click of a different element than the toggle element. Should be one of the following values:

* `false` (default value)
* `true`

### hoverable

A value indicates if the popover content will be displayed on mouse hover. Should be one of the following values:

* `false` (default value)
* `true`