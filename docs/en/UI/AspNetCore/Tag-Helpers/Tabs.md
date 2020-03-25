# Tabs

## Introduction

`abp-tab` is the basic tab navigation content container derived from bootstrap tab element.

Basic usage:

````xml
<abp-tabs>
    <abp-tab title="Home">
             Content_Home
    </abp-tab>
    <abp-tab-link title="Link" href="#" />
    <abp-tab title="profile">
            Content_Profile
    </abp-tab>
    <abp-tab-dropdown title="Contact" name="ContactDropdown">
        <abp-tab title="Contact 1" parent-dropdown-name="ContactDropdown">
            Content_1_Content
        </abp-tab>
        <abp-tab title="Contact 2" parent-dropdown-name="ContactDropdown">
            Content_2_Content
        </abp-tab>
    </abp-tab-dropdown>
</abp-tabs>
````



## Demo

See the [tabs demo page](https://bootstrap-taghelpers.abp.io/Components/Tabs) to see it in action.

## abp-tab Attributes

- **title**: Sets the text of the tab menu.
- **name:** Sets "id" attribute of generated elements. Default value is a Guid. Not needed unless tabs are changed or modified with Jquery.
- **active**: Sets the active tab.

Example:

````xml
<abp-tabs name="TabId">
    <abp-tab name="nav-home" title="Home">
        Content_Home
    </abp-tab>   
    <abp-tab name="nav-profile" active="true" title="profile">
        Content_Profile
    </abp-tab>
    <abp-tab name="nav-contact" title="Contact">
        Content_Contact
    </abp-tab>
</abp-tabs>
````

### Pills

Example:

````xml
<abp-tabs tab-style="Pill">
    <abp-tab title="Home">
         Content_Home
    </abp-tab>
    <abp-tab title="profile">
         Content_Profile
    </abp-tab>
    <abp-tab title="Contact">
         Content_Contact
    </abp-tab>
</abp-tabs>
````

### Vertical

**vertical-header-size**: Sets the column width of tab headers.

Example:

````xml
<abp-tabs tab-style="PillVertical" vertical-header-size="_2" >
    <abp-tab active="true" title="Home">
        Content_Home
    </abp-tab>   
    <abp-tab title="profile">
        Content_Profile
    </abp-tab>
    <abp-tab title="Contact">
        Content_Contact
    </abp-tab>
</abp-tabs>
````
