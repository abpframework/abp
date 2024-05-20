# Colapso

## Introdução

`abp-collapse-body` é o contêiner principal para mostrar e ocultar conteúdo. `abp-collapse-id` é usado para mostrar e ocultar o contêiner de conteúdo. Pode ser acionado tanto com `abp-button` quanto com tags `a`.

Uso básico:

````html
<abp-button button-type="Primary" abp-collapse-id="collapseExample" text="Botão com data-target" />
<a abp-button="Primary" abp-collapse-id="collapseExample"> Link com href </a>

<abp-collapse-body id="collapseExample">       
                    Anim pariatur wolf moon tempor,,, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
</abp-collapse-body>
````



## Demonstração

Veja a [página de demonstração de colapso](https://bootstrap-taghelpers.abp.io/Components/Collapse) para vê-lo em ação.

## Atributos

### show

Um valor que indica se o corpo do colapso será inicializado visível ou oculto. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### multi

Um valor que indica se um `abp-collapse-body` pode ser mostrado ou ocultado por um elemento que pode mostrar/ocultar vários corpos de colapso. Basicamente, esse atributo adiciona a classe "multi-collapse" a `abp-collapse-body`. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

Exemplo:

````xml
<a abp-button="Primary" abp-collapse-id="FirstCollapseExample"> Alternar primeiro elemento </a>
<abp-button button-type="Primary" abp-collapse-id="SecondCollapseExample" text="Alternar segundo elemento" />
<abp-button button-type="Primary" abp-collapse-id="FirstCollapseExample SecondCollapseExample" text="Alternar ambos os elementos" />
        
<abp-row class="mt-3">
    <abp-column size-sm="_6">
        <abp-collapse-body id="FirstCollapseExample" multi="true">
               Curabitur porta porttitor libero eu luctus. Praesent ultrices mattis commodo. Integer sodales massa risus, in molestie enim sagittis blandit
        </abp-collapse-body>
    </abp-column>
    <abp-column size-sm="_6">
        <abp-collapse-body id="SecondCollapseExample" multi="true">
                Anim pariatur  wolf moon tempor,,, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. 
        </abp-collapse-body>
    </abp-column>
</abp-row>
````

## Exemplo de acordeão

`abp-accordion` é o contêiner principal para os itens do acordeão.

Uso básico:

````xml
<abp-accordion>
    <abp-accordion-item title="Item do Grupo Colapsável #1">
                Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry rtat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
    </abp-accordion-item>
    <abp-accordion-item title="Item do Grupo Colapsável #2">
                Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
    </abp-accordion-item>
    <abp-accordion-item title="Item do Grupo Colapsável #3">
                Anim pariatur  wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
    </abp-accordion-item>
</abp-accordion>
````

## Atributos

### active

Um valor que indica se o item do acordeão será inicializado visível ou oculto. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

### title

Um valor que indica o título visível do item do acordeão. Deve ser um valor de string.