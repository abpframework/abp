# List Groups

## Introduction

`abp-list-group` is the main container for list group content. 

Basic usage:

````xml
<abp-list-group>
    <abp-list-group-item>Cras justo odio</abp-list-group-item>
    <abp-list-group-item>Dapibus ac facilisis in</abp-list-group-item>
    <abp-list-group-item>Morbi leo risus</abp-list-group-item>
    <abp-list-group-item>Vestibulum at eros</abp-list-group-item>
</abp-list-group>
````



## Demo

See the [list groups demo page](https://bootstrap-taghelpers.abp.io/Components/ListGroup) to see it in action.

## Attributes

### flush

A value indicates `abp-list-group` items to remove some borders and rounded corners to render list group items edge-to-edge in a parent container. Should be one of the following values:

* `false` (default value)
* `true`

### active

A value indicates if an `abp-list-group-item` to be active. Should be one of the following values:

* `false` (default value)
* `true`

### disabled

A value indicates if an `abp-list-group-item` to be disabled. Should be one of the following values:

* `false` (default value)
* `true`

### href

A value indicates if an `abp-list-group-item` has a link. Should be a string link value. 

### type

A value indicates an `abp-list-group-item` style class with a stateful background and color. Should be one of the following values:

* `Default` (default value)
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

`abp-list-group-item` can also contain additional HTML elements like spans.

Example:

````xml
<abp-list-group>
    <abp-list-group-item>Cras justo odio <span abp-badge-pill="Primary">14</span></abp-list-group-item>
    <abp-list-group-item>Dapibus ac facilisis in <span abp-badge-pill="Primary">2</span></abp-list-group-item>
    <abp-list-group-item>Morbi leo risus <span abp-badge-pill="Primary">1</span></abp-list-group-item>
</abp-list-group>
````
