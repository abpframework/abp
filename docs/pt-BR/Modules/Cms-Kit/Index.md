# Módulo CMS Kit

Este módulo fornece recursos de CMS (Sistema de Gerenciamento de Conteúdo) para sua aplicação. Ele fornece **blocos de construção principais** e **sub-sistemas** totalmente funcionais para criar seu próprio site com recursos de CMS habilitados, ou usar os blocos de construção em seus sites com qualquer finalidade.

> **Este módulo está atualmente disponível apenas para a interface MVC / Razor Pages**. Embora não haja um pacote oficial do Blazor, ele também pode funcionar em uma interface Blazor Server, pois uma interface Blazor Server é na verdade um aplicativo híbrido que é executado em um aplicativo ASP.NET Core MVC / Razor Pages.

Os seguintes recursos estão atualmente disponíveis:

* Fornece um sistema de gerenciamento de [**páginas**](Pages.md) para gerenciar páginas dinâmicas com URLs dinâmicos.
* Fornece um sistema de [**blog**](Blogging.md) para criar e publicar postagens de blog com suporte a vários blogs.
* Fornece um sistema de [**marcação**](Tags.md) para marcar qualquer tipo de recurso, como uma postagem de blog.
* Fornece um sistema de [**comentários**](Comments.md) para adicionar recursos de comentários a qualquer tipo de recurso, como postagem de blog ou uma página de avaliação de produto.
* Fornece um sistema de [**reações**](Reactions.md) para adicionar recursos de reações (emojis) a qualquer tipo de recurso, como uma postagem de blog ou um comentário.
* Fornece um sistema de [**classificação**](Ratings.md) para adicionar recursos de classificação a qualquer tipo de recurso.
* Fornece um sistema de [**menu**](Menus.md) para gerenciar menus públicos dinamicamente.
* Fornece um sistema de [**recursos globais**](Global-Resources.md) para adicionar estilos e scripts globais dinamicamente.
* Fornece um sistema de [**widget dinâmico**](Dynamic-Widget.md) para criar widgets dinâmicos para páginas e postagens de blog.

> Você pode clicar nos links de recursos acima para entender e aprender como usá-los.

Todos os recursos podem ser usados individualmente. Se você desabilitar um recurso, ele desaparecerá completamente de sua aplicação, inclusive das tabelas do banco de dados, com a ajuda do sistema [Recursos Globais](../../Global-Features.md).

## Pré-requisitos

- Este módulo depende do módulo [BlobStoring](../../Blob-Storing.md) para armazenar conteúdo de mídia.
> Certifique-se de que o módulo `BlobStoring` esteja instalado e que pelo menos um provedor esteja configurado corretamente. Para obter mais informações, consulte a [documentação](../../Blob-Storing.md).

- O CMS Kit usa o [cache distribuído](../../Caching.md) para responder mais rapidamente.
> É altamente recomendado usar um cache distribuído, como o [Redis](../../Redis-Cache.md), para garantir a consistência dos dados em implantações distribuídas/clusterizadas.

## Como instalar

O [ABP CLI](../../CLI.md) permite instalar um módulo em uma solução usando o comando `add-module`. Você pode instalar o módulo CMS Kit em um terminal de linha de comando com o seguinte comando:

```bash
abp add-module Volo.CmsKit --skip-db-migrations
```

> Por padrão, o Cms-Kit está desabilitado pelo `GlobalFeature`. Por causa disso, a migração inicial estará vazia. Portanto, você pode pular a migração adicionando `--skip-db-migrations` ao comando de instalação se estiver usando o Entity Framework Core. Após habilitar o recurso global Cms-Kit, adicione uma nova migração.

Após o processo de instalação, abra a classe `GlobalFeatureConfigurator` no projeto `Domain.Shared` de sua solução e coloque o seguinte código no método `Configure` para habilitar todos os recursos no módulo CMS Kit.

```csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.EnableAll();
});
```

Em vez de habilitar todos, você pode preferir habilitar os recursos um por um. O exemplo a seguir habilita apenas os recursos de [marcadores](Tags.md) e [comentários](Comments.md):

````csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.Tags.Enable();
    cmsKit.Comments.Enable();
});
````

> Se você estiver usando o Entity Framework Core, não se esqueça de adicionar uma nova migração e atualizar seu banco de dados.

## Os Pacotes

