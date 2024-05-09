# ASP.NET Core MVC / Razor Pages UI: API de Mensagens JavaScript

A API de Mensagens é usada para mostrar mensagens agradáveis ao usuário como um diálogo bloqueante. A API de Mensagens é uma abstração fornecida pelo ABP Framework e implementada usando a biblioteca [SweetAlert](https://sweetalert.js.org/) por padrão.

## Exemplo Rápido

Use a função `abp.message.success(...)` para mostrar uma mensagem de sucesso:

````js
abp.message.success('Suas alterações foram salvas com sucesso!', 'Parabéns');
````

Isso mostrará um diálogo na interface do usuário:

![js-message-success](../../../images/js-message-success.png)

## Mensagens Informativas

Existem quatro tipos de funções de mensagem informativa:

* `abp.message.info(...)`
* `abp.message.success(...)`
* `abp.message.warn(...)`
* `abp.message.error(...)`

Todos esses métodos recebem dois parâmetros:

* `message`: A mensagem (`string`) a ser mostrada.
* `title`: Um título opcional (`string`).

**Exemplo: Mostrar uma mensagem de erro**

````js
abp.message.error('O número do seu cartão de crédito não é válido!');
````

![js-message-error](../../../images/js-message-error.png)

## Mensagem de Confirmação

A função `abp.message.confirm(...)` pode ser usada para obter uma confirmação do usuário.

**Exemplo**

Use o seguinte código para obter um resultado de confirmação do usuário:

````js
abp.message.confirm('Tem certeza de que deseja excluir a função "admin"?')
.then(function(confirmed){
  if(confirmed){
    console.log('TODO: excluindo a função...');
  }
});
````

A interface resultante será como mostrado abaixo:

![js-message-confirm](../../../images/js-message-confirm.png)

Se o usuário clicou no botão `Sim`, o argumento `confirmed` na função de retorno `then` será `true`.

> "*Tem certeza?*" é o título padrão (localizado com base no idioma atual) e você pode substituí-lo.

### O Valor de Retorno

O valor de retorno da função `abp.message.confirm(...)` é uma promessa, então você pode encadear um retorno de chamada `then` como mostrado acima.

### Parâmetros

A função `abp.message.confirm(...)` possui os seguintes parâmetros:

* `message`: Uma mensagem (string) para mostrar ao usuário.
* `titleOrCallback` (opcional): Um título ou uma função de retorno de chamada. Se você fornecer uma string, ela será mostrada como título. Se você fornecer uma função de retorno de chamada (que recebe um parâmetro `bool`), ela será chamada com o resultado.
* `callback` (opcional): Se você passou um título para o segundo parâmetro, pode passar sua função de retorno de chamada como o terceiro parâmetro.

Passar uma função de retorno de chamada é uma alternativa ao retorno de chamada `then` mostrado acima.

**Exemplo: Fornecendo todos os parâmetros e obtendo o resultado com a função de retorno de chamada**

````js
abp.message.confirm(
  'Tem certeza de que deseja excluir a função "admin"?',
  'Cuidado!',
  function(confirmed){
    if(confirmed){
      console.log('TODO: excluindo a função...');
    }
  });
````

## Configuração do SweetAlert

A API de Mensagens é implementada usando a biblioteca [SweetAlert](https://sweetalert.js.org/) por padrão. Se você deseja alterar sua configuração, pode definir as opções no objeto `abp.libs.sweetAlert.config`. O objeto de configuração padrão é mostrado abaixo:

````js
{
    'default': {
    },
    info: {
        icon: 'info'
    },
    success: {
        icon: 'success'
    },
    warn: {
        icon: 'warning'
    },
    error: {
        icon: 'error'
    },
    confirm: {
        icon: 'warning',
        title: 'Tem certeza?',
        buttons: ['Cancelar', 'Sim']
    }
}
````

> "Tem certeza?", "Cancelar" e "Sim" são textos automaticamente localizados com base no idioma atual.

Portanto, se você deseja definir o ícone `warn`, pode definir assim:

````js
abp.libs.sweetAlert.config.warn.icon = 'error';
````

Consulte a [documentação do SweetAlert](https://sweetalert.js.org/) para todas as opções de configuração.