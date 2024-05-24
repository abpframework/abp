# Tratamento de Exceções

O ABP fornece uma infraestrutura integrada e oferece um modelo padrão para lidar com exceções.

* **Lida automaticamente com todas as exceções** e envia uma **mensagem de erro formatada padrão** para o cliente em uma solicitação de API/AJAX.
* Oculta automaticamente os **erros internos de infraestrutura** e retorna uma mensagem de erro padrão.
* Fornece uma maneira fácil e configurável de **localizar** mensagens de exceção.
* Mapeia automaticamente exceções padrão para **códigos de status HTTP** e fornece uma opção configurável para mapear exceções personalizadas.

## Tratamento Automático de Exceções

O `AbpExceptionFilter` lida com uma exceção se **qualquer uma das seguintes condições** forem atendidas:

* A exceção é lançada por uma **ação do controlador** que retorna um **resultado de objeto** (não um resultado de visualização).
* A solicitação é uma solicitação AJAX (o valor do cabeçalho HTTP `X-Requested-With` é `XMLHttpRequest`).
* O cliente aceita explicitamente o tipo de conteúdo `application/json` (por meio do cabeçalho HTTP `accept`).

Se a exceção for tratada, ela é automaticamente **registrada** e uma **mensagem JSON formatada** é retornada ao cliente.

### Formato da Mensagem de Erro

A mensagem de erro é uma instância da classe `RemoteServiceErrorResponse`. O JSON de erro mais simples tem uma propriedade **message** conforme mostrado abaixo:

````json
{
  "error": {
    "message": "Este tópico está bloqueado e não é possível adicionar uma nova mensagem"
  }
}
````

Existem **campos opcionais** que podem ser preenchidos com base na exceção que ocorreu.

##### Código de Erro

O **código de erro** é um valor de string opcional e único para a exceção. A exceção lançada deve implementar a interface `IHasErrorCode` para preencher este campo. Exemplo de valor JSON:

````json
{
  "error": {
    "code": "App:010042",
    "message": "Este tópico está bloqueado e não é possível adicionar uma nova mensagem"
  }
}
````

O código de erro também pode ser usado para localizar a exceção e personalizar o código de status HTTP (consulte as seções relacionadas abaixo).

##### Detalhes do Erro

Os **detalhes do erro** são um campo opcional da mensagem de erro JSON. A exceção lançada deve implementar a interface `IHasErrorDetails` para preencher este campo. Exemplo de valor JSON:

```json
{
  "error": {
    "code": "App:010042",
    "message": "Este tópico está bloqueado e não é possível adicionar uma nova mensagem",
    "details": "Informações mais detalhadas sobre o erro..."
  }
}
```

##### Erros de Validação

**validationErrors** é um campo padrão que é preenchido se a exceção lançada implementar a interface `IHasValidationErrors`.

````json
{
  "error": {
    "code": "App:010046",
    "message": "Sua solicitação não é válida, corrija e tente novamente!",
    "validationErrors": [{
      "message": "O nome de usuário deve ter no mínimo 3 caracteres.",
      "members": ["userName"]
    },
    {
      "message": "A senha é obrigatória",
      "members": ["password"]
    }]
  }
}
````

`AbpValidationException` implementa a interface `IHasValidationErrors` e é automaticamente lançada pelo framework quando a entrada de uma solicitação não é válida. Portanto, geralmente você não precisa lidar com erros de validação, a menos que tenha lógica de validação altamente personalizada.

### Registro

As exceções capturadas são automaticamente registradas.

#### Nível de Registro

As exceções são registradas com o nível `Error` por padrão. O nível de log pode ser determinado pela exceção se ela implementar a interface `IHasLogLevel`. Exemplo:

````C#
public class MinhaExcecao : Exception, IHasLogLevel
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    //...
}
````

#### Exceções de Registro Próprio

Alguns tipos de exceção podem precisar escrever logs adicionais. Eles podem implementar a interface `IExceptionWithSelfLogging` se necessário. Exemplo:

````C#
public class MinhaExcecao : Exception, IExceptionWithSelfLogging
{
    public void Log(ILogger logger)
    {
        //...log informações adicionais
    }
}
````

> Os métodos de extensão `ILogger.LogException` são usados para escrever logs de exceção. Você pode usar o mesmo método de extensão quando necessário.

## Exceções de Negócios

A maioria de suas próprias exceções será exceções de negócios. A interface `IBusinessException` é usada para marcar uma exceção como uma exceção de negócios.

`BusinessException` implementa a interface `IBusinessException` além das interfaces `IHasErrorCode`, `IHasErrorDetails` e `IHasLogLevel`. O nível de log padrão é `Warning`.

