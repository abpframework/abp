# Proxies de Cliente de API JavaScript Dinâmico

É comum consumir suas APIs HTTP a partir do seu código JavaScript. Para fazer isso, normalmente você lida com chamadas AJAX de baixo nível, como $.ajax, ou melhor [abp.ajax](JavaScript-API/Ajax.md). O ABP Framework fornece **uma maneira melhor** de chamar suas APIs HTTP a partir do seu código JavaScript: Proxies de Cliente de API JavaScript!

## Proxies de Cliente JavaScript Estáticos vs Dinâmicos

O ABP fornece **dois tipos** de sistema de geração de proxy de cliente. Este documento explica os **proxies de cliente dinâmicos**, que geram proxies do lado do cliente em tempo de execução. Você também pode ver a documentação de [Proxies de Cliente de API JavaScript Estáticos](Static-JavaScript-Proxies.md) para aprender como gerar proxies em tempo de desenvolvimento.

A geração de proxy de cliente em tempo de desenvolvimento (estático) tem uma **ligeira vantagem de desempenho**, pois não precisa obter a definição da API HTTP em tempo de execução. No entanto, você deve **regenerar** o código do proxy de cliente sempre que alterar a definição do ponto de extremidade da API. Por outro lado, os proxies de cliente dinâmicos são gerados em tempo de execução e oferecem uma **experiência de desenvolvimento mais fácil**.

## Um Exemplo Rápido

Suponha que você tenha um serviço de aplicativo definido como mostrado abaixo:

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Authors
{
    public interface IAuthorAppService : IApplicationService
    {
        Task<AuthorDto> GetAsync(Guid id);

        Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input);

        Task<AuthorDto> CreateAsync(CreateAuthorDto input);

        Task UpdateAsync(Guid id, UpdateAuthorDto input);

        Task DeleteAsync(Guid id);
    }
}
````

> Você pode seguir o [tutorial de desenvolvimento de aplicativos da web](../../Tutorials/Part-1.md) para aprender como criar [serviços de aplicativos](../../Application-Services.md), expô-los como [APIs HTTP](../../API/Auto-API-Controllers.md) e consumir do código JavaScript como um exemplo completo.

Você pode chamar qualquer um dos métodos como se estivesse chamando uma função JavaScript. A função JavaScript tem o mesmo **nome**, **parâmetros** e **valor de retorno** do método C#.

**Exemplo: Obter a lista de autores**

````js
acme.bookStore.authors.author.getList({
  maxResultCount: 10
}).then(function(result){
  console.log(result.items);
});
````

**Exemplo: Excluir um autor**

```js
acme.bookStore.authors.author
    .delete('7245a066-5457-4941-8aa7-3004778775f0') //Obtenha o id de algum lugar!
    .then(function() {
        abp.notify.info('Excluído com sucesso!');
    });
```

## Detalhes do AJAX

As funções de proxy de cliente JavaScript usam o [abp.ajax](JavaScript-API/Ajax.md) por baixo dos panos. Portanto, você tem os mesmos benefícios, como **tratamento automático de erros**. Além disso, você pode controlar totalmente a chamada AJAX fornecendo as opções.

### O Valor de Retorno

Cada função retorna um [objeto Deferred](https://api.jquery.com/category/deferred-object/). Isso significa que você pode encadear com `then` para obter o resultado, `catch` para lidar com o erro, `always` para executar uma ação assim que a operação for concluída (com sucesso ou falha).

### Opções do AJAX

Cada função recebe um **último parâmetro** adicional após seus próprios parâmetros. O último parâmetro é chamado de `ajaxParams`. É um objeto que substitui as opções do AJAX.

**Exemplo: Definir as opções do AJAX `type` e `dataType`**

````js
acme.bookStore.authors.author
    .delete('7245a066-5457-4941-8aa7-3004778775f0', {
        type: 'POST',
        dataType: 'xml'
    })
    .then(function() {
        abp.notify.info('Excluído com sucesso!');
    });
````

Consulte a documentação do [jQuery.ajax](https://api.jquery.com/jQuery.ajax/) para todas as opções disponíveis.

## Endpoint do Script de Proxy de Serviço

A mágica é feita pelo endpoint `/Abp/ServiceProxyScript` definido pelo ABP Framework e adicionado automaticamente ao layout. Você pode visitar este endpoint em sua aplicação para ver as definições das funções de proxy de cliente. Este arquivo de script é gerado automaticamente pelo ABP Framework com base nas definições dos métodos do lado do servidor e nos detalhes do ponto de extremidade HTTP relacionado.

## Veja Também

* [Proxies de Cliente de API JavaScript Estáticos](Static-JavaScript-Proxies.md)
* [Controladores de API Automáticos](../../API/Auto-API-Controllers.md)
* [Tutorial de Desenvolvimento de Aplicativos da Web](../../Tutorials/Part-1.md)