# ASP.NET Core MVC / Razor Pages UI: JavaScript Auth API

A API de autenticação permite que você verifique as permissões (políticas) para o usuário atual no lado do cliente. Dessa forma, você pode mostrar/ocultar partes da interface do usuário condicionalmente ou executar sua lógica no lado do cliente com base nas permissões atuais.

> Este documento explica apenas a API JavaScript. Consulte o [documento de autorização](../../../Authorization.md) para entender o sistema de autorização e permissão do ABP.

## Uso básico

A função `abp.auth.isGranted(...)` é usada para verificar se uma permissão/política foi concedida ou não:

````js
if (abp.auth.isGranted('DeleteUsers')) {
  //TODO: Excluir o usuário
} else {
  alert("Você não tem permissão para excluir um usuário!");
}
````

## Outros campos e funções

* `abp.auth.isAnyGranted(...)`: Recebe um ou mais nomes de permissão/política e retorna `true` se pelo menos um deles foi concedido.
* `abp.auth.areAllGranted(...)`: Recebe um ou mais nomes de permissão/política e retorna `true` se todos eles foram concedidos.
* `abp.auth.grantedPolicies`: Este é um objeto onde as chaves são os nomes de permissão/política. Aqui você pode encontrar os nomes de permissão/política concedidos.