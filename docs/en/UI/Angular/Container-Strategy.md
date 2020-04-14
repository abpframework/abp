# ContainerStrategy

`ContainerStrategy` is an abstract class exposed by @abp/ng.core package. There are two container strategies extending it: `ClearContainerStrategy` and `InsertIntoContainerStrategy`. Implementing the same methods and properties, both of these strategies help you define how your containers will be prepared and where your content will be projected.



## API

`ClearContainerStrategy` is a class that extends `ContainerStrategy`. It lets you **clear a container before projecting content in it**.


### constructor

```js
constructor(
  public containerRef: ViewContainerRef,
  private index?: number, // works only in InsertIntoContainerStrategy
)
```

- `containerRef` is the `ViewContainerRef` that will be used when projecting the content.


### getIndex

```js
getIndex(): number
```

This method return the given index clamped by `0` and `length` of the `containerRef`. For strategies without an index, it returns `0`.


### prepare

```js
prepare(): void
```

This method is called before content projection. Based on used container strategy, it either clears the container or does nothing (noop).



## ClearContainerStrategy

`ClearContainerStrategy` is a class that extends `ContainerStrategy`. It lets you **clear a container before projecting content in it**.



## InsertIntoContainerStrategy

`InsertIntoContainerStrategy` is a class that extends `ContainerStrategy`. It lets you **project your content at a specific node index in the container**.



## Predefined Container Strategies

Predefined container strategies are accessible via `CONTAINER_STRATEGY` constant.


### Clear

```js
CONTAINER_STRATEGY.Clear(containerRef: ViewContainerRef)
```

Clears given container before content projection.


### Append

```js
CONTAINER_STRATEGY.Append(containerRef: ViewContainerRef)
```

Projected content will be appended to the container.


### Prepend

```js
CONTAINER_STRATEGY.Prepend(containerRef: ViewContainerRef)
```

Projected content will be prepended to the container.


### Insert

```js
CONTAINER_STRATEGY.Insert(
  containerRef: ViewContainerRef,
  index: number,
)
```

Projected content will be inserted into to the container at given index (clamped by `0` and `length` of the `containerRef`).


## See Also

- [ProjectionStrategy](./Projection-Strategy.md)
