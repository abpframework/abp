# Popovers

## Introdução

`abp-popover` é a tag abp para mensagens de popover.

Uso básico:

````xml
<abp-button abp-popover="Oi, eu sou o conteúdo do popover!">
    Popover Padrão
</abp-button>
````

## Demonstração

Veja a página de demonstração de [popovers](https://bootstrap-taghelpers.abp.io/Components/Popovers) para vê-lo em ação.

## Atributos

### disabled

Um valor que indica se o elemento deve ser desabilitado para interação. Se esse valor for definido como `true`, o atributo `dismissable` será ignorado. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### dismissable

Um valor que indica se os popovers devem ser fechados no próximo clique do usuário em um elemento diferente do elemento de ativação. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### hoverable

Um valor que indica se o conteúdo do popover será exibido ao passar o mouse sobre ele. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`