# How to Lazy Load Scripts and Styles

You can use the `LazyLoadService` in @abp/ng.core package in order to lazy load scripts and styles in an easy and explicit way.




## Getting Started

You do not have to provide the `LazyLoadService` at module or component level, because it is already **provided in root**. You can inject and start using it immediately in your components, directives, or services.

```js
import { LazyLoadService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private lazyLoadService: LazyLoadService) {}
}
```




## Usage

You can use the `load` method of `LazyLoadService` to create a `<script>` or `<link>` element in the DOM at the desired position and force the browser to download the target resource.



### How to Load Scripts

The first parameter of `load` method expects a `LoadingStrategy`. If you pass a `ScriptLoadingStrategy` instance, the `LazyLoadService` will create a `<script>` element with given `src` and place it in the designated DOM position.

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

The `load` method returns an observable to which you can subscibe in your component or with an `async` pipe. In the example above, the `NgIf` directive will render `<some-component>` only **if the script gets successfully loaded or is already loaded before**.

> You can subscribe multiple times in your template with `async` pipe. The styles will only be loaded once.

Please refer to [LoadingStrategy](./Loading-Strategy.md) to see all available loading strategies and how you can build your own loading strategy.



### How to Load Styles

If you pass a `StyleLoadingStrategy` instance as the first parameter of `load` method, the `LazyLoadService` will create a `<link>` element with given `href` and place it in the designated DOM position.

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

The `load` method returns an observable to which you can subscibe in your component or with an `AsyncPipe`. In the example above, the `NgIf` directive will render `<some-component>` only **if the style gets successfully loaded or is already loaded before**.

> You can subscribe multiple times in your template with `async` pipe. The styles will only be loaded once.

Please refer to [LoadingStrategy](./Loading-Strategy.md) to see all available loading strategies and how you can build your own loading strategy.



### Advanced Usage

You have quite a bit of freedom to define how your lazy load will be implemented. Here is an example:

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

This code will create a `<link>` element with given url and integrity hash, insert it to to top of the `<head>` element, and retry once after 2 seconds if first try fails.




## API



### loaded: Set<string>

All previously loaded paths are available via this property. It is a simple [JavaScript Set](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Set).



### load(strategy: LoadingStrategy, retryTimes?: number, retryDelay?: number): Observable<Event>

- `strategy` parameter is the primary focus here and is explained above.
- `retryTimes` defines how many times the loading will be tried again before fail (_default: 2_).
- `retryDelay` defines how much delay there will be between retries (_default: 1000_).




## What's Next?

- [TrackByService](./Track-By-Service.md)
