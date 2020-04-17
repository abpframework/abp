# Cards

## Introduction

`abp-card` is a content container derived from bootstrap card element.

Basic usage:

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



##### Using Titles, Text and Links: 

Following tags can be used under main `abp-card` tag

* `abp-card-title`
* `abp-card-subtitle`
* `a abp-card-link`

Sample:

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



##### Using List Groups:

* `abp-list-group flush="true"` : `flush` attribute renders into bootstrap `list-group-flush` class which is used for removing borders and rounded corners to render list group items edge to edge in a parent container.
* `abp-list-group-item`

Kitchen Sink Sample:

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



##### Using Header, Footer and Blockquote:

* `abp-card-header`
* `abp-card-footer`
* `abp-blockquote`

Sample:

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

Quote Sample:

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

Footer Sample:

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

See the [cards demo page](https://bootstrap-taghelpers.abp.io/Components/Cards) to see it in action.

## abp-card Attributes

- **background:** A value indicates the background color of the card.
- **text-color**: A value indicates the color of the text inside the card.
- **border:** A value indicates the color of the border inside the card.

Should be one of the following values:

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

````xml
<abp-card background="Success" text-color="Danger" border="Dark">
````

### sizing

Cards has default 100% with and can be changed with custom CSS, grid classes, grid Sass mixins or [utilities](https://getbootstrap.com/docs/4.0/utilities/sizing/).

````xml
<abp-card style="width: 18rem;">
````

### card-deck and card-columns

`abp-card` can be used inside `card-deck` or `card-columns` aswell.

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
