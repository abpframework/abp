# 卡片

## 介绍

`abp-card` 是从bootstrap card元素派生的内容容器.

基本用法:

````xml
<abp-card style="width: 18rem;">
  <img abp-card-image="Top" src="~/imgs/demo/300x200.png"/>
  <abp-card-body>
    <abp-card-title>Card Title</abp-card-title>
    <abp-card-text>Some quick example text to build on the card title and make up the bulk of the card's content.</abp-card-text>
    <a abp-button="Primary" href="#"> Go somewhere</a>
  </abp-card-body>
</abp-card>
````

##### 使用标题,文本和链接: 

`abp-card` 可以使用以下标签

* `abp-card-title`
* `abp-card-subtitle`
* `a abp-card-link`

示例:

````xml
<abp-card style="width: 18rem;">
    <abp-card-body>
       <abp-card-title>Card title</abp-card-title>
       <abp-card-subtitle class="mb-2 text-muted">Card subtitle</abp-card-subtitle>
       <abp-card-text>Some quick example text to build on the card title and make up the bulk of the card's content.</abp-card-text>
       <a abp-card-link href="#">Card link</a>
       <a abp-card-link href="#">Another link</a>
    </abp-card-body>
</abp-card>
````

##### 使用列表组:

* `abp-list-group flush="true"` : `flush` 属性渲染到 bootstrap `list-group-flush` 的 calss 中,该类用于删除边界和圆角以使列表组项在父容器中并排显示.
* `abp-list-group-item`

示例:

````xml
<abp-card style="width: 18rem;">
    <img abp-card-image="Top" src="~/imgs/demo/300x200.png" />
    <abp-card-body>
       <abp-card-title>Card Title</abp-card-title>
       <abp-card-text>Some quick example text to build on the card title and make up the bulk of the card's content.</abp-card-text>
    </abp-card-body>
    <abp-list-group flush="true">
       <abp-list-group-item>Cras justo odio</abp-list-group-item>
       <abp-list-group-item>Dapibus ac facilisis in</abp-list-group-item>
       <abp-list-group-item>Vestibulum at eros</abp-list-group-item>
    </abp-list-group>
    <abp-card-body>
       <a abp-card-link href="#">Card link</a>
       <a abp-card-link href="#">Another link</a>
    </abp-card-body>
</abp-card>
````

##### 使用页眉,页脚和块引用:

* `abp-card-header`
* `abp-card-footer`
* `abp-blockquote`

示例:

```xml
<abp-card style="width: 18rem;">
    <abp-card-header>Featured</abp-card-header>
    <abp-card-body>
       <abp-card-title> Special title treatment</abp-card-title>
       <abp-card-text>With supporting text below as a natural lead-in to additional content.</abp-card-text>
       <a abp-button="Primary" href="#"> Go somewhere</a>
    </abp-card-body>
</abp-card>
```

引用示例:

```xml
<abp-card>
    <abp-card-header>Quote</abp-card-header>
    <abp-card-body>
<abp-blockquote>
    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante.</p>
    <footer>Someone famous in Source Title</footer>
</abp-blockquote>
    </abp-card-body>
</abp-card>
```

页脚示例:

```xml
<abp-card class="text-center">
    <abp-card-header>Featured</abp-card-header>
    <abp-card-body>
        <abp-blockquote>
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante.</p>
            <footer>Someone famous in Source Title</footer>
        </abp-blockquote>
    </abp-card-body>
    <abp-card-footer class="text-muted"> 2 days ago</abp-card-footer>
</abp-card>
```

## Demo

参阅[卡片demo页面](https://bootstrap-taghelpers.abp.io/Components/Cards)查看示例.

## abp-card Attributes

- **background:** 值指定卡片背景的颜色.
- **text-color**: 值指定卡片内文本的颜色.
- **border:** 值指定卡片边框的颜色.

应为以下值之一:

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

````xml
<abp-card background="Success" text-color="Danger" border="Dark">
````

### sizing

卡片的默认值为100%,可以使用自定义CSS,栅格类,栅格Sass mixins或[utilities](https://getbootstrap.com/docs/4.0/utilities/sizing/)进行更改.

````xml
<abp-card style="width: 18rem;">
````

### card-deck 和 card-columns

`abp-card` 可以在 `card-deck` 或 `card-columns` 里使用.

````xml
<div class="card-deck">
    <abp-card background="Primary">
        <abp-card-header>First Deck</abp-card-header>
        <abp-card-body>
            <abp-card-title> Ace </abp-card-title>
            <abp-card-text>Here is the content for Ace.</abp-card-text>
        </abp-card-body>
    </abp-card>
    <abp-card background="Info">
        <abp-card-header>Second Deck</abp-card-header>
        <abp-card-body>
            <abp-card-title> Beta </abp-card-title>
            <abp-card-text>Beta content.</abp-card-text>
        </abp-card-body>
    </abp-card>
    <abp-card background="Warning">
        <abp-card-header>Third Deck</abp-card-header>
        <abp-card-body>
            <abp-card-title> Epsilon </abp-card-title>
            <abp-card-text>Content for Epsilon.</abp-card-text>
        </abp-card-body>
    </abp-card>
</div>
````
