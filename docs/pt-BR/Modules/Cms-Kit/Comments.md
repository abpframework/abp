# Kit CMS: Comentários

O Kit CMS fornece um sistema de **comentários** para adicionar a funcionalidade de comentários a qualquer tipo de recurso, como postagens de blog, produtos, etc.

## Habilitando a funcionalidade de comentários

Por padrão, as funcionalidades do Kit CMS estão desabilitadas. Portanto, você precisa habilitar as funcionalidades desejadas antes de começar a usá-lo. Você pode usar o sistema de [Funcionalidades Globais](../../Global-Features.md) para habilitar/desabilitar as funcionalidades do Kit CMS durante o desenvolvimento. Alternativamente, você pode usar o [Sistema de Funcionalidades](https://docs.abp.io/pt-BR/abp/latest/Features) do ABP Framework para desabilitar uma funcionalidade do Kit CMS em tempo de execução.

> Verifique a seção ["Como instalar" da documentação do módulo Kit CMS](Index.md#como-instalar) para saber como habilitar/desabilitar as funcionalidades do Kit CMS durante o desenvolvimento.

## Opções

O sistema de comentários fornece um mecanismo para agrupar definições de comentários por tipos de entidade. Por exemplo, se você deseja usar o sistema de comentários para postagens de blog e produtos, você precisa definir dois tipos de entidade chamados `BlogPosts` e `Product` e adicionar comentários sob esses tipos de entidade.

`CmsKitCommentOptions` pode ser configurado na camada de domínio, no método `ConfigureServices` do seu [módulo](https://docs.abp.io/pt-BR/abp/latest/Module-Development-Basics). Exemplo:

```csharp
Configure<CmsKitCommentOptions>(options =>
{
    options.EntityTypes.Add(new CommentEntityTypeDefinition("Product"));
    options.IsRecaptchaEnabled = true; //false por padrão
    options.AllowedExternalUrls = new Dictionary<string, List<string>>
    {
      {
        "Product",
        new List<string>
        {
          "https://abp.io/"
        }
      }
    };
});
```

> Se você estiver usando a [Funcionalidade de Blog](Blogging.md), o ABP Framework define automaticamente um tipo de entidade para a funcionalidade de blog. Você pode facilmente substituir ou remover os tipos de entidade predefinidos no método `Configure` como mostrado acima.

Propriedades de `CmsKitCommentOptions`:

- `EntityTypes`: Lista de tipos de entidade (`CmsKitCommentOptions`) definidos no sistema de comentários.
- `IsRecaptchaEnabled`: Esta flag habilita ou desabilita o reCaptcha para o sistema de comentários. Você pode defini-la como **true** se quiser usar o reCaptcha em seu sistema de comentários.
- `AllowedExternalUrls`: Indica as URLs externas permitidas por tipos de entidade, que podem ser incluídas em um comentário. Se for especificado para um determinado tipo de entidade, apenas as URLs externas especificadas serão permitidas nos comentários.

Propriedades de `CommentEntityTypeDefinition`:

- `EntityType`: Nome do tipo de entidade.

## O Widget de Comentários

O sistema de comentários fornece um [widget](../../UI/AspNetCore/Widgets.md) de comentários para permitir que os usuários enviem comentários para recursos em sites públicos. Você pode simplesmente colocar o widget em uma página como abaixo.

```csharp
@await Component.InvokeAsync(typeof(CommentingViewComponent), new
{
  entityType = "Product",
  entityId = "...",
  isReadOnly = false,
  referralLinks  = new [] {"nofollow"}
})
```

`entityType` foi explicado na seção anterior. `entityId` deve ser o id único do produto, neste exemplo. Se você tiver uma entidade Produto, você pode usar seu Id aqui. `referralLinks` é um parâmetro opcional. Você pode usar este parâmetro para adicionar valores (como "nofollow", "noreferrer" ou qualquer outro valor) aos atributos [rel](https://developer.mozilla.org/pt-BR/docs/Web/HTML/Attributes/rel) dos links.

## Interface do Usuário

### Itens de Menu

Os seguintes itens de menu são adicionados pela funcionalidade de comentários à aplicação de administração:

* **Comentários**: Abre a página de gerenciamento de comentários.

### Páginas

#### Gerenciamento de Comentários

Você pode visualizar e gerenciar comentários nesta página.

![comment-page](../../images/cmskit-module-comment-page.png)

Você também pode visualizar e gerenciar respostas nesta página.

![comments-detail](../../images/cmskit-module-comments-detail.png)

## Internos

### Camada de Domínio

#### Agregados

Este módulo segue o guia de [Melhores Práticas e Convenções de Entidades](https://docs.abp.io/pt-BR/abp/latest/Best-Practices/Entities).

##### Comentário

Um comentário representa um comentário escrito por um usuário.

- `Comment` (raiz do agregado): Representa um comentário escrito no sistema.

#### Repositórios

Este módulo segue o guia de [Melhores Práticas e Convenções de Repositórios](https://docs.abp.io/pt-BR/abp/latest/Best-Practices/Repositories).

Os seguintes repositórios personalizados são definidos para esta funcionalidade:

- `ICommentRepository`

#### Serviços de Domínio

Este módulo segue o guia de [Melhores Práticas e Convenções de Serviços de Domínio](https://docs.abp.io/pt-BR/abp/latest/Best-Practices/Domain-Services).

##### Gerenciador de Comentários

`CommentManager` é usado para realizar algumas operações para a raiz do agregado `Comment`.

### Camada de Aplicação

#### Serviços de Aplicação

- `CommentAdminAppService` (implementa `ICommentAdminAppService`): Implementa os casos de uso do sistema de gerenciamento de comentários, como listar ou remover comentários, etc.
- `CommentPublicAppService` (implementa `ICommentPublicAppService`): Implementa os casos de uso do sistema de gerenciamento de comentários em sites públicos, como listar comentários, adicionar comentários, etc.

### Provedores de Banco de Dados

#### Comum

##### Prefixo de tabela / coleção e esquema

Todas as tabelas/coleções usam o prefixo `Cms` por padrão. Defina propriedades estáticas na classe `CmsKitDbProperties` se você precisar alterar o prefixo da tabela ou definir um nome de esquema (se suportado pelo seu provedor de banco de dados).

##### String de conexão

Este módulo usa `CmsKit` como nome da string de conexão. Se você não definir uma string de conexão com esse nome, ela será usada a string de conexão `Default`.

Consulte a documentação de [strings de conexão](https://docs.abp.io/pt-BR/abp/latest/Connection-Strings) para obter detalhes.

#### Entity Framework Core

##### Tabelas

- CmsComments

#### MongoDB

##### Coleções

- **CmsComments**