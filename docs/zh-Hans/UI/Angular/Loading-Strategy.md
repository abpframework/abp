# LoadingStrategy

`LoadingStrategy` 是@abp/ng.core包暴露的抽象类. 扩展它的有两种加载策略: `ScriptLoadingStrategy` 和 `StyleLoadingStrategy`. 它们实现相同的方法和属性,这两种策略都可以帮助你定义延迟加载的工作方式.

## API

### constructor

```js
constructor(
  public path: string,
  protected domStrategy?: DomStrategy,
  protected crossOriginStrategy?: CrossOriginStrategy
)
```

- `path` 将 `<script>` 元素做为 `src` 与 `<link>` 元素做为 `href` 属性.
- `domStrategy` 是在插入创建的元素时将使用的 `DomStrategy`. (默认值: AppendToHead_)
- `crossOriginStrategy` 是 `CrossOriginStrategy`,它在插入元素之前在创建的元素上使用. (默认值: Anonymous_)

请参阅[DomStrategy](./Dom-Strategy.md)和[CrossOriginStrategy](./Cross-Origin-Strategy.md)文档以了解其用法.

### createElement

```js
createElement(): HTMLScriptElement | HTMLLinkElement
```

该方法创建并返回 `path` 设置为 `src` 或 `href` 的 `<script>` 或 `<link>` 的元素.

### createStream

```js
createStream(): Observable<Event>
```

该方法创建并返回一个observable流,该流在成功时发出,在错误时抛出.

## ScriptLoadingStrategy

`ScriptLoadingStrategy` 是扩展 `LoadingStrategy` 的类. 它使你可以**延迟加载脚本**.

## StyleLoadingStrategy

`StyleLoadingStrategy` 是扩展 `LoadingStrategy` 的类. 它使你可以**延迟加载样式**.

## 预定义的加载策略

可通过 `LOADING_STRATEGY` 常量访问预定义的加载策略.

### AppendAnonymousScriptToHead

```js
LOADING_STRATEGY.AppendAnonymousScriptToHead(src: string, integrity?: string)
```

将给定的参数和 `crossorigin="anonymous"` 设置为创建的 `<script>` 元素的属性,并放置在文档中 `<head>` 标签的**末尾**.

### PrependAnonymousScriptToHead

```js
LOADING_STRATEGY.PrependAnonymousScriptToHead(src: string, integrity?: string)
```

将给定的参数和 `crossorigin="anonymous"` 设置为创建的 `<script>` 元素的属性,并放置在文档中 `<head>` 标签的**开始**.

### AppendAnonymousScriptToBody

```js
LOADING_STRATEGY.AppendAnonymousScriptToBody(src: string, integrity?: string)
```

将给定的参数和 `crossorigin="anonymous"` 设置为创建的 `<script>` 元素的属性,并放置在文档中 `<body>` 标签的**末尾**.


### AppendAnonymousStyleToHead

```js
LOADING_STRATEGY.AppendAnonymousStyleToHead(href: string, integrity?: string)
```

将给定的参数和 `crossorigin="anonymous"` 设置为创建的 `<style>` 元素的属性,并放置在文档中 `<head>` 标签的**末尾**.


### PrependAnonymousStyleToHead

```js
LOADING_STRATEGY.PrependAnonymousStyleToHead(href: string, integrity?: string)
```

将给定的参数和 `crossorigin="anonymous"` 设置为创建的 `<style>` 元素的属性,并放置在文档中 `<head>` 标签的**开始**.

## 另请参阅

- [LazyLoadService](./Lazy-Load-Service.md)
