# ASP.NET Core MVC / Razor Pages UI: API DOM JavaScript

`abp.dom` (Document Object Model) fornece eventos aos quais você pode se inscrever para ser notificado quando elementos são adicionados e removidos dinamicamente da página (DOM).

Isso é especialmente útil se você deseja inicializar os novos elementos carregados. Isso geralmente é necessário quando você adiciona elementos dinamicamente ao DOM (por exemplo, obtém alguns elementos HTML via AJAX) após a inicialização da página.

> O ABP usa o [MutationObserver](https://developer.mozilla.org/en-US/docs/Web/API/MutationObserver) para observar as alterações feitas no DOM.

## Eventos de Nó

### onNodeAdded

Esse evento é acionado quando um elemento é adicionado ao DOM. Exemplo:

````js
abp.dom.onNodeAdded(function(args){
    console.log(args.$el);
});
````

O objeto `args` possui os seguintes campos:

* `$el`: A seleção JQuery para obter o novo elemento inserido no DOM.

### onNodeRemoved

Esse evento é acionado quando um elemento é removido do DOM. Exemplo:

````js
abp.dom.onNodeRemoved(function(args){
    console.log(args.$el);
});
````

O objeto `args` possui os seguintes campos:

* `$el`: A seleção JQuery para obter o elemento removido do DOM.

## Inicializadores Pré-Construídos

O ABP Framework usa os eventos DOM para inicializar algum tipo de elementos HTML quando eles são adicionados ao DOM após a inicialização da página.

> Observe que os mesmos inicializadores também funcionam se esses elementos já estiverem incluídos no DOM inicial. Portanto, se eles forem carregados inicialmente ou de forma tardia, eles funcionam como esperado.

### Inicializador de Formulário

O inicializador de formulário (definido como `abp.dom.initializers.initializeForms`) inicializa os formulários carregados de forma tardia;

* Habilita automaticamente a validação `unobtrusive` no formulário.
* Pode mostrar automaticamente uma mensagem de confirmação quando você envia o formulário. Para habilitar esse recurso, basta adicionar o atributo `data-confirm` com uma mensagem (como `data-confirm="Tem certeza?"`) ao elemento `form`.
* Se o elemento `form` tiver o atributo `data-ajaxForm="true"`, então chama automaticamente o `.abpAjaxForm()` no elemento `form`, para enviar o formulário via AJAX.

Consulte o documento [Forms & Validation](../Forms-Validation.md) para mais informações.

### Inicializador de Script

O inicializador de script (`abp.dom.initializers.initializeScript`) pode executar um código JavaScript para um elemento DOM.

**Exemplo: Carregar de forma tardia um componente e executar algum código quando o elemento for carregado**

Suponha que você tenha um contêiner para carregar o elemento dentro:

````html
<div id="LazyComponent"></div> 
````

E este é o componente que será carregado via AJAX do servidor e inserido no contêiner:

````html
<div data-script-class="MyCustomClass">
    <p>Mensagem de exemplo</p>
</div>
````

`data-script-class="MyCustomClass"` indica a classe JavaScript que será usada para executar alguma lógica nesse elemento:

`MyCustomClass` é um objeto global definido da seguinte forma:

````js
MyCustomClass = function(){

    function initDom($el){
        $el.css('color', 'red');
    }

    return {
        initDom: initDom
    }
};
````

`initDom` é a função que é chamada pelo ABP Framework. O argumento `$el` é o elemento HTML carregado como uma seleção JQuery.

Finalmente, você pode carregar o componente dentro do contêiner após uma chamada AJAX:

````js
$(function () {
    setTimeout(function(){
        $.get('/get-my-element').then(function(response){
           $('#LazyComponent').html(response);
        });
    }, 2000);
});
````

O sistema de Inicialização de Script é especialmente útil se você não sabe como e quando o componente será carregado no DOM. Isso pode ser possível se você desenvolveu um componente de IU reutilizável em uma biblioteca e deseja que o desenvolvedor do aplicativo não precise se preocupar com a inicialização do componente em diferentes casos de uso.

> A inicialização de script não funciona se o componente for carregado no DOM inicial. Nesse caso, você é responsável por inicializá-lo.

### Outros Inicializadores

Os seguintes componentes e bibliotecas do Bootstrap são inicializados automaticamente quando são adicionados ao DOM:

* Tooltip
* Popover
* Timeago