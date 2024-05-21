# ASP.NET Core MVC / Razor Pages: Testando

> Você pode seguir a documentação de Testes de Integração do ASP.NET Core (ASP.NET Core Integration Tests) para aprender detalhes sobre os testes de integração do ASP.NET Core. Este documento explica a infraestrutura de teste adicional fornecida pelo ABP Framework.

## O Modelo de Inicialização do Aplicativo

O Modelo de Inicialização do Aplicativo contém o projeto `.Web` que contém as visualizações/páginas/componentes de interface do usuário do aplicativo e um projeto `.Web.Tests` para testá-los.

![aspnetcore-web-tests-in-solution](../../images/aspnetcore-web-tests-in-solution.png)

## Testando as Páginas Razor

Suponha que você tenha criado uma Página Razor chamada `Issues.cshtml` com o seguinte conteúdo;

**Issues.cshtml.cs**

````csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyProject.Issues;

namespace MyProject.Web.Pages
{
    public class IssuesModel : PageModel
    {
        public List<IssueDto> Issues { get; set; }

        private readonly IIssueAppService _issueAppService;

        public IssuesModel(IIssueAppService issueAppService)
        {
            _issueAppService = issueAppService;
        }

        public async Task OnGetAsync()
        {
            Issues = await _issueAppService.GetListAsync();
        }
    }
}
````

**Issues.cshtml**

````html
@page
@model MyProject.Web.Pages.IssuesModel
<h2>Lista de Problemas</h2>
<table id="IssueTable" class="table">
    <thead>
        <tr>
            <th>Problema</th>
            <th>Fechado?</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var issue in Model.Issues)
        {
            <tr>
                <td>@issue.Title</td>
                <td>
                    @if (issue.IsClosed)
                    {
                        <span>Fechado</span>
                    }
                    else
                    {
                        <span>Aberto</span>
                    }
                </td>
            </tr>
        }
    </tbody>
</table>
````

Esta página simplesmente cria uma tabela com os problemas:

![issue-list](../../images/issue-list.png)

Você pode escrever uma classe de teste dentro do projeto `.Web.Tests` da mesma forma que o exemplo abaixo:

````csharp
using System.Threading.Tasks;
using HtmlAgilityPack;
using Shouldly;
using Xunit;

namespace MyProject.Pages
{
    public class Issues_Tests : MyProjectWebTestBase
    {
        [Fact]
        public async Task Should_Get_Table_Of_Issues()
        {
            // Agir

            var response = await GetResponseAsStringAsync("/Issues");

            // Assert

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            var tableElement = htmlDocument.GetElementbyId("IssueTable");
            tableElement.ShouldNotBeNull();

            var trNodes = tableElement.SelectNodes("//tbody/tr");
            trNodes.Count.ShouldBeGreaterThan(0);
        }
    }
}
````

`GetResponseAsStringAsync` é um método de atalho que vem da classe base que realiza uma solicitação HTTP GET, verifica se o status HTTP resultante é `200` e retorna a resposta como uma `string`.

> Você pode usar o objeto base `Client` (do tipo `HttpClient`) para realizar qualquer tipo de solicitação ao servidor e ler a resposta você mesmo. `GetResponseAsStringAsync` é apenas um método de atalho.

Este exemplo usa a biblioteca [HtmlAgilityPack](https://html-agility-pack.net/) para analisar o HTML recebido e testar se ele contém a tabela de problemas.

> Este exemplo pressupõe que existam alguns problemas iniciais no banco de dados. Consulte a seção *The Data Seed* do documento de Testes (Testing document) para aprender como inserir dados de teste, para que seus testes possam assumir que alguns dados iniciais estão disponíveis no banco de dados.

## Testando os Controladores

Testar um controlador não é diferente. Basta fazer uma solicitação ao servidor com uma URL adequada, obter a resposta e fazer suas asserções.

### Resultado de Visualização

Se o controlador retornar uma visualização, você pode usar um código semelhante para testar o HTML retornado. Veja o exemplo das Páginas Razor acima.

### Resultado de Objeto

Se o controlador retornar um resultado de objeto, você pode usar o método base `GetResponseAsObjectAsync`.

Suponha que você tenha um controlador definido da seguinte forma:

````csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyProject.Issues;
using Volo.Abp.AspNetCore.Mvc;

namespace MyProject.Web.Controllers
{
    [Route("api/issues")]
    public class IssueController : AbpController
    {
        private readonly IIssueAppService _issueAppService;

        public IssueController(IIssueAppService issueAppService)
        {
            _issueAppService = issueAppService;
        }

        [HttpGet]
        public async Task<List<IssueDto>> GetAsync()
        {
            return await _issueAppService.GetListAsync();
        }
    }
}
````

Você pode escrever um código de teste para executar a API e obter o resultado:

````csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using MyProject.Issues;
using Shouldly;
using Xunit;

namespace MyProject.Pages
{
    public class Issues_Tests : MyProjectWebTestBase
    {
        [Fact]
        public async Task Should_Get_Issues_From_Api()
        {
            var issues = await GetResponseAsObjectAsync<List<IssueDto>>("/api/issues");
            
            issues.ShouldNotBeNull();
            issues.Count.ShouldBeGreaterThan(0);
        }
    }
}
````

## Testando o Código JavaScript

O ABP Framework não fornece nenhuma infraestrutura para testar seu código JavaScript. Você pode usar qualquer estrutura de teste e ferramentas para testar seu código JavaScript.

## A Infraestrutura de Teste

O pacote [Volo.Abp.AspNetCore.TestBase](https://www.nuget.org/packages/Volo.Abp.AspNetCore.TestBase) fornece a infraestrutura de teste que está integrada ao ABP Framework e ao ASP.NET Core.

> O pacote Volo.Abp.AspNetCore.TestBase já está instalado no projeto `.Web.Tests`.

Este pacote fornece a classe base `AbpWebApplicationFactoryIntegratedTest` como a classe base fundamental para derivar as classes de teste. Ela é herdada da classe [WebApplicationFactory](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests) fornecida pelo ASP.NET Core.

A classe base `MyProjectWebTestBase` usada acima herda da `AbpWebApplicationFactoryIntegratedTest`, então herdamos indiretamente a `AbpWebApplicationFactoryIntegratedTest`.

Veja também
* [Testes de integração no ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
* [Testes Gerais / Lado do Servidor](../../Testing.md)