# ProjectionStrategy

`ProjectionStrategy` is an abstract class exposed by @abp/ng.core package. There are three projection strategies extending it: `ComponentProjectionStrategy`, `RootComponentProjectionStrategy`, and `TemplateProjectionStrategy`. Implementing the same methods and properties, all of these strategies help you define how your content projection will work.



## ComponentProjectionStrategy

`ComponentProjectionStrategy` is a class that extends `ProjectionStrategy`. It lets you **project a component into a container**.


### constructor

```js
constructor(
  component: T,
  private containerStrategy: ContainerStrategy,
  private contextStrategy?: ContextStrategy,
)
```

- `component` is class of the component you would like to project.
- `containerStrategy` is the `ContainerStrategy` that will be used when projecting the component.
- `contextStrategy` is the `ContextStrategy` that will be used on the projected component. (_default: None_)

Please refer to [ContainerStrategy](./Container-Strategy.md) and [ContextStrategy](./Context-Strategy.md) documentation for their usage.


### injectContent

```js
injectContent(injector: Injector): ComponentRef<T>
```

This method prepares the container, resolves the component, sets its context, and projects it to the container. It returns a `ComponentRef` instance, which you should keep in order to clear projected components later on.



## RootComponentProjectionStrategy

`RootComponentProjectionStrategy` is a class that extends `ProjectionStrategy`. It lets you **project a component into the document**, such as appending it to `<body>`.


### constructor

```js
constructor(
  component: T,
  private contextStrategy?: ContextStrategy,
  private domStrategy?: DomStrategy,
)
```

- `component` is class of the component you would like to project.
- `contextStrategy` is the `ContextStrategy` that will be used on the projected component. (_default: None_)
- `domStrategy` is the `DomStrategy` that will be used when inserting component. (_default: AppendToBody_)

Please refer to [ContextStrategy](./Context-Strategy.md) and [DomStrategy](./Dom-Strategy.md) documentation for their usage.


### injectContent

```js
injectContent(injector: Injector): ComponentRef<T>
```

This method resolves the component, sets its context, and projects it to the document. It returns a `ComponentRef` instance, which you should keep in order to clear projected components later on.



## TemplateProjectionStrategy

`TemplateProjectionStrategy` is a class that extends `ProjectionStrategy`. It lets you **project a template into a container**.


### constructor

```js
constructor(
  template: T,
  private containerStrategy: ContainerStrategy,
  private contextStrategy?: ContextStrategy,
)
```

- `template` is `TemplateRef` you would like to project.
- `containerStrategy` is the `ContainerStrategy` that will be used when projecting the component.
- `contextStrategy` is the `ContextStrategy` that will be used on the projected component. (_default: None_)

Please refer to [ContainerStrategy](./Container-Strategy.md) and [ContextStrategy](./Context-Strategy.md) documentation for their usage.


### injectContent

```js
injectContent(): EmbeddedViewRef<T>
```

This method prepares the container, and projects the template together with the defined context to it. It returns an `EmbeddedViewRef`, which you should keep in order to clear projected templates later on.



## Predefined Projection Strategies

Predefined projection strategies are accessible via `PROJECTION_STRATEGY` constant.


### AppendComponentToBody

```js
PROJECTION_STRATEGY.AppendComponentToBody(
  component: T,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

Sets given context to the component and places it at the **end** of `<body>` tag in the document.


### AppendComponentToContainer

```js
PROJECTION_STRATEGY.AppendComponentToContainer(
  component: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

Sets given context to the component and places it at the **end** of the container.


### AppendTemplateToContainer

```js
PROJECTION_STRATEGY.AppendTemplateToContainer(
  templateRef: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

Sets given context to the template and places it at the **end** of the container.


### PrependComponentToContainer

```js
PROJECTION_STRATEGY.PrependComponentToContainer(
  component: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

Sets given context to the component and places it at the **beginning** of the container.


### PrependTemplateToContainer

```js
PROJECTION_STRATEGY.PrependTemplateToContainer(
  templateRef: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

Sets given context to the template and places it at the **beginning** of the container.


### ProjectComponentToContainer

```js
PROJECTION_STRATEGY.ProjectComponentToContainer(
  component: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

Clears the container, sets given context to the component, and places it **in the cleared** the container.


### ProjectTemplateToContainer

```js
PROJECTION_STRATEGY.ProjectTemplateToContainer(
  templateRef: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

Clears the container, sets given context to the template, and places it **in the cleared** the container.


## See Also

- [DomInsertionService](./Dom-Insertion-Service.md)
