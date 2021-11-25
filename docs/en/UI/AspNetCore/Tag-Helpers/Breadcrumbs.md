# Breadcrumbs

## Introduction

`abp-breadcrumb` is the main container for breadcrumb items. 

Basic usage:

````html
<abp-breadcrumb>
    <abp-breadcrumb-item href="#" title="Home" />
    <abp-breadcrumb-item href="#" title="Library"/>
    <abp-breadcrumb-item title="Page"/>
</abp-breadcrumb>
````

## Demo

See the [breadcrumbs demo page](https://bootstrap-taghelpers.abp.io/Components/Breadcrumbs) to see it in action.

## abp-breadcrumb-item Attributes

- **title**: Sets the text of the breadcrumb item.
- **active**: Sets the active breadcrumb item. Last item is active by default, if no other item is active.
- **href**: A value indicates if an `abp-breadcrumb-item` has a link. Should be a string link value. 
