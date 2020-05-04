# 进度条

## 介绍

`abp-progress-bar` 是进度条状态的abp标签.

基本用法:

````xml
<abp-progress-bar value="70" />

<abp-progress-bar type="Warning" value="25"> %25 </abp-progress-bar>

<abp-progress-bar type="Success" value="40" strip="true"/>

<abp-progress-bar type="Dark" value="10" min-value="5" max-value="15" strip="true"> %50 </abp-progress-bar>

<abp-progress-group>
    <abp-progress-part type="Success" value="25"/>
    <abp-progress-part type="Danger" value="10" strip="true"> %10 </abp-progress-part>
    <abp-progress-part type="Primary" value="50" animation="true" strip="true" />
</abp-progress-group>
````

## Demo

参阅[进度条demo页面](https://bootstrap-taghelpers.abp.io/Components/Progressbars)查看示例.

## Attributes

### value

指定当前进度条的进度.

### type

指定进度条的背景颜色. 应为下列值之一:

* `Default` (默认值)
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`

### min-value

进度条的最小值. 默认值是0.

### max-value

进度条的最大值. 默认值是100.

### strip

指定进度条的背景样式是否被去除. 应为以下值之一:

* `false` (默认值)
* `true`

### animation

指定进度条的背景样式是否为动画. 应为以下值之一:

* `false` (默认值)
* `true`