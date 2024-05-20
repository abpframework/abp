# Módulo de Gerenciamento de Configurações

O Módulo de Gerenciamento de Configurações implementa a interface `ISettingStore` (consulte [o sistema de configurações](../Settings.md)) para armazenar os valores das configurações em um banco de dados e fornece a interface `ISettingManager` para gerenciar (alterar) os valores das configurações no banco de dados.

> O módulo de Gerenciamento de Configurações já está instalado e configurado nos [modelos de inicialização](../Startup-Templates/Index.md). Portanto, na maioria das vezes, você não precisa adicionar manualmente este módulo à sua aplicação.

## ISettingManager

`ISettingManager` é usado para obter e definir os valores das configurações. Exemplos:

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;

namespace Demo
{
    public class MyService : ITransientDependency
    {
        private readonly ISettingManager _settingManager;

        // Injeta o serviço ISettingManager
        public MyService(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task FooAsync()
        {
            Guid user1Id = ...;
            Guid tenant1Id = ...;

            // Obtém/define um valor de configuração para o usuário atual ou para o usuário especificado
            
            string layoutType1 =
                await _settingManager.GetOrNullForCurrentUserAsync("App.UI.LayoutType");
            string layoutType2 =
                await _settingManager.GetOrNullForUserAsync("App.UI.LayoutType", user1Id);

            await _settingManager.SetForCurrentUserAsync("App.UI.LayoutType", "LeftMenu");
            await _settingManager.SetForUserAsync(user1Id, "App.UI.LayoutType", "LeftMenu");

            // Obtém/define um valor de configuração para o locatário atual ou para o locatário especificado
            
            string layoutType3 =
                await _settingManager.GetOrNullForCurrentTenantAsync("App.UI.LayoutType");
            string layoutType4 =
                await _settingManager.GetOrNullForTenantAsync("App.UI.LayoutType", tenant1Id);
            
            await _settingManager.SetForCurrentTenantAsync("App.UI.LayoutType", "LeftMenu");
            await _settingManager.SetForTenantAsync(tenant1Id, "App.UI.LayoutType", "LeftMenu");

            // Obtém/define um valor de configuração global e padrão
            
            string layoutType5 =
                await _settingManager.GetOrNullGlobalAsync("App.UI.LayoutType");
            string layoutType6 =
                await _settingManager.GetOrNullDefaultAsync("App.UI.LayoutType");

            await _settingManager.SetGlobalAsync("App.UI.LayoutType", "TopMenu");
        }
    }
}

````

Portanto, você pode obter ou definir um valor de configuração para diferentes provedores de valores de configuração (Padrão, Global, Usuário, Locatário... etc).

> Use a interface `ISettingProvider` em vez da `ISettingManager` se você apenas precisa ler os valores das configurações, pois ela implementa o cache e suporta todos os cenários de implantação. Você pode usar a `ISettingManager` se estiver criando uma interface de gerenciamento de configurações.

### Cache de Configurações

Os valores das configurações são armazenados em cache usando o sistema de [cache distribuído](../Caching.md). Sempre use o `ISettingManager` para alterar os valores das configurações, pois ele gerencia o cache para você.

## Provedores de Gerenciamento de Configurações

O módulo de Gerenciamento de Configurações é extensível, assim como o [sistema de configurações](../Settings.md). Você pode estendê-lo definindo provedores de gerenciamento de configurações. Existem 5 provedores de gerenciamento de configurações pré-construídos registrados na seguinte ordem:

* `DefaultValueSettingManagementProvider`: Obtém o valor do valor padrão da definição da configuração. Ele não pode definir o valor padrão, pois os valores padrão são codificados na definição da configuração.
* `ConfigurationSettingManagementProvider`: Obtém o valor do serviço [IConfiguration](../Configuration.md). Ele não pode definir o valor de configuração, pois não é possível alterar os valores de configuração em tempo de execução.
* `GlobalSettingManagementProvider`: Obtém ou define o valor global (em todo o sistema) para uma configuração.
* `TenantSettingManagementProvider`: Obtém ou define o valor da configuração para um locatário.
* `UserSettingManagementProvider`: Obtém o valor da configuração para um usuário.

O `ISettingManager` usa os provedores de gerenciamento de configurações nos métodos de obtenção/definição. Normalmente, cada provedor de gerenciamento de configurações define métodos de extensão no serviço `ISettingManagement` (como `SetForUserAsync` definido pelo provedor de gerenciamento de configurações de usuário).

Se você deseja criar seu próprio provedor, implemente a interface `ISettingManagementProvider` ou herde da classe base `SettingManagementProvider`:

````csharp
public class CustomSettingProvider : SettingManagementProvider, ITransientDependency
{
    public override string Name => "Custom";

