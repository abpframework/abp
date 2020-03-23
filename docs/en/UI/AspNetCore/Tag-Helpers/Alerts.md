# Alerts

## Introduction

`abp-alert` is a main element to create an alert.

Basic usage:

````xml
<abp-alert alert-type="Primary">
    A simple primary alert—check it out!
</abp-alert>
````



## Demo

See the [alerts demo page](https://bootstrap-taghelpers.abp.io/Components/Alerts) to see it in action.

## Attributes

### alert-type

A value indicates the type of the alert. Should be one of the following values:

* `Default` (default value)
* `Primary`
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`

Example:

````xml
<abp-alert alert-type="Warning">
    A simple warning  alert—check it out!
</abp-alert>
````

### alert-link

A value provides matching colored links within any alert. 

Example:

````xml
<abp-alert alert-type="Danger">
    A simple danger alert with <a abp-alert-link href="#">an example link</a>. Give it a click if you like.
</abp-alert>
````

### dismissible

A value to make the alert dismissible.

Example:

````xml
<abp-alert alert-type="Warning" dismissible="true">
    Holy guacamole! You should check in on some of those fields below.
</abp-alert>
````

### Additional content

`abp-alert` can also contain additional HTML elements like headings, paragraphs and dividers.

Example:

````xml
<abp-alert alert-type="Success">
    <h4>Well done!</h4>
    <p>Aww yeah, you successfully read this important alert message. This example text is going to run a bit longer so that you can see how spacing within an alert works with this kind of content.</p>
    <hr>
    <p class="mb-0">Whenever you need to, be sure to use margin utilities to keep things nice and tidy.</p>
</abp-alert>
````
