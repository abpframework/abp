# Extensões de Ação de Entidade para a Interface do Usuário do ASP.NET Core

## Introdução

O sistema de extensão de ação de entidade permite adicionar uma **nova ação** ao menu de ações de uma entidade. Uma ação **Clique em Mim** foi adicionada à página de *Gerenciamento de Usuários* abaixo:

![user-action-extension-click-me](../../images/user-action-extension-click-me.png)

Você pode executar qualquer ação (abrir um modal, fazer uma chamada de API HTTP, redirecionar para outra página... etc) escrevendo seu código personalizado. Você pode acessar a entidade atual em seu código.

## Como Configurar

Neste exemplo, adicionaremos uma ação "Clique em Mim!" e executaremos um código JavaScript para a página de gerenciamento de usuários do [Módulo de Identidade](../../Modules/Identity.md).

### Criar um Arquivo JavaScript

Primeiro, adicione um novo arquivo JavaScript à sua solução. Nós adicionamos dentro da pasta `/Pages/Identity/Users` do projeto `.Web`:

![user-action-extension-on-solution](../../images/user-action-extension-on-solution.png)

Aqui está o conteúdo deste arquivo JavaScript:

```js
var clickMeAction = {
    text: 'Clique em Mim!',
    action: function(data) {
        //TODO: Escreva seu código personalizado
        alert(data.record.userName);
    }
};

abp.ui.extensions.entityActions
    .get('identity.user')
    .addContributor(function(actionList) {
        actionList.addTail(clickMeAction);
    });
```

Na função `action`, você pode fazer qualquer coisa que precisar. Consulte a seção API para obter um uso detalhado.

### Adicionar o Arquivo à Página de Gerenciamento de Usuários

Em seguida, você precisa adicionar este arquivo JavaScript à página de gerenciamento de usuários. Você pode aproveitar o poder do [Sistema de Agrupamento e Minificação](Bundling-Minification.md).

Escreva o seguinte código dentro do método `ConfigureServices` da sua classe de módulo:

```csharp
Configure<AbpBundlingOptions>(options =>
{
    options.ScriptBundles.Configure(
        typeof(Volo.Abp.Identity.Web.Pages.Identity.Users.IndexModel).FullName,
        bundleConfiguration =>
        {
            bundleConfiguration.AddFiles(
                "/Pages/Identity/Users/my-user-extensions.js"
            );
        });
});
```

Essa configuração adiciona `my-user-extensions.js` à página de gerenciamento de usuários do Módulo de Identidade. `typeof(Volo.Abp.Identity.Web.Pages.Identity.Users.IndexModel).FullName` é o nome do pacote na página de gerenciamento de usuários. Essa é uma convenção comum usada para todos os módulos comerciais do ABP.

Isso é tudo. Execute sua aplicação para ver o resultado.

## API

Esta seção explica os detalhes da API JavaScript `abp.ui.extensions.entityActions`.

### abp.ui.extensions.entityActions.get(entityName)

Este método é usado para acessar as ações de entidade de um módulo específico. Ele recebe um parâmetro:

* **entityName**: O nome da entidade definido pelo módulo relacionado.

### abp.ui.extensions.entityActions.get(entityName).actions

A propriedade `actions` é usada para recuperar uma [lista duplamente encadeada](../Common/Utils/Linked-List.md) de ações previamente definidas para uma entidade. Todos os contribuidores são executados para preparar a lista final de ações. Isso é normalmente chamado pelos módulos para mostrar as ações na grade. No entanto, você pode usá-lo se estiver construindo suas próprias interfaces extensíveis.

### abp.ui.extensions.entityActions.get(entityName).addContributor(contributeCallback)

O método `addContributor` cobre todos os cenários, por exemplo, se você deseja adicionar sua ação em uma posição diferente na lista, alterar ou remover um item de ação existente. `addContributor` com o seguinte parâmetro:

* **contributeCallback**: Uma função de retorno de chamada que é chamada sempre que a lista de ações deve ser criada. Você pode modificar livremente a lista de ações dentro deste método de retorno de chamada.

#### Exemplo

```js
var clickMe2Action = {
    text: 'Clique em Mim 2!',
    icon: 'fas fa-hand-point-right',
    action: function(data) {
        //TODO: Escreva seu código personalizado
        alert(data.record.userName);
    }
};

abp.ui.extensions.entityActions
    .get('identity.user')
    .addContributor(function(actionList) {
        // Remover um item da actionList
        actionList.dropHead();
        
        // Adicionar o novo item à actionList
        actionList.addHead(clickMe2Action);
    });
```

> `actionList` é uma [lista encadeada](../Common/Utils/Linked-List.md). Você pode usar seus métodos para construir uma lista de colunas da maneira que precisar.