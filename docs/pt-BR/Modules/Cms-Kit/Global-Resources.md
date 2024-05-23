# CMS Kit: Recursos Globais

O sistema de Recursos Globais do CMS Kit permite adicionar estilos e scripts globais dinamicamente.

## Habilitando o Recurso de Recursos Globais

Por padrão, os recursos do CMS Kit estão desabilitados. Portanto, você precisa habilitar os recursos que deseja antes de começar a usá-lo. Você pode usar o sistema de [Recursos Globais](../../Global-Features.md) para habilitar/desabilitar os recursos do CMS Kit durante o desenvolvimento. Alternativamente, você pode usar o [Sistema de Recursos](https://docs.abp.io/en/abp/latest/Features) do ABP Framework para desabilitar um recurso do CMS Kit em tempo de execução.

> Verifique a seção "Como Instalar" da documentação do Módulo CMS Kit para saber como habilitar/desabilitar os recursos do CMS Kit durante o desenvolvimento.

## A Interface do Usuário

### Itens do Menu

O módulo CMS Kit adiciona os seguintes itens ao menu principal, sob o item de menu *Recursos Globais*:

* **Recursos Globais**: Página de gerenciamento de recursos globais.

A classe `CmsKitAdminMenus` possui as constantes para os nomes dos itens do menu.

### Página de Recursos Globais

A página de Recursos Globais é usada para gerenciar estilos e scripts globais no sistema.

![cms-kit-global-resources-page](../../images/cmskit-module-global-resources-page.png)

# Internos

## Camada de Domínio

#### Agregados

Este módulo segue o guia de [Melhores Práticas e Convenções de Entidades](https://docs.abp.io/en/abp/latest/Best-Practices/Entities).

- `GlobalResource` (raiz do agregado): Armazena um recurso.

#### Repositórios

Este módulo segue o guia de [Melhores Práticas e Convenções de Repositórios](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories).

Os seguintes repositórios personalizados são definidos para este recurso:

- `IGlobalResourceRepository`

#### Serviços de Domínio

Este módulo segue o guia de [Melhores Práticas e Convenções de Serviços de Domínio](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services).

##### Gerenciador de Recursos Globais

`GlobalResourceManager` é usado para realizar operações para a raiz do agregado `GlobalResource`.

### Camada de Aplicação

#### Serviços de Aplicação

- `GlobalResourceAdminAppService` (implementa `IGlobalResourceAdminAppService`): Implementa as operações de gerenciamento do sistema de recursos globais.
- `GlobalResourcePublicAppService` (implementa `IGlobalResourcePublicAppService`): Implementa os casos de uso públicos do sistema de recursos globais.

#### Banco de Dados

#### Entity Framework Core

##### Tabelas

- CmsGlobalResources

#### MongoDB

##### Coleções

- CmsGlobalResources