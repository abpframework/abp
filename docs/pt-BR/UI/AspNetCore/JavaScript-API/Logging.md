# ASP.NET Core MVC / Razor Pages UI: API de Registro de JavaScript

A API `abp.log` é usada para escrever logs simples no lado do cliente.

> Os logs são escritos no console, usando o `console.log`, por padrão.

> Este documento é para registro simples no lado do cliente. Consulte o documento [Logging](../../../Logging.md) para o sistema de registro no lado do servidor.

## Uso Básico

Use um dos métodos `abp.log.xxx(...)` com base na gravidade da mensagem de log.

````js
abp.log.debug("Algum log de debug aqui..."); // Registrando uma mensagem de debug simples
abp.log.info({ name: "john", age: 42 }); // Registrando um objeto como um log de informação
abp.log.warn("Uma mensagem de aviso"); // Registrando uma mensagem de aviso
abp.log.error('Ocorreu um erro...'); // Mensagem de erro
abp.log.fatal('A conexão de rede foi perdida!'); // Erro fatal
````

## Níveis de Log

Existem 5 níveis para uma mensagem de log:

* DEBUG = 1
* INFO = 2
* WARN = 3
* ERROR = 4
* FATAL = 5

Esses são definidos no objeto `abp.log.levels` (como `abp.log.levels.WARN`).

### Alterando o Nível de Log Atual

Você pode controlar o nível de log da seguinte forma:

````js
abp.log.level = abp.log.levels.WARN;
````

O nível de log padrão é `DEBUG`.

### Registrando Especificando o Nível

Em vez de chamar a função `abp.log.info(...)`, você pode usar o `abp.log.log` especificando o nível de log como parâmetro:

````js
abp.log.log("mensagem de log...", abp.log.levels.INFO);
````