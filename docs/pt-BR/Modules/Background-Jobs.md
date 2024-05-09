# Módulo de Tarefas em Segundo Plano

O módulo de Tarefas em Segundo Plano implementa a interface `IBackgroundJobStore` e torna possível usar o gerenciador de tarefas em segundo plano padrão do ABP Framework. Se você não deseja usar este módulo, então você deve implementar a interface `IBackgroundJobStore` por conta própria.

> Este documento aborda apenas o módulo de tarefas em segundo plano que persiste as tarefas em segundo plano em um banco de dados. Consulte o documento [tarefas em segundo plano](../Background-Jobs.md) para obter mais informações sobre o sistema de tarefas em segundo plano.

## Como Instalar

Este módulo vem pré-instalado (como pacotes NuGet/NPM). Você pode continuar a usá-lo como pacote e obter atualizações facilmente, ou pode incluir seu código-fonte em sua solução (consulte o comando `get-source` [CLI](../CLI.md)) para desenvolver seu próprio módulo personalizado.

### O Código Fonte

O código-fonte deste módulo pode ser acessado [aqui](https://github.com/abpframework/abp/tree/dev/modules/background-jobs). O código-fonte é licenciado com [MIT](https://choosealicense.com/licenses/mit/), então você pode usá-lo e personalizá-lo livremente.

## Internos

### Camada de Domínio

#### Agregados

- `BackgroundJobRecord` (raiz do agregado): Representa um registro de tarefa em segundo plano.

#### Repositórios

Os seguintes repositórios personalizados são definidos para este módulo:

- `IBackgroundJobRepository`

### Provedores de Banco de Dados

#### Comum

##### Prefixo da tabela / coleção e esquema

Todas as tabelas/coleções usam o prefixo `Abp` por padrão. Defina propriedades estáticas na classe `BackgroundJobsDbProperties` se você precisar alterar o prefixo da tabela ou definir um nome de esquema (se suportado pelo seu provedor de banco de dados).

##### String de conexão

Este módulo usa `AbpBackgroundJobs` como nome da string de conexão. Se você não definir uma string de conexão com esse nome, ela será usada a string de conexão `Default`. Consulte a documentação sobre [strings de conexão](https://docs.abp.io/en/abp/latest/Connection-Strings) para obter mais detalhes.

#### Entity Framework Core

##### Tabelas

- **AbpBackgroundJobs**

#### MongoDB

##### Coleções

- **AbpBackgroundJobs**

## Veja Também

* [Sistema de tarefas em segundo plano](../Background-Jobs.md)