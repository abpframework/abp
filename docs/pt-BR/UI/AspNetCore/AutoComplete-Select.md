# ASP.NET Core MVC / Razor Pages: Auto-Complete Select
Um componente de seleção simples às vezes não é útil com uma grande quantidade de dados. O ABP fornece uma implementação de seleção que funciona com paginação e pesquisa no lado do servidor usando o [Select2](https://select2.org/). Ele funciona bem com escolhas únicas ou múltiplas.

Uma captura de tela pode ser mostrada abaixo.

| Único | Múltiplo |
| --- | --- |
| ![autocomplete-select-example](../../images/abp-select2-single.png) |![autocomplete-select-example](../../images/abp-select2-multiple.png) |

## Começando

Esta é uma funcionalidade central e é usada pelo ABP Framework. Não há instalação personalizada ou pacotes adicionais necessários.

## Uso

Um uso simples é apresentado abaixo.

```html
<select asp-for="Book.AuthorId" 
    class="auto-complete-select"
    data-autocomplete-api-url="/api/app/author"
    data-autocomplete-display-property="name"
    data-autocomplete-value-property="id"
    data-autocomplete-items-property="items"
    data-autocomplete-filter-param-name="filter"
    data-autocomplete-allow-clear="true">

    <!-- Você pode definir a(s) opção(ões) selecionada(s) aqui  -->
    <option selected value="@SelectedAuthor.Id">@SelectedAuthor.Name</option>
</select>
```

O select deve ter a classe `auto-complete-select` e os seguintes atributos:

- `data-autocomplete-api-url`: * URL do endpoint da API para obter os itens da seleção. Será enviado uma requisição **GET** para esta URL.
- `data-autocomplete-display-property`: * Nome da propriedade para exibição. _(Por exemplo: `name` ou `title`. Nome da propriedade da entidade/dto.)_.
- `data-autocomplete-value-property`: * Nome da propriedade identificadora. _(Por exemplo: `id`)_.
- `data-autocomplete-items-property`: * Nome da propriedade da coleção no objeto de resposta. _(Por exemplo: `items`)_
- `data-autocomplete-filter-param-name`: * Nome da propriedade de texto de filtro. _(Por exemplo: `filter`)_.
- `data-autocomplete-selected-item-name`: Texto para exibir como item selecionado.
- `data-autocomplete-parent-selector`: Expressão seletora jQuery para o DOM pai. _(Se estiver em um modal, é sugerido enviar o seletor do modal como este parâmetro)_.
- `data-autocomplete-allow-clear`: Se `true`, permitirá limpar o valor selecionado. Valor padrão: `false`.
- `data-autocomplete-placeholder`: Texto de espaço reservado para exibir quando nenhum valor estiver selecionado.

Além disso, o(s) valor(es) selecionado(s) deve(m) ser definido(s) com as tags `<option>` dentro do select, uma vez que a paginação é aplicada e as opções selecionadas podem não ter sido carregadas ainda.


### Escolhas Múltiplas
O AutoComplete Select suporta escolhas múltiplas. Se a tag select tiver o atributo `multiple`, permitirá escolher várias opções.

```html
<select asp-for="Book.TagIds" 
    class="auto-complete-select"
    multiple="multiple"
    data-autocomplete-api-url="/api/app/tags"
    data-autocomplete-display-property="name"
    data-autocomplete-value-property="id"
    data-autocomplete-items-property="items"
    data-autocomplete-filter-param-name="filter">
    @foreach(var tag in SelectedTags)
    {
        <option selected value="@tag.Id">@tag.Name</option>
    }
</select>
```

Será automaticamente vinculado a uma coleção do tipo de valor definido.
```csharp
    public List<Guid> TagIds { get; set; }
```

## Avisos
Se o usuário autenticado não tiver permissão na URL fornecida, o usuário receberá um erro de autorização. Tenha cuidado ao projetar esse tipo de interface de usuário.
Você pode criar um endpoint/método específico, [não autorizado](../../Authorization.md), para obter a lista de itens, para que a página possa recuperar dados de pesquisa de uma entidade dependente sem dar permissão de leitura completa aos usuários.