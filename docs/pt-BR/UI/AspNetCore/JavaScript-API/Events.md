# ASP.NET Core MVC / Razor Pages UI: API de Eventos JavaScript

O objeto `abp.event` é um serviço simples que é usado para publicar e se inscrever em eventos globais **no navegador**.

> Esta API não está relacionada a eventos locais ou distribuídos do lado do servidor. Ela funciona dentro dos limites do navegador para permitir que os componentes da interface do usuário (partes do código) se comuniquem de forma desacoplada.

## Uso Básico

### Publicando Eventos

Use `abp.event.trigger` para publicar eventos.

**Exemplo: Publicar um evento *Basket Updated* (Carrinho Atualizado)**

````js
abp.event.trigger('basketUpdated');
````

Isso acionará todas as chamadas de retorno inscritas.

### Se inscrevendo nos Eventos

Use `abp.event.on` para se inscrever em eventos.

**Exemplo: Consumir o evento *Basket Updated* (Carrinho Atualizado)**

````js
abp.event.on('basketUpdated', function() {
  console.log('Manipulou o evento basketUpdated...');
});
````

Você começará a receber eventos depois de se inscrever no evento.

### Cancelando a inscrição nos Eventos

Se você precisar cancelar a inscrição em um evento pré-inscrito, poderá usar a função `abp.event.off(eventName, callback)`. Nesse caso, você tem a chamada de retorno como uma declaração de função separada.

**Exemplo: Inscrever e cancelar a inscrição**

````js
function onBasketUpdated() {
  console.log('Manipulou o evento basketUpdated...');
}

//Inscrever
abp.event.on('basketUpdated', onBasketUpdated);

//Cancelar a inscrição
abp.event.off('basketUpdated', onBasketUpdated);
````

Você não receberá mais eventos depois de cancelar a inscrição no evento.

## Argumentos do Evento

Você pode passar argumentos (de qualquer quantidade) para o método `trigger` e obtê-los na chamada de retorno da inscrição.

**Exemplo: Adicionar o carrinho como argumento do evento**

````js
//Inscrever-se no evento
abp.event.on('basketUpdated', function(basket) {
  console.log('O novo objeto do carrinho: ');
  console.log(basket);
});

//Acionar o evento
abp.event.trigger('basketUpdated', {
  items: [
    {
      "productId": "123",
      "count": 2
    },
    {
      "productId": "832",
      "count": 1
    }
  ]
});
````

### Múltiplos Argumentos

Se você deseja passar vários argumentos, pode passar como `abp.event.on('basketUpdated', arg0, arg1, agr2)`. Em seguida, você pode adicionar a mesma lista de argumentos à função de chamada de retorno no lado do assinante.

> **Dica:** Alternativamente, você pode enviar um único objeto que possui um campo separado para cada argumento. Isso facilita a extensão/mudança dos argumentos do evento no futuro sem quebrar os assinantes.