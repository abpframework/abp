# ASP.NET Core MVC / Razor Pages UI: API de Bloqueio/Busy em JavaScript

A API de Bloqueio desabilita (bloqueia) a página ou uma parte da página.

## Uso Básico

**Exemplo: Bloquear (desabilitar) a página completa**

````js
abp.ui.block();
````

**Exemplo: Bloquear (desabilitar) um elemento HTML**

````js
abp.ui.block('#MyContainer');
````

**Exemplo: Habilitar novamente o elemento ou página previamente bloqueados:**

````js
abp.ui.unblock();
````

## Opções

O método `abp.ui.block()` pode receber um objeto de opções que pode conter os seguintes campos:

* `elm`: Um seletor opcional para encontrar o elemento a ser bloqueado (por exemplo, `#MyContainerId`). Se não for fornecido, a página inteira será bloqueada. O seletor também pode ser passado diretamente para o método `block()` como mostrado acima.
* `busy`: Defina como `true` para mostrar um indicador de progresso na área bloqueada.
* `promise`: Um objeto de promessa com callbacks `always` ou `finally`. Isso pode ser útil se você quiser desbloquear automaticamente a área bloqueada quando uma operação adiada for concluída.

**Exemplo: Bloquear um elemento com indicador de ocupado**

````js
abp.ui.block({
  elm: '#MySection',
  busy: true
});
````

A interface resultante será parecida com a seguinte:

![ui-busy](../../../images/ui-busy.png)

## setBusy

`abp.ui.setBusy(...)` e `abp.ui.clearBusy()` são funções de atalho se você quiser usar o bloqueio com a opção `busy`.

**Exemplo: Bloquear com ocupado**

````js
abp.ui.setBusy('#MySection');
````

Então você pode usar `abp.ui.clearBusy();` para reabilitar a área/página ocupada.