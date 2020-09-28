# Badges

## Introduction

`abp-badge` and  `abp-badge-pill` are abp tags for badges.

Basic usage:

````csharp
<span abp-badge="Primary">Primary</span>
<a abp-badge="Info" href="#">Info</a>
<a abp-badge-pill="Danger" href="#">Danger</a>
````



## Demo

See the [badges demo page](https://bootstrap-taghelpers.abp.io/Components/Badges) to see it in action.

### Values

* Indicates the type of the badge. Should be one of the following values:

  * `_` (default value)
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

````csharp
<span abp-badge-pill="Danger">Danger</span>
````
