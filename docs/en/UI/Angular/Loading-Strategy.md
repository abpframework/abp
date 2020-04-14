# LoadingStrategy

`LoadingStrategy` is an abstract class exposed by @abp/ng.core package. There are two loading strategies extending it: `ScriptLoadingStrategy` and `StyleLoadingStrategy`. Implementing the same methods and properties, both of these strategies help you define how your lazy loading will work.




## API

### constructor

```js
constructor(
  public path: string,
  protected domStrategy?: DomStrategy,
  protected crossOriginStrategy?: CrossOriginStrategy
)
```

- `path` is set to `<script>` elements as `src` and `<link>` elements as `href` attribute.
- `domStrategy` is the `DomStrategy` that will be used when inserting the created element. (_default: AppendToHead_)
- `crossOriginStrategy` is the `CrossOriginStrategy` that will be used on the created element before inserting it. (_default: Anonymous_)

Please refer to [DomStrategy](./Dom-Strategy.md) and [CrossOriginStrategy](./Cross-Origin-Strategy.md) documentation for their usage.


### createElement

```js
createElement(): HTMLScriptElement | HTMLLinkElement
```

This method creates and returns a `<script>` or `<link>` element with `path` set as `src` or `href`.


### createStream

```js
createStream(): Observable<Event>
```

This method creates and returns an observable stream that emits on success and throws on error.



## ScriptLoadingStrategy

`ScriptLoadingStrategy` is a class that extends `LoadingStrategy`. It lets you **lazy load a script**.



## StyleLoadingStrategy

`StyleLoadingStrategy` is a class that extends `LoadingStrategy`. It lets you **lazy load a style**.



## Predefined Loading Strategies

Predefined loading strategies are accessible via `LOADING_STRATEGY` constant.


### AppendAnonymousScriptToHead

```js
LOADING_STRATEGY.AppendAnonymousScriptToHead(src: string, integrity?: string)
```

Sets given paremeters and `crossorigin="anonymous"` as attributes of created `<script>` element and places it at the **end** of `<head>` tag in the document.


### PrependAnonymousScriptToHead

```js
LOADING_STRATEGY.PrependAnonymousScriptToHead(src: string, integrity?: string)
```

Sets given paremeters and `crossorigin="anonymous"` as attributes of created `<script>` element and places it at the **beginning** of `<head>` tag in the document.


### AppendAnonymousScriptToBody

```js
LOADING_STRATEGY.AppendAnonymousScriptToBody(src: string, integrity?: string)
```

Sets given paremeters and `crossorigin="anonymous"` as attributes of created `<script>` element and places it at the **end** of `<body>` tag in the document.


### AppendAnonymousStyleToHead

```js
LOADING_STRATEGY.AppendAnonymousStyleToHead(href: string, integrity?: string)
```

Sets given paremeters and `crossorigin="anonymous"` as attributes of created `<style>` element and places it at the **end** of `<head>` tag in the document.


### PrependAnonymousStyleToHead

```js
LOADING_STRATEGY.PrependAnonymousStyleToHead(href: string, integrity?: string)
```

Sets given paremeters and `crossorigin="anonymous"` as attributes of created `<style>` element and places it at the **beginning** of `<head>` tag in the document.


## See Also

- [LazyLoadService](./Lazy-Load-Service.md)