Normalmente, você tem um código de erro relacionado a uma exceção de negócios específica. Por exemplo:

````C#
throw new BusinessException(QaErrorCodes.CanNotVoteYourOwnAnswer);
````

`QaErrorCodes.CanNotVoteYourOwnAnswer` é apenas uma `const string`. O seguinte formato de código de erro é recomendado:

````
<namespace-do-código>:<código-de-erro>
````

**namespace-do-código** é um **valor único** específico para o seu módulo/aplicação. Exemplo:

````
Volo.Qa:010002
````

`Volo.Qa` é o namespace do código aqui. O namespace do código será então usado ao **localizar** mensagens de exceção.

* Você pode **lançar diretamente** uma `BusinessException` ou **derivar** seus próprios tipos de exceção dela quando necessário.
* Todas as propriedades são opcionais para a classe `BusinessException`. Mas geralmente você define a propriedade `ErrorCode` ou `Message`.

## Localização de Exceções

Um problema ao lançar exceções é como localizar mensagens de erro ao enviá-las para o cliente. O ABP oferece dois modelos e suas variantes.

### Exceção Amigável ao Usuário

Se uma exceção implementa a interface `IUserFriendlyException`, então o ABP não altera suas propriedades `Message` e `Details` e a envia diretamente para o cliente.

A classe `UserFriendlyException` é a implementação integrada da interface `IUserFriendlyException`. Exemplo de uso:

````C#
throw new UserFriendlyException(
    "O nome de usuário deve ser único!"
);
````

Dessa forma, **não há necessidade de localização**. Se você deseja localizar a mensagem, pode injetar e usar o **localizador de strings padrão** (consulte o [documento de localização](Localization.md)). Exemplo:

````C#
throw new UserFriendlyException(_stringLocalizer["UserNameShouldBeUniqueMessage"]);
````

Em seguida, defina no **recurso de localização** para cada idioma. Exemplo:

````json
{
  "culture": "pt",
  "texts": {
    "UserNameShouldBeUniqueMessage": "O nome de usuário deve ser único!"
  }
}
````

O localizador de strings já suporta **mensagens parametrizadas**. Por exemplo:

````C#
throw new UserFriendlyException(_stringLocalizer["UserNameShouldBeUniqueMessage", "john"]);
````

Em seguida, o texto de localização pode ser:

````json
"UserNameShouldBeUniqueMessage": "O nome de usuário deve ser único! '{0}' já está em uso!"
````

* A interface `IUserFriendlyException` é derivada da `IBusinessException` e a classe `UserFriendlyException` é derivada da classe `BusinessException`.

### Usando Códigos de Erro

`UserFriendlyException` é bom, mas tem alguns problemas em usos avançados:

* Requer que você **injete o localizador de strings** em todos os lugares e sempre o use ao lançar exceções.
* No entanto, em alguns casos, pode **não ser possível** injetar o localizador de strings (em um contexto estático ou em um método de entidade).

Em vez de localizar a mensagem ao lançar a exceção, você pode separar o processo usando **códigos de erro**.

Primeiro, defina o mapeamento do **namespace-do-código** para o **recurso de localização** na configuração do módulo:

````C#
services.Configure<AbpExceptionLocalizationOptions>(options =>
{
    options.MapCodeNamespace("Volo.Qa", typeof(QaResource));
});
````

Então, qualquer uma das exceções com o namespace `Volo.Qa` será localizada usando seu recurso de localização fornecido. O recurso de localização deve sempre ter uma entrada com a chave do código de erro. Exemplo:

````json
{
  "culture": "pt",
  "texts": {
    "Volo.Qa:010002": "Você não pode votar em sua própria resposta!"
  }
}
````

Então uma exceção de negócios pode ser lançada com o código de erro:

````C#
throw new BusinessException(QaDomainErrorCodes.CanNotVoteYourOwnAnswer);
````

* Lançar qualquer exceção que implemente a interface `IHasErrorCode` se comporta da mesma maneira. Portanto, a abordagem de localização de código de erro não é exclusiva para a classe `BusinessException`.
* Não é necessário definir uma string localizada para uma mensagem de erro. Se não estiver definido, o ABP envia a mensagem de erro padrão para o cliente. Ele não usa a propriedade `Message` da exceção! se você deseja isso, use a `UserFriendlyException` (ou use um tipo de exceção que implemente a interface `IUserFriendlyException`).

#### Usando Parâmetros de Mensagem

Se você tiver uma mensagem de erro parametrizada, poderá defini-la com a propriedade `Data` da exceção. Por exemplo:

