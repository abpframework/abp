# Modais

> Este documento explica os detalhes do `abp-modal` Tag Helper, que simplifica a construção da marcação HTML para uma caixa de diálogo modal. Leia [essa documentação](../Modals.md) para aprender como trabalhar com modais.

## Introdução

`abp-modal` é um elemento principal para criar um modal.

Uso básico:

````xml
<abp-button button-type="Primary" data-toggle="modal" data-target="#myModal">Abrir modal</abp-button>

<abp-modal centered="true" scrollable="true" size="Large" id="myModal">
   <abp-modal-header title="Título do modal"></abp-modal-header>
   <abp-modal-body>
       Uau, você está lendo este texto em um modal!
   </abp-modal-body>
   <abp-modal-footer buttons="Fechar"></abp-modal-footer>
</abp-modal>
````

## Demonstração

Veja a [página de demonstração de modais](https://bootstrap-taghelpers.abp.io/Components/Modals) para vê-lo em ação.

## Atributos

### centered

Um valor que indica o posicionamento do modal. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### Scrollable

Um valor que indica a rolagem do modal. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### size

Um valor que indica o tamanho do modal. Deve ser um dos seguintes valores:

* `Default` (valor padrão)
* `Small`
* `Large`
* `ExtraLarge`

### static

Um valor que indica se o modal será estático. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### Conteúdo adicional

`abp-modal-footer` pode ter vários botões com opção de alinhamento.

Adicione `@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal` à sua página.

Exemplo:

````xml
<abp-button button-type="Primary" data-toggle="modal" data-target="#myModal">Abrir modal</abp-button>

<abp-modal centered="true" size="Large" id="myModal" static="true">
    <abp-modal-header title="Título do modal"></abp-modal-header>
    <abp-modal-body>
        Uau, você está lendo este texto em um modal!
    </abp-modal-body>
    <abp-modal-footer buttons="@(AbpModalButtons.Save|AbpModalButtons.Close)" button-alignment="Between"></abp-modal-footer>
</abp-modal>
````

### button-alignment

Um valor que indica o posicionamento dos botões do rodapé do modal. Deve ser um dos seguintes valores:

* `Default` (valor padrão)
* `Start`
* `Center`
* `Around`
* `Between`
* `End`