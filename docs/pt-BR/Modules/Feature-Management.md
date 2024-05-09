# Módulo de Gerenciamento de Recursos

O módulo de Gerenciamento de Recursos implementa a interface `IFeatureManagementStore` definida pelo [Sistema de Recursos](../Features.md).

> Este documento aborda apenas o módulo de gerenciamento de recursos que persiste os valores dos recursos em um banco de dados. Consulte o documento [recursos](../Features.md) para obter mais informações sobre o sistema de recursos.

## Como Instalar

Este módulo vem pré-instalado (como pacotes NuGet/NPM). Você pode continuar a usá-lo como pacote e obter atualizações facilmente, ou pode incluir seu código-fonte em sua solução (consulte o comando `get-source` [CLI](../CLI.md)) para desenvolver seu próprio módulo personalizado.

### O Código Fonte

O código-fonte deste módulo pode ser acessado [aqui](https://github.com/abpframework/abp/tree/dev/modules/feature-management). O código-fonte é licenciado com [MIT](https://choosealicense.com/licenses/mit/), portanto, você pode usá-lo e personalizá-lo livremente.

## Interface do Usuário

### Diálogo de Gerenciamento de Recursos

O módulo de gerenciamento de recursos fornece um diálogo reutilizável para gerenciar recursos relacionados a um objeto. Por exemplo, o [Módulo de Gerenciamento de Inquilinos](Tenant-Management.md) o utiliza para gerenciar os recursos dos inquilinos na página de Gerenciamento de Inquilinos.

![features-module-opening](../images/features-module-opening.png)

Quando você clica em *Ações* -> *Recursos* para um inquilino, o diálogo de gerenciamento de recursos é aberto. Uma captura de tela de exemplo deste diálogo com dois recursos definidos:

![features-modal](../images/features-modal.png)

Neste diálogo, você pode habilitar, desabilitar ou definir valores para os recursos de um inquilino.

## IFeatureManager

`IFeatureManager` é o serviço principal fornecido por este módulo. Ele é usado para ler e alterar os valores de configuração para os inquilinos em um aplicativo multi-inquilino. `IFeatureManager` é normalmente usado pelo *Diálogo de Gerenciamento de Recursos*. No entanto, você pode injetá-lo se precisar definir um valor de recurso.

> Se você apenas deseja ler os valores dos recursos, use o `IFeatureChecker` conforme explicado no documento [Recursos](../Features.md).

**Exemplo: Obter/definir o valor de um recurso para um inquilino**

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.FeatureManagement;

namespace Demo
{
    public class MyService : ITransientDependency
    {
        private readonly IFeatureManager _featureManager;

        public MyService(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        public async Task SetFeatureDemoAsync(Guid tenantId, string value)
        {
            await _featureManager
                .SetForTenantAsync(tenantId, "Recurso1", value);
            
            var currentValue = await _featureManager
                .GetOrNullForTenantAsync("Recurso1", tenantId);
        }
    }
}
````

## Provedores de Gerenciamento de Recursos

O Módulo de Gerenciamento de Recursos é extensível, assim como o [sistema de recursos](../Features.md). Você pode estendê-lo definindo provedores de gerenciamento de recursos. Existem 3 provedores de gerenciamento de recursos pré-construídos registrados na seguinte ordem:

* `DefaultValueFeatureManagementProvider`: Obtém o valor do valor padrão da definição do recurso. Ele não pode definir o valor padrão, pois os valores padrão são codificados na definição do recurso.
* `EditionFeatureManagementProvider`: Obtém ou define os valores dos recursos para uma edição. A edição é um grupo de recursos atribuídos a inquilinos. O sistema de edição não foi implementado pelo módulo de Gerenciamento de Inquilinos. Você pode implementá-lo por conta própria ou adquirir o [Módulo SaaS](https://commercial.abp.io/modules/Volo.Saas) do ABP Commercial, que o implementa e também fornece mais recursos SaaS, como assinatura e pagamento.
* `TenantFeatureManagementProvider`: Obtém ou define os valores dos recursos para inquilinos.

`IFeatureManager` usa esses provedores nos métodos de obtenção/definição. Normalmente, cada provedor de gerenciamento de recursos define métodos de extensão no serviço `IFeatureManager` (como `SetForTenantAsync` definido pelo provedor de gerenciamento de recursos de inquilinos).

Se você deseja criar seu próprio provedor, implemente a interface `IFeatureManagementProvider` ou herde da classe base `FeatureManagementProvider`:

````csharp
public class CustomFeatureProvider : FeatureManagementProvider
{
    public override string Name => "Custom";

    public CustomFeatureProvider(IFeatureManagementStore store)
        : base(store)
    {
    }
}
````

A classe base `FeatureManagementProvider` faz a implementação padrão (usando o `IFeatureManagementStore`) para você. Você pode substituir os métodos base conforme necessário. Todo provedor deve ter um nome exclusivo, que é `Custom` neste exemplo (mantenha-o curto, pois ele é salvo no banco de dados para cada registro de valor de recurso).

Depois de criar sua classe de provedor, você deve registrá-la usando a classe de opções `FeatureManagementOptions` [options class](../Options.md):

````csharp
Configure<FeatureManagementOptions>(options =>
{
    options.Providers.Add<CustomFeatureProvider>();
});
````

A ordem dos provedores é importante. Os provedores são executados na ordem inversa. Isso significa que o `CustomFeatureProvider` é executado primeiro neste exemplo. Você pode inserir seu provedor em qualquer ordem na lista `Providers`.

## Veja também

* [Recursos](../Features.md)