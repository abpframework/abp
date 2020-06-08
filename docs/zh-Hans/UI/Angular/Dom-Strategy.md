# DomStrategy

`DomStrategy` 是@abp/ng.core包暴露出的抽象类. 它的实例定义了如何将元素附加到DOM以及如何被其它类(如`LoadingStrategy`)使用.

## API

### 构造函数

```js
constructor(
  public target?: HTMLElement,
  public position?: InsertPosition
)
```

- `target` 是一个 HTMLElement (默认值: document.head_).
- `position` 定义将创建的元素放置在何处. 可以在[此处](https://developer.mozilla.org/en-US/docs/Web/API/Element/insertAdjacentElement)找到所有可能的 `position` 值(默认值: 'beforeend'_).

### insertElement

```js
insertElement(element: HTMLElement): void
```

该方法根据 `postion` 将给定 `元素` 插入到目标中.

## 预定义DOM策略

可以通过 `DOM_STRATEGY` 常量访问预定义的dom策略.


### AppendToBody

```js
DOM_STRATEGY.AppendToBody()
```

`insertElement` 将给定 `元素` 放在 `<body>` 的末尾.


### AppendToHead

```js
DOM_STRATEGY.AppendToHead()
```

`insertElement` 将给定 `元素` 放在 `<head>` 的末尾.

### PrependToHead

```js
DOM_STRATEGY.PrependToHead()
```

`insertElement` 将给定 `元素` 放在 `<head>` 的头部.


### AfterElement

```js
DOM_STRATEGY.AfterElement(target: HTMLElement)
```

`insertElement` 将给定 `元素` 放在 `target` 之后 (做为同级元素).

### BeforeElement

```js
DOM_STRATEGY.BeforeElement(target: HTMLElement)
```

`insertElement` 将给定 `元素` 放在 `target` 之前 (做为同级元素).

## 另请参阅

- [DomInsertionService](./Dom-Insertion-Service.md)
- [LazyLoadService](./Lazy-Load-Service.md)
- [LoadingStrategy](./Loading-Strategy.md)
- [ContentStrategy](./Content-Strategy.md)
- [ProjectionStrategy](./Projection-Strategy.md)
