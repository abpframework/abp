# ContentStrategy

`ContentStrategy` is an abstract class exposed by @abp/ng.core package. It helps you create inline scripts or styles.

## API


### constructor

```js
constructor(
  public content: string,
  protected domStrategy?: DomStrategy,
  protected contentSecurityStrategy?: ContentSecurityStrategy,
  protected options?: ElementOptions = {},
)
```

- `content` is set to `<script>` and `<style>` elements as `textContent` property.
- `domStrategy` is the `DomStrategy` that will be used when inserting the created element. (_default: AppendToHead_)
- `contentSecurityStrategy` is the `ContentSecurityStrategy` that will be used on the created element before inserting it. (_default: None_)
- `options` can be used to pass any option to the element that will be created. e.g: `{ id: "some-id" }` (_default: empty object_)

Please refer to [DomStrategy](./Dom-Strategy.md) and [ContentSecurityStrategy](./Content-Security-Strategy.md) documentation for their usage.


### createElement

```js
createElement(): HTMLScriptElement | HTMLStyleElement
```

This method creates and returns a `<script>` or `<style>` element with `content` set as `textContent`.


### insertElement

```js
insertElement(): void
```

This method creates and inserts a `<script>` or `<style>` element.


## ScriptContentStrategy

`ScriptContentStrategy` is a class that extends `ContentStrategy`. It lets you **insert a `<script>` element to the DOM**.

## StyleContentStrategy

`StyleContentStrategy` is a class that extends `ContentStrategy`. It lets you **insert a `<style>` element to the DOM**.


## Predefined Content Strategies

Predefined content strategies are accessible via `CONTENT_STRATEGY` constant.


### AppendScriptToBody

```js
CONTENT_STRATEGY.AppendScriptToBody(content: string, options?: ElementOptions<HTMLScriptElement>)
```

Creates a `<script>` element with the given content and places it at the **end** of `<body>` tag in the document.


### AppendScriptToHead

```js
CONTENT_STRATEGY.AppendScriptToHead(content: string, options?: ElementOptions<HTMLScriptElement>)
```

Creates a `<script>` element with the given content and places it at the **end** of `<head>` tag in the document.


### AppendStyleToHead

```js
CONTENT_STRATEGY.AppendStyleToHead(content: string, options?: ElementOptions<HTMLStyleElement>)
```

Creates a `<style>` element with the given content and places it at the **end** of `<head>` tag in the document.


### PrependStyleToHead

```js
CONTENT_STRATEGY.PrependStyleToHead(content: string, options?: ElementOptions<HTMLStyleElement>)
```

Creates a `<style>` element with the given content and places it at the **beginning** of `<head>` tag in the document.


## See Also

- [DomInsertionService](./Dom-Insertion-Service.md)