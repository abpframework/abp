# Grupos de botões

## Introdução

`abp-button-group` é o contêiner principal para elementos de botão agrupados.

Uso básico:

````html
<abp-button-group>
    <abp-button button-type="Secondary">Esquerda</abp-button>
    <abp-button button-type="Secondary">Meio</abp-button>
    <abp-button button-type="Secondary">Direita</abp-button>
</abp-button-group>
````

## Demonstração

Veja a página de demonstração de [grupos de botões](https://bootstrap-taghelpers.abp.io/Components/Button-groups) para vê-lo em ação.

## Atributos

### direction

Um valor que indica a direção dos botões. Deve ser um dos seguintes valores:

* `Horizontal` (valor padrão)
* `Vertical`

### size

Um valor que indica o tamanho dos botões no grupo. Deve ser um dos seguintes valores:

* `Default` (valor padrão)
* `Small`
* `Medium`
* `Large`