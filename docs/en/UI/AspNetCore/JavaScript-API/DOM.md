# ASP.NET Core MVC / Razor Pages UI: JavaScript DOM API

`abp.dom` (Document Object Model) provides events that you can subscribe to get notified when elements dynamically added to and removed from the page (DOM).

It is especially helpful if you want to initialize the new loaded elements. This is generally needed when you dynamically add elements to DOM (for example, get some HTML elements via AJAX) after page initialization.

> ABP uses the [MutationObserver](https://developer.mozilla.org/en-US/docs/Web/API/MutationObserver) to observe the changes made on the DOM.

## Node Events

### onNodeAdded

This event is triggered when an element is added to the DOM. Example:

````js
abp.dom.onNodeAdded(function(args){
    console.log(args.$el);
});
````

`args` object has the following fields;

* `$el`: The JQuery selection to get the new element inserted to the DOM.

### onNodeRemoved

This event is triggered when an element is removed from the DOM. Example:

````js
abp.dom.onNodeRemoved(function(args){
    console.log(args.$el);
});
````

`args` object has the following fields;

* `$el`: The JQuery selection to get the element removed from the DOM.

## Pre-Build Initializers

ABP Framework is using the DOM events to initialize some kind of HTML elements when they are added to the DOM after than the page was already initialized.

> Note that the same initializers also work if these elements were already included in the initial DOM. So, whether they are initially or lazy loaded, they work as expected.

### Form Initializer

The Form initializer (defined as `abp.dom.initializers.initializeForms`) initializes the lazy loaded forms;

* Automatically enabled the `unobtrusive` validation on the form.
* Can automatically show a confirmation message when you submit the form. To enable this feature, just add `data-confirm` attribute with a message (like `data-confirm="Are you sure?"`) to the `form` element.
* If the `form` element has `data-ajaxForm="true"` attribute, then automatically calls the `.abpAjaxForm()` on the `form` element, to make the form posted via AJAX.

See the [Forms & Validation](../Forms-Validation.md) document for more.

### Script Initializer

Script initializer (`abp.dom.initializers.initializeScript`) can execute a JavaScript code for a DOM element.

**Example: Lazy load a component and execute some code when the element has loaded**

Assume that you've a container to load the element inside:

````html
<div id="LazyComponent"></div>	
````

And this is the component that will be loaded via AJAX from the server and inserted into the container:

````html
<div data-script-class="MyCustomClass">
    <p>Sample message</p>
</div>
````

`data-script-class="MyCustomClass"` indicates the JavaScript class that will be used to perform some logic on this element:

`MyCustomClass` is a global object defined as shown below:

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

`initDom` is the function that is called by the ABP Framework. The `$el` argument is the loaded HTML element as a JQuery selection.

Finally, you can load the component inside the container after an AJAX call:

````js
$(function () {
    setTimeout(function(){
        $.get('/get-my-element').then(function(response){
           $('#LazyComponent').html(response);
        });
    }, 2000);
});
````

Script Initialization system is especially helpful if you don't know how and when the component will be loaded into the DOM. This can be possible if you've developed a reusable UI component in a library and you want the application developer shouldn't care how to initialize the component in different use cases.

> Script initialization doesn't work if the component was loaded in the initial DOM. In this case, you are responsible to initialize it.

### Other Initializers

The following Bootstrap components and libraries are automatically initialized when they are added to the DOM:

* Tooltip
* Popover
* Timeage

