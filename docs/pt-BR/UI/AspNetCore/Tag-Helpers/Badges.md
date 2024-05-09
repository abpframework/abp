# Distintivos

## Introdução

`abp-badge` e `abp-badge-pill` são atributos de Tag Helper ABP para tags html `a` e `span`.

Uso básico:

````html
<span abp-badge="Primary">Primary</span>
<a abp-badge="Info" href="#">Info</a>
<a abp-badge-pill="Danger" href="#">Danger</a>
````

## Demonstração

Veja a página de demonstração de [distintivos](https://bootstrap-taghelpers.abp.io/Components/Badges) para vê-los em ação.

### Valores

* Indica o tipo de distintivo. Deve ser um dos seguintes valores:

  * `Default`
  * `Primary`
  * `Secondary`
  * `Success`
  * `Danger`
  * `Warning`
  * `Info`
  * `Light`
  * `Dark`

Exemplo:

````html
<span abp-badge-pill="Danger">Danger</span>
````