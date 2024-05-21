# Barras de Progresso

## Introdução

`abp-progress-bar` é a tag abp para o status da barra de progresso.

Uso básico:

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

## Demonstração

Veja a página de demonstração das [barras de progresso](https://bootstrap-taghelpers.abp.io/Components/Progressbars) para vê-las em ação.

## Atributos

### value

Um valor que indica o progresso atual da barra.

### type

Um valor que indica a cor de fundo da barra de progresso. Deve ser um dos seguintes valores:

* `Default` (valor padrão)
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`

### min-value

Valor mínimo da barra de progresso. O padrão é 0.

### max-value

Valor máximo da barra de progresso. O padrão é 100.

### strip

Um valor que indica se o estilo de fundo da barra de progresso é listrado. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### animation

Um valor que indica se o estilo de fundo listrado da barra de progresso é animado. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`