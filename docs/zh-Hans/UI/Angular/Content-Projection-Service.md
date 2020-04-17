# 内容投影

你可以使用位于@abp/ng.core包中的 `ContentProjectionService` 简单明确的投影内容.

## 入门

你不必在模块或组件级别提供 `ContentProjectionService`,因为它已经在**根中提供了**. 你可以在组件中注入并开始使用它. 为了获得更好的类型支持,你可以将迭代项目的类型传递给它.

```js
import { ContentProjectionService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private contentProjectionService: ContentProjectionService) {}
}
```

## 用法

你可以使用 `ContentProjectionService` 的 `projectContent` 方法在你的项目中动态的渲染组件和模板.

### 如何将组件投影到根级别

如果将 `RootComponentProjectionStrategy` 做为 `projectContent` 方法的第一个参数,那么 `ContentProjectionService` 会解析投影组件并放在根级别,它还为组件传递上下文.

```js
const strategy = PROJECTION_STRATEGY.AppendComponentToBody(
  SomeOverlayComponent,
  { someOverlayProp: "SOME_VALUE" }
);

const componentRef = this.contentProjectionService.projectContent(strategy);
```

在上面的示例中, `SomeOverlayComponent` 组件放置在 `<body>` 的**末尾**并返回 `ComponentRef`. 另外将应用给定的上下文,因此组件的 `someOverlayProp` 被设置为 `SOME_VALUE`.

> 你应该总是返回 `ComponentRef` 实例,因为它是对投影组件的引用,在你需要时使用该引用销毁投影视图和组件实例.

### 如何将组件和模板投影到容器中

如果将 `ComponentProjectionStrategy` 或 `TemplateProjectionStrategy` 做为 `projectContent` 方法的第一个参数,并且传递 `ViewContainerRef` 做为策略的第二个参数传递. 那么 `ContentProjectionService` 把组件或模板投影到给定的容器中,它还为组件或模板传递上下文.

```js
const strategy = PROJECTION_STRATEGY.ProjectComponentToContainer(
  SomeComponent,
  viewContainerRefOfTarget,
  { someProp: "SOME_VALUE" }
);

const componentRef = this.contentProjectionService.projectContent(strategy);
```

在上面的示例中,`viewContainerRefOfTarget`(它是一个`ViewContainerRef` 实例)将被清除,并把 `SomeComponent` 组件放在其中. 另外将应用给定的上下文,因此组件的 `someProp` 被设置为 `SOME_VALUE`.

> 你应该总是返回 `ComponentRef` 或 `EmbeddedViewRef` ,因为它是对投影内容的引用,在你需要时使用该引用销毁它们.

请参考[ProjectionStrategy](./Projection-Strategy.md)查看所有可用的投影策略以及如何构建自己的投影策略.

## API

### projectContent

```js
projectContent<T extends Type<any> | TemplateRef<any>>(
    projectionStrategy: ProjectionStrategy<T>,
    injector = this.injector,
): ComponentRef<C> | EmbeddedViewRef<C>
```

- `projectionStrategy` 参数是此处的要点,在上面进行了说明.
- `injector` 参数是 `Injector` 实例,你可以传递到投影内容. 在 `TemplateProjectionStrategy` 并没有使用到它.

## 下一步是什么?

- [TrackByService](./Track-By-Service.md)