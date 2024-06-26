# Abas

## Introdução

`abp-tab` é o contêiner básico de conteúdo de navegação por abas derivado do elemento de aba do Bootstrap.

Uso básico:

````xml
<abp-tabs>
    <abp-tab title="Home">
             Conteúdo_Início
    </abp-tab>
    <abp-tab-link title="Link" href="#" />
    <abp-tab title="perfil">
            Conteúdo_Perfil
    </abp-tab>
    <abp-tab-dropdown title="Contato" name="DropdownContato">
        <abp-tab title="Contato 1" parent-dropdown-name="DropdownContato">
            Conteúdo_1_Conteúdo
        </abp-tab>
        <abp-tab title="Contato 2" parent-dropdown-name="DropdownContato">
            Conteúdo_2_Conteúdo
        </abp-tab>
    </abp-tab-dropdown>
</abp-tabs>
````

## Demonstração

Veja a [página de demonstração de abas](https://bootstrap-taghelpers.abp.io/Components/Tabs) para vê-la em ação.

## Atributos do abp-tab

- **title**: Define o texto do menu da aba.
- **name:** Define o atributo "id" dos elementos gerados. O valor padrão é um Guid. Não é necessário, a menos que as abas sejam alteradas ou modificadas com Jquery.
- **active**: Define a aba ativa.

Exemplo:

````xml
<abp-tabs name="IdDaAba">
    <abp-tab name="nav-inicio" title="Home">
        Conteúdo_Início
    </abp-tab>   
    <abp-tab name="nav-perfil" active="true" title="perfil">
        Conteúdo_Perfil
    </abp-tab>
    <abp-tab name="nav-contato" title="Contato">
        Conteúdo_Contato
    </abp-tab>
</abp-tabs>
````

### Pílulas

Exemplo:

````xml
<abp-tabs tab-style="Pill">
    <abp-tab title="Home">
         Conteúdo_Início
    </abp-tab>
    <abp-tab title="perfil">
         Conteúdo_Perfil
    </abp-tab>
    <abp-tab title="Contato">
         Conteúdo_Contato
    </abp-tab>
</abp-tabs>
````

### Vertical

**vertical-header-size**: Define a largura da coluna dos cabeçalhos das abas.

Exemplo:

````xml
<abp-tabs tab-style="PillVertical" vertical-header-size="_2" >
    <abp-tab active="true" title="Home">
        Conteúdo_Início
    </abp-tab>   
    <abp-tab title="perfil">
        Conteúdo_Perfil
    </abp-tab>
    <abp-tab title="Contato">
        Conteúdo_Contato
    </abp-tab>
</abp-tabs>
````