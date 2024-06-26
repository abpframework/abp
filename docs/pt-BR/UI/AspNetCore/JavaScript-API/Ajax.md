# ASP.NET Core MVC / Páginas Razor UI JavaScript AJAX API

A API `abp.ajax` fornece uma maneira conveniente de realizar chamadas AJAX para o servidor. Ela usa internamente o `$.ajax` do JQuery, mas automatiza algumas tarefas comuns para você:

* Lida automaticamente com os erros e localiza-os, informando o usuário (usando o [abp.message](Message.md)). Então, normalmente você não precisa se preocupar com os erros.
* Adiciona automaticamente um token de **anti-falsificação** ao cabeçalho HTTP para satisfazer a validação de proteção CSRF no lado do servidor.
* Define automaticamente as opções padrão e permite configurar as opções padrão em um único local.
* Pode **bloquear** uma parte da interface do usuário (ou a página inteira) durante a operação AJAX.
* Permite personalizar completamente qualquer chamada AJAX, usando as opções padrão do `$.ajax`.

> Embora o `abp.ajax` torne a chamada AJAX mais fácil, normalmente você usará o sistema [Dynamic JavaScript Client Proxy](../Dynamic-JavaScript-Proxies.md) para fazer chamadas às suas APIs HTTP do lado do servidor. O `abp.ajax` pode ser usado quando você precisa realizar operações AJAX de baixo nível.

## Uso básico

O `abp.ajax` aceita um objeto de opções que é aceito pelo [$.ajax](https://api.jquery.com/jquery.ajax/#jQuery-ajax-settings) padrão. Todas as opções padrão são válidas. Ele retorna uma [promessa](https://api.jquery.com/category/deferred-object/) como valor de retorno.

**Exemplo: Obter a lista de usuários**

````js
abp.ajax({
  type: 'GET',
  url: '/api/identity/users'
}).then(function(result){
  console.log(result);
});
````

Este comando registra a lista de usuários no console, se você estiver **logado** na aplicação e tiver [permissão](../../../Authorization.md) para a página de gerenciamento de usuários do [Módulo de Identidade](../../../Modules/Identity.md).

## Tratamento de erros

A chamada AJAX de exemplo acima mostra uma **mensagem de erro** se você não estiver logado na aplicação ou não tiver as permissões necessárias para realizar essa solicitação:

![ajax-error](../../../images/ajax-error.png)

Todos os tipos de erros são tratados automaticamente pelo `abp.ajax`, a menos que você queira desabilitá-lo.

### Resposta de erro padrão

O `abp.ajax` é compatível com o sistema de tratamento de exceções do [ABP Framework](../../../Exception-Handling.md) e lida corretamente com o formato de erro padrão retornado pelo servidor. Uma mensagem de erro típica é um JSON como o abaixo:

````json
{
  "error": {
    "code": "App:010042",
    "message": "Este tópico está bloqueado e não é possível adicionar uma nova mensagem",
    "details": "Informações mais detalhadas sobre o erro..."
  }
}
````

A mensagem de erro é mostrada diretamente ao usuário, usando as propriedades `message` e `details`.

### Resposta de erro não padrão e códigos de status HTTP

Ele também lida com erros mesmo se o formato de erro padrão não for enviado pelo servidor. Isso pode acontecer se você ignorar o sistema de tratamento de exceções do ABP e construir manualmente a resposta HTTP no servidor. Nesse caso, os **códigos de status HTTP** são considerados.

Os seguintes códigos de status HTTP são predefinidos:

* **401**: Mostra uma mensagem de erro como "*Você deve estar autenticado (fazer login) para realizar esta operação*". Quando os usuários clicam no botão OK, eles são redirecionados para a página inicial da aplicação para fazer login novamente.
* **403**: Mostra uma mensagem de erro como "*Você não tem permissão para realizar esta operação*".
* **404**: Mostra uma mensagem de erro como "*O recurso solicitado não foi encontrado no servidor*".
* **Outros**: Mostra uma mensagem de erro genérica como "*Ocorreu um erro. Detalhes do erro não enviados pelo servidor*".

Todas essas mensagens são localizadas com base no idioma atual do usuário.

### Tratando manualmente os erros

Como o `abp.ajax` retorna uma promessa, você sempre pode encadear uma chamada `.catch(...)` para registrar um retorno de chamada que é executado se a solicitação AJAX falhar.

**Exemplo: Mostrar um alerta se a solicitação AJAX falhar**

````js
abp.ajax({
  type: 'GET',
  url: '/api/identity/users'
}).then(function(result){
  console.log(result);
}).catch(function(){
  alert("solicitação falhou :(");
});
````

Enquanto seu retorno de chamada é executado, o ABP ainda trata o erro por si só. Se você quiser desabilitar o tratamento automático de erros, passe `abpHandleError: false` nas opções do `abp.ajax`.

**Exemplo: Desabilitar o tratamento automático de erros**

````js
abp.ajax({
  type: 'GET',
  url: '/api/identity/users',
  abpHandleError: false // DESABILITAR O TRATAMENTO AUTOMÁTICO DE ERROS
}).then(function(result){
  console.log(result);
}).catch(function(){
  alert("solicitação falhou :(");
});
````

Se você definir `abpHandleError: false` e não capturar o erro você mesmo, então o erro será ocultado e a solicitação falhará silenciosamente. O `abp.ajax` ainda registra o erro no console do navegador (consulte a seção *Configuração* para substituí-lo).

## Configuração

O `abp.ajax` possui uma **configuração global** que você pode personalizar com base em seus requisitos.

### Opções padrão do AJAX

O objeto `abp.ajax.defaultOpts` é usado para configurar as opções padrão usadas ao realizar uma chamada AJAX, a menos que você as substitua. O valor padrão deste objeto é mostrado abaixo:

````js
{
    dataType: 'json',
    type: 'POST',
    contentType: 'application/json',
    headers: {
        'X-Requested-With': 'XMLHttpRequest'
    }
}
````

Portanto, se você quiser alterar o tipo de solicitação padrão, pode fazer da seguinte forma:

````js
abp.ajax.defaultOpts.type = 'GET';
````

Escreva este código antes de todo o seu código JavaScript. Normalmente, você deseja colocar essa configuração em um arquivo JavaScript separado e adicioná-lo ao layout usando o [bundle](../Bundling-Minification.md) global.

### Registrar/Mostrar erros

As seguintes funções podem ser substituídas para personalizar o registro e a exibição das mensagens de erro:

* A função `abp.ajax.logError` registra os erros usando o [abp.log.error(...)](Logging.md) por padrão.
* A função `abp.ajax.showError` mostra a mensagem de erro usando o [abp.message.error(...)](Message.md) por padrão.
* A função `abp.ajax.handleErrorStatusCode` trata diferentes códigos de status HTTP e mostra mensagens diferentes com base no código.
* A função `abp.ajax.handleAbpErrorResponse` trata os erros enviados com o formato de erro padrão do ABP.
* A função `abp.ajax.handleNonAbpErrorResponse` trata as respostas de erro não padrão.
* A função `abp.ajax.handleUnAuthorizedRequest` trata as respostas com o código de status `401` e redireciona os usuários para a página inicial da aplicação.

**Exemplo: Substituir a função `logError`**

````js
abp.ajax.logError = function(error) {
    //...
}
````

### Outras opções

* A função `abp.ajax.ajaxSendHandler` é usada para interceptar as solicitações AJAX e adicionar um token de anti-falsificação ao cabeçalho HTTP. Observe que isso funciona para todas as solicitações AJAX, mesmo se você não usar o `abp.ajax`.