    public CustomSettingProvider(ISettingManagementStore store) 
        : base(store)
    {
    }
}
````

A classe base `SettingManagementProvider` faz a implementação padrão (usando o `ISettingManagementStore`) para você. Você pode substituir os métodos base conforme necessário. Todo provedor deve ter um nome exclusivo, que é `Custom` neste exemplo (mantenha-o curto, pois ele é salvo no banco de dados para cada registro de valor de configuração).

Depois de criar sua classe de provedor, você deve registrá-la usando a classe de opções `SettingManagementOptions` [options class](../Options.md):

````csharp
Configure<SettingManagementOptions>(options =>
{
    options.Providers.Add<CustomSettingProvider>();
});
````

A ordem dos provedores é importante. Os provedores são executados na ordem inversa. Isso significa que o `CustomSettingProvider` é executado primeiro neste exemplo. Você pode inserir seu provedor em qualquer ordem na lista `Providers`.

## Veja também

* [Configurações](../Settings.md)

## Interface de Gerenciamento de Configurações

O módulo de Gerenciamento de Configurações fornece a interface de configuração de e-mail por padrão.

![Interface de Configuração de E-mail](../images/setting-management-email-ui.png)

> Você pode clicar no botão Enviar e-mail de teste para enviar um e-mail de teste e verificar suas configurações de e-mail.

Ele é extensível; você pode adicionar suas guias a esta página para as configurações de sua aplicação.

### Interface de Usuário MVC

#### Criar um Componente de Visualização de Configuração

Crie a pasta `MySettingGroup` dentro da pasta `Components`. Adicione um novo componente de visualização. Nomeie-o como `MySettingGroupViewComponent`:

![MySettingGroupViewComponent](../images/my-setting-group-view-component.png)

Abra o arquivo `MySettingGroupViewComponent.cs` e altere todo o conteúdo conforme mostrado abaixo:

```csharp
public class MySettingGroupViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Components/MySettingGroup/Default.cshtml");
    }
}
```

> Você também pode usar o método `InvokeAsync`, neste exemplo, usamos o método `Invoke`.

#### Default.cshtml

Crie um arquivo `Default.cshtml` dentro da pasta `MySettingGroup`.

Abra o arquivo `Default.cshtml` e altere todo o conteúdo conforme mostrado abaixo:

```html
<div>
  <p>Página do meu grupo de configurações</p>
</div>
```

#### BookStoreSettingPageContributor

Crie um arquivo `BookStoreSettingPageContributor.cs` dentro da pasta `Settings`:

![BookStoreSettingPageContributor](../images/my-setting-group-page-contributor.png)

O conteúdo do arquivo é mostrado abaixo:

```csharp
public class BookStoreSettingPageContributor : ISettingPageContributor
{
    public Task ConfigureAsync(SettingPageCreationContext context)
    {
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.MySettingGroup",
                "MySettingGroup",
                typeof(MySettingGroupViewComponent),
                order : 1
            )
        );

        return Task.CompletedTask;
    }

    public Task<bool> CheckPermissionsAsync(SettingPageCreationContext context)
    {
        // Você pode verificar as permissões aqui
        return Task.FromResult(true);
    }
}
```

Abra o arquivo `BookStoreWebModule.cs` e adicione o seguinte código:

```csharp
Configure<SettingManagementPageOptions>(options =>
{
    options.Contributors.Add(new BookStoreSettingPageContributor());
});
```

#### Executar a Aplicação

Acesse a rota `/SettingManagement` para ver as alterações:

![Guia de Configurações Personalizadas](../images/my-setting-group-ui.png)

### Interface de Usuário Blazor

#### Criar um Componente Razor

Crie a pasta `MySettingGroup` dentro da pasta `Pages`. Adicione um novo componente Razor. Nomeie-o como `MySettingGroupComponent`:

![MySettingGroupComponent](../images/my-setting-group-component.png)

Abra o arquivo `MySettingGroupComponent.razor` e altere todo o conteúdo conforme mostrado abaixo:

```csharp
<Row>
    <p>meu grupo de configurações</p>
</Row>
```

#### BookStoreSettingComponentContributor

Crie um arquivo `BookStoreSettingComponentContributor.cs` dentro da pasta `Settings`:

![BookStoreSettingComponentContributor](../images/my-setting-group-component-contributor.png)

O conteúdo do arquivo é mostrado abaixo:

```csharp
public class BookStoreSettingComponentContributor : ISettingComponentContributor
{
    public Task ConfigureAsync(SettingComponentCreationContext context)
    {
        context.Groups.Add(
            new SettingComponentGroup(
                "Volo.Abp.MySettingGroup",
                "MySettingGroup",
                typeof(MySettingGroupComponent),
                order : 1
            )
        );

        return Task.CompletedTask;
    }

    public Task<bool> CheckPermissionsAsync(SettingComponentCreationContext context)
    {
        // Você pode verificar as permissões aqui
        return Task.FromResult(true);
    }
}
```

Abra o arquivo `BookStoreBlazorModule.cs` e adicione o seguinte código:

```csharp
Configure<SettingManagementComponentOptions>(options =>
{
    options.Contributors.Add(new BookStoreSettingComponentContributor());
});
```

#### Executar a Aplicação

Acesse a rota `/setting-management` para ver as alterações:

![Guia de Configurações Personalizadas](../images/my-setting-group-blazor.png)

### Interface de Usuário Angular

#### Criar um Componente

Crie um componente com o seguinte comando:

```bash
yarn ng generate component my-settings
```

Abra o arquivo `app.component.ts` e modifique o arquivo conforme mostrado abaixo:

```js
import { Component } from '@angular/core';
import { SettingTabsService } from '@abp/ng.setting-management/config'; // importando SettingTabsService
import { MySettingsComponent } from './my-settings/my-settings.component'; // importando MySettingsComponent

@Component(/* metadados do componente */)
export class AppComponent {
  constructor(private settingTabs: SettingTabsService) // injetando MySettingsComponent
  {
    // adicionado abaixo
    settingTabs.add([
      {
        name: 'MySettings',
        order: 1,
        requiredPolicy: 'chave da política aqui',
        component: MySettingsComponent,
      },
    ]);
  }
}
```

#### Executar a Aplicação

Acesse a rota `/setting-management` para ver as alterações:

![Guia de Configurações Personalizadas](../images/custom-settings.png)