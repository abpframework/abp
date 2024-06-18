# Módulo de Registro de Auditoria

O Módulo de Registro de Auditoria basicamente implementa o `IAuditingStore` para salvar os objetos de registro de auditoria em um banco de dados.

> Este documento abrange apenas o módulo de registro de auditoria que persiste os registros de auditoria em um banco de dados. Consulte o documento [registro de auditoria](../Audit-Logging.md) para obter mais informações sobre o sistema de registro de auditoria.

## Como Instalar

Este módulo vem pré-instalado (como pacotes NuGet/NPM). Você pode continuar a usá-lo como pacote e obter atualizações facilmente, ou pode incluir seu código-fonte em sua solução (consulte o comando `get-source` [CLI](../CLI.md)) para desenvolver seu módulo personalizado.

### O Código Fonte

O código fonte deste módulo pode ser acessado [aqui](https://github.com/abpframework/abp/tree/dev/modules/audit-logging). O código fonte é licenciado com [MIT](https://choosealicense.com/licenses/mit/), então você pode usá-lo e personalizá-lo livremente.

## Internos

### Camada de Domínio

#### Agregados

- `AuditLog` (raiz do agregado): Representa um registro de log de auditoria no sistema.
  - `EntityChange` (coleção): Entidades alteradas do registro de auditoria.
  - `AuditLogAction` (coleção): Ações executadas do registro de auditoria.

#### Repositórios

Os seguintes repositórios personalizados são definidos para este módulo:

- `IAuditLogRepository`

### Provedores de Banco de Dados

#### Comum

##### Prefixo da tabela / coleção e esquema

Todas as tabelas/coleções usam o prefixo `Abp` por padrão. Defina propriedades estáticas na classe `AbpAuditLoggingDbProperties` se você precisar alterar o prefixo da tabela ou definir um nome de esquema (se suportado pelo seu provedor de banco de dados).

##### String de conexão

Este módulo usa `AbpAuditLogging` como nome da string de conexão. Se você não definir uma string de conexão com esse nome, ela será usada a string de conexão `Default`. Consulte a documentação sobre [strings de conexão](https://docs.abp.io/en/abp/latest/Connection-Strings) para obter mais detalhes.

#### Entity Framework Core

##### Tabelas

- **AbpAuditLogs**
  - AbpAuditLogActions
  - AbpEntityChanges
    - AbpEntityPropertyChanges

#### MongoDB

##### Coleções

- **AbpAuditLogs**

## Veja Também

* [Sistema de registro de auditoria](../Audit-Logging.md)