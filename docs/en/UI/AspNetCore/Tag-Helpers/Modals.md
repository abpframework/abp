# Modals

> This document explains the details of the `abp-modal` Tag Helper, which simplifies to build the HTML markup for a modal dialog. Read [that documentation](../Modals.md) to learn how to work with modals.

## Introduction

`abp-modal` is a main element to create a modal.

Basic usage:

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

See the [modals demo page](https://bootstrap-taghelpers.abp.io/Components/Modals) to see it in action.

## Attributes

### centered

A value indicates the positioning of the modal. Should be one of the following values:

* `false` (default value)
* `true`

### Scrollable

A value indicates the scrolling of the modal. Should be one of the following values:

* `false` (default value)
* `true`

### size

A value indicates the size of the modal. Should be one of the following values:

* `Default` (default value)
* `Small`
* `Large`
* `ExtraLarge`

### static

A value indicates if the modal will be static. Should be one of the following values:

* `false` (default value)
* `true`

### Additional content

`abp-modal-footer` can have multiple buttons with alignment option.

Add `@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal` to your page.

Example:

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

A value indicates the positioning of your modal footer buttons. Should be one of the following values: 

* `Default` (default value)
* `Start`
* `Center`
* `Around`
* `Between`
* `End`
