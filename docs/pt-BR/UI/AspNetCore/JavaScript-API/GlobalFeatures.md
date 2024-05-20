# ASP.NET Core MVC / Razor Pages UI: API de Recursos Globais JavaScript

A API `abp.globalFeatures` permite que você obtenha os recursos habilitados dos [Recursos Globais](../../../Global-Features.md) no lado do cliente.

> Este documento apenas explica a API JavaScript. Consulte o documento [Recursos Globais](../../../Global-Features.md) para entender o sistema de Recursos Globais do ABP.

## Uso

````js
// Obtém todos os recursos globais habilitados.
> abp.globalFeatures.enabledFeatures

[ 'Shopping.Payment', 'Ecommerce.Subscription' ]


// Verifica se o recurso global está habilitado.
> abp.globalFeatures.isEnabled('Ecommerce.Subscription')

true

> abp.globalFeatures.isEnabled('My.Subscription')

false
````