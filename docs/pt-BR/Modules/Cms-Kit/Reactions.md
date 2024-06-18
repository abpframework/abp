# Sistema de Reações

O kit CMS fornece um sistema de **reações** para adicionar recursos de reações a qualquer tipo de recurso, como postagens de blog ou comentários.

O componente de reação permite que os usuários reajam ao seu conteúdo por meio de ícones/emojis pré-definidos. Aqui está como o componente de reações pode parecer:

![reactions](../../images/cmskit-module-reactions.png)

Você também pode personalizar os ícones de reação mostrados no componente de reação.

## Habilitando o Recurso de Reação

Por padrão, os recursos do CMS Kit estão desabilitados. Portanto, você precisa habilitar os recursos que deseja antes de começar a usá-lo. Você pode usar o sistema de [Recurso Global](../../Global-Features.md) para habilitar/desabilitar recursos do CMS Kit durante o desenvolvimento. Alternativamente, você pode usar o [Sistema de Recursos](https://docs.abp.io/en/abp/latest/Features) do ABP Framework para desabilitar um recurso do CMS Kit em tempo de execução.

> Verifique a seção ["Como Instalar" da documentação do Módulo CMS Kit](Index.md#how-to-install) para ver como habilitar/desabilitar recursos do CMS Kit durante o desenvolvimento.

## Opções

O sistema de reação fornece um mecanismo para agrupar reações por tipos de entidade. Por exemplo, se você deseja usar o sistema de reação para produtos, você precisa definir um tipo de entidade chamado `Produto` e, em seguida, adicionar reações sob o tipo de entidade definido.

`CmsKitReactionOptions` pode ser configurado na camada de domínio, no método `ConfigureServices` do seu [módulo](https://docs.abp.io/en/abp/latest/Module-Development-Basics). Exemplo:

```csharp
Configure<CmsKitReactionOptions>(options =>
{
    options.EntityTypes.Add(
        new ReactionEntityTypeDefinition(
            "Produto",
            reactions: new[]
            {
                new ReactionDefinition(StandardReactions.Smile),
                new ReactionDefinition(StandardReactions.ThumbsUp),
                new ReactionDefinition(StandardReactions.ThumbsDown),
                new ReactionDefinition(StandardReactions.Confused),
                new ReactionDefinition(StandardReactions.Eyes),
                new ReactionDefinition(StandardReactions.Heart)
            }));
});
```

> Se você estiver usando os recursos [Comentário](Comments.md) ou [Blogging](Blogging.md), o framework ABP define reações predefinidas para esses recursos automaticamente.

Propriedades de `CmsKitReactionOptions`:

- `EntityTypes`: Lista de tipos de entidade definidos (`CmsKitReactionOptions`) no sistema de reação.

Propriedades de `ReactionEntityTypeDefinition`:

- `EntityType`: Nome do tipo de entidade.
- `Reactions`: Lista de reações definidas (`ReactionDefinition`) no tipo de entidade.

## O Widget de Reações

O sistema de reação fornece um widget de reação para permitir que os usuários enviem reações para recursos. Você pode colocar o widget em uma página da seguinte forma:

```csharp
@await Component.InvokeAsync(typeof(ReactionSelectionViewComponent), new
{
  entityType = "Produto",
  entityId = "..."
})
```

`entityType` foi explicado na seção anterior. `entityId` deve ser o id único do produto, neste exemplo. Se você tiver uma entidade Produto, pode usar seu Id aqui.

# Internos

## Camada de Domínio

#### Agregados

Este módulo segue o guia de [Melhores Práticas e Convenções de Entidades](https://docs.abp.io/en/abp/latest/Best-Practices/Entities).

##### UserReaction

Uma reação do usuário representa uma reação específica de um usuário.

- `UserReaction` (raiz do agregado): Representa uma reação específica no sistema.

#### Repositórios

Este módulo segue o guia de [Melhores Práticas e Convenções de Repositórios](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories).

Os seguintes repositórios personalizados são definidos para este recurso:

- `IUserReactionRepository`

#### Serviços de Domínio

Este módulo segue o guia de [Melhores Práticas e Convenções de Serviços de Domínio](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services).

##### Reaction Manager

`ReactionManager` é usado para realizar algumas operações para a raiz do agregado `UserReaction`.

### Camada de Aplicação

#### Serviços de Aplicação

- `ReactionPublicAppService` (implementa `IReactionPublicAppService`): Implementa os casos de uso do sistema de reação.

### Provedores de Banco de Dados

#### Comum

##### Prefixo da tabela/collection e esquema

Todas as tabelas/collections usam o prefixo `Cms` por padrão. Defina propriedades estáticas na classe `CmsKitDbProperties` se você precisar alterar o prefixo da tabela ou definir um nome de esquema (se suportado pelo seu provedor de banco de dados).

##### String de conexão

Este módulo usa `CmsKit` como nome da string de conexão. Se você não definir uma string de conexão com esse nome, ela será usada a string de conexão `Default`.

Consulte a documentação de [strings de conexão](https://docs.abp.io/en/abp/latest/Connection-Strings) para obter detalhes.

#### Entity Framework Core

##### Tabelas

- CmsUserReactions

#### MongoDB

##### Coleções

- **CmsUserReactions**