# ABP Tag Helpers

O ABP Framework define um conjunto de **componentes de tag helper** para simplificar o desenvolvimento da interface do usuário para aplicativos ASP.NET Core (MVC / Razor Pages).

## Wrappers de Componentes Bootstrap

A maioria dos tag helpers são wrappers do [Bootstrap](https://getbootstrap.com/) (v5+). Codificar o Bootstrap não é tão fácil, não é tão seguro em termos de tipo e contém muitas tags HTML repetitivas. Os ABP Tag Helpers tornam isso mais **fácil** e **seguro em termos de tipo**.

Não temos como objetivo envolver 100% dos componentes do Bootstrap. Ainda é possível escrever **código de estilo nativo do Bootstrap** (na verdade, os tag helpers geram código nativo do Bootstrap no final), mas sugerimos usar os tag helpers sempre que possível.

O ABP Framework também adiciona alguns **recursos úteis** aos componentes padrão do Bootstrap.

Aqui está a lista de componentes que são envolvidos pelo ABP Framework:

* [Alertas](Alerts.md)
* [Badges](Badges.md)
* [Blockquote](Blockquote.md)
* [Bordas](Borders.md)
* [Breadcrumb](Breadcrumbs.md)
* [Botões](Buttons.md)
* [Cards](Cards.md)
* [Carousel](Carousel.md)
* [Collapse](Collapse.md)
* [Dropdowns](Dropdowns.md)
* [Figuras](Figure.md)
* [Grids](Grids.md)
* [List Groups](List-Groups.md)
* [Modals](Modals.md)
* [Navegação](Navs.md)
* [Paginator](Paginator.md)
* [Popovers](Popovers.md)
* [Barras de Progresso](Progress-Bars.md)
* [Tabelas](Tables.md)
* [Abas](Tabs.md)
* [Tooltips](Tooltips.md)

> Até que todos os tag helpers sejam documentados, você pode visitar https://bootstrap-taghelpers.abp.io/ para vê-los com exemplos ao vivo.

## Elementos de Formulário

Os **Abp Tag Helpers** adicionam novos recursos aos **Tag Helpers de entrada e seleção do Asp.Net Core MVC** padrão e os envolvem com controles de formulário do **Bootstrap**. Consulte a documentação de [Elementos de Formulário](Form-elements.md).

## Formulários Dinâmicos

Os **Abp Tag Helpers** oferecem uma maneira fácil de construir formulários completos do **Bootstrap**. Consulte a documentação de [Formulários Dinâmicos](Dynamic-Forms.md).