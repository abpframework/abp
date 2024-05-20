# Cabeçalhos de Segurança

O ABP Framework permite que você adicione cabeçalhos de segurança frequentemente usados em sua aplicação. Os seguintes cabeçalhos de segurança serão adicionados como cabeçalhos de resposta à sua aplicação se você usar o middleware `UseAbpSecurityHeaders`:

* `X-Content-Type-Options`: Informa ao navegador para não tentar adivinhar qual pode ser o tipo MIME de um recurso e apenas aceitar o tipo MIME retornado pelo servidor.
* `X-XSS-Protection`: Esta é uma funcionalidade do Internet Explorer, Chrome e Safari que impede o carregamento de páginas quando detectam ataques de cross-site scripting (XSS) refletidos.
* `X-Frame-Options`: Este cabeçalho pode ser usado para indicar se um navegador deve ou não ser permitido a renderizar uma página em uma tag `<iframe>`. Ao especificar o valor desse cabeçalho como *SAMEORIGIN*, você pode fazer com que a página seja exibida em um frame na mesma origem da própria página.
* `Content-Security-Policy`: Este cabeçalho de resposta permite restringir quais recursos (como JavaScript, CSS, imagens, manifestos, etc.) podem ser carregados e os URLs de onde eles podem ser carregados. Esse cabeçalho de segurança só será adicionado se você configurar a classe `AbpSecurityHeadersOptions` e ativá-lo.

## Configuração

### AbpSecurityHeadersOptions

`AbpSecurityHeadersOptions` é a classe principal para habilitar o cabeçalho `Content-Security-Policy`, definir seu valor e adicionar outros cabeçalhos de segurança que você deseja adicionar à sua aplicação.

**Exemplo:**

```csharp
Configure<AbpSecurityHeadersOptions>(options => 
{
    options.UseContentSecurityPolicyHeader = true; //false por padrão
    options.ContentSecurityPolicyValue = "object-src 'none'; form-action 'self'; frame-ancestors 'none'"; //valor padrão

    //adicionando cabeçalhos de segurança adicionais
    options.Headers["Referrer-Policy"] = "no-referrer";
});
```

> Se o cabeçalho for o mesmo, os cabeçalhos de segurança adicionais que você definiu têm precedência sobre os cabeçalhos de segurança padrão. Em outras palavras, eles substituem os valores padrão dos cabeçalhos de segurança.

## Middleware de Cabeçalhos de Segurança

O middleware de Cabeçalhos de Segurança é um middleware da pipeline de solicitação do ASP.NET Core que adiciona cabeçalhos de segurança predefinidos à sua aplicação, incluindo `X-Content-Type-Options`, `X-XSS-Protection` e `X-Frame-Options`. Além disso, esse middleware também inclui esses cabeçalhos de segurança exclusivos em sua aplicação se você configurar a classe `AbpSecurityHeadersOptions` conforme mencionado acima.

**Exemplo:**

```csharp
app.UseAbpSecurityHeaders();
```

> Você pode adicionar esse middleware após `app.UseRouting()` no método `OnApplicationInitialization` da classe do seu módulo para registrá-lo na pipeline de solicitação. Esse middleware já está configurado nos [ABP Commercial Startup Templates](https://docs.abp.io/en/commercial/latest/startup-templates/index), portanto, você não precisa adicioná-lo manualmente se estiver usando um desses modelos de inicialização.

Depois de registrar o middleware `UseAbpSecurityHeaders` na pipeline de solicitação, os cabeçalhos de segurança definidos serão exibidos nos cabeçalhos de resposta, como na figura abaixo:

![](../../images/security-response-headers.png)

## Nonce de Script da Política de Segurança de Conteúdo

O Abp Framework fornece uma propriedade para adicionar um valor de nonce dinâmico ao cabeçalho Content-Security-Policy. Com esse recurso, ele adiciona automaticamente um valor de nonce dinâmico ao lado do cabeçalho. E com a ajuda do helper de tag de script, ele adiciona esse valor de [`nonce de script`](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/nonce) às tags de script em suas páginas (o `ScriptNonceTagHelper` no namespace `Volo.Abp.AspNetCore.Mvc.UI.Bundling` deve ser anexado como um taghelper).
> Se você precisar adicionar manualmente o nonce de script, pode usar 'Html.GetScriptNonce()' para adicionar o valor de nonce ou 'Html.GetScriptNonceAttribute()' para adicionar o valor do atributo nonce.

Esse recurso está desabilitado por padrão. Você pode ativá-lo definindo a propriedade `UseContentSecurityPolicyScriptNonce` da classe `AbpSecurityHeadersOptions` como `true`.

### Ignorar Nonce de Script

Você pode ignorar o nonce de script para algumas páginas ou alguns seletores. Você pode usar as propriedades `IgnoredScriptNoncePaths` e `IgnoredScriptNonceSelectors` da classe `AbpSecurityHeadersOptions`.

**Exemplo:**

```csharp
Configure<AbpSecurityHeadersOptions>(options => 
{
    //adicionando nonce de script-src
    options.UseContentSecurityPolicyScriptNonce = true; //false por padrão

    //ignorar a origem do nonce de script para esses caminhos
    options.IgnoredScriptNoncePaths.Add("/minha-pagina");

    //ignorar o nonce de script por Elsa Workflows e outros seletores
    options.IgnoredScriptNonceSelectors.Add(context =>
    {
        var endpoint = context.GetEndpoint();
        return Task.FromResult(endpoint?.Metadata.GetMetadata<PageRouteMetadata>()?.RouteTemplate == "/{SUA_PAGINA_PRINCIPAL}");
    });
});
```

### Ignorar Cabeçalhos de Segurança do Abp

Você pode ignorar os Cabeçalhos de Segurança do Abp para algumas ações ou páginas. Você pode usar o atributo `IgnoreAbpSecurityHeaderAttribute` para isso.

**Exemplo:**

```csharp
@using Volo.Abp.AspNetCore.Security
@attribute [IgnoreAbpSecurityHeaderAttribute]
```

**Exemplo:**

```csharp
[IgnoreAbpSecurityHeaderAttribute]
public class IndexModel : AbpPageModel
{
    public void OnGet()
    {
    }
}
```