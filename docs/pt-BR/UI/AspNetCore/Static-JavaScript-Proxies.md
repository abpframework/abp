# Proxies de Cliente Estático da API JavaScript

É comum consumir suas APIs HTTP a partir do seu código JavaScript. Para fazer isso, normalmente você lida com chamadas AJAX de baixo nível, como $.ajax, ou melhor [abp.ajax](JavaScript-API/Ajax.md). O ABP Framework fornece **uma maneira melhor** de chamar suas APIs HTTP a partir do seu código JavaScript: Proxies de Cliente da API JavaScript!

## Proxies de Cliente JavaScript Estáticos vs Dinâmicos

O ABP fornece **dois tipos** de sistema de geração de proxy de cliente. Este documento explica os **proxies de cliente estáticos**, que geram código do lado do cliente durante o desenvolvimento. Você também pode ver a documentação [Proxies de Cliente JavaScript Dinâmicos da API](Dynamic-JavaScript-Proxies.md) para aprender como usar proxies gerados em tempo de execução.

A geração de proxy de cliente em tempo de desenvolvimento (estático) tem uma **ligeira vantagem de desempenho**, pois não precisa obter a definição da API HTTP em tempo de execução. No entanto, você deve **regenerar** o código do proxy do cliente sempre que alterar a definição do ponto de extremidade da API. Por outro lado, os proxies de cliente dinâmicos são gerados em tempo de execução e oferecem uma **experiência de desenvolvimento mais fácil**.

## Um Exemplo Rápido

### O Serviço de Aplicativo

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

### Gerando o Código JavaScript

O lado do servidor deve estar em execução ao gerar o código do proxy do cliente. Portanto, execute primeiro a aplicação que hospeda suas APIs HTTP (pode ser a aplicação Web ou a aplicação HttpApi.Host, dependendo da estrutura da sua solução).

Abra um terminal de linha de comando na pasta raiz do seu projeto da web (`.csproj`) e digite o seguinte comando:

````bash
abp generate-proxy -t js -u https://localhost:53929/
````

> Se você ainda não instalou, deve instalar o [ABP CLI](../../CLI.md). Altere a URL de exemplo para a URL raiz da sua aplicação.

Este comando deve gerar os seguintes arquivos na pasta `ClientProxies`:

![static-js-proxy-example](../../images/static-js-proxy-example.png)

`app-proxy.js` é o arquivo de proxy gerado neste exemplo. Aqui, um exemplo de função de proxy neste arquivo:

````js
acme.bookStore.authors.author.get = function(id, ajaxParams) {
  return abp.ajax($.extend(true, {
    url: abp.appPath + 'api/app/author/' + id + '',
    type: 'GET'
  }, ajaxParams));
};
````

> O comando `generate-proxy` gera proxies apenas para as APIs que você definiu em sua aplicação (assume `app` como o nome do módulo). Se você está desenvolvendo uma aplicação modular, pode especificar o parâmetro `-m` (ou `--module`) para especificar o módulo para o qual deseja gerar proxies. Consulte a seção *generate-proxy* na documentação do [ABP CLI](../CLI.md) para outras opções.

### Usando as Funções de Proxy

Para usar as funções de proxy, primeiro importe o arquivo `app-proxy.js` para a sua página:

````html
<abp-script src="/client-proxies/app-proxy.js"/>
````

> Usamos o [abp-script tag helper](Bundling-Minification.md) neste exemplo. Você pode usar a tag `script` padrão, mas o `abp-script` é a maneira recomendada de importar arquivos JavaScript para suas páginas.

Agora, você pode chamar qualquer um dos métodos do serviço de aplicativo a partir do seu código JavaScript, assim como chamar uma função JavaScript. A função JavaScript tem o mesmo **nome**, **parâmetros** e **valor de retorno** do método C#.

**Exemplo: Obter um único autor**

````js
acme.bookStore.authors.author
    .get("7245a066-5457-4941-8aa7-3004778775f0") //Obtenha o id de algum lugar!
    .then(function(result){
      console.log(result);
    });
````

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

## Desabilitando Proxies JavaScript Dinâmicos

Quando você cria um aplicativo ou módulo, a abordagem de [geração dinâmica de proxy de cliente](Dynamic-JavaScript-Proxies.md) é usada por padrão. Se você deseja usar os proxies de cliente gerados estaticamente para o seu aplicativo, você deve desabilitá-lo explicitamente para o seu aplicativo ou módulo no método `ConfigureServices` da sua [classe de módulo](../../Module-Development-Basics.md), como no exemplo a seguir:

````csharp
Configure<DynamicJavaScriptProxyOptions>(options =>
{
    options.DisableModule("app");
});
````

`app` representa o aplicativo principal neste exemplo, o que funciona se você estiver criando um aplicativo. Se você estiver desenvolvendo um módulo de aplicativo, use o nome do seu módulo.

## Detalhes do AJAX

As funções de proxy do cliente JavaScript usam o [abp.ajax](JavaScript-API/Ajax.md) por baixo dos panos. Portanto, você tem os mesmos benefícios, como **tratamento automático de erros**. Além disso, você pode controlar totalmente a chamada AJAX fornecendo as opções.

### O Valor de Retorno

Cada função retorna um [objeto Deferred](https://api.jquery.com/category/deferred-object/). Isso significa que você pode encadear com `then` para obter o resultado, `catch` para lidar com o erro, `always` para executar uma ação assim que a operação for concluída (sucesso ou falha).

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

## Veja Também

* [Proxies de Cliente JavaScript Dinâmicos da API](Dynamic-JavaScript-Proxies.md)
* [Controladores de API Automáticos](../../API/Auto-API-Controllers.md)
* [Tutorial de Desenvolvimento de Aplicativos da Web](../../Tutorials/Part-1.md)