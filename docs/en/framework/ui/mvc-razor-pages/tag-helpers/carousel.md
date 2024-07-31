# Carousel

## Introduction

`abp-carousel` is a the abp tag for carousel element.

Basic usage:

````html
<abp-carousel>
    <abp-carousel-item src=""></abp-carousel-item>
    <abp-carousel-item src=""></abp-carousel-item>
    <abp-carousel-item src=""></abp-carousel-item>
</abp-carousel>
````



## Demo

See the [carousel_demo page](https://bootstrap-taghelpers.abp.io/Components/Carousel) to see it in action.

## Attributes

### id

A value sets the id of the carousel. If not set, generated id will be set whenever the tag is created.

### controls

A value to enable the controls (previous and next buttons) on carousel. Should be one of the following values:

* `false`
* `true`

### indicators

A value to enables the indicators on carousel. Should be one of the following values:

* `false`
* `true`

### crossfade

A value to enables the fade animation instead of slide on carousel. Should be one of the following values:

* `false`
* `true`

## abp-carousel-item Attributes

### caption-title

A value sets the caption title of the carousel item.

### caption

A value sets the caption of the carousel item.

### src

A link value sets the source of the image displayed on carousel item.

### active

A value to set the active carousel item. Should be one of the following values:

* `false`
* `true`

### alt

A value sets the alternate text for the carousel item image when the image can not be displayed.

