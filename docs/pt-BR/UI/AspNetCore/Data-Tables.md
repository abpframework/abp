# ASP.NET Core MVC / Razor Pages: Tabelas de Dados

Uma Tabela de Dados (também conhecida como Data Grid) é um componente de interface do usuário para mostrar dados tabulares aos usuários. Existem muitos componentes/bibliotecas de Tabela de Dados e **você pode usar qualquer um que preferir** com o ABP Framework. No entanto, os modelos de inicialização vêm com a biblioteca [DataTables.Net](https://datatables.net/) já **pré-instalada e configurada**. O ABP Framework fornece adaptadores para essa biblioteca e facilita o uso com os endpoints da API.

Uma captura de tela de exemplo da página de gerenciamento de usuários que mostra a lista de usuários em uma tabela de dados:

![exemplo-datatables](../../images/exemplo-datatables.png)

## Integração com o DataTables.Net

Antes de tudo, você pode seguir a documentação oficial para entender como o [DataTables.Net](https://datatables.net/) funciona. Esta seção se concentrará nos complementos e pontos de integração do ABP, em vez de cobrir completamente o uso dessa biblioteca.

### Um Exemplo Rápido

Você pode seguir o [tutorial de desenvolvimento de aplicativos da web](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC) para um exemplo completo de aplicativo que usa o DataTables.Net como Tabela de Dados. Esta seção mostra um exemplo minimalista.

Você não precisa fazer nada para adicionar a biblioteca DataTables.Net à página, pois ela já está adicionada ao [pacote](Bundling-Minification.md) global por padrão.

Primeiro, adicione um `abp-table` como mostrado abaixo, com um `id`:

````html
<abp-table striped-rows="true" id="BooksTable"></abp-table>
````

> `abp-table` é um [Tag Helper](Tag-Helpers/Index.md) definido pelo ABP Framework, mas uma simples tag `<table...>` também funcionaria.

Em seguida, chame o plugin `DataTable` no seletor da tabela:

````js
var dataTable = $('#BooksTable').DataTable(
    abp.libs.datatables.normalizeConfiguration({
        serverSide: true,
        paging: true,
        order: [[1, "asc"]],
        searching: false,
        ajax: abp.libs.datatables.createAjax(acme.bookStore.books.book.getList),
        columnDefs: [
            {
                title: l('Ações'),
                rowAction: {
                    items:
                        [
                            {
                                text: l('Editar'),
                                action: function (data) {
                                    ///...
                                }
                            }
                        ]
                }
            },
            {
                title: l('Nome'),
                data: "name"
            },
            {
                title: l('Data de Publicação'),
                data: "publishDate",
                render: function (data) {
                    return luxon
                        .DateTime
                        .fromISO(data, {
                            locale: abp.localization.currentCulture.name
                        }).toLocaleString();
                }
            },
            {
                title: l('Preço'),
                data: "price"
            }
        ]
    })
);
````

O código de exemplo acima usa algumas funcionalidades de integração do ABP que serão explicadas nas próximas seções.

### Normalização de Configuração

A função `abp.libs.datatables.normalizeConfiguration` recebe uma configuração do DataTables e a normaliza para simplificá-la;

* Define a opção `scrollX` como `true`, se não estiver definida.
* Define o índice `target` para as definições de coluna.
* Define a opção `language` para [localizar](../../Localization.md) a tabela no idioma atual.

#### Configuração Padrão

`normalizeConfiguration` usa a configuração padrão. Você pode alterar a configuração padrão usando o objeto `abp.libs.datatables.defaultConfigurations`. Exemplo:

````js
abp.libs.datatables.defaultConfigurations.scrollX = false;
````

Aqui estão todas as opções de configuração;

* `scrollX`: `false` por padrão.
* `dom`: O valor padrão é `<"dataTable_filters"f>rt<"row dataTable_footer"<"col-auto"l><"col-auto"i><"col"p>>`.
* `language`: Uma função que retorna o texto de localização usando o idioma atual.

### Adaptador AJAX

O DataTables.Net possui seu próprio formato de dados esperado ao obter os resultados de uma chamada AJAX para o servidor para obter os dados da tabela. Eles estão especialmente relacionados a como os parâmetros de paginação e ordenação são enviados e recebidos. O ABP Framework também oferece suas próprias convenções para a comunicação [AJAX](JavaScript-API/Ajax.md) cliente-servidor.

O método `abp.libs.datatables.createAjax` (usado no exemplo acima) adapta o formato dos dados de solicitação e resposta e funciona perfeitamente com o sistema [Dynamic JavaScript Client Proxy](Dynamic-JavaScript-Proxies.md).

Isso funciona automaticamente, então na maioria das vezes você não precisa saber como funciona. Consulte o documento [DTO](../../Data-Transfer-Objects.md) se você quiser aprender mais sobre `IPagedAndSortedResultRequest`, `IPagedResult` e outras interfaces padrão e classes DTO base usadas na comunicação cliente-servidor.

O `createAjax` também permite que você personalize os parâmetros de solicitação e manipule as respostas.

**Exemplo:**

````csharp
var inputAction = function (requestData, dataTableSettings) {
    return {
        id: $('#Id').val(),
        name: $('#Name').val(),
    };
};

var responseCallback = function(result) {

    // seu código personalizado.

    return {
        recordsTotal: result.totalCount,
        recordsFiltered: result.totalCount,
        data: result.items
    };
};

ajax: abp.libs.datatables.createAjax(acme.bookStore.books.book.getList, inputAction, responseCallback)
````

Se você não precisa acessar ou modificar o `requestData` ou o `dataTableSettings`, você pode especificar um objeto simples como segundo parâmetro.

````js
ajax: abp.libs.datatables.createAjax(
    acme.bookStore.books.book.getList, 
    { id: $('#Id').val(), name: $('#Name').val() }
)
````

### Ações de Linha

`rowAction` é uma opção definida pelo ABP Framework para as definições de coluna para mostrar um botão suspenso para executar ações para uma linha na tabela.

A captura de tela de exemplo abaixo mostra as ações para cada usuário na tabela de gerenciamento de usuários:

![exemplo-datatables](../../images/exemplo-datatables-acao-linha.png)

`rowAction` é definido como parte de uma definição de coluna:

````csharp
{
    title: l('Ações'),
    rowAction: {
        //TODO: CONFIGURAÇÃO
    }
},
````

**Exemplo: Mostrar ações *Editar* e *Excluir* para uma linha de livro**

````js
{
    title: l('Ações'),
    rowAction: {
        items:
            [
                {
                    text: l('Editar'),
                    action: function (data) {
                        //TODO: Abrir um modal para editar o livro
                    }
                },
                {
                    text: l('Excluir'),
                    confirmMessage: function (data) {
                        return "Tem certeza de que deseja excluir o livro " + data.record.name;
                    },
                    action: function (data) {
                        acme.bookStore.books.book
                            .delete(data.record.id)
                            .then(function() {
                                abp.notify.info("Excluído com sucesso!");
                                data.table.ajax.reload();
                            });
                    }
                }
            ]
    }
},
````

#### Itens de Ação

`items` é uma matriz de definições de ação. Uma definição de ação pode ter as seguintes opções;

* `text`: O texto (uma `string`) para esta ação a ser mostrada no menu suspenso de ações.
* `action`: Uma `function` que é executada quando o usuário clica na ação. A função recebe um argumento `data` que possui os seguintes campos;
  * `data.record`: Este é o objeto de dados relacionado à linha. Você pode acessar os campos de dados como `data.record.id`, `data.record.name`... etc.
  * `data.table`: A instância DataTables.
* `confirmMessage`: Uma `function` (veja o exemplo acima) que retorna uma mensagem (`string`) para mostrar um diálogo para obter uma confirmação do usuário antes de executar a `action`. Exemplo de diálogo de confirmação:

![exemplo-datatables-acao-linha-confirmacao](../../images/exemplo-datatables-acao-linha-confirmacao.png)

Você pode usar o sistema de [localização](JavaScript-API/Localization.md) para mostrar uma mensagem localizada.

* `visible`: Um `bool` ou uma `function` que retorna um `bool`. Se o resultado for `false`, então a ação não é mostrada no menu suspenso de ações. Isso geralmente é combinado com o sistema de [autorização](JavaScript-API/Auth.md) para ocultar a ação se o usuário não tiver permissão para executar essa ação. Exemplo:

````js
visible: abp.auth.isGranted('BookStore.Books.Delete');
````

Se você definir uma `function`, então a `function` tem dois argumentos: `record` (o objeto de dados da linha relacionada) e a `table` (a instância DataTables). Portanto, você pode decidir mostrar/ocultar a ação dinamicamente, com base nos dados da linha e em outras condições.

* `iconClass`: Pode ser usado para mostrar um ícone de fonte, como um ícone [Font-Awesome](https://fontawesome.com/) (ex: `fas fa-trash-alt`), próximo ao texto da ação. Captura de tela de exemplo:

![exemplo-datatables-acao-linha-icon](../../images/exemplo-datatables-acao-linha-icon.png)

* `enabled`: Uma `function` que retorna um `bool` para desabilitar a ação. A `function` recebe um objeto `data` com dois campos: `data.record` é o objeto de dados relacionado à linha e `data.table` é a instância DataTables.
* `displayNameHtml`: Defina isso como `true` se o valor `text` contiver tags HTML.

Existem algumas regras com os itens de ação;

* Se nenhum dos itens de ação for visível, a coluna de ações não será renderizada.

### Formato de Dados

#### O Problema

Veja a coluna *Data de Criação* no exemplo abaixo:

````js
{
    title: l('CreationTime'),
    data: "creationTime",
    render: function (data) {
        return luxon
            .DateTime
            .fromISO(data, {
                locale: abp.localization.currentCulture.name
            }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
    }
}
````

O `render` é uma opção padrão do DataTables para renderizar o conteúdo da coluna por uma função personalizada. Este exemplo usa a biblioteca [luxon](https://moment.github.io/luxon/) (que é instalada por padrão) para escrever um valor legível por humanos do `creationTime` no idioma atual do usuário. Exemplo de saída da coluna:

![exemplo-datatables-custom-render-date](../../images/exemplo-datatables-custom-render-date.png)

Se você não definir a opção de renderização, o resultado será feio e não amigável ao usuário:

![exemplo-datatables-custom-render-date](../../images/exemplo-datatables-default-render-date.png)

No entanto, renderizar um `DateTime` é quase o mesmo e repetir a mesma lógica de renderização em todos os lugares vai contra o princípio DRY (Don't Repeat Yourself!).

#### Opção dataFormat

A opção `dataFormat` da coluna especifica o formato de dados que é usado para renderizar os dados da coluna. A mesma saída poderia ser alcançada usando a seguinte definição de coluna:

````js
{
    title: l('CreationTime'),
    data: "creationTime",
    dataFormat: 'datetime'
}
````

`dataFormat: 'datetime'` especifica o formato de dados para esta coluna. Existem alguns `dataFormat`s pré-definidos:

* `boolean`: Mostra um ícone de `check` para o valor `true` e um ícone de `times` para o valor `false` e é útil para renderizar valores `bool`.
* `date`: Mostra a parte da data de um valor `DateTime`, formatado com base na cultura atual.
* `datetime`: Mostra a data e a hora (excluindo segundos) de um valor `DateTime`, formatado com base na cultura atual.

### Renderizadores Padrão

A opção `abp.libs.datatables.defaultRenderers` permite que você defina novos formatos de dados e defina renderizadores para eles.

**Exemplo: Renderizar ícones de masculino/feminino com base no gênero**

````js
abp.libs.datatables.defaultRenderers['gender'] = function(value) {
    if (value === 'f') {
        return '<i class="fa fa-venus"></i>';
    } else {
        return '<i class="fa fa-mars"></i>';
    }
};
````

Supondo que os valores possíveis para os dados de uma coluna sejam `f` e `m`, o formato de dados `gender` mostra ícones femininos/masculinos em vez dos textos `f` e `m`. Agora você pode definir `dataFormat: 'gender'` para uma definição de coluna que tenha os valores de dados adequados.

> Você pode escrever os renderizadores padrão em um único arquivo JavaScript e adicioná-lo ao [Pacote de Scripts Global](Bundling-Minification.md), para que você possa reutilizá-los em todas as páginas.

## Outras Tabelas de Dados

Você pode usar qualquer biblioteca que preferir. Por exemplo, [veja este artigo](https://community.abp.io/articles/using-devextreme-components-with-the-abp-framework-zb8z7yqv) para aprender como usar o DevExtreme Data Grid em seus aplicativos.