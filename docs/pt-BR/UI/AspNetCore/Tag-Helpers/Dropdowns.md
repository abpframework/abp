# Dropdowns

## Introdução

`abp-dropdown` é o contêiner principal para o conteúdo do dropdown.

Uso básico:

````xml
<abp-dropdown>
    <abp-dropdown-button text="Botão do dropdown" />
    <abp-dropdown-menu>
        <abp-dropdown-item href="#">Ação</abp-dropdown-item>
        <abp-dropdown-item href="#">Outra ação</abp-dropdown-item>
        <abp-dropdown-item href="#">Algo mais aqui</abp-dropdown-item>
    </abp-dropdown-menu>
</abp-dropdown>
````

## Demonstração

Veja a [página de demonstração do dropdown](https://bootstrap-taghelpers.abp.io/Components/Dropdowns) para vê-lo em ação.

## Atributos

### direction

Um valor que indica em qual direção os botões do dropdown serão exibidos. Deve ser um dos seguintes valores:

* `Down` (valor padrão)
* `Up`
* `Right`
* `Left`

### dropdown-style

Um valor que indica se um `abp-dropdown-button` terá um ícone dividido para o dropdown. Deve ser um dos seguintes valores:

* `Single` (valor padrão)
* `Split`

## Itens do menu

`abp-dropdown-menu` é o contêiner principal para os itens do menu dropdown.

Uso básico:

````xml
<abp-dropdown>
    <abp-dropdown-button button-type="Secondary" text="Dropdown"/>
    <abp-dropdown-menu>
        <abp-dropdown-header>Cabeçalho do Dropdown</abp-dropdown-header>
        <abp-dropdown-item href="#">Ação</abp-dropdown-item>
        <abp-dropdown-item active="true" href="#">Ação ativa</abp-dropdown-item>
        <abp-dropdown-item disabled="true" href="#">Ação desativada</abp-dropdown-item>
        <abp-dropdown-divider/>
        <abp-dropdown-item-text>Texto do item do dropdown</abp-dropdown-item-text>
        <abp-dropdown-item href="#">Algo mais aqui</abp-dropdown-item>
    </abp-dropdown-menu>
</abp-dropdown>
````

## Atributos

### align

Um valor que indica em qual direção os itens do `abp-dropdown-menu` serão alinhados. Deve ser um dos seguintes valores:

* `Start` (valor padrão)
* `End`

### Conteúdo adicional

`abp-dropdown-menu` também pode conter elementos HTML adicionais como títulos, parágrafos, divisores ou elementos de formulário.

Exemplo:

````xml
<abp-dropdown >
    <abp-dropdown-button button-type="Secondary" text="Dropdown com Formulário"/>
    <abp-dropdown-menu>
        <form class="px-4 py-3">
            <abp-input asp-for="EmailAddress"></abp-input>
            <abp-input asp-for="Password"></abp-input>
            <abp-input asp-for="RememberMe"></abp-input>
            <abp-button button-type="Primary" text="Entrar" type="submit" />
        </form>
        <abp-dropdown-divider></abp-dropdown-divider>
        <abp-dropdown-item href="#">Novo por aqui? Cadastre-se</abp-dropdown-item>
        <abp-dropdown-item href="#">Esqueceu a senha?</abp-dropdown-item>
    </abp-dropdown-menu>
</abp-dropdown>
````