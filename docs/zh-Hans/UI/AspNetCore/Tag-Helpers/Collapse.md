# Collapse

## 介绍

`abp-collapse-body` 是显示和隐藏内容的主要容器. `abp-collapse-id` 用于显示和隐藏内容容器. 可以通过 `abp-button` 和 `a` 标签触发.

基本用法:

````xml
<abp-button button-type="Primary" abp-collapse-id="collapseExample" text="Button with data-target" />
<a abp-button="Primary" abp-collapse-id="collapseExample"> Link with href </a>

<abp-collapse-body id="collapseExample">       
                    Anim pariatur wolf moon tempor,,, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
</abp-collapse-body>
````

## Demo

参阅[collapse demo 页面](https://bootstrap-taghelpers.abp.io/Components/Collapse)查看示例.

## Attributes

### show

指定折叠主体初始化时是折叠还是展开. 应为以下值之一:

* `false` (默认值)
* `true`

### multi

指定 `abp-collapse-body` 是否可以通过显示/隐藏多个折叠体的元素显示或隐藏. 此属性做为 "multi-collapse" class 添加到 `abp-collapse-body`. 应为以下值之一:

* `false` (默认值)
* `true`

示例:

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

## 手风琴示例

`abp-accordion` 是手风琴项的主容器.

基本用法:

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

指定手风琴项目在初始化时是显示还是隐藏. 应为以下值之一:

* `false` (默认值)
* `true`

### title

指定手风琴项的标题. 应为字符串类型的值.
