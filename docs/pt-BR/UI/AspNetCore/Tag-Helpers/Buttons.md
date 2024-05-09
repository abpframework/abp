# Botões

## Introdução

`abp-button` é o elemento principal para criar botões.

Uso básico:

````xml
<abp-button button-type="Primary">Clique em Mim</abp-button>
````

## Demonstração

Veja a página de demonstração de [botões](https://bootstrap-taghelpers.abp.io/Components/Buttons) para vê-lo em ação.

## Atributos

### button-type

Um valor que indica o estilo/tipo principal do botão. Deve ser um dos seguintes valores:

* `Default` (valor padrão)
* `Primary`
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`
* `Outline_Primary`
* `Outline_Secondary`
* `Outline_Success`
* `Outline_Danger`
* `Outline_Warning`
* `Outline_Info`
* `Outline_Light`
* `Outline_Dark`
* `Link`

### size

Um valor que indica o tamanho do botão. Deve ser um dos seguintes valores:

* `Default` (valor padrão)
* `Small`
* `Medium`
* `Large`
* `Block`
* `Block_Small`
* `Block_Medium`
* `Block_Large`

### busy-text

Um texto que é mostrado quando o botão está ocupado.

Para tornar o botão ocupado:

````xml
$('#btnTest').buttonBusy(true);
````

Para torná-lo utilizável novamente:

````xml
$('#btnTest').buttonBusy(false);
````

### text

O texto do botão. Isso é um atalho se você simplesmente deseja definir um texto para o botão. Exemplo:

````xml
<abp-button button-type="Primary" text="Clique em Mim" />
````

Nesse caso, você pode usar uma tag de auto-fechamento para torná-lo mais curto.

### icon

Usado para definir um ícone para o botão. Funciona com as classes de ícone do [Font Awesome](https://fontawesome.com/) por padrão. Exemplo:

````xml
<abp-button icon="address-card" text="Endereço" />
````

##### icon-type

Se você não deseja usar o font-awesome, você tem duas opções:

1. Defina `icon-type` como `Other` e escreva a classe CSS do ícone de fonte que você está usando.
2. Se você não usa um ícone de fonte, use as tags de abertura e fechamento manualmente e escreva qualquer código dentro das tags.

### disabled

Defina `true` para desabilitar o botão inicialmente.