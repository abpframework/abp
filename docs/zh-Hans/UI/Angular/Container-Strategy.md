# ContainerStrategy

`ContainerStrategy` 是 @abp/ng.core 包暴露出的抽象类. 有两种扩展容器扩展策略: `ClearContainerStrategy` 和 `InsertIntoContainerStrategy`. 它们实现了相同的方法和属性,这两种策略都可以帮助你定义容器的准备方式和内容的投影位置.

## API

`ClearContainerStrategy` 是一个扩展了 `ContainerStrategy` 的类. 它允许你**将内容投影之前清除容器**.

### 构造函数

```js
constructor(
  public containerRef: ViewContainerRef,
  private index?: number, // works only in InsertIntoContainerStrategy
)
```

- `containerRef` 是在投影内容时使用的 `ViewContainerRef`.

### getIndex

```js
getIndex(): number
```

该方法返回被 `0` 和 `containerRef` `length` 限制的给定索引. 对于没有索引的策略,它返回`0`.

### prepare

```js
prepare(): void
```

此方法在内容投影之前调用. 基于使用的容器策略,它要么清除容器,要么什么都不做(空操作).

## ClearContainerStrategy

`ClearContainerStrategy` 是一个扩展了 `ContainerStrategy` 的类. 它允许你**将内容投影之前清除容器**.

## InsertIntoContainerStrategy

`InsertIntoContainerStrategy` 是一个扩展了 `ContainerStrategy` 的类. 它允许你**将内容投影到容器中的特定节点索引上**.

## 预定义的容器策略

可以通过 `CONTAINER_STRATEGY` 常量访问预定义的容器策略.

### Clear

```js
CONTAINER_STRATEGY.Clear(containerRef: ViewContainerRef)
```

在内容投影之前清除给定的容器.


### Append

```js
CONTAINER_STRATEGY.Append(containerRef: ViewContainerRef)
```

将投影内容附加到容器中.


### Prepend

```js
CONTAINER_STRATEGY.Prepend(containerRef: ViewContainerRef)
```

将投影的内容预先写入容器中.

### Insert

```js
CONTAINER_STRATEGY.Insert(
  containerRef: ViewContainerRef,
  index: number,
)
```

将投影内容按照给定的索引(在`0` 到 `containerRef`的长度之间)插入到容器中.

## 另请参阅

- [ProjectionStrategy](./Projection-Strategy.md)
