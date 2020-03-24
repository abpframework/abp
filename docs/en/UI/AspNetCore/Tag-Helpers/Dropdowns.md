# Dropdowns

## Introduction

`abp-dropdown` is the main container for dropdown content. 

Basic usage:

````xml
<abp-dropdown>
    <abp-dropdown-button text="Dropdown button" />
    <abp-dropdown-menu>
        <abp-dropdown-item href="#">Action</abp-dropdown-item>
        <abp-dropdown-item href="#">Another action</abp-dropdown-item>
        <abp-dropdown-item href="#">Something else here</abp-dropdown-item>
    </abp-dropdown-menu>
</abp-dropdown>
````



## Demo

See the [dropdown demo page](https://bootstrap-taghelpers.abp.io/Components/Dropdowns) to see it in action.

## Attributes

### direction

A value indicates which direction the dropdown buttons will be displayed to. Should be one of the following values:

* `Down` (default value)
* `Up`
* `Right`
* `Left`

### dropdown-style

A value indicates if an `abp-dropdown-button` will have split icon for dropdown. Should be one of the following values:

* `Single` (default value)
* `Split`



## Menu items

`abp-dropdown-menu` is the main container for dropdown menu items. 

Basic usage:

````xml
<abp-dropdown>
    <abp-dropdown-button button-type="Secondary" text="Dropdown"/>
    <abp-dropdown-menu>
        <abp-dropdown-header>Dropdown Header</abp-dropdown-header>
        <abp-dropdown-item href="#">Action</abp-dropdown-item>
        <abp-dropdown-item active="true" href="#">Active action</abp-dropdown-item>
        <abp-dropdown-item disabled="true" href="#">Disabled action</abp-dropdown-item>
        <abp-dropdown-divider/>
        <abp-dropdown-item-text>Dropdown Item Text</abp-dropdown-item-text>
        <abp-dropdown-item href="#">Something else here</abp-dropdown-item>
    </abp-dropdown-menu>
</abp-dropdown>
````

## Attributes

### align

A value indicates which direction `abp-dropdown-menu` items will be aligned to. Should be one of the following values:

* `Left` (default value)
* `Right`

### Additional content

`abp-dropdown-menu` can also contain additional HTML elements like headings, paragraphs, dividers or form element.

Example:

````xml
<abp-dropdown >
    <abp-dropdown-button button-type="Secondary" text="Dropdown With Form"/>
    <abp-dropdown-menu>
        <form class="px-4 py-3">
            <abp-input asp-for="EmailAddress"></abp-input>
            <abp-input asp-for="Password"></abp-input>
            <abp-input asp-for="RememberMe"></abp-input>
            <abp-button button-type="Primary" text="Sign In" type="submit" />
        </form>
        <abp-dropdown-divider></abp-dropdown-divider>
        <abp-dropdown-item href="#">New around here? Sign up</abp-dropdown-item>
        <abp-dropdown-item href="#">Forgot password?</abp-dropdown-item>
    </abp-dropdown-menu>
</abp-dropdown>
````
