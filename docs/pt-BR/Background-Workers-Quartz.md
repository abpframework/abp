# Gerenciador de Trabalhadores em Segundo Plano Quartz

[Quartz](https://www.quartz-scheduler.net/) é um avançado gerenciador de trabalhadores em segundo plano. Você pode integrar o Quartz com o ABP Framework para usá-lo em vez do [gerenciador de trabalhadores em segundo plano padrão](Background-Workers.md). O ABP simplesmente integra o Quartz.

## Instalação

É sugerido usar o [ABP CLI](CLI.md) para instalar este pacote.

### Usando o ABP CLI

Abra uma janela de linha de comando na pasta do projeto (arquivo .csproj) e digite o seguinte comando:

````bash
abp add-package Volo.Abp.BackgroundWorkers.Quartz
````

### Instalação Manual

Se você deseja instalar manualmente:

1. Adicione o pacote NuGet [Volo.Abp.BackgroundWorkers.Quartz](https://www.nuget.org/packages/Volo.Abp.BackgroundWorkers.Quartz) ao seu projeto:

   ````
   Install-Package Volo.Abp.BackgroundWorkers.Quartz
   ````

2. Adicione o módulo `AbpBackgroundWorkersQuartzModule` à lista de dependências do seu módulo:

````csharp
[DependsOn(
    //...outras dependências
    typeof(AbpBackgroundWorkersQuartzModule) //Adicione a nova dependência do módulo
    )]
public class SeuModulo : AbpModule
{
}
````

> A integração do trabalhador em segundo plano do Quartz fornece o adaptador `QuartzPeriodicBackgroundWorkerAdapter` para adaptar as classes derivadas `PeriodicBackgroundWorkerBase` e `AsyncPeriodicBackgroundWorkerBase`. Portanto, você ainda pode seguir o [documento de trabalhadores em segundo plano](Background-Workers.md) para definir o trabalhador em segundo plano.

## Configuração

Veja [Configuração](Background-Jobs-Quartz#Configuração).

## Criar um Trabalhador em Segundo Plano

Um trabalho em segundo plano é uma classe que deriva da classe base `QuartzBackgroundWorkerBase`. Por exemplo, uma classe de trabalhador simples é mostrada abaixo:

```` csharp
public class MeuTrabalhadorDeLog : QuartzBackgroundWorkerBase
{
    public MeuTrabalhadorDeLog()
    {
        JobDetail = JobBuilder.Create<MeuTrabalhadorDeLog>().WithIdentity(nameof(MeuTrabalhadorDeLog)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(MeuTrabalhadorDeLog)).StartNow().Build();
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executando MeuTrabalhadorDeLog..!");
        return Task.CompletedTask;
    }
}
````

Nós simplesmente implementamos o método Execute para escrever um log. O trabalhador em segundo plano é um **singleton por padrão**. Se desejar, você também pode implementar uma [interface de dependência](Dependency-Injection#Interfaces-de-Dependência) para registrá-lo com outro ciclo de vida.

> Dica: Adicionar identidade aos trabalhadores em segundo plano é uma boa prática, pois o Quartz distingue trabalhos diferentes com base na identidade.

## Adicionar ao BackgroundWorkerManager

Os trabalhadores em segundo plano padrão são **adicionados automaticamente** ao BackgroundWorkerManager quando a aplicação é **inicializada**. Você pode definir o valor da propriedade `AutoRegister` como `false`, se desejar adicioná-lo manualmente:

```` csharp
public class MeuTrabalhadorDeLog : QuartzBackgroundWorkerBase
{
    public MeuTrabalhadorDeLog()
    {
        AutoRegister = false;
        JobDetail = JobBuilder.Create<MeuTrabalhadorDeLog>().WithIdentity(nameof(MeuTrabalhadorDeLog)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(MeuTrabalhadorDeLog)).StartNow().Build();
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executando MeuTrabalhadorDeLog..!");
        return Task.CompletedTask;
    }
}
````

Se você deseja desabilitar globalmente a adição automática de trabalhadores, você pode desabilitá-la globalmente através das opções `AbpBackgroundWorkerQuartzOptions`:

```csharp
[DependsOn(
    //...outras dependências
    typeof(AbpBackgroundWorkersQuartzModule) //Adicione a nova dependência do módulo
    )]
public class SeuModulo : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundWorkerQuartzOptions>(options =>
        {
            options.IsAutoRegisterEnabled = false;
        });
    }
}
```

## Tópicos Avançados

### Personalizar o ScheduleJob

Suponha que você tenha um trabalhador que é executado a cada 10 minutos, mas devido a um servidor indisponível por 30 minutos, 3 execuções são perdidas. Você deseja executar todas as execuções perdidas quando o servidor estiver disponível novamente. Você deve definir seu trabalhador em segundo plano da seguinte forma:

```csharp
public class MeuTrabalhadorDeLog : QuartzBackgroundWorkerBase
{
    public MeuTrabalhadorDeLog()
    {
        JobDetail = JobBuilder.Create<MeuTrabalhadorDeLog>().WithIdentity(nameof(MeuTrabalhadorDeLog)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(MeuTrabalhadorDeLog)).WithSimpleSchedule(s=>s.WithIntervalInMinutes(1).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires()).Build();

        ScheduleJob = async scheduler =>
        {
            if (!await scheduler.CheckExists(JobDetail.Key))
            {
                await scheduler.ScheduleJob(JobDetail, Trigger);
            }
        };
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executando MeuTrabalhadorDeLog..!");
        return Task.CompletedTask;
    }
}
```

No exemplo, definimos o intervalo de execução do trabalhador como 10 minutos e definimos `WithMisfireHandlingInstructionIgnoreMisfires`. Personalizamos o `ScheduleJob` e adicionamos o trabalhador ao Quartz somente quando o trabalhador em segundo plano não existe.

### Mais

Consulte a [documentação](https://www.quartz-scheduler.net/documentation/index.html) do Quartz para obter mais informações.
