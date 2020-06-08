# ContentSecurityStrategy

`ContentSecurityStrategy` 是@abp/ng.core包暴露出的抽象类. 它可以根据[内容安全策略](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy)帮助你将内联脚本或样式标记为安全.

## API

### 构造函数

```js
constructor(public nonce?: string)
```

- `nonce` 启用将内联脚本或样式列入白名单,避免在[script-src](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/script-src#Unsafe_inline_script)和[style-src](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/style-src#Unsafe_inline_styles)指令中使用 `unsafe-inline`.

### applyCSP

```js
applyCSP(element: HTMLScriptElement | HTMLStyleElement): void
```

该方法将上述属性映射到给定`element`.

## LooseContentSecurityPolicy

`LooseContentSecurityPolicy` 是扩展了 `ContentSecurityStrategy` 的类. 它需要 `nonce` 和带有给定 `<script>` 或 `<style>` 标记的标签.

## NoContentSecurityPolicy

`NoContentSecurityPolicy` 是扩展了 `ContentSecurityStrategy` 的类. 它不会将内联脚本和样式标记为安全. 你可以将其视为空操作的替代方案.
s

## 预定义内容安全策略

可以通过 `CONTENT_SECURITY_STRATEGY` 常量访问预定义的内容安全策略.

### Loose

```js
CONTENT_SECURITY_STRATEGY.Loose(nonce: string)
```

`nonce` 会被设置.

### None

```js
CONTENT_SECURITY_STRATEGY.None()
```

什么都不会做.

## 另请参阅

- [DomInsertionService](./Dom-Insertion-Service.md)
- [ContentStrategy](./Content-Strategy.md)