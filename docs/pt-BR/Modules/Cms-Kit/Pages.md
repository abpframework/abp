# Kit CMS: Páginas

O sistema de páginas do Kit CMS permite que você crie páginas dinâmicas especificando URLs, que é a funcionalidade fundamental de um CMS.

## Habilitando a Funcionalidade de Páginas

Por padrão, as funcionalidades do Kit CMS estão desabilitadas. Portanto, você precisa habilitar as funcionalidades que deseja antes de começar a usá-lo. Você pode usar o sistema de [Funcionalidades Globais](../../Global-Features.md) para habilitar/desabilitar as funcionalidades do Kit CMS durante o desenvolvimento. Alternativamente, você pode usar o [Sistema de Funcionalidades](https://docs.abp.io/en/abp/latest/Features) do ABP Framework para desabilitar uma funcionalidade do Kit CMS em tempo de execução.

> Verifique a seção "Como Instalar" da documentação do Módulo Kit CMS (Index.md#how-to-install) para saber como habilitar/desabilitar as funcionalidades do Kit CMS durante o desenvolvimento.

## A Interface do Usuário

### Itens do Menu

O módulo Kit CMS adiciona os seguintes itens ao menu principal, sob o item de menu *CMS*:

* **Páginas**: Página de gerenciamento de páginas.

A classe `CmsKitAdminMenus` possui as constantes para os nomes dos itens do menu.

### Páginas

#### Gerenciamento de Páginas

A página **Páginas** é usada para gerenciar páginas dinâmicas no sistema. Você pode criar/editar páginas com rotas e conteúdos dinâmicos nesta página:

![pages-edit](../../images/cmskit-module-pages-edit.png)

Depois de criar páginas, você pode definir uma delas como página inicial. Então, sempre que alguém navegar para a página inicial do seu aplicativo, eles verão o conteúdo dinâmico da página que você definiu nesta página.

![pages-page](../../images/cmskit-module-pages-page.png)

Além disso, quando você cria uma página, você pode acessá-la através da URL `/{slug}`.