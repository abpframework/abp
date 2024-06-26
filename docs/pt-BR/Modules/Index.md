# Módulos de Aplicação

ABP é um **framework de aplicação modular** que consiste em dezenas de **pacotes NuGet & NPM**. Ele também fornece uma infraestrutura completa para construir seus próprios módulos de aplicação, que podem ter entidades, serviços, integração de banco de dados, APIs, componentes de interface do usuário, entre outros.

Existem **dois tipos de módulos**. Eles não têm nenhuma diferença estrutural, mas são categorizados por funcionalidade e propósito:

* [**Módulos do framework**](https://github.com/abpframework/abp/tree/dev/framework/src): Estes são **módulos principais do framework** como cache, envio de e-mails, temas, segurança, serialização, validação, integração com o EF Core, integração com o MongoDB... etc. Eles não possuem funcionalidades de aplicação/negócio, mas facilitam o desenvolvimento diário fornecendo infraestrutura comum, integração e abstrações.
* [**Módulos de aplicação**](https://github.com/abpframework/abp/tree/dev/modules): Esses módulos implementam funcionalidades específicas de aplicação/negócio, como blogs, gerenciamento de documentos, gerenciamento de identidade, gerenciamento de locatários... etc. Eles geralmente possuem suas próprias entidades, serviços, APIs e componentes de interface do usuário.

## Módulos de Aplicação de Código Aberto

Existem alguns módulos de aplicação **gratuitos e de código aberto** desenvolvidos e mantidos como parte do ABP Framework.

* [**Conta**](Account.md): Fornece uma interface de usuário para o gerenciamento de contas e permite que o usuário faça login/registo na aplicação.
* [**Registro de Auditoria**](Audit-Logging.md): Persiste registros de auditoria em um banco de dados.
* [**Trabalhos em Segundo Plano**](Background-Jobs.md): Persiste trabalhos em segundo plano ao usar o gerenciador de trabalhos em segundo plano padrão.
* [**Kit CMS**](Cms-Kit/Index.md): Um conjunto de recursos reutilizáveis de *Sistema de Gerenciamento de Conteúdo*.
* [**Documentação**](Docs.md): Usado para criar um site de documentação técnica. A própria documentação do ABP já utiliza este módulo.
* [**Gerenciamento de Recursos**](Feature-Management.md): Usado para persistir e gerenciar os [recursos](../Features.md).
* **[Identidade](Identity.md)**: Gerencia unidades organizacionais, funções, usuários e suas permissões, com base na biblioteca Microsoft Identity.
* [**IdentityServer**](IdentityServer.md): Integra-se ao IdentityServer4.
* [**OpenIddict**](OpenIddict.md): Integra-se ao OpenIddict.
* [**Gerenciamento de Permissões**](Permission-Management.md): Usado para persistir permissões.
* **[Gerenciamento de Configurações](Setting-Management.md)**: Usado para persistir e gerenciar as [configurações](../Settings.md).
* [**Gerenciamento de Locatários**](Tenant-Management.md): Gerencia locatários para uma aplicação [multi-locatário](../Multi-Tenancy.md).
* [**Explorador de Arquivos Virtuais**](Virtual-File-Explorer.md): Fornece uma interface de usuário simples para visualizar arquivos em um [sistema de arquivos virtual](../Virtual-File-System.md).

Veja [o repositório do GitHub](https://github.com/abpframework/abp/tree/dev/modules) para o código-fonte de todos os módulos.

## Módulos de Aplicação Comerciais

A licença [ABP Commercial](https://commercial.abp.io/) fornece **módulos de aplicação pré-construídos adicionais** em cima do framework ABP. Veja a [lista de módulos](https://commercial.abp.io/modules) fornecida pelo ABP Commercial.