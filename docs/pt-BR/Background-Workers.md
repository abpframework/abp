# Background Workers

## Introdução

Background workers são threads independentes simples na aplicação que são executadas em segundo plano. Geralmente, eles são executados periodicamente para realizar algumas tarefas. Exemplos:

* Um background worker pode ser executado periodicamente para **excluir logs antigos**.
* Um background worker pode ser executado periodicamente para **identificar usuários inativos** e **enviar e-mails** para fazer com que os usuários retornem à sua aplicação.

## Criando um Background Worker

Um background worker deve implementar diretamente ou indiretamente a interface `IBackgroundWorker`.

> Um background worker é inerentemente [singleton](Dependency-Injection.md). Portanto, apenas uma única instância da sua classe de worker é instanciada e executada.

### BackgroundWorkerBase

`BackgroundWorkerBase` é uma maneira fácil de criar um background worker.

````csharp
public class MyWorker : BackgroundWorkerBase
{
    public override Task StartAsync(CancellationToken cancellationToken = default)
    {
        //...
    }

    public override Task StopAsync(CancellationToken cancellationToken = default)
    {
        //...
    }
}
````

Inicie o seu worker no método `StartAsync` (que é chamado quando a aplicação é iniciada) e pare no método `StopAsync` (que é chamado quando a aplicação é encerrada).

> Você pode implementar diretamente a interface `IBackgroundWorker`, mas `BackgroundWorkerBase` fornece algumas propriedades úteis, como `Logger`.

### AsyncPeriodicBackgroundWorkerBase

Vamos supor que queremos tornar um usuário inativo se ele não tiver feito login na aplicação nos últimos 30 dias. A classe `AsyncPeriodicBackgroundWorkerBase` simplifica a criação de workers periódicos, então vamos usá-la no exemplo abaixo:

````csharp
public class PassiveUserCheckerWorker : AsyncPeriodicBackgroundWorkerBase
{
    public PassiveUserCheckerWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory
        ) : base(
            timer, 
            serviceScopeFactory)
    {
        Timer.Period = 600000; //10 minutos
    }

    protected async override Task DoWorkAsync(
        PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("Iniciando: Definindo status de usuários inativos...");

        //Resolver dependências
        var userRepository = workerContext
            .ServiceProvider
            .GetRequiredService<IUserRepository>();

        //Realizar o trabalho
        await userRepository.UpdateInactiveUserStatusesAsync();

        Logger.LogInformation("Concluído: Definindo status de usuários inativos...");
    }
}
````

* `AsyncPeriodicBackgroundWorkerBase` usa o objeto `AbpAsyncTimer` (um timer thread-safe) para determinar **o período**. Podemos definir a propriedade `Period` no construtor.
* É necessário implementar o método `DoWorkAsync` para **executar** o trabalho periódico.
* É uma boa prática **resolver as dependências** a partir do `PeriodicBackgroundWorkerContext` em vez de usar injeção de dependência no construtor. Isso ocorre porque `AsyncPeriodicBackgroundWorkerBase` usa um `IServiceScope` que é **descartado** quando o trabalho é concluído.
* `AsyncPeriodicBackgroundWorkerBase` **captura e registra exceções** lançadas pelo método `DoWorkAsync`.

## Registrando o Background Worker

Após criar a classe do background worker, você deve adicioná-la ao `IBackgroundWorkerManager`. O local mais comum é o método `OnApplicationInitializationAsync` da sua classe de módulo:

````csharp
[DependsOn(typeof(AbpBackgroundWorkersModule))]
public class MyModule : AbpModule
{
    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<PassiveUserCheckerWorker>();
    }
}
````

`context.AddBackgroundWorkerAsync(...)` é um método de extensão que simplifica a expressão abaixo:

````csharp
await context.ServiceProvider
    .GetRequiredService<IBackgroundWorkerManager>()
    .AddAsync(
        context
            .ServiceProvider
            .GetRequiredService<PassiveUserCheckerWorker>()
    );
````

Dessa forma, ele resolve o background worker fornecido e o adiciona ao `IBackgroundWorkerManager`.

Embora geralmente adicionemos workers no método `OnApplicationInitializationAsync`, não há restrições quanto a isso. Você pode injetar o `IBackgroundWorkerManager` em qualquer lugar e adicionar workers em tempo de execução. O gerenciador de background workers irá parar e liberar todos os workers registrados quando a aplicação for encerrada.

## Opções

A classe `AbpBackgroundWorkerOptions` é usada para [definir opções](Options.md) para os background workers. Atualmente, há apenas uma opção:

* `IsEnabled` (padrão: true): Usado para **habilitar/desabilitar** o sistema de background workers para a sua aplicação.

> Consulte o documento [Options](Options.md) para aprender como definir opções.

## Mantendo a Aplicação Sempre em Execução

Os background workers só funcionam se a sua aplicação estiver em execução. Se você hospedar a execução do job em segundo plano na sua aplicação web (esse é o comportamento padrão), você deve garantir que a sua aplicação web esteja configurada para sempre estar em execução. Caso contrário, os jobs em segundo plano só funcionarão enquanto a sua aplicação estiver em uso.

## Executando em um Cluster

Tenha cuidado se você executar várias instâncias da sua aplicação simultaneamente em um ambiente de cluster. Nesse caso, cada aplicação executa o mesmo worker, o que pode criar conflitos se os workers estiverem executando em recursos compartilhados (processando os mesmos dados, por exemplo).

Se isso for um problema para os seus workers, você tem as seguintes opções:

* Implemente os seus background workers de forma que eles funcionem em um ambiente de cluster sem problemas. Usar o [lock distribuído](Distributed-Locking.md) para garantir o controle de concorrência é uma forma de fazer isso. Um background worker em uma instância da aplicação pode manipular um lock distribuído, então os workers em outras instâncias da aplicação aguardarão pelo lock. Dessa forma, apenas um worker realiza o trabalho real, enquanto os outros esperam ociosos. Se você implementar isso, os seus workers serão executados com segurança, independentemente de como a aplicação estiver implantada.
* Pare os background workers (defina `AbpBackgroundWorkerOptions.IsEnabled` como `false`) em todas as instâncias da aplicação, exceto em uma delas, para que apenas a instância única execute os workers.
* Pare os background workers (defina `AbpBackgroundWorkerOptions.IsEnabled` como `false`) em todas as instâncias da aplicação e crie uma aplicação dedicada (talvez uma aplicação console executando em seu próprio contêiner ou um serviço do Windows em execução em segundo plano) para executar todas as tarefas em segundo plano. Essa pode ser uma boa opção se os seus background workers consumirem muitos recursos do sistema (CPU, RAM ou Disco), para que você possa implantar essa aplicação de background em um servidor dedicado e suas tarefas em segundo plano não afetem o desempenho da sua aplicação.

## Integrações

O sistema de background workers é extensível e você pode alterar o gerenciador de background workers padrão com a sua própria implementação ou com uma das integrações pré-construídas.

Veja as alternativas de gerenciador de workers pré-construídas:

* [Quartz Background Worker Manager](Background-Workers-Quartz.md)
* [Hangfire Background Worker Manager](Background-Workers-Hangfire.md)

## Veja Também

* [Background Jobs](Background-Jobs.md)
