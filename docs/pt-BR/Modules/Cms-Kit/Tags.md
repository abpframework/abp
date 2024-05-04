# Gerenciamento de Tags

O CMS Kit fornece um sistema de **tag** para marcar qualquer tipo de recurso, como uma postagem de blog.

## Habilitando o recurso de Gerenciamento de Tags

Por padrão, os recursos do CMS Kit estão desabilitados. Portanto, você precisa habilitar os recursos que deseja antes de começar a usá-lo. Você pode usar o sistema de [Recursos Globais](../../Global-Features.md) para habilitar/desabilitar os recursos do CMS Kit durante o desenvolvimento. Alternativamente, você pode usar o [Sistema de Recursos](https://docs.abp.io/en/abp/latest/Features) do ABP Framework para desabilitar um recurso do CMS Kit em tempo de execução.

> Verifique a seção ["Como instalar" da documentação do Módulo CMS Kit](Index.md#how-to-install) para saber como habilitar/desabilitar os recursos do CMS Kit durante o desenvolvimento.

## Opções

O sistema de tags fornece um mecanismo para agrupar tags por tipos de entidade. Por exemplo, se você deseja usar o sistema de tags para postagens de blog e produtos, você precisa definir dois tipos de entidade chamados `BlogPosts` e `Product` e adicionar tags sob esses tipos de entidade.

`CmsKitTagOptions` pode ser configurado na camada de domínio, no método `ConfigureServices` da sua classe [módulo](https://docs.abp.io/en/abp/latest/Module-Development-Basics).

**Exemplo: Adicionando suporte a tags para produtos**

```csharp
Configure<CmsKitTagOptions>(options =>
{
    options.EntityTypes.Add(new TagEntityTypeDefiniton("Product"));
});
```

> Se você estiver usando o [Recurso de Blogging](Blogging.md), o framework ABP define automaticamente um tipo de entidade para o recurso de blog.

Propriedades de `CmsKitTagOptions`:

- `EntityTypes`: Lista de tipos de entidade definidos (`TagEntityTypeDefiniton`) no sistema de tags.

Propriedades de `TagEntityTypeDefiniton`:

- `EntityType`: Nome do tipo de entidade.
- `DisplayName`: O nome de exibição do tipo de entidade. Você pode usar um nome de exibição amigável para mostrar a definição do tipo de entidade no site de administração.
- `CreatePolicies`: Lista de nomes de políticas/permissões que permitem aos usuários criar tags sob o tipo de entidade.
- `UpdatePolicies`: Lista de nomes de políticas/permissões que permitem aos usuários atualizar tags sob o tipo de entidade.
- `DeletePolicies`: Lista de nomes de políticas/permissões que permitem aos usuários excluir tags sob o tipo de entidade.

## O Widget de Tag

O sistema de tags fornece um widget de tag [widget](../../UI/AspNetCore/Widgets.md) para exibir as tags associadas a um recurso que foi configurado para tags. Você pode simplesmente colocar o widget em uma página como a abaixo:

```csharp
@await Component.InvokeAsync(typeof(TagViewComponent), new
{
  entityType = "Product",
  entityId = "...",
  urlFormat = "/products?tagId={TagId}&tagName={TagName}"
})
```

`entityType` foi explicado na seção anterior. Neste exemplo, o `entityId` deve ser o id único do produto. Se você tiver uma entidade `Product`, você pode usar seu Id aqui. `urlFormat` é o formato de string do URL que será gerado para cada tag. Você pode usar os espaços reservados `{TagId}` e `{TagName}` para preencher o URL. Por exemplo, o formato de URL acima preencherá URLs como `/products?tagId=1&tagName=tag1`.

## O Widget de Tags Populares

O sistema de tags fornece um widget de tags populares [widget](../../UI/AspNetCore/Widgets.md) para exibir tags populares de um recurso que foi configurado para tags. Você pode simplesmente colocar o widget em uma página como abaixo:

```csharp
@await Component.InvokeAsync(typeof(PopularTagsViewComponent), new
{
  entityType = "Product",
  urlFormat = "/products?tagId={TagId}&tagName={TagName}",
  maxCount = 10
})
```

`entityType` foi explicado na seção anterior. `urlFormat` foi explicado na seção anterior. `maxCount` é o número máximo de tags a serem exibidas.

## Interface do Usuário

### Itens de Menu

Os seguintes itens de menu são adicionados pelo recurso de tags à aplicação de administração:

* **Tags**: Abre a página de gerenciamento de tags.

### Páginas

#### Gerenciamento de Tags

Esta página pode ser usada para criar, editar e excluir tags para os tipos de entidade.

![tags-page](../../images/cmskit-module-tags-page.png)

Você pode criar ou editar uma tag existente nesta página.

![tag-edit](../../images/cmskit-module-tag-edit.png)

## Internos

### Camada de Domínio

#### Agregados

Este módulo segue o guia de [Melhores Práticas e Convenções de Entidades](https://docs.abp.io/en/abp/latest/Best-Practices/Entities).

##### Tag

Uma tag representa uma tag sob o tipo de entidade.

- `Tag` (raiz do agregado): Representa uma tag no sistema.

##### EntityTag

Uma entidade tag representa uma conexão entre a tag e a entidade marcada.

- `EntityTag`(entidade): Representa uma conexão entre a tag e a entidade marcada.

#### Repositórios

Este módulo segue o guia de [Melhores Práticas e Convenções de Repositórios](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories).

Os seguintes repositórios personalizados são definidos para este recurso:

- `ITagRepository`
- `IEntityTagRepository`

#### Serviços de Domínio

Este módulo segue o guia de [Melhores Práticas e Convenções de Serviços de Domínio](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services).

##### Gerenciador de Tags

`TagManager` realiza algumas operações para a raiz do agregado `Tag`.

##### Gerenciador de Entidade Tag

`EntityTagManager` realiza algumas operações para a entidade `EntityTag`.

### Camada de Aplicação

#### Serviços de Aplicação

- `TagAdminAppService` (implementa `ITagAdminAppService`).
- `EntityTagAdminAppService` (implementa `IEntityTagAdminAppService`).
- `TagAppService` (implementa `ITagAppService`).

### Provedores de Banco de Dados

#### Comum

##### Prefixo de Tabela / Coleção e esquema

Todas as tabelas/coleções usam o prefixo `Cms` por padrão. Defina propriedades estáticas na classe `CmsKitDbProperties` se você precisar alterar o prefixo da tabela ou definir um nome de esquema (se suportado pelo seu provedor de banco de dados).

##### String de conexão

Este módulo usa `CmsKit` como nome da string de conexão. Se você não definir uma string de conexão com esse nome, ela será usada a string de conexão `Default`.

Consulte a documentação de [strings de conexão](https://docs.abp.io/en/abp/latest/Connection-Strings) para obter detalhes.

#### Entity Framework Core

##### Tabelas

- CmsTags
- CmsEntityTags

#### MongoDB

##### Coleções

- **CmsTags**
- **CmsEntityTags**