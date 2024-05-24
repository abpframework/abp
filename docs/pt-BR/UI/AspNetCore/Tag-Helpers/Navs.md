# Navs

## Introdução

`abp-nav` é o componente básico de tag helper derivado do elemento de navegação do bootstrap.

Uso básico:

````html
<abp-nav nav-style="Pill" align="Center">
    <abp-nav-item>
<a abp-nav-link active="true" href="#">Ativo</a>
    </abp-nav-item>
    <abp-nav-item>
<a abp-nav-link href="#">Link de navegação mais longo</a>
    </abp-nav-item>
    <abp-nav-item>
<a abp-nav-link href="#">link</a>
    </abp-nav-item>
    <abp-nav-item>
<a abp-nav-link disabled="true" href="#">desativado</a>
    </abp-nav-item>
</abp-nav>
````

## Demonstração

Veja a [página de demonstração de navegação](https://bootstrap-taghelpers.abp.io/Components/Navs) para vê-la em ação.

## Atributos do abp-nav

- **nav-style**: O valor indica o posicionamento e estilo dos itens contidos. Deve ser um dos seguintes valores:
  * `Default` (valor padrão)
  * `Vertical`
  * `Pill`
  * `PillVertical`
- **align:** O valor indica o alinhamento dos itens contidos:
  * `Default` (valor padrão)
  * `Start`
  * `Center`
  * `End`

### Atributos do abp-nav-bar

- **nav-style**: O valor indica o layout de cores da barra de navegação base. Deve ser um dos seguintes valores:
  * `Default` (valor padrão)
  * `Dark`
  * `Light`
  * `Dark_Primary`
  * `Dark_Secondary`
  * `Dark_Success`
  * `Dark_Danger`
  * `Dark_Warning`
  * `Dark_Info`
  * `Dark_Dark`
  * `Dark_Link`
  * `Light_Primary`
  * `Light_Secondary`
  * `Light_Success`
  * `Light_Danger`
  * `Light_Warning`
  * `Light_Info`
  * `Light_Dark`
  * `Light_Link`
- **size:** O valor indica o tamanho da barra de navegação base. Deve ser um dos seguintes valores:
  * `Default` (valor padrão)
  * `Sm`
  * `Md`
  * `Lg`
  * `Xl`

### Atributos do abp-nav-item

**dropdown**: Um valor que define o item de navegação como um menu suspenso, se fornecido. Pode ser um dos seguintes valores:

* `false` (valor padrão)
* `true`

Exemplo:

````html
<abp-nav-bar size="Lg" navbar-style="Dark_Warning">
    <a abp-navbar-brand href="#">Navbar</a>
    <abp-navbar-toggle>
        <abp-navbar-nav>
            <abp-nav-item active="true">
                <a abp-nav-link href="#">Início <span class="sr-only">(atual)</span></a>
            </abp-nav-item>
            <abp-nav-item>
                <a abp-nav-link href="#">Link</a>
            </abp-nav-item>
            <abp-nav-item dropdown="true">
                <abp-dropdown>
                    <abp-dropdown-button nav-link="true" text="Dropdown" />
                    <abp-dropdown-menu>
                        <abp-dropdown-header>Cabeçalho do menu suspenso</abp-dropdown-header>
                        <abp-dropdown-item href="#" active="true">Ação</abp-dropdown-item>
                        <abp-dropdown-item href="#" disabled="true">Outra ação desativada</abp-dropdown-item>
                        <abp-dropdown-item href="#">Algo mais aqui</abp-dropdown-item>
                        <abp-dropdown-divider />
                        <abp-dropdown-item href="#">Link separado</abp-dropdown-item>
                    </abp-dropdown-menu>
                </abp-dropdown>
            </abp-nav-item>
            <abp-nav-item>
                <a abp-nav-link disabled="true" href="#">Desativado</a>
            </abp-nav-item>
        </abp-navbar-nav>            
        <span abp-navbar-text>
          Texto de exemplo
        </span>
    </abp-navbar-toggle>
</abp-nav-bar>
```