# 轮播

## 介绍

`abp-carousel` 是abp标签轮播元素

基本用法:

````csharp
<abp-carousel>
    <abp-carousel-item src=""></abp-carousel-item>
    <abp-carousel-item src=""></abp-carousel-item>
    <abp-carousel-item src=""></abp-carousel-item>
</abp-carousel>
````

## Demo

参阅[轮播demo页面](https://bootstrap-taghelpers.abp.io/Components/Carousel)查看示例.

## Attributes

### id

轮播的ID. 如果未设置则会生成一个ID.

### controls

用于启用轮播上的控件(previous和next按钮). 应为以下值之一:

* `false`
* `true`

### indicators

启用轮播指标. 应为以下值之一:

* `false`
* `true`

### crossfade

用于启用淡入淡出动画而不是在轮播上滑动. 应为以下值之一:

* `false`
* `true`

## abp-carousel-item Attributes

### caption-title

设置轮播项的标题

### caption

设置轮播项的说明.

### src

链接值设置显示在轮播项上的图像的来源.

### active

设置活动轮播项. 应为以下值之一:

* `false`
* `true`

### alt

当无法显示图像时,该值设置轮播项目图像的替代文本.