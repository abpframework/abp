# Módulo de Gerenciamento de Inquilinos

O [Multi-Tenancy](../Multi-Tenancy.md) é uma das principais características do ABP Framework. Ele fornece a infraestrutura fundamental para construir sua própria solução SaaS (Software-as-a-Service). O sistema de multi-tenancy do ABP abstrai onde seus inquilinos são armazenados, fornecendo a interface `ITenantStore`. Tudo que você precisa fazer é implementar essa interface.

**O módulo de gerenciamento de inquilinos é uma implementação da interface `ITenantStore`. Ele armazena inquilinos em um banco de dados. Ele também fornece uma interface de usuário para gerenciar seus inquilinos e suas [funcionalidades](../Features.md).**

> Por favor, **consulte a documentação do [Multi-Tenancy](../Multi-Tenancy.md)** para entender o sistema de multi-tenancy do ABP Framework. Este documento se concentra no módulo de gerenciamento de inquilinos.

### Sobre o Módulo SaaS Comercial do ABP

O [Módulo SaaS](https://commercial.abp.io/modules/Volo.Saas) é uma implementação alternativa deste módulo com mais funcionalidades e possibilidades. Ele é distribuído como parte da assinatura do [ABP Commercial](https://commercial.abp.io/).

## Como Instalar

Este módulo vem pré-instalado (como pacotes NuGet/NPM) quando você [cria uma nova solução](https://abp.io/get-started) com o ABP Framework. Você pode continuar a usá-lo como pacote e obter atualizações facilmente, ou pode incluir seu código-fonte em sua solução (consulte o comando `get-source` da [CLI](../CLI.md)) para desenvolver seu próprio módulo personalizado.

### O Código Fonte

O código-fonte deste módulo pode ser acessado [aqui](https://github.com/abpframework/abp/tree/dev/modules/tenant-management). O código-fonte é licenciado com [MIT](https://choosealicense.com/licenses/mit/), então você pode usá-lo e personalizá-lo livremente.

## Interface de Usuário

Este módulo adiciona o item de menu "*Administração -> Gerenciamento de Inquilinos -> Inquilinos*" ao menu principal do aplicativo, que abre a página mostrada abaixo:

![module-tenant-management-page](../images/module-tenant-management-page.png)

Nesta página, você vê todos os inquilinos. Você pode criar um novo inquilino conforme mostrado abaixo:

![module-tenant-management-new-tenant](../images/module-tenant-management-new-tenant.png)

Neste modal;

* **Nome**: O nome único do inquilino. Se você usar subdomínios para seus inquilinos (como https://algum-inquilino.seu-domínio.com), este será o nome do subdomínio.
* **Endereço de E-mail do Administrador**: Endereço de e-mail do usuário administrador para este inquilino.
* **Senha do Administrador**: A senha do usuário administrador para este inquilino.

Quando você clica no botão *Ações* próximo a um inquilino, você verá as ações que pode realizar:

![module-tenant-management-actions](../images/module-tenant-management-actions.png)

### Gerenciando as Funcionalidades do Inquilino

A ação Funcionalidades abre um modal para habilitar/desabilitar/configurar [funcionalidades](../Features.md) para o inquilino relacionado. Aqui, um exemplo de modal:

![features-modal](../images/features-modal.png)

### Gerenciando as Funcionalidades do Host

O botão *Gerenciar Funcionalidades do Host* é usado para configurar as funcionalidades do lado do host, se você usar as funcionalidades do seu aplicativo também no lado do host.

## Eventos Distribuídos

Este módulo define os seguintes ETOs (Event Transfer Objects) para permitir que você se inscreva em alterações nas entidades do módulo;

- `TenantEto` é publicado em alterações feitas em uma entidade `Tenant`.

**Exemplo: Receber uma notificação quando um novo inquilino for criado**

```cs
public class MeuManipulador :
    IDistributedEventHandler<EntityCreatedEto<TenantEto>>,
    ITransientDependency
{
    public async Task HandleEventAsync(EntityCreatedEto<TenantEto> eventData)
    {
        TenantEto tenant = eventData.Entity;
        // TODO: ...
    }
}
```



`TenantEto` é configurado para publicar automaticamente os eventos. Você deve configurar-se para os outros. Consulte o documento [Distributed Event Bus](https://github.com/abpframework/abp/blob/rel-7.3/docs/en/Distributed-Event-Bus.md) para aprender detalhes dos eventos pré-definidos.

> A inscrição nos eventos distribuídos é especialmente útil para cenários distribuídos (como arquitetura de microsserviços). Se você estiver construindo um aplicativo monolítico ou ouvindo eventos no mesmo processo que executa o Módulo de Gerenciamento de Inquilinos, então a inscrição nos [eventos locais](https://github.com/abpframework/abp/blob/rel-7.3/docs/en/Local-Event-Bus.md) pode ser mais eficiente e fácil.

## Internos

Esta seção pode ser usada como referência se você quiser [personalizar](../Customizing-Application-Modules-Guide.md) este módulo sem alterar [seu código-fonte](https://github.com/abpframework/abp/tree/dev/modules/tenant-management).

### Camada de Domínio

#### Agregados

* `Tenant`

#### Repositórios

* `ITenantRepository`

#### Serviços de Domínio

* `TenantManager`

### Camada de Aplicação

#### Serviços de Aplicação

* `TenantAppService`

#### Permissões

- `AbpTenantManagement.Tenants`: Gerenciamento de inquilinos.
- `AbpTenantManagement.Tenants.Create`: Criar um novo inquilino.
- `AbpTenantManagement.Tenants.Update`: Editar um inquilino existente.
- `AbpTenantManagement.Tenants.Delete`: Excluir um inquilino existente.
- `AbpTenantManagement.Tenants.ManageFeatures`: Gerenciar as funcionalidades dos inquilinos.

### Integração com o Entity Framework Core

* `TenantManagementDbContext` (implementa `ITenantManagementDbContext`)

**Tabelas do Banco de Dados:**

* `AbpTenants`
* `AbpTenantConnectionStrings`

### Integração com o MongoDB

* `TenantManagementMongoDbContext` (implementa `ITenantManagementMongoDbContext`)

**Coleções do Banco de Dados:**

* `AbpTenants` (inclui também a string de conexão)

## Avisos

O ABP Framework permite usar a abordagem *banco de dados por inquilino*, que permite que um inquilino tenha um banco de dados dedicado. Este módulo possui a infraestrutura fundamental para tornar essa implementação possível (consulte seu código-fonte), no entanto, ele não implementa a camada de aplicação e as funcionalidades de interface do usuário para fornecê-lo como uma implementação pronta para uso. Você pode implementar esses recursos por conta própria ou considerar o uso do [Módulo SaaS Comercial do ABP](https://docs.abp.io/en/commercial/latest/modules/saas), que o implementa completamente e fornece muito mais recursos de negócios.

## Veja Também

* [Multi-Tenancy](../Multi-Tenancy.md)
* [Módulo SaaS Comercial do ABP](https://docs.abp.io/en/commercial/latest/modules/saas)