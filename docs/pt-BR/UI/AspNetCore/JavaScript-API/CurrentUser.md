# ASP.NET Core MVC / Razor Pages UI: API JavaScript do Usuário Atual

`abp.currentUser` é um objeto que contém informações sobre o usuário atual da aplicação.

> Este documento explica apenas a API JavaScript. Consulte o documento [CurrentUser](../../../CurrentUser.md) para obter informações sobre o usuário atual no lado do servidor.

## Usuário Autenticado

Se o usuário estiver autenticado, este objeto será algo como abaixo:

````js
{
  isAuthenticated: true,
  id: "34f1f4a7-13cc-4b91-84d1-b91c87afa95f",
  tenantId: null,
  userName: "john",
  name: "John",
  surName: "Nash",
  email: "john.nash@abp.io",
  emailVerified: true,
  phoneNumber: null,
  phoneNumberVerified: false,
  roles: ["moderator","supporter"]
}
````

Portanto, `abp.currentUser.userName` retorna `john` neste caso.

## Usuário Anônimo

Se o usuário não estiver autenticado, este objeto será algo como abaixo:

````js
{
  isAuthenticated: false,
  id: null,
  tenantId: null,
  userName: null,
  name: null,
  surName: null,
  email: null,
  emailVerified: false,
  phoneNumber: null,
  phoneNumberVerified: false,
  roles: []
}
````

Você pode verificar `abp.currentUser.isAuthenticated` para entender se o usuário está autenticado ou não.