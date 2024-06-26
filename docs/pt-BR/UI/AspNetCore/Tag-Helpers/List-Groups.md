# Listas de Grupos

## Introdução

`abp-list-group` é o contêiner principal para o conteúdo do grupo de listas.

Uso básico:

````xml
<abp-list-group>
    <abp-list-group-item>Cras justo odio</abp-list-group-item>
    <abp-list-group-item>Dapibus ac facilisis in</abp-list-group-item>
    <abp-list-group-item>Morbi leo risus</abp-list-group-item>
    <abp-list-group-item>Vestibulum at eros</abp-list-group-item>
</abp-list-group>
````

## Demonstração

Veja a [página de demonstração de grupos de listas](https://bootstrap-taghelpers.abp.io/Components/ListGroup) para vê-lo em ação.

## Atributos

### flush

Um valor indica que os itens `abp-list-group` devem remover algumas bordas e cantos arredondados para renderizar os itens do grupo de listas de ponta a ponta em um contêiner pai. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### active

Um valor indica se um `abp-list-group-item` deve estar ativo. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### disabled

Um valor indica se um `abp-list-group-item` deve estar desativado. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### href

Um valor indica se um `abp-list-group-item` possui um link. Deve ser um valor de link de string.

### type

Um valor indica uma classe de estilo `abp-list-group-item` com um plano de fundo e cor com estado. Deve ser um dos seguintes valores:

* `Default` (valor padrão)
* `Primary`
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`
* `Link`

### Conteúdo adicional

`abp-list-group-item` também pode conter elementos HTML adicionais, como spans.

Exemplo:

````xml
<abp-list-group>
    <abp-list-group-item>Cras justo odio <span abp-badge-pill="Primary">14</span></abp-list-group-item>
    <abp-list-group-item>Dapibus ac facilisis in <span abp-badge-pill="Primary">2</span></abp-list-group-item>
    <abp-list-group-item>Morbi leo risus <span abp-badge-pill="Primary">1</span></abp-list-group-item>
</abp-list-group>
````