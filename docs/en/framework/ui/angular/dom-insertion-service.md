# Inserting Scripts & Styles to DOM

You can use the `DomInsertionService` in @abp/ng.core package in order to insert scripts and styles in an easy and explicit way.

## Getting Started

You do not have to provide the `DomInsertionService` at module or component level, because it is already **provided in root**. You can inject and start using it immediately in your components, directives, or services.

```js
import { DomInsertionService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private domInsertionService: DomInsertionService) {}
}
```

## Usage

You can use the `insertContent` method of `DomInsertionService` to create a `<script>` or `<style>` element with given content in the DOM at the desired position. There is also the `projectContent` method for dynamically rendering components and templates.

### How to Insert Scripts

The first parameter of `insertContent` method expects a `ContentStrategy`. If you pass a `ScriptContentStrategy` instance, the `DomInsertionService` will create a `<script>` element with given `content` and place it in the designated DOM position.

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

In the example above, `<script>alert()</script>` element will place at the **end** of `<body>` and `scriptElement` will be an `HTMLScriptElement`.

Please refer to [ContentStrategy](./Content-Strategy.md) to see all available content strategies and how you can build your own content strategy.

> Important Note: `DomInsertionService` does not insert the same content twice. In order to add a content again, you first should remove the old content using `removeContent` method.

### How to Insert Styles

If you pass a `StyleContentStrategy` instance as the first parameter of `insertContent` method, the `DomInsertionService` will create a `<style>` element with given `content` and place it in the designated DOM position.

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

In the example above, `<style>body {margin: 0;}</style>` element will place at the **end** of `<head>` and `styleElement` will be an `HTMLStyleElement`.

Please refer to [ContentStrategy](./Content-Strategy.md) to see all available content strategies and how you can build your own content strategy.

> Important Note: `DomInsertionService` does not insert the same content twice. In order to add a content again, you first should remove the old content using `removeContent` method.

### How to Remove Inserted Scripts & Styles

If you pass the inserted `HTMLScriptElement` or `HTMLStyleElement` element as the first parameter of `removeContent` method, the `DomInsertionService` will remove the given element.

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

In the example above, `<style>body {margin: 0;}</style>` element **will be removed** from `<head>` when the component is destroyed.

## API

### insertContent

```js
insertContent<T extends HTMLScriptElement | HTMLStyleElement>(
  contentStrategy: ContentStrategy<T>,
): T
```

- `contentStrategy` parameter is the primary focus here and is explained above.
- returns `HTMLScriptElement` or `HTMLStyleElement` based on given strategy.

### removeContent

```js
removeContent(element: HTMLScriptElement | HTMLStyleElement): void
```

- `element` parameter is the inserted `HTMLScriptElement` or `HTMLStyleElement` element, which was returned by `insertContent` method.

### has

```js
has(content: string): boolean
```

The `has` method returns a boolean value that indicates the given content has already been added to the DOM or not.

- `content` parameter is the content of the inserted `HTMLScriptElement` or `HTMLStyleElement` element.
