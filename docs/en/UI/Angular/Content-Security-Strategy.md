# ContentSecurityStrategy

`ContentSecurityStrategy` is an abstract class exposed by @abp/ng.core package. It helps you mark inline scripts or styles as safe in terms of [Content Security Policy](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy).




## API


### constructor

```js
constructor(public nonce?: string)
```

- `nonce` enables whitelisting inline script or styles in order to avoid using `unsafe-inline` in [script-src](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/script-src#Unsafe_inline_script) and [style-src](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/style-src#Unsafe_inline_styles) directives.


### applyCSP

```js
applyCSP(element: HTMLScriptElement | HTMLStyleElement): void
```

This method maps the aforementioned properties to the given `element`.




## LooseContentSecurityPolicy

`LooseContentSecurityPolicy` is a class that extends `ContentSecurityStrategy`. It requires `nonce` and marks given `<script>` or `<style>` tag with it.




## NoContentSecurityPolicy

`NoContentSecurityPolicy` is a class that extends `ContentSecurityStrategy`. It does not mark inline scripts and styles as safe. You can consider it as a noop alternative.




## Predefined Content Security Strategies

Predefined content security strategies are accessible via `CONTENT_SECURITY_STRATEGY` constant.


### Loose

```js
CONTENT_SECURITY_STRATEGY.Loose(nonce: string)
```

`nonce` will be set.


### None

```js
CONTENT_SECURITY_STRATEGY.None()
```

Nothing will be done.




## See Also

- [DomInsertionService](./Dom-Insertion-Service.md)
- [ContentStrategy](./Content-Strategy.md)

