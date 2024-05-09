# ASP.NET Core MVC / Razor Pages UI: API de Recursos JavaScript

A API `abp.features` permite que você verifique recursos ou obtenha os valores dos recursos no lado do cliente. Você pode ler o valor atual de um recurso apenas no lado do cliente se for permitido pela definição do recurso (no lado do servidor).

> Este documento explica apenas a API JavaScript. Consulte o documento [Recursos](../../../Recursos.md) para entender o sistema de Recursos do ABP.

## Uso Básico

````js
// Obtém um valor como string.
var valor = abp.features.get('ExportarParaExcel');

// Verifica se o recurso está habilitado
var habilitado = abp.features.isEnabled('ExportarParaExcel.Habilitado');
````

## Todos os Valores

`abp.features.values` pode ser usado para acessar todos os valores dos recursos.

Um exemplo de valor desse objeto é mostrado abaixo:

````js
{
  Identity.DoisFatores: "Opcional",
  ExportarParaExcel.Habilitado: "true",
  ...
}
````