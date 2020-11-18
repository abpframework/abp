# Navs

## Introduction

`abp-nav` is the basic tag helper component derived from bootstrap nav element.

Basic usage:

````html
<abp-nav nav-style="Pill" align="Center">
    <abp-nav-item>
<a abp-nav-link active="true" href="#">Active</a>
    </abp-nav-item>
    <abp-nav-item>
<a abp-nav-link href="#">Longer nav link</a>
    </abp-nav-item>
    <abp-nav-item>
<a abp-nav-link href="#">link</a>
    </abp-nav-item>
    <abp-nav-item>
<a abp-nav-link disabled="true" href="#">disabled</a>
    </abp-nav-item>
</abp-nav>
````

## Demo

See the [navs demo page](https://bootstrap-taghelpers.abp.io/Components/Navs) to see it in action.

## abp-nav Attributes

- **nav-style**:  The value indicates the positioning and style of the containing items. Should be one of the following values: 
  * `Default` (default value)
  * `Vertical`
  * `Pill`
  * `PillVertical`
- **align:** The value indicates the alignment of the containing items: 
  * `Default` (default value)
  * `Start`
  * `Center`
  * `End`

### abp-nav-bar Attributes

- **nav-style**:  The value indicates the color layout of the base navigation bar. Should be one of the following values: 
  * `Default` (default value)
  * `Dark`
  * `Light`
  * `Dark_Primary`
  * `Dark_Secondary`
  * `Dark_Success`
  * `Dark_Danger`
  * `Dark_Warning`
  * `Dark_Info`
  * `Dark_Dark`
  * `Dark_Link`
  * `Light_Primary`
  * `Light_Secondary`
  * `Light_Success`
  * `Light_Danger`
  * `Light_Warning`
  * `Light_Info`
  * `Light_Dark`
  * `Light_Link`
- **size:** The value indicates size of the base navigation bar. Should be one of the following values: 
  * `Default` (default value)
  * `Sm`
  * `Md`
  * `Lg`
  * `Xl`

### abp-nav-item Attributes

**dropdown**: A value that sets the navigation item to be a dropdown menu if provided. Can be one of the following values: 

* `false` (default value)
* `true`

Example:

````html
<abp-nav-bar size="Lg" navbar-style="Dark_Warning">
    <a abp-navbar-brand href="#">Navbar</a>
    <abp-navbar-toggle>
        <abp-navbar-nav>
            <abp-nav-item active="true">
                <a abp-nav-link href="#">Home <span class="sr-only">(current)</span></a>
            </abp-nav-item>
            <abp-nav-item>
                <a abp-nav-link href="#">Link</a>
            </abp-nav-item>
            <abp-nav-item dropdown="true">
                <abp-dropdown>
                    <abp-dropdown-button nav-link="true" text="Dropdown" />
                    <abp-dropdown-menu>
                        <abp-dropdown-header>Dropdown header</abp-dropdown-header>
                        <abp-dropdown-item href="#" active="true">Action</abp-dropdown-item>
                        <abp-dropdown-item href="#" disabled="true">Another disabled action</abp-dropdown-item>
                        <abp-dropdown-item href="#">Something else here</abp-dropdown-item>
                        <abp-dropdown-divider />
                        <abp-dropdown-item href="#">Separated link</abp-dropdown-item>
                    </abp-dropdown-menu>
                </abp-dropdown>
            </abp-nav-item>
            <abp-nav-item>
                <a abp-nav-link disabled="true" href="#">Disabled</a>
            </abp-nav-item>
        </abp-navbar-nav>            
        <span abp-navbar-text>
          Sample Text
        </span>
    </abp-navbar-toggle>
</abp-nav-bar>
````
