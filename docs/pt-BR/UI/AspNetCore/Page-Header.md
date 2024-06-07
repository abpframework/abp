# ASP.NET Core MVC / Razor Pages: Cabeçalho da Página

O serviço `IPageLayout` pode ser usado para definir o título da página, o item de menu selecionado e os itens do breadcrumb para uma página. É responsabilidade do [tema](Theming.md) renderar esses elementos na página.

## IPageLayout

O `IPageLayout` pode ser injetado em qualquer página/view para definir as propriedades do cabeçalho da página.

### Título da Página

O título da página pode ser definido da seguinte forma:

```csharp
@inject IPageLayout PageLayout
@{
    PageLayout.Content.Title = "Lista de Livros";
}
```

* O título da página é definido na tag HTML `title` (além do [nome da marca/aplicativo](Branding.md)).
* O tema pode renderizar o título da página antes do conteúdo da página (ainda não implementado pelo Tema Básico).

### Breadcrumb

> **O [Tema Básico](Basic-Theme.md) atualmente não implementa o breadcrumb.**
> 
> O [Tema LeptonX Lite](../../Themes/LeptonXLite/AspNetCore.md) suporta o breadcrumb.

Itens do breadcrumb podem ser adicionados ao `PageLayout.Content.BreadCrumb`.

**Exemplo: Adicionar Gerenciamento de Idioma aos itens do breadcrumb.**

```
PageLayout.Content.BreadCrumb.Add("Gerenciamento de Idioma");
```

O tema então renderiza o breadcrumb. Um exemplo do resultado renderizado pode ser:

![exemplo-breadcrumb](../../images/exemplo-breadcrumb.png)

* O ícone Home é renderizado por padrão. Defina `PageLayout.Content.BreadCrumb.ShowHome` como `false` para ocultá-lo.
* O nome da página atual (obtido do `PageLayout.Content.Title`) é adicionado por padrão como o último item. Defina `PageLayout.Content.BreadCrumb.ShowCurrent` como `false` para ocultá-lo.

Qualquer item que você adicionar é inserido entre o Home e os itens da página atual. Você pode adicionar quantos itens forem necessários. O método `BreadCrumb.Add(...)` recebe três parâmetros:

* `text`: O texto a ser exibido para o item do breadcrumb.
* `url` (opcional): Uma URL para navegar, se o usuário clicar no item do breadcrumb.
* `icon` (opcional): Uma classe de ícone (como `fas fa-user-tie` para Font-Awesome) para exibir junto com o `text`.

### O Item de Menu Selecionado

> **O [Tema Básico](Basic-Theme.md) atualmente não implementa o item de menu selecionado, pois não é aplicável ao menu superior, que é a única opção para o Tema Básico no momento.**
>
> O [Tema LeptonX Lite](../../Themes/LeptonXLite/AspNetCore.md) suporta o item de menu selecionado.

Você pode definir o nome do item de menu relacionado a esta página:

```csharp
PageLayout.Content.MenuItemName = "BookStore.Books";
```

O nome do item de menu deve corresponder a um nome de item de menu único definido usando o sistema de [Navegação / Menu](Navigation-Menu.md). Nesse caso, espera-se que o tema torne o item de menu "ativo" no menu principal.