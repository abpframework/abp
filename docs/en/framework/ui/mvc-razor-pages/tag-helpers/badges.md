# Badges

## Introduction

`abp-badge` and  `abp-badge-pill` are ABP Tag Helper attributes for `a` and `span` html tags.

Basic usage:

````html
<span abp-badge="Primary">Primary</span>
<a abp-badge="Info" href="#">Info</a>
<a abp-badge-pill="Danger" href="#">Danger</a>
````



## Demo

See the [badges demo page](https://bootstrap-taghelpers.abp.io/Components/Badges) to see it in action.

### Values

* Indicates the type of the badge. Should be one of the following values:

  * `Default` 
  * `Primary`
  * `Secondary`
  * `Success`
  * `Danger`
  * `Warning`
  * `Info`
  * `Light`
  * `Dark`

Example:

````html
<span abp-badge-pill="Danger">Danger</span>
````

