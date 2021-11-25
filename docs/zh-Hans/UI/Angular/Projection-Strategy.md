# ProjectionStrategy

`ProjectionStrategy` 是@abp/ng.core包暴露出的抽象类. 有三种扩展它的投影策略: `ComponentProjectionStrategy`, `RootComponentProjectionStrategy` 和 `TemplateProjectionStrategy`. 它们实现相同的方法和属性,均可以帮助你定义内容投影的工作方式.

## ComponentProjectionStrategy

`ComponentProjectionStrategy` 是扩展 `ProjectionStrategy` 的类. 它使你可以将**组件投影到容器中**.

### constructor

```js
constructor(
  component: T,
  private containerStrategy: ContainerStrategy,
  private contextStrategy?: ContextStrategy,
)
```

- `component` 是你要投影的组件的类.
- `containerStrategy` 是在投影组件时将使用的 `ContainerStrategy`.
- `contextStrategy` 是将在投影组件上使用的 `ContextStrategy`. (默认值: None_)

请参阅[ContainerStrategy](./Container-Strategy.md)和[ContextStrategy](./Context-Strategy.md)文档以了解其用法.

### injectContent

```js
injectContent(injector: Injector): ComponentRef<T>
```

该方法准备容器,解析组件,设置其上下文并将其投影到容器中. 它返回一个 `ComponentRef` 实例,你应该保留该实例以便以后清除投影的组件.

## RootComponentProjectionStrategy

`RootComponentProjectionStrategy` 是扩展 `ProjectionStrategy` 的类. 它使你可以将**组件投影到文档中**,例如将其附加到 `<body>`.

### constructor

```js
constructor(
  component: T,
  private contextStrategy?: ContextStrategy,
  private domStrategy?: DomStrategy,
)
```

- `component` 是你要投影的组件的类.
- `contextStrategy` 是将在投影组件上使用的 `ContextStrategy`. (默认值: None_)
- `domStrategy` 是插入组件时将使用的 `DomStrategy`. (默认值: AppendToBody_)

请参阅[ContextStrategy](./Context-Strategy.md)和[DomStrategy](./Dom-Strategy.md)文档以了解其用法.

### injectContent

```js
injectContent(injector: Injector): ComponentRef<T>
```

该方法解析组件,设置其上下文并将其投影到文档中. 它返回一个 `ComponentRef` 实例,你应该保留该实例以便以后清除投影的组件.

## TemplateProjectionStrategy

`TemplateProjectionStrategy` 是扩展 `ProjectionStrategy` 的类.它使你可以将**模板投影到容器中**.

### constructor

```js
constructor(
  template: T,
  private containerStrategy: ContainerStrategy,
  private contextStrategy?: ContextStrategy,
)
```

- `template` 是你要投影的 `TemplateRef`.
- `containerStrategy` 是在投影组件时将使用的 `ContainerStrategy`.
- `contextStrategy` 是将在投影组件上使用的 `ContextStrategy`. (默认值: None_)

请参阅[ContainerStrategy](./Container-Strategy.md)和[ContextStrategy](./Context-Strategy.md)文档以了解其用法.

### injectContent

```js
injectContent(): EmbeddedViewRef<T>
```

该方法准备容器,并将模板及其定义的上下文一起投影到容器. 它返回一个 `EmbeddedViewRef` 实例,你应该保留该实例以便以后清除投影的模板.

## 预定义的投影策略

可以通过 `PROJECTION_STRATEGY` 常量访问预定义的投影策略.

### AppendComponentToBody

```js
PROJECTION_STRATEGY.AppendComponentToBody(
  component: T,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

将给定上下文设置到组件并将放置在文档中 `<body>` 标签的**末尾**.

### AppendComponentToContainer

```js
PROJECTION_STRATEGY.AppendComponentToContainer(
  component: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

将给定上下文设置到组件并将放置在容器的**末尾**.

### AppendTemplateToContainer

```js
PROJECTION_STRATEGY.AppendTemplateToContainer(
  templateRef: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

将给定上下文设置到模板并将其放置在容器的**末尾**.

### PrependComponentToContainer

```js
PROJECTION_STRATEGY.PrependComponentToContainer(
  component: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

将给定上下文设置到组件并将其放置在容器的**开头**.

### PrependTemplateToContainer

```js
PROJECTION_STRATEGY.PrependTemplateToContainer(
  templateRef: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

将给定上下文设置到模板并将其放置在容器的**开头**.


### ProjectComponentToContainer

```js
PROJECTION_STRATEGY.ProjectComponentToContainer(
  component: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

清除容器,将给定的上下文设置到组件并放在**已清除**的容器中.

### ProjectTemplateToContainer

```js
PROJECTION_STRATEGY.ProjectTemplateToContainer(
  templateRef: T,
  containerRef: ViewContainerRef,
  contextStrategy?: ComponentContextStrategy<T>,
)
```

清除容器,将给定的上下文设置到模板并放在**已清除**的容器中.

## 另请参阅

- [DomInsertionService](./Dom-Insertion-Service.md)
