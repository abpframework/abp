# Kit CMS: Blogging

A funcionalidade de blogging fornece a interface necessária para gerenciar e renderizar blogs e postagens de blog.

## Habilitando a funcionalidade de blogging

Por padrão, as funcionalidades do Kit CMS estão desabilitadas. Portanto, você precisa habilitar as funcionalidades que deseja antes de começar a usá-lo. Você pode usar o sistema de [Global Feature](../../Global-Features.md) para habilitar/desabilitar as funcionalidades do Kit CMS durante o desenvolvimento. Alternativamente, você pode usar o [Sistema de Funcionalidades](https://docs.abp.io/en/abp/latest/Features) do ABP Framework para desabilitar uma funcionalidade do Kit CMS em tempo de execução.

> Verifique a seção "Como instalar" da documentação do módulo Kit CMS (Index.md#how-to-install) para saber como habilitar/desabilitar as funcionalidades do Kit CMS durante o desenvolvimento.

## Interface do usuário

### Itens do menu

Os seguintes itens de menu são adicionados pela funcionalidade de blogging à aplicação de administração:

* **Blogs**: Página de gerenciamento de blogs.
* **Postagens de blog**: Página de gerenciamento de postagens de blog.

## Páginas

### Blogs

A página de blogs é usada para criar e gerenciar blogs no seu sistema.

![página-de-blogs](../../images/cmskit-module-blogs-page.png)

Uma captura de tela do modal de criação de novo blog:

![edição-de-blogs](../../images/cmskit-module-blogs-edit.png)

**Slug** é a parte da URL do blog. Para este exemplo, a URL raiz do blog se torna `seu-domínio.com/blogs/blog-técnico/`.

- Você pode alterar o slug padrão usando a constante `CmsBlogsWebConsts.BlogRoutePrefix`. Por exemplo, se você definir como `foo`, a URL raiz do blog se torna `seu-domínio.com/foo/blog-técnico/`.

    ```csharp
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        CmsBlogsWebConsts.BlogsRoutePrefix = "foo";
    }
    ```

#### Funcionalidades do blog

A funcionalidade de blog usa algumas das outras funcionalidades do Kit CMS. Você pode habilitar ou desabilitar as funcionalidades clicando na ação de funcionalidades para um blog.

![ação-de-funcionalidades-de-blogs](../../images/cmskit-module-blogs-feature-action.png)

Você pode selecionar/deselecionar as funcionalidades desejadas para as postagens de blog.

![diálogo-de-funcionalidades](../../images/cmskit-module-features-dialog-2.png)

##### Barra de navegação rápida na postagem de blog

Se você habilitar "Barra de navegação rápida nas postagens de blog", será habilitado o índice de rolagem conforme mostrado abaixo.

![índice-de-rolagem](../../images/cmskit-module-features-scroll-index.png)

### Gerenciamento de postagens de blog

Ao criar blogs, você pode gerenciar as postagens de blog nesta página.

![página-de-postagens-de-blog](../../images/cmskit-module-blog-posts-page.png)

Você pode criar e editar uma postagem de blog existente nesta página. Se você habilitar funcionalidades específicas, como tags, poderá definir tags para a postagem de blog nesta página.

![edição-de-postagem-de-blog](../../images/cmskit-module-blog-post-edit.png)

## Internos

### Camada de domínio

#### Agregados

Este módulo segue o guia de [Melhores Práticas e Convenções de Entidades](https://docs.abp.io/en/abp/latest/Best-Practices/Entities).

- `Blog` _(raiz do agregado)_: Apresenta blogs da aplicação.
- `BlogPost` _(raiz do agregado)_: Apresenta postagens de blog nos blogs.
- `BlogFeature` _(raiz do agregado)_: Apresenta o estado de habilitação/desabilitação das funcionalidades do blog, como reações, avaliações, comentários, etc.

#### Repositórios

Este módulo segue o guia de [Melhores Práticas e Convenções de Repositórios](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories). Os seguintes repositórios são definidos para esta funcionalidade:

- `IBlogRepository`
- `IBlogPostRepository`
- `IBlogFeatureRepository`

#### Serviços de domínio

Este módulo segue o guia de [Melhores Práticas e Convenções de Serviços de Domínio](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services).

- `BlogManager`
- `BlogPostManager`
- `BlogFeatureManager`

### Camada de aplicação

#### Serviços de aplicação

##### Comum

- `BlogFeatureAppService` _(implementa `IBlogFeatureAppService`)_

##### Administração

- `BlogAdminAppService` _(implementa `IBlogAdminAppService`)_
- `BlogFeatureAdminAppService` _(implementa `IBlogFeatureAdminAppService`)_
- `BlogPostAdminAppService` _(implementa `IBlogPostAdminAppService`)_

##### Público

- `BlogPostPublicAppService` _(implementa `IBlogPostPublicAppService`)_

### Provedores de banco de dados

#### Entity Framework Core

##### Tabelas

- CmsBlogs
- CmsBlogPosts
- CmsBlogFeatures

#### MongoDB

##### Coleções

- CmsBlogs
- CmsBlogPosts
- CmsBlogFeatures

## Extensões de entidade

Verifique a seção "Extensões de Entidade" da documentação do módulo Kit CMS (Index.md#entity-extensions) para saber como estender as entidades da funcionalidade de Blogging do módulo Kit CMS.