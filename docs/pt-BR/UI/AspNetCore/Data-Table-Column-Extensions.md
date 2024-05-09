# Extensões de Colunas de Tabela de Dados para a Interface do Usuário ASP.NET Core

## Introdução

O sistema de extensão de colunas de tabela de dados permite adicionar uma **nova coluna de tabela** na interface do usuário. O exemplo abaixo adiciona uma nova coluna com o título "Número de Seguro Social":

![user-action-extension-click-me](../../images/table-column-extension-example.png)

Você pode usar as opções de coluna padrão para controlar a coluna da tabela com precisão.

> Observe que esta é uma API de baixo nível para controlar a coluna da tabela. Se você deseja mostrar uma propriedade de extensão na tabela, consulte o documento [extensão de entidade de módulo](../../Module-Entity-Extensions.md).

## Como Configurar

### Criar um Arquivo JavaScript

Primeiro, adicione um novo arquivo JavaScript à sua solução. Nós adicionamos dentro da pasta `/Pages/Identity/Users` do projeto `.Web`:

![user-action-extension-on-solution](../../images/user-action-extension-on-solution.png)

Aqui está o conteúdo deste arquivo JavaScript:

```js
abp.ui.extensions.tableColumns
    .get('identity.user')
    .addContributor(function (columnList) {
        columnList.addTail({ //adicionar como a última coluna
            title: 'Número de Seguro Social',
            data: 'extraProperties.SocialSecurityNumber',
            orderable: false,
            render: function (data, type, row) {
                if (row.extraProperties.SocialSecurityNumber) {
                    return '<strong>' + 
                        row.extraProperties.SocialSecurityNumber + 
                        '<strong>';
                } else {
                    return '<i class="text-muted">indefinido</i>';
                }
            }
        });
    });
```

Este exemplo define uma função `render` personalizada para retornar um HTML personalizado para ser renderizado na coluna.

### Adicionar o Arquivo à Página de Gerenciamento de Usuários

Em seguida, você precisa adicionar este arquivo JavaScript à página de gerenciamento de usuários. Você pode aproveitar o poder do sistema de [Agrupamento e Minificação](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Bundling-Minification).

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

Esta configuração adiciona `my-user-extensions.js` à página de gerenciamento de usuários do Módulo de Identidade. `typeof(Volo.Abp.Identity.Web.Pages.Identity.Users.IndexModel).FullName` é o nome do pacote na página de gerenciamento de usuários. Esta é uma convenção comum usada para todos os módulos comerciais do ABP.

### Renderizando a Coluna

Este exemplo pressupõe que você definiu uma propriedade extra `SocialSecurityNumber` usando o sistema de [extensão de entidade de módulo](../../Module-Entity-Extensions.md). No entanto;

* Você pode adicionar uma nova coluna relacionada a uma propriedade existente do usuário (que não foi adicionada à tabela por padrão). Exemplo:

````js
abp.ui.extensions.tableColumns
    .get('identity.user')
    .addContributor(function (columnList) {
        columnList.addTail({
            title: 'Telefone confirmado?',
            data: 'phoneNumberConfirmed',
            render: function (data, type, row) {
                if (row.phoneNumberConfirmed) {
                    return '<strong style="color: green">SIM<strong>';
                } else {
                    return '<i class="text-muted">NÃO</i>';
                }
            }
        });
    });
````

* Você pode adicionar uma nova coluna personalizada que não está relacionada a nenhuma propriedade da entidade, mas sim a uma informação completamente personalizada. Exemplo:

````js
abp.ui.extensions.tableColumns
    .get('identity.user')
    .addContributor(function (columnList) {
        columnList.addTail({
            title: 'Coluna personalizada',
            data: {},
            orderable: false,
            render: function (data) {
                if (data.phoneNumber) {
                    return "ligar: " + data.phoneNumber;
                } else {
                    return '';
                }
            }
        });
    });
````

## API

Esta seção explica os detalhes da API JavaScript `abp.ui.extensions.tableColumns`.

### abp.ui.extensions.tableColumns.get(nomeEntidade)

Este método é usado para acessar as colunas da tabela para uma entidade de um módulo específico. Ele recebe um parâmetro:

* **nomeEntidade**: O nome da entidade definido pelo módulo relacionado.

### abp.ui.extensions.tableColumns.get(nomeEntidade).columns

A propriedade `columns` é usada para recuperar uma [lista duplamente encadeada](../Common/Utils/Linked-List.md) de colunas previamente definidas para uma tabela. Todos os contribuidores são executados na ordem para preparar a lista final de colunas. Isso é normalmente chamado pelos módulos para mostrar as colunas na tabela. No entanto, você pode usá-lo se estiver construindo suas próprias interfaces extensíveis.

### abp.ui.extensions.tableColumns.get(nomeEntidade).addContributor(contributeCallback [, order])

O método `addContributor` cobre todos os cenários, por exemplo, se você deseja adicionar sua coluna em uma posição diferente na lista, alterar ou remover uma coluna existente. `addContributor` tem os seguintes parâmetros:

* **contributeCallback**: Uma função de retorno de chamada que é chamada sempre que a lista de colunas deve ser criada. Você pode modificar livremente a lista de colunas dentro deste método de retorno de chamada.
* **order** (opcional): A ordem da chamada na lista de chamadas. Sua chamada é adicionada ao final da lista (portanto, você tem a oportunidade de modificar as colunas adicionadas pelos contribuidores anteriores). Você pode definir `0` para adicionar seu contribuidor como o primeiro item.

#### Exemplo

```js
var myColumnDefinition = {
    title: 'Coluna personalizada',
    data: {},
    orderable: false,
    render: function(data) {
        if (data.phoneNumber) {
            return "ligar: " + data.phoneNumber;
        } else {
            return '';
        }
    }
};

abp.ui.extensions.tableColumns
    .get('identity.user')
    .addContributor(function (columnList) {
        // Remover um item da lista de ações
        columnList.dropHead();

        // Adicionar um novo item à lista de ações
        columnList.addHead(myColumnDefinition);
    });
```

> `columnList` é uma [lista encadeada](../Common/Utils/Linked-List.md). Você pode usar seus métodos para construir uma lista de colunas da maneira que precisar.