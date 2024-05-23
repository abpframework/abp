# Sistema de Avaliação

O kit CMS fornece um sistema de **avaliação** para adicionar recursos de avaliação a qualquer tipo de recurso, como postagens de blog, comentários, etc. Aqui está como o componente de avaliação se parece em uma página de exemplo:

![avaliações](../../images/cmskit-module-ratings.png)

## Habilitando o Recurso de Avaliação

Por padrão, os recursos do CMS Kit estão desabilitados. Portanto, você precisa habilitar os recursos que deseja antes de começar a usá-lo. Você pode usar o sistema de [Recurso Global](../../Global-Features.md) para habilitar/desabilitar os recursos do CMS Kit durante o desenvolvimento. Alternativamente, você pode usar o [Sistema de Recursos](https://docs.abp.io/en/abp/latest/Features) do ABP Framework para desabilitar um recurso do CMS Kit em tempo de execução.

> Verifique a seção ["Como Instalar" da documentação do Módulo CMS Kit](Index.md#how-to-install) para ver como habilitar/desabilitar os recursos do CMS Kit durante o desenvolvimento.

## Opções

O sistema de avaliação fornece um mecanismo para agrupar avaliações por tipos de entidade. Por exemplo, se você deseja usar o sistema de avaliação para produtos, você precisa definir um tipo de entidade chamado `Produto` e, em seguida, adicionar avaliações sob o tipo de entidade definido.

`CmsKitRatingOptions` pode ser configurado na camada de domínio, no método `ConfigureServices` do seu [módulo](https://docs.abp.io/en/abp/latest/Module-Development-Basics). Exemplo:

```csharp
Configure<CmsKitRatingOptions>(options =>
{
    options.EntityTypes.Add(new RatingEntityTypeDefinition("Produto"));
});
```

> Se você estiver usando o [Recurso de Blog](Blogging.md), o framework ABP define automaticamente um tipo de entidade para o recurso de blog. Você pode facilmente substituir ou remover os tipos de entidade predefinidos no método `Configure` como mostrado acima.

Propriedades de `CmsKitRatingOptions`:

- `EntityTypes`: Lista de tipos de entidade definidos (`RatingEntityTypeDefinition`) no sistema de avaliação.

Propriedades de `RatingEntityTypeDefinition`:

- `EntityType`: Nome do tipo de entidade.

## O Widget de Avaliação

O sistema de avaliação fornece um widget de avaliação para permitir que os usuários enviem avaliações para recursos em sites públicos. Você pode simplesmente colocar o widget em uma página como abaixo.

```csharp
@await Component.InvokeAsync(typeof(RatingViewComponent), new
{
  entityType = "Produto",
  entityId = "entityId",
  isReadOnly = false
})
```

`entityType` foi explicado na seção anterior. `entityId` deve ser o id único do produto, neste exemplo. Se você tiver uma entidade Produto, você pode usar seu Id aqui.

# Internos

## Camada de Domínio

#### Agregados

Este módulo segue o guia de [Melhores Práticas e Convenções de Entidades](https://docs.abp.io/en/abp/latest/Best-Practices/Entities).

##### Avaliação

Uma avaliação representa uma avaliação dada por um usuário.

- `Avaliação` (raiz do agregado): Representa uma avaliação dada no sistema.

#### Repositórios

Este módulo segue o guia de [Melhores Práticas e Convenções de Repositórios](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories).

Os seguintes repositórios personalizados são definidos para este recurso:

- `IRatingRepository`

#### Serviços de Domínio

Este módulo segue o guia de [Melhores Práticas e Convenções de Serviços de Domínio](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services).

##### Gerenciador de Reações

`RatingManager` é usado para realizar algumas operações para a raiz do agregado `Avaliação`.

### Camada de Aplicação

#### Serviços de Aplicação

- `RatingPublicAppService` (implementa `IRatingPublicAppService`): Implementa os casos de uso do sistema de avaliação.

### Provedores de Banco de Dados

#### Comum

##### Prefixo da tabela / coleção e esquema

Todas as tabelas/coleções usam o prefixo `Cms` por padrão. Defina propriedades estáticas na classe `CmsKitDbProperties` se você precisar alterar o prefixo da tabela ou definir um nome de esquema (se suportado pelo seu provedor de banco de dados).

##### String de conexão

Este módulo usa `CmsKit` como nome da string de conexão. Se você não definir uma string de conexão com esse nome, ela será usada a string de conexão `Default`.

Consulte a documentação de [strings de conexão](https://docs.abp.io/en/abp/latest/Connection-Strings) para obter detalhes.

#### Entity Framework Core

##### Tabelas

- CmsRatings

#### MongoDB

##### Coleções

- **CmsRatings**