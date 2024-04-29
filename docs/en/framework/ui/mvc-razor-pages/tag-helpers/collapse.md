# Collapse

## Introduction

`abp-collapse-body` is the main container for showing and hiding content. `abp-collapse-id` is used to show and hide the content container. Can be triggered with both `abp-button` and `a` tags.

Basic usage:

````html
<abp-button button-type="Primary" abp-collapse-id="collapseExample" text="Button with data-target" />
<a abp-button="Primary" abp-collapse-id="collapseExample"> Link with href </a>

<abp-collapse-body id="collapseExample">       
                    Anim pariatur wolf moon tempor,,, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
</abp-collapse-body>
````



## Demo

See the [collapse demo page](https://bootstrap-taghelpers.abp.io/Components/Collapse) to see it in action.

## Attributes

### show

A value indicates if the collapse body will be initialized visible or hidden. Should be one of the following values:

* `false` (default value)
* `true`

### multi

A value indicates if an `abp-collapse-body` can be shown or hidden by an element that can show/hide multiple collapse bodies. Basically, this attribute adds "multi-collapse" class to `abp-collapse-body`. Should be one of the following values:

* `false` (default value)
* `true`

Sample:

````xml
<a abp-button="Primary" abp-collapse-id="FirstCollapseExample"> Toggle first element </a>
<abp-button button-type="Primary" abp-collapse-id="SecondCollapseExample" text="Toggle second element" />
<abp-button button-type="Primary" abp-collapse-id="FirstCollapseExample SecondCollapseExample" text="Toggle both elements" />
        
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

## Accordion example

`abp-accordion` is the main container for the accordion items. 

Basic usage:

````xml
<abp-accordion>
    <abp-accordion-item title="Collapsible Group Item #1">
                Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry rtat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
    </abp-accordion-item>
    <abp-accordion-item title="Collapsible Group Item #2">
                Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
    </abp-accordion-item>
    <abp-accordion-item title="Collapsible Group Item #3">
                Anim pariatur  wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
    </abp-accordion-item>
</abp-accordion>
````

## Attributes

### active

A value indicates if the accordion item will be initialized visible or hidden. Should be one of the following values:

* `false` (default value)
* `true`

### title

A value indicates the visible title of the accordion item. Should be a string value.
