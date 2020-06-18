# 如何懒加载 Scripts 与 Styles

你可以使用@abp/ng.core包中的 `LazyLoadService` 以简单明了的方式延迟加载脚本和样式.

## 入门

你不必在模块或组件/指令级别提供 `LazyLoadService`,因为它已经在**根中**中提供了. 你可以在组件,指令或服务中注入并使用它.

```js
import { LazyLoadService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private lazyLoadService: LazyLoadService) {}
}
```

## 用法

你可以使用 `LazyLoadService` 的 `load` 方法在DOM中的所需位置创建 `<script>` 或 `<link>` 元素并强制浏览器下载目标资源.

### 如何加载 Scripts

`load` 方法的第一个参数需要一个 `LoadingStrategy`. 如果传递 `ScriptLoadingStrategy` 实例,`LazyLoadService` 将使用给定的 `src` 创建一个 `<script>` 元素并放置在指定的DOM位置.

```js
import { LazyLoadService, LOADING_STRATEGY } from '@abp/ng.core';

@Component({
  template: `
    <some-component *ngIf="libraryLoaded$ | async"></some-component>
  `
})
class DemoComponent {
  libraryLoaded$ = this.lazyLoad.load(
    LOADING_STRATEGY.AppendAnonymousScriptToHead('/assets/some-library.js'),
  );

  constructor(private lazyLoadService: LazyLoadService) {}
}
```

`load` 方法返回一个 `observable`,你可以在组件中或通过 `AsyncPipe` 订阅它. 在上面的示例中**仅当脚本成功加载或之前已经加载脚本时**, `NgIf` 指令才会呈现 `<some-component>`.

> 你可以使用 `async` 管道在模板中多次订阅,脚本将仅加载一次

请参阅[LoadingStrategy](./Loading-Strategy.md)查看所有可用的加载策略以及如何构建自己的加载策略.

### 如何加载 Styles

如果传递给 `load` 方法第一个参数为 `StyleLoadingStrategy` 实例,`LazyLoadService` 将使用给定的 `href` 创建一个 `<link>` 元素并放置在指定的DOM位置.

```js
import { LazyLoadService, LOADING_STRATEGY } from '@abp/ng.core';

@Component({
  template: `
    <some-component *ngIf="stylesLoaded$ | async"></some-component>
  `
})
class DemoComponent {
  stylesLoaded$ = this.lazyLoad.load(
    LOADING_STRATEGY.AppendAnonymousStyleToHead('/assets/some-styles.css'),
  );

  constructor(private lazyLoadService: LazyLoadService) {}
}
```

`load` 方法返回一个 `observable`,你可以在组件中或通过 `AsyncPipe` 订阅它. 在上面的示例中**仅当样式成功加载或之前已经加载样式时**, `NgIf` 指令才会呈现 `<some-component>`.

> 你可以使用 `async` 管道在模板中多次订阅,样式将仅加载一次

请参阅[LoadingStrategy](./Loading-Strategy.md)查看所有可用的加载策略以及如何构建自己的加载策略.

### 高级用法

你有**很大的自由度来定义延迟加载的工作方式**. 示例:

```js
const domStrategy = DOM_STRATEGY.PrependToHead();

const crossOriginStrategy = CROSS_ORIGIN_STRATEGY.Anonymous(
  'sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh',
);

const loadingStrategy = new StyleLoadingStrategy(
  'https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css',
  domStrategy,
  crossOriginStrategy,
);

this.lazyLoad.load(loadingStrategy, 1, 2000);
```

此代码将创建具有给定URL和完整性哈希的 `<link>` 元素,将其插入到 `<head>` 元素的顶部,如果第一次尝试失败,则在2秒后重试一次.

一个常见的用例是在**使用功能之前加载多个脚本/样式**:

```js
import { LazyLoadService, LOADING_STRATEGY } from '@abp/ng.core';
import { frokJoin } from 'rxjs';

@Component({
  template: `
    <some-component *ngIf="scriptsAndStylesLoaded$ | async"></some-component>
  `
})
class DemoComponent {
  private stylesLoaded$ = forkJoin(
    this.lazyLoad.load(
      LOADING_STRATEGY.PrependAnonymousStyleToHead('/assets/library-dark-theme.css'),
    ),
    this.lazyLoad.load(
      LOADING_STRATEGY.PrependAnonymousStyleToHead('/assets/library.css'),
    ),
  );

  private scriptsLoaded$ = forkJoin(
    this.lazyLoad.load(
      LOADING_STRATEGY.AppendAnonymousScriptToHead('/assets/library.js'),
    ),
    this.lazyLoad.load(
      LOADING_STRATEGY.AppendAnonymousScriptToHead('/assets/other-library.css'),
    ),
  );

  scriptsAndStylesLoaded$ = forkJoin(this.scriptsLoaded$, this.stylesLoaded$);

  constructor(private lazyLoadService: LazyLoadService) {}
}
```

RxJS `forkJoin` 并行加载所有脚本和样式,并仅在加载所有脚本和样式时才放行. 因此放置 `<some-component>` 时,所有必需的依赖项都可用的.

> 注意到我们在文档头上添加了样式吗? 有时这是必需的因为你的应用程序样式可能会覆盖某些库样式. 在这种情况下你必须注意前置样式的顺序. 它们将一一放置,**并且在放置前,最后一个放置在最上面**.

另一个常见的用例是**按顺序加载依赖脚本**:

```js
import { LazyLoadService, LOADING_STRATEGY } from '@abp/ng.core';
import { concat } from 'rxjs';

@Component({
  template: `
    <some-component *ngIf="scriptsLoaded$ | async"></some-component>
  `
})
class DemoComponent {
  scriptsLoaded$ = concat(
    this.lazyLoad.load(
      LOADING_STRATEGY.PrependAnonymousScriptToHead('/assets/library.js'),
    ),
    this.lazyLoad.load(
      LOADING_STRATEGY.AppendAnonymousScriptToHead('/assets/script-that-requires-library.js'),
    ),
  );

  constructor(private lazyLoadService: LazyLoadService) {}
}
```

在此示例中,第二个文件需要预先加载第一个文件, RxJS `concat` 函数将允许你以给定的顺序一个接一个地加载所有脚本,并且仅在加载所有脚本时放行.

## API

### loaded

```js
loaded: Set<string>
```

所有以前加载的路径都可以通过此属性访问. 它是一个简单的[JavaScript集]

### load

```js
load(strategy: LoadingStrategy, retryTimes?: number, retryDelay?: number): Observable<Event>
```

- `strategy` 是主要参数,上面已经介绍过.
- `retryTimes` 定义加载失败前再次尝试多少次(默认值:2).
- `retryDelay` 定义重试之间的延迟(默认为1000).

## 下一步是什么?

- [DomInsertionService](./Dom-Insertion-Service.md)