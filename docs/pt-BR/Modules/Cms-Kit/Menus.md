# CMS Kit: Menus

O sistema de menus do CMS Kit permite gerenciar menus públicos de forma dinâmica.

## Habilitando o recurso de menu

Por padrão, os recursos do CMS Kit estão desabilitados. Portanto, é necessário habilitar os recursos desejados antes de começar a usá-lo. Você pode usar o sistema de [Recursos Globais](../../Global-Features.md) para habilitar/desabilitar os recursos do CMS Kit durante o desenvolvimento. Alternativamente, você pode usar o [Sistema de Recursos](https://docs.abp.io/en/abp/latest/Features) do ABP Framework para desabilitar um recurso do CMS Kit em tempo de execução.

> Verifique a seção "Como instalar" da documentação do módulo CMS Kit (Index.md#how-to-install) para saber como habilitar/desabilitar os recursos do CMS Kit durante o desenvolvimento.

## A interface do usuário

### Itens de menu

O módulo CMS Kit adiciona os seguintes itens ao menu principal, sob o item de menu *CMS*:

* **Menus**: Página de gerenciamento de menus.

A classe `CmsKitAdminMenus` possui as constantes para os nomes dos itens de menu.

### Menus

#### Gerenciamento de menus

A página de menus é usada para gerenciar menus públicos dinâmicos no sistema.

![cms-kit-menus-page](../../images/cmskit-module-menus-page.png)

Os itens de menu criados serão visíveis no lado público do site, como mostrado abaixo:

![cms-kit-public-menus](../../images//cmskit-module-menus-public.png)

## Internos

### Camada de domínio

#### Agregados

Este módulo segue o guia de [Melhores Práticas e Convenções de Entidades](https://docs.abp.io/en/abp/latest/Best-Practices/Entities).

- `MenuItem` (raiz do agregado): Um item de menu representa um único nó na árvore de menus.

#### Repositórios

Este módulo segue o guia de [Melhores Práticas e Convenções de Repositórios](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories).

Os seguintes repositórios personalizados são definidos para este recurso:

- `IMenuItemRepository`

#### Serviços de domínio

Este módulo segue o guia de [Melhores Práticas e Convenções de Serviços de Domínio](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services).

##### Gerenciador de itens de menu

`MenuItemManager` é usado para realizar algumas operações para a raiz do agregado `MenuItemManager`.

### Camada de aplicação

#### Serviços de aplicação

- `MenuItemAdminAppService` (implementa `IMenuItemAdminAppService`): Implementa as operações de gerenciamento do sistema de menus.
- `MenuItemPublicAppService` (implementa `IMenuItemPublicAppService`): Implementa os casos de uso públicos do sistema de menus.

### Provedores de banco de dados

#### Comum

##### Prefixo de tabela / coleção e esquema

Todas as tabelas/coleções usam o prefixo `Cms` por padrão. Defina propriedades estáticas na classe `CmsKitDbProperties` se você precisar alterar o prefixo da tabela ou definir um nome de esquema (se suportado pelo seu provedor de banco de dados).

##### String de conexão

Este módulo usa `CmsKit` como nome da string de conexão. Se você não definir uma string de conexão com esse nome, ela será substituída pela string de conexão `Default`.

Consulte a documentação sobre [strings de conexão](https://docs.abp.io/en/abp/latest/Connection-Strings) para obter mais detalhes.

#### Entity Framework Core

##### Tabelas

- CmsMenuItems

#### MongoDB

##### Coleções

- CmsMenuItems