# Cartões

## Introdução

`abp-card` é um contêiner de conteúdo derivado do elemento de cartão do Bootstrap.

Uso básico:

````xml
<abp-card style="width: 18rem;">
  <img abp-card-image="Top" src="~/imgs/demo/300x200.png"/>
  <abp-card-body>
    <abp-card-title>Título do Cartão</abp-card-title>
    <abp-card-text>Algum texto de exemplo rápido para construir o título do cartão e compor a maior parte do conteúdo do cartão.</abp-card-text>
    <a abp-button="Primary" href="#">Ir para algum lugar</a>
  </abp-card-body>
</abp-card>
````



##### Usando Títulos, Texto e Links: 

As seguintes tags podem ser usadas sob a tag principal `abp-card`

* `abp-card-title`
* `abp-card-subtitle`
* `a abp-card-link`

Exemplo:

````xml
<abp-card style="width: 18rem;">
    <abp-card-body>
       <abp-card-title>Título do cartão</abp-card-title>
       <abp-card-subtitle class="mb-2 text-muted">Subtítulo do cartão</abp-card-subtitle>
       <abp-card-text>Algum texto de exemplo rápido para construir o título do cartão e compor a maior parte do conteúdo do cartão.</abp-card-text>
       <a abp-card-link href="#">Link do cartão</a>
       <a abp-card-link href="#">Outro link</a>
    </abp-card-body>
</abp-card>
````



##### Usando Grupos de Listas:

* `abp-list-group flush="true"` : O atributo `flush` renderiza a classe `list-group-flush` do Bootstrap, que é usada para remover bordas e cantos arredondados para renderizar os itens do grupo de listas de ponta a ponta em um contêiner pai.
* `abp-list-group-item`

Exemplo completo:

````xml
<abp-card style="width: 18rem;">
    <img abp-card-image="Top" src="~/imgs/demo/300x200.png" />
    <abp-card-body>
       <abp-card-title>Título do Cartão</abp-card-title>
       <abp-card-text>Algum texto de exemplo rápido para construir o título do cartão e compor a maior parte do conteúdo do cartão.</abp-card-text>
    </abp-card-body>
    <abp-list-group flush="true">
       <abp-list-group-item>Cras justo odio</abp-list-group-item>
       <abp-list-group-item>Dapibus ac facilisis in</abp-list-group-item>
       <abp-list-group-item>Vestibulum at eros</abp-list-group-item>
    </abp-list-group>
    <abp-card-body>
       <a abp-card-link href="#">Link do cartão</a>
       <a abp-card-link href="#">Outro link</a>
    </abp-card-body>
</abp-card>
````



##### Usando Cabeçalho, Rodapé e Citação:

* `abp-card-header`
* `abp-card-footer`
* `abp-blockquote`

Exemplo:

```xml
<abp-card style="width: 18rem;">
    <abp-card-header>Destaque</abp-card-header>
    <abp-card-body>
       <abp-card-title>Tratamento especial de título</abp-card-title>
       <abp-card-text>Com texto de suporte abaixo como uma introdução natural a conteúdo adicional.</abp-card-text>
       <a abp-button="Primary" href="#">Ir para algum lugar</a>
    </abp-card-body>
</abp-card>
```

Exemplo de citação:

```xml
<abp-card>
    <abp-card-header>Citação</abp-card-header>
    <abp-card-body>
<abp-blockquote>
    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante.</p>
    <footer>Alguém famoso em Título da Fonte</footer>
</abp-blockquote>
    </abp-card-body>
</abp-card>
```

Exemplo de rodapé:

```xml
<abp-card class="text-center">
    <abp-card-header>Destaque</abp-card-header>
    <abp-card-body>
        <abp-blockquote>
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante.</p>
            <footer>Alguém famoso em Título da Fonte</footer>
        </abp-blockquote>
    </abp-card-body>
    <abp-card-footer class="text-muted">2 dias atrás</abp-card-footer>
</abp-card>
```



## Demonstração

Veja a [página de demonstração de cartões](https://bootstrap-taghelpers.abp.io/Components/Cards) para vê-lo em ação.

## Atributos do abp-card

- **background:** Um valor que indica a cor de fundo do cartão.
- **text-color**: Um valor que indica a cor do texto dentro do cartão.
- **border:** Um valor que indica a cor da borda dentro do cartão.

Deve ser um dos seguintes valores:

* `Default` (valor padrão)
* `Primary`
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`

Exemplo:

````xml
<abp-card background="Success" text-color="Danger" border="Dark">
````

### dimensionamento

Os cartões têm largura padrão de 100% e podem ser alterados com CSS personalizado, classes de grade, mixins de grade Sass ou [utilitários](https://getbootstrap.com/docs/4.0/utilities/sizing/).

````xml
<abp-card style="width: 18rem;">
````

### card-deck e card-columns

`abp-card` também pode ser usado dentro de `card-deck` ou `card-columns`.

````xml
<div class="card-deck">
    <abp-card background="Primary">
        <abp-card-header>Primeiro Deck</abp-card-header>
        <abp-card-body>
            <abp-card-title>Ás</abp-card-title>
            <abp-card-text>Aqui está o conteúdo para Ás.</abp-card-text>
        </abp-card-body>
    </abp-card>
    <abp-card background="Info">
        <abp-card-header>Segundo Deck</abp-card-header>
        <abp-card-body>
            <abp-card-title>Beta</abp-card-title>
            <abp-card-text>Conteúdo Beta.</abp-card-text>
        </abp-card-body>
    </abp-card>
    <abp-card background="Warning">
        <abp-card-header>Terceiro Deck</abp-card-header>
        <abp-card-body>
            <abp-card-title>Epsilon</abp-card-title>
            <abp-card-text>Conteúdo para Epsilon.</abp-card-text>
        </abp-card-body>
    </abp-card>
</div>
````