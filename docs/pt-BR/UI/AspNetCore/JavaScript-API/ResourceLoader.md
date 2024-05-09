# ASP.NET Core MVC / Razor Pages UI: API de Carregamento de Recursos JavaScript

`abp.ResourceLoader` é um serviço que pode carregar um arquivo JavaScript ou CSS sob demanda. Ele garante que o arquivo seja carregado apenas uma vez, mesmo que seja solicitado várias vezes.

## Carregando Arquivos de Script

A função `abp.ResourceLoader.loadScript(...)` **carrega** um arquivo JavaScript do servidor e o **executa**.

**Exemplo: Carregar um arquivo JavaScript**

````js
abp.ResourceLoader.loadScript('/Pages/my-script.js');
````

### Parâmetros

A função `loadScript` pode receber três parâmetros;

* `url` (obrigatório, `string`): A URL do arquivo de script a ser carregado.
* `loadCallback` (opcional, `function`): Uma função de retorno de chamada que é chamada assim que o script é carregado e executado. Nessa função de retorno de chamada, você pode usar com segurança o código no arquivo de script. Essa função de retorno de chamada é chamada mesmo se o arquivo já tiver sido carregado anteriormente.
* `failCallback` (opcional, `function`): Uma função de retorno de chamada que é chamada se o carregamento do script falhar.

**Exemplo: Fornecer o argumento `loadCallback`**

````js
abp.ResourceLoader.loadScript('/Pages/my-script.js', function() {
  console.log('carregado com sucesso :)');
});
````

## Carregando Arquivos de Estilo

A função `abp.ResourceLoader.loadStyle(...)` adiciona um elemento `link` à `head` do documento para a URL fornecida, para que o arquivo CSS seja carregado automaticamente pelo navegador.

**Exemplo: Carregar um arquivo CSS**

````js
abp.ResourceLoader.loadStyle('/Pages/my-styles.css');
````