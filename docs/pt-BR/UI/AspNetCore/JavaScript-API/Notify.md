# ASP.NET Core MVC / Razor Pages UI: API de Notificação JavaScript

A API de Notificação é usada para mostrar notificações de interface do usuário no estilo toast, que desaparecem automaticamente para o usuário final. Por padrão, ela é implementada pela biblioteca [Toastr](https://github.com/CodeSeven/toastr).

## Exemplo Rápido

Use a função `abp.notify.success(...)` para mostrar uma mensagem de sucesso:

````js
abp.notify.success(
    'O produto "Acme Atom Re-Arranger" foi excluído com sucesso.',
    'Produto Excluído'
);
````

Uma mensagem de notificação é exibida na parte inferior direita da página:

![js-message-success](../../../images/js-notify-success.png)

## Tipos de Notificação

Existem quatro tipos de notificações pré-definidas;

* `abp.notify.success(...)`
* `abp.notify.info(...)`
* `abp.notify.warn(...)`
* `abp.notify.error(...)`

Todos os métodos acima recebem os seguintes parâmetros;

* `message`: Uma mensagem (`string`) para mostrar ao usuário.
* `title`: Um título opcional (`string`).
* `options`: Opções adicionais a serem passadas para a biblioteca subjacente, para o Toastr por padrão.

## Configuração do Toastr

A API de notificação é implementada pela biblioteca [Toastr](https://github.com/CodeSeven/toastr) por padrão. Você pode ver suas próprias opções de configuração.

**Exemplo: Mostrar mensagens toast no canto superior direito da página**

````js
toastr.options.positionClass = 'toast-top-right';
````

> O ABP define essa opção como `toast-bottom-right` por padrão. Você pode substituí-la como mostrado acima.