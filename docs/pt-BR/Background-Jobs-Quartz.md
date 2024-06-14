# Gerenciador de Tarefas em Segundo Plano Quartz

[Quartz](https://www.quartz-scheduler.net/) é um avançado gerenciador de tarefas em segundo plano. Você pode integrar o Quartz com o ABP Framework para usá-lo em vez do [gerenciador de tarefas em segundo plano padrão](Background-Jobs.md). Dessa forma, você pode usar a mesma API de tarefas em segundo plano para o Quartz e seu código será independente do Quartz. Se preferir, você também pode usar diretamente a API do Quartz.

> Consulte o [documento de tarefas em segundo plano](Background-Jobs.md) para aprender como usar o sistema de tarefas em segundo plano. Este documento mostra apenas como instalar e configurar a integração com o Quartz.

## Instalação

É sugerido usar o [ABP CLI](CLI.md) para instalar este pacote.

### Usando o ABP CLI

Abra uma janela de linha de comando na pasta do projeto (arquivo .csproj) e digite o seguinte comando:

````bash
abp add-package Volo.Abp.BackgroundJobs.Quartz
````

> Se você ainda não o fez, primeiro precisa instalar o [ABP CLI](CLI.md). Para outras opções de instalação, consulte [a página de descrição do pacote](https://abp.io/package-detail/Volo.Abp.BackgroundJobs.Quartz).

### Instalação Manual

Se você deseja instalar manualmente:

1. Adicione o pacote NuGet [Volo.Abp.BackgroundJobs.Quartz](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.Quartz) ao seu projeto:

   ````
   Install-Package Volo.Abp.BackgroundJobs.Quartz
   ````

2. Adicione o `AbpBackgroundJobsQuartzModule` à lista de dependências do seu módulo:

````csharp
[DependsOn(
    //...outras dependências
    typeof(AbpBackgroundJobsQuartzModule) //Adicione a nova dependência do módulo
    )]
public class SeuModulo : AbpModule
{
}
````

## Configuração

O Quartz é uma biblioteca muito configurável e o framework ABP fornece `AbpQuartzOptions` para isso. Você pode usar o método `PreConfigure` na classe do seu módulo para pré-configurar essa opção. O ABP a usará ao inicializar o módulo Quartz. Por exemplo:

````csharp
[DependsOn(
    //...outras dependências
    typeof(AbpBackgroundJobsQuartzModule) //Adicione a nova dependência do módulo
    )]
public class SeuModulo : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<AbpQuartzOptions>(options =>
        {
            options.Properties = new NameValueCollection
            {
                ["quartz.jobStore.dataSource"] = "BackgroundJobsDemoApp",
                ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.tablePrefix"] = "QRTZ_",
                ["quartz.serializer.type"] = "json",
                ["quartz.dataSource.BackgroundJobsDemoApp.connectionString"] = configuration.GetConnectionString("Quartz"),
                ["quartz.dataSource.BackgroundJobsDemoApp.provider"] = "SqlServer",
                ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
            };
        });
    }
}
````

A partir da versão 3.1 do ABP, adicionamos o `Configurator` ao `AbpQuartzOptions` para configurar o Quartz. Por exemplo:

````csharp
[DependsOn(
    //...outras dependências
    typeof(AbpBackgroundJobsQuartzModule) //Adicione a nova dependência do módulo
    )]
public class SeuModulo : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<AbpQuartzOptions>(options =>
        {
            options.Configurator = configure =>
            {
                configure.UsePersistentStore(storeOptions =>
                {
                    storeOptions.UseProperties = true;
                    storeOptions.UseJsonSerializer();
                    storeOptions.UseSqlServer(configuration.GetConnectionString("Quartz"));
                    storeOptions.UseClustering(c =>
                    {
                        c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                        c.CheckinInterval = TimeSpan.FromSeconds(10);
                    });
                });
            };
        });
    }
}
````

> Você pode escolher a maneira que preferir para configurar o Quartz.

O Quartz armazena informações de tarefas e agendamento **em memória por padrão**. No exemplo, usamos a pré-configuração do [padrão de opções](Options.md) para alterá-lo para o banco de dados. Para mais configurações do Quartz, consulte a [documentação do Quartz](https://www.quartz-scheduler.net/).

## Tratamento de Exceções

### Estratégia de tratamento de exceções padrão

Quando ocorre uma exceção na tarefa em segundo plano, o ABP fornece a **estratégia de tratamento padrão** que tenta novamente a cada 3 segundos, até 3 vezes. Você pode alterar a contagem de tentativas e o intervalo de tentativa por meio das opções `AbpBackgroundJobQuartzOptions`:

```csharp
[DependsOn(
    //...outras dependências
    typeof(AbpBackgroundJobsQuartzModule) //Adicione a nova dependência do módulo
    )]
public class SeuModulo : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobQuartzOptions>(options =>
        {
            options.RetryCount = 1;
            options.RetryIntervalMillisecond = 1000;
        });
    }
}
```

### Personalizar a estratégia de tratamento de exceções

Você pode personalizar a estratégia de tratamento de exceções por meio das opções `AbpBackgroundJobQuartzOptions`:

```csharp
[DependsOn(
    //...outras dependências
    typeof(AbpBackgroundJobsQuartzModule) //Adicione a nova dependência do módulo
    )]
public class SeuModulo : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobQuartzOptions>(options =>
        {
            options.RetryStrategy = async (retryIndex, executionContext, exception) =>
            {
                // personalizar o tratamento de exceções
            };
        });
    }
}
```</source>