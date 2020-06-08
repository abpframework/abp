# Dom插入(Scripts与Styles)

你可以使用@abp/ng.core包提供的 `DomInsertionService` 以简单的方式的插入脚本与样式.

## 入门

你不必在模块或组件级别提供 `DomInsertionService` ,因为它已经在**根中**提供. 你可以在组件,指令或服务中直接注入并使用它.

```js
import { DomInsertionService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private domInsertionService: DomInsertionService) {}
}
```

## 用法

你可以使用 `DomInsertionService` 提供的 `insertContent` 方法去创建一个 `<script>` 或  `<style>` 元素到DOM的指定位置. 还有 `projectContent` 方法用于渲染组件和模板.

### 如何插入Scripts

`insertContent` 方法的第一个参数需要一个 `ContentStrategy`. 如果传递 `ScriptContentStrategy` 实例, `DomInsertionService` 将创建具有给定内容的 `<script>` 元素并放置在指定的DOM位置.

```js
import { DomInsertionService, CONTENT_STRATEGY } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private domInsertionService: DomInsertionService) {}

  ngOnInit() {
    const scriptElement = this.domInsertionService.insertContent(
      CONTENT_STRATEGY.AppendScriptToBody('alert()')
    );
  }
}
```

在上面的示例中,将 `<script>alert()</script>` 元素放置在 `<body>`的末尾, `scriptElement` 类型是一个 `HTMLScriptElement`.

请参考[ContentStrategy](./Content-Strategy.md)查看所有可用的内容策略以及如何构建自己的内容策略.

> 重要说明: `DomInsertionService` 不会两次插入相同的内容. 为了再次添加内容你首先应该使用 `removeContent` 方法删除旧内容.

### 如何插入Styles

`insertContent` 方法的第一个参数需要一个 `ContentStrategy`. 如果传递 `StyleContentStrategy` 实例, `DomInsertionService` 将创建具有给定内容的 `<style>` 元素并放置在指定的DOM位置.

```js
import { DomInsertionService, CONTENT_STRATEGY } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private domInsertionService: DomInsertionService) {}

  ngOnInit() {
    const styleElement = this.domInsertionService.insertContent(
      CONTENT_STRATEGY.AppendStyleToHead('body {margin: 0;}')
    );
  }
}
```

在上面的示例中,将 `<style>body {margin: 0;}</style>` 元素放置在 `<head>`的末尾, `styleElement` 类型是一个 `HTMLStyleElement`.

请参考[ContentStrategy](./Content-Strategy.md)查看所有可用的内容策略以及如何构建自己的内容策略.
.
> 重要说明: `DomInsertionService` 不会两次插入相同的内容. 为了再次添加内容你首先应该使用 `removeContent` 方法删除旧内容.

### 如何删除已插入的 Scripts & Styles

如果你传递 `HTMLScriptElement` 或 `HTMLStyleElement` 做为 `removeContent` 方法的第一个参数, `DomInsertionService` 将删除给定的元素.

```js
import { DomInsertionService, CONTENT_STRATEGY } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  private styleElement: HTMLStyleElement;

  constructor(private domInsertionService: DomInsertionService) {}

  ngOnInit() {
    this.styleElement = this.domInsertionService.insertContent(
      CONTENT_STRATEGY.AppendStyleToHead('body {margin: 0;}')
    );
  }

  ngOnDestroy() {
    this.domInsertionService.removeContent(this.styleElement);
  }
}
```

在上面的示例中,销毁组件时,将从 `<head>` 中删除 `<style>body {margin: 0;}</style>` 元素.

## API

### insertContent

```js
insertContent<T extends HTMLScriptElement | HTMLStyleElement>(
  contentStrategy: ContentStrategy<T>,
): T
```

- `contentStrategy` 是方法的重要参数,已经在上方进行说明.
- 根据给定的策略返回 `HTMLScriptElement` 或 `HTMLStyleElement`.

### removeContent

```js
removeContent(element: HTMLScriptElement | HTMLStyleElement): void
```

- `element` 参数是已插入的 `HTMLScriptElement` 或 `HTMLStyleElement` 元素,它们应由 `insertContent` 方法返回.

### has

```js
has(content: string): boolean
```

`has` 返回一个布尔值,用于表示给定的内容是否插入到DOM.

- `content` 参数是 `HTMLScriptElement` 或 `HTMLStyleElement` 元素的内容.

## 下一步是什么?

- [ContentProjectionService](./Content-Projection-Service.md)