# ASP.NET Core MVC / Razor Pages: Branding

## IBrandingProvider

`IBrandingProvider` é uma interface simples que é usada para mostrar o nome e o logotipo da aplicação no layout.

A captura de tela abaixo mostra *MyProject* como o nome da aplicação:

![branding-nobrand](../../images/branding-nobrand.png)

Você pode implementar a interface `IBrandingProvider` ou herdar da classe `DefaultBrandingProvider` para definir o nome da aplicação:

````csharp
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace MyProject.Web
{
    [Dependency(ReplaceServices = true)]
    public class MyProjectBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Book Store";

        public override string LogoUrl => "/logo.png";
    }
}
````

> Atualmente, definir o `AppName` só é aplicável ao [Tema Básico](../../Themes/Basic.md), não tem efeito nos outros [temas oficiais](../../Themes/Index.md).

O resultado será como mostrado abaixo:

![bookstore-added-logo](../../images/bookstore-added-logo.png)

`IBrandingProvider` possui as seguintes propriedades:

* `AppName`: O nome da aplicação.
* `LogoUrl`: Uma URL para mostrar o logotipo da aplicação.
* `LogoReverseUrl`: Uma URL para mostrar o logotipo da aplicação em um tema de cor reversa (escuro, por exemplo).

> **Dica**: `IBrandingProvider` é usado em cada atualização de página. Para uma aplicação multi-inquilino, você pode retornar um nome de aplicação específico do inquilino para personalizá-lo por inquilino.

## Sobrescrevendo a Área de Branding

Você pode consultar o [Guia de Customização de UI](Customization-User-Interface.md) para aprender como substituir a área de branding por um componente de visualização personalizado.