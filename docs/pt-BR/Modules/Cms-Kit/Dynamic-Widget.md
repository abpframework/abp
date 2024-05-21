# Widget Dinâmico

O kit CMS fornece um [widget](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Widgets) dinâmico usado para renderizar os componentes previamente desenvolvidos pelo software no conteúdo das páginas e postagens de blog. Isso significa que você pode usar conteúdo dinâmico em conteúdo estático. Vamos mencionar como você pode fazer isso. Você tem duas opções para definir o widget no sistema: escrevendo e usando a interface do usuário.

### Adicionando o widget
Primeiramente, mostraremos como usar o sistema de widgets escrevendo manualmente no conteúdo das páginas e postagens de blog.

Vamos definir o componente de visualização

```csharp
[Widget]
[ViewComponent(Name = "CmsToday")]
public class TodayViewComponent : AbpViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("~/ViewComponents/Today.cshtml",
         new TodayViewComponent());
    }
} 
```

```html
@model Volo.CmsKit.ViewComponents.TodayViewComponent

<p>Bem-vindo ao componente de hoje</p>
<p>@DateTime.Now.ToString()</p>

```

Agora é hora de configurar no arquivo YourModule.cs
```csharp
Configure<CmsKitContentWidgetOptions>(options =>
    {
        options.AddWidget("Today","CmsToday");
    }); 
```

Agora você está pronto para adicionar seu widget escrevendo.
[Widget Type="Today"]

Após concluir as etapas acima, você pode ver a saída à direita da captura de tela abaixo.
![cmskit-without-parameter.png](../../images/cmskit-without-parameter.png)

### Adicionando usando a interface do usuário
Agora mencionaremos a segunda opção, usando a interface do usuário.
Uma vez que escrever essas definições pode resultar em alguns erros, adicionamos um novo recurso para usar o sistema de widgets facilmente. À direita do editor, você verá o botão `W` personalizado para adicionar um widget dinâmico, como na imagem abaixo. Não se esqueça, por favor, que este é o modo de design e você precisa visualizar sua página no modo de visualização após salvar. Além disso, a guia `Preview` no editor estará pronta para verificar sua saída facilmente para configurações de widget nos recursos seguintes.

![cms-kit-page-editor](../../images/cms-kit-page-editor.png)

### Adicionando usando a interface do usuário com parâmetros
Vamos melhorar o exemplo acima adicionando um novo parâmetro chamado formato. Com esse recurso, podemos usar o sistema de widgets com muitos cenários diferentes, mas sem prolongar o documento. Além disso, esses exemplos podem ser expandidos com injeção de dependência e obtenção de valores do banco de dados, mas usaremos um exemplo básico. Vamos adicionar o parâmetro de formato para personalizar a data.

```csharp
[Widget]
[ViewComponent(Name = "CmsToday")]
public class TodayViewComponent : AbpViewComponent
{
    public string Format { get; set; }

    public IViewComponentResult Invoke(string format)
    {
        return View("~/ViewComponents/Today.cshtml",
         new TodayViewComponent() { Format = format });
    }
} 
```

```html
@model Volo.CmsKit.ViewComponents.TodayViewComponent

<p>Bem-vindo ao componente de hoje</p>
<p>@DateTime.Now.ToString(Format)</p>

```

Vamos definir o componente de formato.
```csharp
[Widget]
[ViewComponent(Name = "Format")]
public class FormatViewComponent : AbpViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("~/ViewComponents/Format.cshtml",
         new FormatViewModel());
    }  
}

public class FormatViewModel
{
    [DisplayName("Formate sua data no componente")]
    public string Format { get; set; }
}
```
> Nota importante: Para obter as propriedades corretamente, você deve definir a propriedade `name` na página razor ou pode usar o componente ABP. O ABP lida com isso automaticamente.

```html
@using Volo.CmsKit.ViewComponents
@model FormatViewModel

<div>
    <abp-input asp-for="Format" />
</div>
```

```csharp
Configure<CmsKitContentWidgetOptions>(options =>
    {
        options.AddWidget("Today", "CmsToday", "Format");
    }); 
```

![cmskit-module-editor-parameter](../../images/cmskit-module-editor-parameter.png)

Nesta imagem, após escolher seu widget (em outro caso, ele muda automaticamente de acordo com sua configuração, o meu é `Today`. Seu nome de parâmetro é `parameterWidgetName` e seu valor é `Format`) você verá o próximo widget. Insira valores de entrada ou escolha-os e clique em `Add`. Você verá a saída sublinhada no editor. À direita da imagem, você também pode ver sua saída pré-visualizada.

Você pode editar essa saída manualmente se fizer algum código errado para isso (valor incorreto ou erro de digitação), você não verá o widget, mesmo assim, sua página será visualizada com sucesso.

## Opções
Para configurar o widget, você deve definir o código abaixo no arquivo YourModule.cs

```csharp
Configure<CmsKitContentWidgetOptions>(options =>
    {
        options.AddWidget(widgetType: "Today", widgetName: "CmsToday", parameterWidgetName: "Format");
    }); 
```

Vamos analisar esses parâmetros em detalhes
* `widgetType` é usado para o usuário final e nomes mais legíveis. A palavra em negrito a seguir representa o widgetType.
[Widget Type="**Today**" Format="yyyy-dd-mm HH:mm:ss"].

* `widgetName` é usado para o nome do seu widget usado no código para o nome do `ViewComponent`.

* `parameterWidgetName` é usado no lado do componente do editor para ver no modal `Add Widget`.
Após escolher o tipo de widget na lista suspensa (agora apenas definido `Format`) e renderizar este widget automaticamente. É necessário apenas para ver a interface do usuário uma vez usando parâmetros.