# Carrossel

## Introdução

`abp-carousel` é a tag abp para o elemento carrossel.

Uso básico:

````html
<abp-carousel>
    <abp-carousel-item src=""></abp-carousel-item>
    <abp-carousel-item src=""></abp-carousel-item>
    <abp-carousel-item src=""></abp-carousel-item>
</abp-carousel>
````

## Demonstração

Veja a página [carousel_demo](https://bootstrap-taghelpers.abp.io/Components/Carousel) para vê-lo em ação.

## Atributos

### id

Um valor que define o id do carrossel. Se não for definido, um id gerado será definido quando a tag for criada.

### controls

Um valor para habilitar os controles (botões anterior e próximo) no carrossel. Deve ser um dos seguintes valores:

* `false`
* `true`

### indicators

Um valor para habilitar os indicadores no carrossel. Deve ser um dos seguintes valores:

* `false`
* `true`

### crossfade

Um valor para habilitar a animação de fade em vez de slide no carrossel. Deve ser um dos seguintes valores:

* `false`
* `true`

## Atributos do abp-carousel-item

### caption-title

Um valor que define o título da legenda do item do carrossel.

### caption

Um valor que define a legenda do item do carrossel.

### src

Um valor de link que define a origem da imagem exibida no item do carrossel.

### active

Um valor para definir o item ativo do carrossel. Deve ser um dos seguintes valores:

* `false`
* `true`

### alt

Um valor que define o texto alternativo para a imagem do item do carrossel quando a imagem não pode ser exibida.