# ContextStrategy

`ContextStrategy` is an abstract class exposed by @abp/ng.core package. There are three context strategies extending it: `ComponentContextStrategy`, `TemplateContextStrategy`, and `NoContextStrategy`. Implementing the same methods and properties, all of these strategies help you define how projected content will get their context.



## ComponentContextStrategy

`ComponentContextStrategy` is a class that extends `ContextStrategy`. It lets you **pass context to a projected component**.


### constructor

```js
constructor(public context: Partial<InferredInstanceOf<T>>) {}
```

- `T` refers to component type here, i.e. `Type<C>`.
- `InferredInstanceOf` is a utility type exposed by @abp/ng.core package. It infers component shape.
- `context` will be mapped to properties of the projected component.


### setContext

```js
setContext(componentRef: ComponentRef<InferredInstanceOf<T>>): Partial<InferredInstanceOf<T>>
```

This method maps each prop of the context to the component property with the same name and calls change detection. It returns the context after mapping.



## TemplateContextStrategy

`TemplateContextStrategy` is a class that extends `ContextStrategy`. It lets you **pass context to a projected template**.


### constructor

```js
constructor(public context: Partial<InferredContextOf<T>>) {}
```

- `T` refers to template context type here, i.e. `TemplateRef<C>`.
- `InferredContextOf` is a utility type exposed by @abp/ng.core package. It infers context shape.
- `context` will be mapped to properties of the projected template.


### setContext

```js
setContext(): Partial<InferredContextOf<T>>
```

This method does nothing and only returns the context, because template context is not mapped but passed in as parameter to `createEmbeddedView` method.



## NoContextStrategy

`NoContextStrategy` is a class that extends `ContextStrategy`. It lets you **skip passing any context to projected content**.


### constructor

```js
constructor()
```

Unlike other context strategies, `NoContextStrategy` contructor takes no parameters.


### setContext

```js
setContext(): undefined
```

Since there is no context, this method gets no parameters and will return `undefined`.



## Predefined Context Strategies

Predefined context strategies are accessible via `CONTEXT_STRATEGY` constant.


### None

```js
CONTEXT_STRATEGY.None()
```

This strategy will not pass any context to the projected content.


### Component

```js
CONTEXT_STRATEGY.Component(context: Partial<InferredContextOf<T>>)
```

This strategy will help you pass the given context to the projected component.


### Template

```js
CONTEXT_STRATEGY.Template(context: Partial<InferredContextOf<T>>)
```

This strategy will help you pass the given context to the projected template.


## See Also

- [ProjectionStrategy](./Projection-Strategy.md)
