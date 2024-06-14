# Trilha de navegação

## Introdução

`abp-breadcrumb` é o contêiner principal para os itens da trilha de navegação.

Uso básico:

````html
<abp-breadcrumb>
    <abp-breadcrumb-item href="#" title="Início" />
    <abp-breadcrumb-item href="#" title="Biblioteca"/>
    <abp-breadcrumb-item title="Página"/>
</abp-breadcrumb>
````

## Demonstração

Veja a página de demonstração de [trilhas de navegação](https://bootstrap-taghelpers.abp.io/Components/Breadcrumbs) para vê-la em ação.

## Atributos do abp-breadcrumb-item

- **title**: Define o texto do item da trilha de navegação.
- **active**: Define o item da trilha de navegação ativo. O último item é ativo por padrão, se nenhum outro item estiver ativo.
- **href**: Um valor que indica se um `abp-breadcrumb-item` possui um link. Deve ser um valor de link em formato de string.