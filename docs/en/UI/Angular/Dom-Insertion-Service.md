# How to Insert Scripts and Styles

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
    this.domInsertionService.insertContent(
      CONTENT_STRATEGY.AppendScriptToBody('alert()')
    );
  }
}
```

In the example above, `<script>alert()</script>` element will place at the **end** of `<body>`.

Please refer to [ContentStrategy](./Content-Strategy.md) to see all available content strategies and how you can build your own content strategy.

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
    this.domInsertionService.insertContent(
      CONTENT_STRATEGY.AppendStyleToHead('body {margin: 0;}')
    );
  }
}
```

In the example above, `<style>body {margin: 0;}</style>` element will place at the **end** of `<head>`.

Please refer to [ContentStrategy](./Content-Strategy.md) to see all available content strategies and how you can build your own content strategy.

### How to Project Components & Templates

If you pass a `ProjectionStrategy` as the first parameter of `projectContent` method, the `DomInsertionService` will resolve the projected component or template and place it at the designated target, such as containers or document body. If provided, it will also pass the component or the template a context.

```js
const componentRef = this.domInsertionService.projectContent(
  PROJECTION_STRATEGY.AppendComponentToBody(SomeOverlayComponent)
);
```

In the example above, `SomeOverlayComponent` component will placed at the **end** of `<body>` and a `ComponentRef` will be returned.

> You should keep the returned `ComponentRef` instance, as it is a reference to the projected component and you will need that reference to destroy the projected view and the component instance.

```js
const componentRef = this.domInsertionService.projectContent(
  PROJECTION_STRATEGY.ProjectComponentToContainer(
    SomeOverlayComponent,
    viewContainerRefOfTarget,
    { someProp: "SOME_VALUE" }
  )
);
```

In this example, the `viewContainerRefOfTarget`, which is a `ViewContainerRef` instance, will be cleared and `SomeOverlayComponent` component will placed inside it. Moreover, the given context will be applied, so `someProp` of the component will be set to `SOME_VALUE`.

Please refer to [ProjectionStrategy](./Projection-Strategy.md) to see all available projection strategies and how you can build your own projection strategy.

## API

### insertContent

```js
injectContent(injector: Injector): ComponentRef<C> | EmbeddedViewRef<C>
```

`injector` parameter is the `Injector` instance you can pass to the projected content. It is not used in `TemplateProjectionStrategy`.


## What's Next?

- [TrackByService](./Track-By-Service.md)
