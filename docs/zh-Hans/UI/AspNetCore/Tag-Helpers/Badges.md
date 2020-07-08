# 徽章

## 结合扫

`abp-badge` 和  `abp-badge-pill` 是abp徽章标签.

基本用法:

````csharp
<span abp-badge="Primary">Primary</span>
<a abp-badge="Info" href="#">Info</a>
<a abp-badge-pill="Danger" href="#">Danger</a>
````

## Demo

参阅[徽章demo页面](https://bootstrap-taghelpers.abp.io/Components/Badges)查看示例.

### Values

* 表示徽章的类型. 应为下列值之一:

  * `_` (默认值)
  * `Default` (默认值)
  * `Primary`
  * `Secondary`
  * `Success`
  * `Danger`
  * `Warning`
  * `Info`
  * `Light`
  * `Dark`

示例:

````csharp
<span abp-badge-pill="Danger">Danger</span>
````
