# CrossOriginStrategy

`CrossOriginStrategy` is a class exposed by @abp/ng.core package. Its instances define how a source referenced by an element will be retrieved by the browser and are consumed by other classes such as `LoadingStrategy`.


## API


### constructor

```js
constructor(
  public crossorigin: 'anonymous' | 'use-credentials',
  public integrity?: string
)
```

- `crossorigin` is mapped to [the HTML attribute with the same name](https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/crossorigin).
- `integrity` is a hash for validating a remote resource. Its use is explained [here](https://developer.mozilla.org/en-US/docs/Web/Security/Subresource_Integrity).


### setCrossOrigin

```js
setCrossOrigin(element: HTMLElement): void
```

This method maps the aforementioned properties to the given `element`.




## Predefined Cross-Origin Strategies

Predefined cross-origin strategies are accessible via `CROSS_ORIGIN_STRATEGY` constant.


### Anonymous

```js
CROSS_ORIGIN_STRATEGY.Anonymous(integrity?: string)
```

`crossorigin` will be set as `"anonymous"` and `integrity` is optional.


### UseCredentials

```js
CROSS_ORIGIN_STRATEGY.UseCredentials(integrity?: string)
```

`crossorigin` will be set as `"use-credentials"` and `integrity` is optional.
