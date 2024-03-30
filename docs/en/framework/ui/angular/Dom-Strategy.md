# DomStrategy

`DomStrategy` is a class exposed by @abp/ng.core package. Its instances define how an element will be attached to the DOM and are consumed by other classes such as `LoadingStrategy`.


## API


### constructor

```js
constructor(
  public target?: HTMLElement,
  public position?: InsertPosition
)
```

- `target` is an HTMLElement (_default: document.head_).
- `position` defines where the created element will be placed. All possible values of `position` can be found [here](https://developer.mozilla.org/en-US/docs/Web/API/Element/insertAdjacentElement) (_default: 'beforeend'_).


### insertElement

```js
insertElement(element: HTMLElement): void
```

This method inserts given `element` to `target` based on the `position`.



## Predefined Dom Strategies

Predefined dom strategies are accessible via `DOM_STRATEGY` constant.


### AppendToBody

```js
DOM_STRATEGY.AppendToBody()
```

`insertElement` will place the given `element` at the end of `<body>`.


### AppendToHead

```js
DOM_STRATEGY.AppendToHead()
```

`insertElement` will place the given `element` at the end of `<head>`.


### PrependToHead

```js
DOM_STRATEGY.PrependToHead()
```

`insertElement` will place the given `element` at the beginning of `<head>`.


### AfterElement

```js
DOM_STRATEGY.AfterElement(target: HTMLElement)
```

`insertElement` will place the given `element` after (as a sibling to) the `target`.


### BeforeElement

```js
DOM_STRATEGY.BeforeElement(target: HTMLElement)
```

`insertElement` will place the given `element` before (as a sibling to) the `target`.




## See Also

- [DomInsertionService](./Dom-Insertion-Service.md)
- [LazyLoadService](./Lazy-Load-Service.md)
- [LoadingStrategy](./Loading-Strategy.md)
- [ContentStrategy](./Content-Strategy.md)
- [ProjectionStrategy](./Projection-Strategy.md)
