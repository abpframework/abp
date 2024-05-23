# ASP.NET Core MVC / Razor Pages UI

## Introdução

O ABP Framework oferece uma maneira conveniente e confortável de criar aplicativos da web usando o ASP.NET Core MVC / Razor Pages como framework de interface do usuário.

> O ABP não oferece uma nova/forma personalizada de desenvolvimento de UI. Você pode continuar usando suas habilidades atuais para criar a UI. No entanto, ele oferece muitos recursos para facilitar o desenvolvimento e ter uma base de código mais sustentável.

### MVC vs Razor Pages

O ASP.NET Core oferece dois modelos para o desenvolvimento de UI:

* **[MVC (Model-View-Controller)](https://docs.microsoft.com/en-us/aspnet/core/mvc/)** é a maneira clássica que existe desde a versão 1.0. Este modelo pode ser usado para criar páginas/componentes de UI e APIs HTTP.
* **[Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/)** foi introduzido com o ASP.NET Core 2.0 como uma nova maneira de criar páginas da web.

**O ABP Framework suporta ambos** os modelos MVC e Razor Pages. No entanto, é sugerido criar as **páginas de UI com a abordagem Razor Pages** e usar o **modelo MVC para construir APIs HTTP**. Portanto, todos os módulos pré-construídos, exemplos e documentação são baseados nas Razor Pages para o desenvolvimento de UI, enquanto você sempre pode aplicar o padrão MVC para criar suas próprias páginas.

### Modularidade

A [modularidade](../../Module-Development-Basics.md) é um dos principais objetivos do ABP Framework. Não é diferente para a UI; É possível desenvolver aplicativos modulares e módulos de aplicativos reutilizáveis com páginas e componentes de UI isolados e reutilizáveis.

O [modelo de inicialização do aplicativo](../../Startup-Templates/Application.md) vem com alguns módulos de aplicativos pré-instalados. Esses módulos têm suas próprias páginas de UI incorporadas em seus próprios pacotes NuGet. Você não vê o código deles em sua solução, mas eles funcionam como esperado em tempo de execução.

## Sistema de Temas

O ABP Framework fornece um completo [sistema de temas](Theming.md) com os seguintes objetivos:

* Módulos de aplicativos reutilizáveis são desenvolvidos de forma **independente de temas**, para que possam funcionar com qualquer tema de UI.
* O tema de UI é **decidido pelo aplicativo final**.
* O tema é distribuído por meio de pacotes NuGet/NPM, para que seja **facilmente atualizável**.
* O aplicativo final pode **personalizar** o tema selecionado.

### Temas Atuais

Atualmente, três temas são **oficialmente fornecidos**:

* O [Tema Básico](Basic-Theme.md) é o tema minimalista com o estilo Bootstrap simples. É **open source e gratuito**.
* O [Tema Lepton](https://commercial.abp.io/themes) é um tema **comercial** desenvolvido pela equipe principal do ABP e faz parte da licença [ABP Commercial](https://commercial.abp.io/).
* O [Tema LeptonX](https://x.leptontheme.com/) é um tema que possui escolhas [comerciais](https://docs.abp.io/en/commercial/latest/themes/lepton-x/mvc) e [lite](../../Themes/LeptonXLite/AspNetCore.md).

Também existem alguns temas desenvolvidos pela comunidade para o ABP Framework (você pode pesquisar na web).

### Bibliotecas Base

Existem um conjunto de bibliotecas JavaScript/CSS padrão que são pré-instaladas e suportadas por todos os temas:

- [Twitter Bootstrap](https://getbootstrap.com/) como o framework HTML/CSS fundamental.
- [JQuery](https://jquery.com/) para manipulação do DOM.
- [DataTables.Net](https://datatables.net/) para grades de dados.
- [JQuery Validation](https://jqueryvalidation.org/) para validação do lado do cliente e [unobtrusive](https://github.com/aspnet/jquery-validation-unobtrusive) validation
- [FontAwesome](https://fontawesome.com/) como a biblioteca fundamental de fontes CSS.
- [SweetAlert](https://sweetalert.js.org/) para mostrar mensagens de alerta e caixas de diálogo de confirmação.
- [Toastr](https://github.com/CodeSeven/toastr) para mostrar notificações de toast.
- [Lodash](https://lodash.com/) como uma biblioteca de utilitários.
- [Luxon](https://moment.github.io/luxon/) para operações de data/hora.
- [JQuery Form](https://github.com/jquery-form/form) para formulários AJAX.
- [bootstrap-datepicker](https://github.com/uxsolutions/bootstrap-datepicker) para mostrar seletores de data.
- [Select2](https://select2.org/) para melhores caixas de seleção/combo.
- [Timeago](http://timeago.yarp.com/) para mostrar carimbos de data/hora fuzzy atualizados automaticamente.
- [malihu-custom-scrollbar-plugin](https://github.com/malihu/malihu-custom-scrollbar-plugin) para barras de rolagem personalizadas.

Você pode usar essas bibliotecas diretamente em seus aplicativos, sem precisar importar manualmente sua página.

### Layouts

Os temas fornecem os layouts padrão. Portanto, você tem layouts responsivos com os recursos padrão já implementados. A captura de tela abaixo foi tirada do Layout do Aplicativo do [Tema Básico](Basic-Theme.md):

![basic-theme-application-layout](../../images/basic-theme-application-layout.png)

Consulte o documento [Theming](Theming.md) para obter mais opções de layout e outros detalhes.

### Partes do Layout

Um layout típico consiste em várias partes. O sistema de [temas](Theming.md) fornece [menus](Navigation-Menu.md), [toolbars](Toolbars.md), [hooks de layout](Layout-Hooks.md) e mais para controlar dinamicamente o layout pelo seu aplicativo e pelos módulos que você está usando.

## Recursos

Esta seção destaca alguns dos recursos fornecidos pelo ABP Framework para a UI do ASP.NET Core MVC / Razor Pages.

### Proxies de Cliente de API JavaScript Dinâmico

O sistema de Proxies de Cliente de API JavaScript Dinâmico permite que você consuma suas APIs HTTP do lado do servidor a partir do seu código de cliente JavaScript, assim como chamar funções locais.

**Exemplo: Obter uma lista de autores do servidor**

````js
acme.bookStore.authors.author.getList({
  maxResultCount: 10
}).then(function(result){
  console.log(result.items);
});
````

`acme.bookStore.authors.author.getList` é uma função gerada automaticamente que faz internamente uma chamada AJAX para o servidor.

Consulte o documento [Proxies de Cliente de API JavaScript Dinâmico](Dynamic-JavaScript-Proxies.md) para mais informações.

### Tag Helpers do Bootstrap

O ABP torna mais fácil e seguro escrever HTML do Bootstrap.

**Exemplo: Renderizar um modal do Bootstrap**

````html
<abp-modal>
    <abp-modal-header title="Título do Modal" />
    <abp-modal-body>
        Uau, você está lendo este texto em um modal!
    </abp-modal-body>
    <abp-modal-footer buttons="@(AbpModalButtons.Save|AbpModalButtons.Close)"></abp-modal-footer>
</abp-modal>
````

Consulte o documento [Tag Helpers](Tag-Helpers/Index.md) para mais informações.

### Formulários e Validação

O ABP fornece os tag helpers `abp-dynamic-form` e `abp-input` para simplificar drasticamente a criação de um formulário totalmente funcional que automatiza a localização, validação e envio AJAX.

**Exemplo: Use `abp-dynamic-form` para criar um formulário completo com base em um modelo**

````html
<abp-dynamic-form abp-model="Movie" submit-button="true" />
````

Consulte o documento [Formulários e Validação](Forms-Validation.md) para obter mais detalhes.

### Agrupamento e Minificação / Bibliotecas do Lado do Cliente

O ABP fornece um sistema de Agrupamento e Minificação flexível e modular para criar pacotes e minificar arquivos de estilo/script em tempo de execução.

````html
<abp-style-bundle>
    <abp-style src="/libs/bootstrap/css/bootstrap.css" />
    <abp-style src="/libs/font-awesome/css/font-awesome.css" />
    <abp-style src="/libs/toastr/toastr.css" />
    <abp-style src="/styles/my-global-style.css" />
</abp-style-bundle>
````

Além disso, o sistema de Gerenciamento de Pacotes do Lado do Cliente oferece uma maneira modular e consistente de gerenciar dependências de bibliotecas de terceiros.

Consulte os documentos [Agrupamento e Minificação](Bundling-Minification.md) e [Gerenciamento de Pacotes do Lado do Cliente](Client-Side-Package-Management.md).

### APIs JavaScript

As [APIs JavaScript](JavaScript-API/Index.md) fornecem abstrações sólidas para a localização, configurações, permissões, recursos... etc. do lado do servidor. Elas também fornecem uma maneira simples de mostrar mensagens e **notificações** ao usuário.

### Modais, Alertas, Widgets e Mais

O ABP Framework fornece muitas soluções integradas para requisitos comuns de aplicativos;

* O [Sistema de Widgets](Widgets.md) pode ser usado para criar widgets reutilizáveis e criar painéis de controle.
* Os [Alertas de Página](Page-Alerts.md) facilitam a exibição de alertas ao usuário.
* O [Gerenciador de Modais](Modals.md) fornece uma maneira simples de construir e usar modais.
* A integração com [Data Tables](Data-Tables.md) facilita a criação de grades de dados.

## Personalização

Existem muitas maneiras de personalizar o tema e as UIs dos módulos pré-construídos. Você pode substituir componentes, páginas, recursos estáticos, pacotes e muito mais. Consulte o [Guia de Personalização da Interface do Usuário](Customization-User-Interface.md) para mais informações.