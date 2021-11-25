# ContextStrategy

`ContextStrategy` 是@abp/ng.core包暴露出的抽象类. 有三种扩展容器扩展策略: `ComponentContextStrategy` , `TemplateContextStrategy` 和 `NoContextStrategy`. 它们实现了相同的方法和属性,这三种策略都可以帮助你获得定义投影内容上下文

## ComponentContextStrategy

`ComponentContextStrategy` 是扩展了 `ContextStrategy` 的类, 它允许你**将上下文传递给一个投影组件**.

### 构造函数

```js
constructor(public context: Partial<InferredInstanceOf<T>>) {}
```

- `T` 在这里引用组件类型, 例. `Type<C>`.
- `InferredInstanceOf` 是 @abp/ng.core 包暴露的实用类型. 它可以推断组件.
- `context` 是将映射到投影组件的属性.

### setContext

```js
setContext(componentRef: ComponentRef<InferredInstanceOf<T>>): Partial<InferredInstanceOf<T>>
```

该方法将上下文的每个属性映射到具有相同名称的组件属性,并调用更改检测.映射后返回上下文.

## TemplateContextStrategy

`TemplateContextStrategy` 是扩展了 `ContextStrategy` 的类. 它允许你**将上下文传递到投影模板**.

### 构造函数

```js
constructor(public context: Partial<InferredContextOf<T>>) {}
```

- `T` 在这里引用组件类型, 例. `Type<C>`.
- `InferredInstanceOf` 是 @abp/ng.core 包暴露的实用类型. 它可以推断组件.
- `context` 是将映射到投影组件的属性.

### setContext

```js
setContext(): Partial<InferredContextOf<T>>
```

此方法不执行任何操作它仅返回上下文,因为模板上下文没有被映射. 而是作为参数传递给 `createEmbeddedView` 方法.

## NoContextStrategy

`NoContextStrategy` 是扩展了 `ContextStrategy` 的类. 它允许你**跳过将任何上下文传递到投影内容**.

### 构造函数

```js
constructor()
```

不像其它的策略, `NoContextStrategy` 构造函数没有参数.

### setContext

```js
setContext(): undefined
```

由于没有上下文,方法没有参数并且返回`undefined`.

## 预定义上下文策略

可以通过 `CONTEXT_STRATEGY` 常量访问预定义的上下文策略.

### None

```js
CONTEXT_STRATEGY.None()
```

该策略不会将任何上下文传递到投影内容.

### Component

```js
CONTEXT_STRATEGY.Component(context: Partial<InferredContextOf<T>>)
```

该策略将帮助你将给定的上下文传递到投影组件.


### Template

```js
CONTEXT_STRATEGY.Template(context: Partial<InferredContextOf<T>>)
```

该策略将帮助你将给定的上下文传递到投影模板.

## 另请参阅

- [ProjectionStrategy](./Projection-Strategy.md)