````C#
throw new BusinessException("App:010046")
{
    Data =
    {
        {"UserName", "john"}
    }
};

````

Felizmente, há uma maneira mais simples de codificar isso:

````C#
throw new BusinessException("App:010046")
    .WithData("UserName", "john");
````

Em seguida, o texto localizado pode conter o parâmetro `UserName`:

````json
{
  "culture": "pt",
  "texts": {
    "App:010046": "O nome de usuário deve ser único. '{UserName}' já está em uso!"
  }
}
````

* `WithData` pode ser encadeado com mais de um parâmetro (como `.WithData(...).WithData(...)`).

## Mapeamento de Códigos de Status HTTP

O ABP tenta determinar automaticamente o código de status HTTP mais adequado para tipos comuns de exceção seguindo estas regras:

* Para a `AbpAuthorizationException`:
  * Retorna `401` (não autorizado) se o usuário não estiver logado.
  * Retorna `403` (proibido) se o usuário estiver logado.
* Retorna `400` (requisição inválida) para a `AbpValidationException`.
* Retorna `404` (não encontrado) para a `EntityNotFoundException`.
* Retorna `403` (proibido) para a `IBusinessException` (e `IUserFriendlyException` já que estende a `IBusinessException`).
* Retorna `501` (não implementado) para a `NotImplementedException`.
* Retorna `500` (erro interno do servidor) para outras exceções (que são assumidas como exceções de infraestrutura).

O `IHttpExceptionStatusCodeFinder` é usado para determinar automaticamente o código de status HTTP. A implementação padrão é a classe `DefaultHttpExceptionStatusCodeFinder`. Pode ser substituída ou estendida conforme necessário.

### Mapeamentos Personalizados

A determinação automática do código de status HTTP pode ser substituída por mapeamentos personalizados. Por exemplo:

````C#
services.Configure<AbpExceptionHttpStatusCodeOptions>(options =>
{
    options.Map("Volo.Qa:010002", HttpStatusCode.Conflict);
});
````

## Inscrevendo-se nas Exceções

É possível ser informado quando o Framework ABP **manipula uma exceção**. Ele registra automaticamente todas as exceções no [logger padrão](Logging.md), mas você pode querer fazer mais.

Nesse caso, crie uma classe derivada da classe `ExceptionSubscriber` em sua aplicação:

````csharp
public class MeuAssinanteDeExcecao : ExceptionSubscriber
{
    public async override Task HandleAsync(ExceptionNotificationContext context)
    {
        //TODO...
    }
}
````

O objeto `context` contém informações necessárias sobre a exceção ocorrida.

> Você pode ter vários assinantes, cada um recebe uma cópia da exceção. As exceções lançadas pelo seu assinante são ignoradas (mas ainda registradas).

## Exceções Integradas

Alguns tipos de exceção são automaticamente lançados pelo framework:

- `AbpAuthorizationException` é lançada se o usuário atual não tiver permissão para realizar a operação solicitada. Consulte [autorização](Authorization.md) para mais informações.
- `AbpValidationException` é lançada se a entrada da solicitação atual não for válida. Consulte [validação](Validation.md) para mais informações.
- `EntityNotFoundException` é lançada se a entidade solicitada não estiver disponível. Isso é lançado principalmente por [repositórios](Repositories.md).

Você também pode lançar esses tipos de exceção em seu código (embora raramente seja necessário).

## AbpExceptionHandlingOptions

`AbpExceptionHandlingOptions` é o principal [objeto de opções](Options.md) para configurar o sistema de tratamento de exceções. Você pode configurá-lo no método `ConfigureServices` do seu [módulo](Module-Development-Basics.md):

````csharp
Configure<AbpExceptionHandlingOptions>(options =>
{
    options.SendExceptionsDetailsToClients = true;
    options.SendStackTraceToClients = false;
});
````

Aqui está uma lista das opções que você pode configurar:

* `SendExceptionsDetailsToClients` (padrão: `false`): Você pode habilitar ou desabilitar o envio de detalhes da exceção para o cliente.
* `SendStackTraceToClients` (padrão: `true`): Você pode habilitar ou desabilitar o envio da pilha de chamadas da exceção para o cliente. Se você deseja enviar a pilha de chamadas para o cliente, deve definir tanto as opções `SendStackTraceToClients` quanto `SendExceptionsDetailsToClients` como `true`, caso contrário, a pilha de chamadas não será enviada para o cliente.

## Veja Também

* [Tutorial em vídeo](https://abp.io/video-courses/essentials/exception-handling)
