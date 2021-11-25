# Projecting Angular Content

You can use the `ContentProjectionService` in @abp/ng.core package in order to project content in an easy and explicit way.

## Getting Started

You do not have to provide the `ContentProjectionService` at module or component level, because it is already **provided in root**. You can inject and start using it immediately in your components, directives, or services.

```js
import { ContentProjectionService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private contentProjectionService: ContentProjectionService) {}
}
```

## Usage

You can use the `projectContent` method of `ContentProjectionService` to render components and templates dynamically in your project.

### How to Project Components to Root Level

If you pass a `RootComponentProjectionStrategy` as the first parameter of `projectContent` method, the `ContentProjectionService` will resolve the projected component and place it at the root level. If provided, it will also pass the component a context.

```js
const strategy = PROJECTION_STRATEGY.AppendComponentToBody(
  SomeOverlayComponent,
  { someOverlayProp: "SOME_VALUE" }
);

const componentRef = this.contentProjectionService.projectContent(strategy);
```

In the example above, `SomeOverlayComponent` component will placed at the **end** of `<body>` and a `ComponentRef` will be returned. Additionally, the given context will be applied, so `someOverlayProp` of the component will be set to `SOME_VALUE`.

> You should keep the returned `ComponentRef` instance, as it is a reference to the projected component and you will need that reference to destroy the projected view and the component instance.

### How to Project Components and Templates into a Container

If you pass a `ComponentProjectionStrategy` or `TemplateProjectionStrategy` as the first parameter of `projectContent` method, and a `ViewContainerRef` as the second parameter of that strategy, the `ContentProjectionService` will project the component or template to the given container. If provided, it will also pass the component or the template a context.

```js
const strategy = PROJECTION_STRATEGY.ProjectComponentToContainer(
  SomeComponent,
  viewContainerRefOfTarget,
  { someProp: "SOME_VALUE" }
);

const componentRef = this.contentProjectionService.projectContent(strategy);
```

In this example, the `viewContainerRefOfTarget`, which is a `ViewContainerRef` instance, will be cleared and `SomeComponent` component will be placed inside it. In addition, the given context will be applied and `someProp` of the component will be set to `SOME_VALUE`.

> You should keep the returned `ComponentRef` or `EmbeddedViewRef`, as they are a reference to the projected content and you will need them to destroy it when necessary.

Please refer to [ProjectionStrategy](./Projection-Strategy.md) to see all available projection strategies and how you can build your own projection strategy.

## API

### projectContent

```js
projectContent<T extends Type<any> | TemplateRef<any>>(
    projectionStrategy: ProjectionStrategy<T>,
    injector = this.injector,
): ComponentRef<C> | EmbeddedViewRef<C>
```

- `projectionStrategy` parameter is the primary focus here and is explained above.
- `injector` parameter is the `Injector` instance you can pass to the projected content. It is not used in `TemplateProjectionStrategy`.
