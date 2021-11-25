# ContentStrategy

`ContentStrategy` 是@abp/ng.core包暴露出的抽象类. 它可以帮助你创建内联脚本或样式.

## API

### 构造函数

```js
constructor(
  public content: string,
  protected domStrategy?: DomStrategy,
  protected contentSecurityStrategy?: ContentSecurityStrategy
)
```

- `content` 是被设置为 `<script>` 和 `<style>` 元素的 `textContent` 属性.
- `domStrategy` 是在插入创建的元素时将使用的 `DomStrategy` . (默认值: AppendToHead_)
- `contentSecurityStrategy` 是 `ContentSecurityStrategy`, 在创建的元素插入使用它它.  (默认值: None_)

请参考[DomStrategy](./Dom-Strategy.md)和[ContentSecurityStrategy](./Content-Security-Strategy.md)文档了解它们的用法.

### createElement

```js
createElement(): HTMLScriptElement | HTMLStyleElement
```

该方法创建并且返回 `<script>` 或 `<style>` 元素, 将 `content` 设置为 `textContent`.

### insertElement

```js
insertElement(): void
```

该方法创建并且插入一个 `<script>` 或 `<style>` 元素.

## ScriptContentStrategy

`ScriptContentStrategy` 是扩展了 `ContentStrategy` 的类. 你可以使用它将 **`<script>`元素插入DOM**.

## StyleContentStrategy

`StyleContentStrategy` 是扩展了 `ContentStrategy` 的类. 你可以使用它将 **`<style>`元素插入DOM**.

## 预定义内容策略

预定义的内容策略可以通过 `CONTENT_STRATEGY` 常量访问.

### AppendScriptToBody

```js
CONTENT_STRATEGY.AppendScriptToBody(content: string)
```

创建具有给定内容的 `<script>` 元素, 并放置在文档中`<body>`标记的**末尾**.

### AppendScriptToHead

```js
CONTENT_STRATEGY.AppendScriptToHead(content: string)
```

创建具有给定内容的 `<script>` 元素, 并放置在文档中`<head>`标签的**末尾**.

### AppendStyleToHead

```js
CONTENT_STRATEGY.AppendStyleToHead(content: string)
```

创建具有给定内容的 `<style>` 元素, 并放置在文档中`<head>`标签的**末尾**.

### PrependStyleToHead

```js
CONTENT_STRATEGY.PrependStyleToHead(content: string)
```

创建具有给定内容的 `<style>` 元素, 并放置在文档中`<head>`标签的**头部**.

## 另请参阅

- [DomInsertionService](./Dom-Insertion-Service.md)