Este módulo segue o [guia de melhores práticas para desenvolvimento de módulos](https://docs.abp.io/en/abp/latest/Best-Practices/Index) e consiste em vários pacotes NuGet e NPM. Consulte o guia se você quiser entender os pacotes e as relações entre eles.

Os pacotes do CMS kit são projetados para vários cenários de uso. Se você verificar os [pacotes do CMS kit](https://www.nuget.org/packages?q=Volo.CmsKit), verá que alguns pacotes têm sufixos `Admin` e `Public`. A razão é que o módulo possui duas camadas de aplicação, considerando que eles podem ser usados em diferentes tipos de aplicativos. Essas camadas de aplicação usam uma única camada de domínio:

 - Os pacotes `Volo.CmsKit.Admin.*` contêm as funcionalidades necessárias para aplicativos de administração (back office).
 - Os pacotes `Volo.CmsKit.Public.*` contêm as funcionalidades usadas em sites públicos onde os usuários leem postagens de blog ou deixam comentários.
 - Os pacotes `Volo.CmsKit.*` (sem sufixo Admin/Public) são chamados de pacotes unificados. Os pacotes unificados são atalhos para adicionar pacotes Admin e Public (da camada relacionada) separadamente. Se você tiver um único aplicativo para administração e site público, você pode usar esses pacotes.

## Internos

### Prefixo de tabela / coleção e esquema

Todas as tabelas/coleções usam o prefixo `Cms` por padrão. Defina propriedades estáticas na classe `CmsKitDbProperties` se você precisar alterar o prefixo da tabela ou definir um nome de esquema (se suportado pelo provedor de banco de dados).

### String de conexão

Este módulo usa `CmsKit` como nome da string de conexão. Se você não definir uma string de conexão com esse nome, ela será substituída pela string de conexão `Default`.

Consulte a documentação sobre [strings de conexão](https://docs.abp.io/en/abp/latest/Connection-Strings) para obter mais detalhes.

## Extensões de Entidade

O sistema de extensão de entidade do módulo (https://docs.abp.io/en/abp/latest/Module-Entity-Extensions) é um sistema de extensão **de alto nível** que permite **definir novas propriedades** para entidades existentes dos módulos dependentes. Ele automaticamente **adiciona propriedades à entidade**, **banco de dados**, **API HTTP** e **interface do usuário** em um único ponto.

Para estender as entidades do módulo CMS Kit, abra a classe `YourProjectNameModuleExtensionConfigurator` dentro do projeto `DomainShared` e altere o método `ConfigureExtraProperties` conforme mostrado abaixo.

```csharp
public static void ConfigureExtraProperties()
{
    OneTimeRunner.Run(() =>
    {
        ObjectExtensionManager.Instance.Modules()
            .ConfigureCmsKit(cmsKit =>
            {
                cmsKit.ConfigureBlog(plan => // estende a entidade Blog
                {
                    plan.AddOrUpdateProperty<string>( //tipo de propriedade: string
                      "BlogDescription", //nome da propriedade
                      property => {
                        //regras de validação
                        property.Attributes.Add(new RequiredAttribute()); //adiciona o atributo obrigatório à propriedade definida

                        //...outras configurações para esta propriedade
                      }
                    );
                });
              
                cmsKit.ConfigureBlogPost(blogPost => // estende a entidade BlogPost
                    {
                        blogPost.AddOrUpdateProperty<string>( //tipo de propriedade: string
                        "BlogPostDescription", //nome da propriedade
                        property => {
                            //regras de validação
                            property.Attributes.Add(new RequiredAttribute()); //adiciona o atributo obrigatório à propriedade definida
                            property.Attributes.Add(
                            new StringLengthAttribute(MyConsts.MaximumDescriptionLength) {
                                MinimumLength = MyConsts.MinimumDescriptionLength
                            }
                            );

                            //...outras configurações para esta propriedade
                        }
                        );
                });  
            });
    });
}
```
 
* O método `ConfigureCmsKit(...)` é usado para configurar as entidades do módulo CMS Kit.

* `cmsKit.ConfigureBlog(...)` é usado para configurar a entidade **Blog** do módulo CMS Kit. Você pode adicionar ou atualizar suas propriedades extras na entidade **Blog**. 

* `cmsKit.ConfigureBlogPost(...)` é usado para configurar a entidade **BlogPost** do módulo CMS Kit. Você pode adicionar ou atualizar suas propriedades extras na entidade **BlogPost**.

* Você também pode definir algumas regras de validação para a propriedade que você definiu. No exemplo acima, foram adicionados `RequiredAttribute` e `StringLengthAttribute` para a propriedade chamada **"BlogPostDescription"**. 

* Quando você define a nova propriedade, ela será automaticamente adicionada à **Entidade**, **API HTTP** e **UI** para você. 
  * Depois de definir uma propriedade, ela aparece nos formulários de criação e atualização da entidade relacionada. 
  * As novas propriedades também aparecem na tabela de dados da página relacionada.