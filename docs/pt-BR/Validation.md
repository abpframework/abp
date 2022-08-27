# Validação

O sistema de validação é utilizado para validar a entrada do usuário o a requisição do cliente para uma ação da controller ou por um serviço. 

ABP é compatível com o sistema de Validação de Modelos do ASP.NET Core e tudo escrito na [sua documentação](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation) já é válido para aplicações baseadas no ABP. Logo, esse documento foca nas funcionalidades do ABP ao invés de repetir a documentação da Microsoft.

Em adição, o ABP adiciona os seguintes benefícios: 

* Define `IValidationEnabled` para adicionar validação automática para uma classe arbitrária. Já que todos [application services](Application-Services.md) herdados o implementam, eles também são validados automáticamente.
* Automáticamente traduz os erros de validação para os atributos do data annotation.
* Provê serviços extensívels para validar a chamado de um método ou o estado de um objeto.
* Provê integração com o [FluentValidation](https://fluentvalidation.net/)

## Validando DTOs

Essa seção introduz brevemente o sistema de validação. Para mais detalhes, veja a [Documentação da Validação de Modelo em ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation).

### Atributos Data Nottation

Utilizar data annotations é uma maneira simples de implementar uma validação formar para um [DTO](Data-Transfer-Objects.md) de uma forma declarativa. Exemplo:

````csharp
public class CreateBookDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; }

    [Range(0, 999.99)]
    public decimal Price { get; set; }
}
````
Quando você utilzar essa classe como parâmetro para um [application service](Application-Services.md) ou um controller, ele será automáticamente validado e a validação localizada será lançada ([e tratada](Exception-Handling.md) pelo ABP framework).

### IValidatableObject

`IValidatavleObject` pode ser implementado por um DTO para executar uma lógica customizada de validação. `CreateBookDto` no exemplo a seguir implementa essa interface e verifica se o `Name` é igual a `Description` e retorna um erro de validação nesse caso.

````csharp
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Acme.BookStore
{
    public class CreateBookDto : IValidatableObject
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Range(0, 999.99)]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (Name == Description)
            {
                yield return new ValidationResult(
                    "Name and Description can not be the same!",
                    new[] { "Name", "Description" }
                );
            }
        }
    }
}
````

#### Resolvendo um serviço.

Se você precisa resolver um serviço do [sistema de injeção de dependências](Dependency-Injection.md), você pode utilizar o objeto `ValidationContext`.

````csharp
var myService = validationContext.GetRequiredService<IMyService>();
````

> Enquando resolver os serviços no método `Validate` permite qualquer possibilidade, não é um boa prática implementar sua lógica de validação do domínios no DTOs. Matenha os DTOs simples. Seu propósito é transferir dados (DTO: Data Transfer Object, ou Objeto de Transferência de Dados).

## Infraestrutura de Validação.

Essa seção explica alguns serviços adicionais providos pelo ABP Framework. 

### Interface IValidationEnabled

`IValidationEnabled` é um marcado vazio de interface que pode ser implementado por qualquer classe (registrada e resolvida do [DI](Dependency-Injection.md)) e deixe o ABP framework realizar o sistema de validação para os métodos da classe. Por exemplo: 

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Acme.BookStore
{
    public class MyService : ITransientDependency, IValidationEnabled
    {
        public virtual async Task DoItAsync(MyInput input)
        {
            //...
        }
    }
}
````

> O ABP framework utiliza o sistema de [dynamic proxying / interception](Dynamic-Proxying-Interceptors.md) para realizar a validação. Para o fazer funcionar, seu métido deve ser **virtual** ou seu serivço deve ser injetado e utilizado através de uma **interface** (como `IMyService`).

#### Habilitando e Desabilitando Validações

Você pode utilizar o `[DisableValidation]` para desabilitar para métodos, classes e propriedades.

````csharp
[DisableValidation]
public Void MyMethod()
{
}

[DisableValidation]
public class InputClass
{
    public string MyProperty { get; set; }
}

public class InputClass
{
    [DisableValidation]
    public string MyProperty { get; set; }
}
````

### AbpValidationException

Uma vez que o ABP determina um erro de validação, é lançada uma validação do tipo `AbpValidationException`. O código da sua aplicação poderá lançar o `AbpValidationException`, mas na maioria das vezes não será necessário.

* A propriedade `ValidationErrors` do `AbpValidationException` contem a lista com os erros de validação.
* O nível de log do `AbpValidationException` é definido como `Warning`. Todos são erros de validação são logados no [Sistema de Logging](Logging.md).
* `AbpValidationException` é tratado automáticamente pelo ABP framework e é convertido para um erro utilizável com o status code HTTP 400. Veja a documentação de [Manipulação de Exceção](Exception-Handling.md) para mais informações.

## Tópicos Avançados

### IObjectValidator

Além da validação automática, você pode querer validar um objeto manualmente. Nesse caso, [injete](Dependency-Injection.md) e use o serviço `IObjectValidator`:

* O método `ValidateAsync` valida o objeto informado baseado nas regras de validação e lança uma `AbpValidationException` se não estiver em um estado válido.

* `GetErrorsAsync` não lança uma exceção, somente retorna os erros de validação.

`IObjectValidator` é implementado pelo `ObjectValidator` por padrão. `ObjectValidator` é estensível; você pode implementar a interface `IObjectValidationContributor` para contribuir com uma lógica customizada. Exemplo:

````csharp
public class MyObjectValidationContributor
    : IObjectValidationContributor, ITransientDependency
{
    public Task AddErrorsAsync(ObjectValidationContext context)
    {
        //Get the validating object
        var obj = context.ValidatingObject;

        //Add the validation errors if available
        context.Errors.Add(...);
        return Task.CompletedTask;
    }
}
````

* Lembre-se de registrar sua classe no [DI](Dependency-Injection.md) (Implementando )
* Remember to register your class to the [DI](Dependency-Injection.md) (implementar `ITransientDependency` é exatamente como nesse exemplo)
* ABP vai automáticamente descobrir sua classe e utilizá-la em qualquer tipo de validação de objetos (incluindo chamadas de métodos de validação atumáticas).

### IMethodInvocationValidator

`IMethodInvocationValidator` é utilizado para validar a chamada de um método. Ele utiliza internamente o `IObjectValidator` para validar os objetos passados para na chamada do método. Você normalmente não precisa desse serivço já que ele é utilizado automáticamente pelo framework, mas você pode querer reutilziar ou substituir na sua aplicação em alguns casos raros.

## Integração com FluentValidation

O pacote Volo.Abp.FluentValidation integra a biblioteca FluentValidation com o sistema de validaçõa (implementando o `IObjectValidationContributor`). Veja o [documento de Integração com o FluentValidation](FluentValidation.md) para mais informações.
