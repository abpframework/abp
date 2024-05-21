# Alertas

## Introdução

`abp-alert` é um elemento principal para criar um alerta.

Uso básico:

````xml
<abp-alert alert-type="Primary">
    Um alerta primário simples - confira!
</abp-alert>
````



## Demonstração

Veja a página de demonstração de [alertas](https://bootstrap-taghelpers.abp.io/Components/Alerts) para vê-lo em ação.

## Atributos

### alert-type

Um valor que indica o tipo de alerta. Deve ser um dos seguintes valores:

* `Default` (valor padrão)
* `Primary`
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`

Exemplo:

````xml
<abp-alert alert-type="Warning">
    Um alerta de aviso simples - confira!
</abp-alert>
````

### alert-link

Um valor que fornece links coloridos correspondentes dentro de qualquer alerta. 

Exemplo:

````xml
<abp-alert alert-type="Danger">
    Um alerta de perigo simples com <a abp-alert-link href="#">um exemplo de link</a>. Clique nele se quiser.
</abp-alert>
````

### dismissible

Um valor para tornar o alerta dispensável.

Exemplo:

````xml
<abp-alert alert-type="Warning" dismissible="true">
    Santo guacamole! Você deve verificar alguns desses campos abaixo.
</abp-alert>
````

### Conteúdo adicional

`abp-alert` também pode conter elementos HTML adicionais, como títulos, parágrafos e divisores.

Exemplo:

````xml
<abp-alert alert-type="Success">
    <h4>Bom trabalho!</h4>
    <p>Aww yeah, você leu com sucesso esta importante mensagem de alerta. Este texto de exemplo vai ficar um pouco mais longo para que você possa ver como o espaçamento dentro de um alerta funciona com esse tipo de conteúdo.</p>
    <hr>
    <p class="mb-0">Sempre que precisar, certifique-se de usar utilitários de margem para manter as coisas organizadas.</p>
</abp-alert>
````