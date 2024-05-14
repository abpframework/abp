# Tabelas do Banco de Dados

Esta documentação descreve todas as tabelas do banco de dados e seus propósitos. Você pode ler esta documentação para obter conhecimento geral das tabelas do banco de dados que vêm de cada módulo.

## [Módulo de Registro de Auditoria](Audit-Logging.md)

### AbpAuditLogs

Esta tabela armazena informações sobre os registros de auditoria no aplicativo. Cada registro representa um log de auditoria e rastreia as ações realizadas no aplicativo.

### AbpAuditLogActions

Esta tabela armazena informações sobre as ações realizadas no aplicativo, que são registradas para fins de auditoria.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpAuditLogs](#abpauditlogs) | Id | Vincula cada ação a um log de auditoria específico. |

### AbpEntityChanges

Esta tabela armazena informações sobre as alterações de entidade no aplicativo, que são registradas para fins de auditoria.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpAuditLogs](#abpauditlogs) | Id | Vincula cada alteração de entidade a um log de auditoria específico. |

### AbpEntityPropertyChanges

Esta tabela armazena informações sobre as alterações de propriedade em entidades no aplicativo, que são registradas para fins de auditoria.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpEntityChanges](#abpentitychanges) | Id | Vincula cada alteração de propriedade a uma alteração de entidade específica. |

## [Módulo de Tarefas em Segundo Plano](Background-Jobs.md)

### AbpBackgroundJobs

Esta tabela armazena informações sobre as tarefas em segundo plano no aplicativo e facilita seu gerenciamento e rastreamento eficientes. Cada entrada na tabela contém detalhes de uma tarefa em segundo plano, incluindo o nome da tarefa, argumentos, contagem de tentativas, próxima tentativa, última tentativa, status abandonado e prioridade.

## [Módulo de Gerenciamento de Inquilinos](Tenant-Management.md)

### AbpTenants

Esta tabela armazena informações sobre os inquilinos. Cada registro representa um inquilino e contém informações sobre o inquilino, como nome e outros detalhes.

### AbpTenantConnectionStrings

Esta tabela armazena informações sobre as strings de conexão do banco de dados do inquilino. Quando você define uma string de conexão para um inquilino, um novo registro será adicionado a esta tabela. Você pode consultar este banco de dados para obter strings de conexão por inquilinos.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpTenants](#abptenants) | Id | A coluna `Id` na tabela `AbpTenants` é usada para associar a string de conexão do inquilino ao inquilino correspondente. |

## Módulo de Blogging

### BlgUsers

Esta tabela armazena informações sobre os usuários do blog. Quando um novo usuário de identidade é criado, um novo registro será adicionado a esta tabela.

### BlgBlogs

Esta tabela serve para armazenar informações do blog e separar semanticamente as postagens de cada blog.

### BlgPosts

Esta tabela armazena informações sobre as postagens do blog. Você pode consultar esta tabela para obter postagens de blog por blogs.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [BlgBlogs](#blgblogs) | Id | Para associar a postagem do blog ao blog correspondente. |

### BlgComments

Esta tabela armazena informações sobre os comentários feitos nas postagens do blog. Você pode consultar esta tabela para obter comentários por postagens.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [BlgPosts](#blgposts) | Id | Vincula o comentário à postagem do blog correspondente. |
| [BlgComments](#blgcomments) | Id | Vincula o comentário ao comentário pai. |

### BlgTags

Esta tabela armazena informações sobre as tags. Quando uma nova tag é usada, um novo registro será adicionado a esta tabela. Você pode consultar esta tabela para obter tags por blogs.

### BlgPostTags

Esta tabela é usada para associar tags a postagens de blog, a fim de categorizar e organizar o conteúdo. Você pode consultar esta tabela para obter tags de postagens por postagens.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [BlgTags](#blgtags) | Id | Vincula a tag da postagem à tag correspondente. |
| [BlgPosts](#blgposts) | Id | Vincula a tag da postagem à postagem do blog correspondente. |

## [Módulo CMS Kit](Cms-Kit/Index.md)

### CmsUsers

Esta tabela armazena informações sobre os usuários do módulo CMS Kit. Quando um novo usuário de identidade é criado, um novo registro será adicionado a esta tabela.

### CmsBlogs

Esta tabela serve para armazenar informações do blog e separar semanticamente as postagens de cada blog.

### CmsBlogPosts

Esta tabela armazena informações sobre as postagens do blog. Você pode consultar esta tabela para obter postagens de blog por blogs.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [CmsUsers](#cmsusers) | Id | Vincula a postagem do blog ao autor correspondente. |

### CmsBlogFeatures

Esta tabela armazena informações sobre os recursos do blog. Você pode consultar esta tabela para obter recursos do blog por blogs.

### CmsComments

Esta tabela é utilizada pelo sistema de comentários do CMS Kit para armazenar comentários feitos nas postagens do blog. Você pode consultar esta tabela para obter comentários por postagens.

### CmsTags

Esta tabela armazena informações sobre as tags. Quando uma nova tag é usada, um novo registro será adicionado a esta tabela. Você pode consultar esta tabela para obter tags por blogs.

### CmsEntityTags

Esta tabela é utilizada pelo sistema de gerenciamento de tags para armazenar tags e sua relação com várias entidades, permitindo assim a categorização e organização eficiente do conteúdo. Você pode consultar esta tabela para obter tags de entidades por entidades.

### CmsGlobalResources

Esta tabela é uma tabela de banco de dados para o sistema de recursos globais do CMS Kit, permitindo a adição dinâmica de estilos e scripts globais.

### CmsMediaDescriptors

Esta tabela é utilizada pelo módulo CMS Kit para gerenciar arquivos de mídia usando o módulo [BlobStoring](../Blob-Storing.md).

### CmsMenuItems

Esta tabela é usada pelo sistema de menu do CMS Kit para gerenciar e armazenar informações sobre menus públicos dinâmicos, incluindo detalhes como nomes de exibição de itens de menu, URLs e relacionamentos hierárquicos.

### CmsPages

Esta tabela é utilizada pelo sistema de páginas do CMS Kit para armazenar páginas dinâmicas dentro do aplicativo, incluindo informações como URLs de página, títulos e conteúdo.

### CmsRatings

Esta tabela é utilizada pelo sistema de classificação do CMS Kit para armazenar classificações feitas em postagens de blog. Você pode consultar esta tabela para obter classificações por postagens.

### CmsUserReactions

Esta tabela é utilizada pelo sistema de reações do CMS Kit para armazenar reações feitas em postagens de blog. Você pode consultar esta tabela para obter reações por postagens.

## [Módulo de Documentação](Docs.md)

### DocsProjects

Esta tabela armazena informações do projeto para categorizar documentos de acordo com diferentes projetos.

### DocsDocuments

Esta tabela recupera o documento se ele não for encontrado no cache. A documentação está sendo atualizada quando o conteúdo é recuperado do banco de dados.

### DocsDocumentContributors

Esta tabela armazena informações sobre os contribuidores dos documentos. Você pode consultar esta tabela para obter contribuidores de documentos por documentos.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [DocsDocuments](#docsdocuments) | Id | Vincula o contribuidor do documento ao documento correspondente. |

## [Módulo de Gerenciamento de Recursos](Feature-Management.md)

### AbpFeatureGroups

Esta tabela armazena informações sobre os grupos de recursos no aplicativo. Por exemplo, você pode agrupar todos os recursos na tabela [`AbpFeatures`](#abpfeatures) relacionados ao módulo `Identity` sob o grupo `Identity`.

### AbpFeatures

Esta tabela armazena informações sobre os recursos no aplicativo. Você pode usar a coluna `Name` para vincular cada recurso com seu valor de recurso correspondente na tabela [`AbpFeatureValues`](#abpfeaturevalues), para que você possa gerenciar e organizar facilmente os recursos.

### AbpFeatureValues

Esta tabela armazena os valores dos recursos para diferentes provedores. Você pode usar a coluna `Name` para vincular cada valor de recurso com seu recurso correspondente na tabela [`AbpFeatures`](#abpfeatures), para que você possa gerenciar e organizar facilmente os recursos.

## [Módulo de Identidade](Identity.md)

### AbpUsers

Esta tabela armazena informações sobre os usuários de identidade no aplicativo.

### AbpRoles

Esta tabela armazena informações sobre os papéis no aplicativo. Os papéis são usados para gerenciar e controlar o acesso a diferentes partes do aplicativo, atribuindo permissões e reivindicações aos papéis e, em seguida, atribuindo esses papéis aos usuários. Esta tabela é importante para gerenciar e organizar os papéis no aplicativo e para definir os direitos de acesso dos usuários.

### AbpClaimTypes

Esta tabela armazena informações sobre os tipos de reivindicação usados no aplicativo. Você pode usar as colunas `Name` e `Regex` para filtrar os tipos de reivindicação por nome e padrão regex, respectivamente, para que você possa gerenciar e rastrear facilmente os tipos de reivindicação no aplicativo.

### AbpLinkUsers

Esta tabela é útil para vincular várias contas de usuário em diferentes inquilinos ou aplicativos a um único usuário, permitindo que eles alternem facilmente entre suas contas.

### AbpUserClaims

Esta tabela pode gerenciar o controle de acesso baseado em usuário, permitindo atribuir reivindicações aos usuários, que descrevem os direitos de acesso do usuário individual.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | Vincula a reivindicação do usuário ao usuário correspondente. |

### AbpUserLogins

Esta tabela pode armazenar informações sobre os logins externos do usuário, como login com Facebook, Google, etc., e também pode ser usada para rastrear o histórico de login dos usuários.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | Vincula o login do usuário ao usuário correspondente. |

### AbpUserRoles

Esta tabela pode gerenciar o controle de acesso baseado em usuário, permitindo atribuir papéis aos usuários, que descrevem os direitos de acesso do usuário individual.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | Vincula o papel do usuário ao usuário correspondente. |
| [AbpRoles](#abproles) | Id | Vincula o papel do usuário ao papel correspondente. |

### AbpUserTokens

Esta tabela pode armazenar informações sobre tokens de atualização, tokens de acesso e outros tokens usados no aplicativo. Também pode ser usado para invalidar ou revogar tokens de usuário.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | Vincula o token do usuário ao usuário correspondente. |

### AbpOrganizationUnits

Esta tabela é útil para criar e gerenciar uma estrutura hierárquica da organização, permitindo agrupar usuários e atribuir papéis com base na estrutura da organização. Você pode usar as colunas `Code` e `ParentId` para filtrar as unidades organizacionais por código e ID do pai, respectivamente, para que você possa gerenciar e rastrear facilmente as unidades organizacionais no aplicativo.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpOrganizationUnits](#abporganizationunits) | ParentId | Vincula a unidade organizacional à sua unidade organizacional pai. |

### AbpOrganizationUnitRoles

Esta tabela é útil para gerenciar o controle de acesso baseado em função no nível das unidades organizacionais, permitindo atribuir diferentes papéis a diferentes partes da estrutura da organização. Você pode usar as colunas `OrganizationUnitId` e `RoleId` para filtrar os papéis por ID da unidade organizacional e ID do papel, respectivamente, para que você possa gerenciar e rastrear facilmente os papéis atribuídos às unidades organizacionais no aplicativo.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpOrganizationUnits](#abporganizationunits) | Id | Vincula o papel da unidade organizacional à unidade organizacional correspondente. |
| [AbpRoles](#abproles) | Id | Vincula o papel da unidade organizacional ao papel correspondente. |

### AbpUserOrganizationUnits

Esta tabela armazena informações sobre as unidades organizacionais atribuídas aos usuários no aplicativo. Esta tabela pode gerenciar relacionamentos entre usuário e unidade organizacional e agrupar usuários com base na estrutura da organização.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | Vincula a unidade organizacional do usuário ao usuário correspondente. |
| [AbpOrganizationUnits](#abporganizationunits) | Id | Vincula a unidade organizacional do usuário à unidade organizacional correspondente. |

### AbpRoleClaims

Esta tabela é útil para gerenciar o controle de acesso baseado em função, permitindo atribuir reivindicações aos papéis, que descrevem os direitos de acesso dos usuários que pertencem a esse papel.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpRoles](#abproles) | Id | Vincula a reivindicação do papel ao papel correspondente. |

### AbpSecurityLogs

Esta tabela registra operações e alterações importantes relacionadas às contas de usuário, permitindo que os usuários salvem os logs de segurança para referência futura.

## [Gerenciamento de Permissões](Permission-Management.md)

### AbpPermissionGroups

Esta tabela é importante para gerenciar e organizar as permissões no aplicativo, agrupando-as em categorias lógicas.

### AbpPermissions

Esta tabela é importante para gerenciar e controlar o acesso a diferentes partes do aplicativo e para definir as permissões granulares que compõem as permissões ou papéis maiores.

### AbpPermissionGrants

A tabela armazena e gerencia as permissões no aplicativo e mantém o controle das permissões concedidas, para quem e quando. Colunas como `Name`, `ProviderName`, `ProviderKey`, `TenantId` podem ser usadas para filtrar as permissões concedidas por nome, nome do provedor, chave do provedor e ID do inquilino, respectivamente, para que você possa gerenciar e rastrear facilmente as permissões concedidas no aplicativo.

## [Gerenciamento de Configurações](Setting-Management.md)

### AbpSettings

Esta tabela armazena pares de chave-valor de configurações para o aplicativo e permite a configuração dinâmica do aplicativo sem a necessidade de recompilação.

## [OpenIddict](OpenIddict.md)

### OpenIddictApplications

Esta tabela pode armazenar informações sobre as aplicações OpenID Connect, incluindo o ID do cliente, segredo do cliente, URI de redirecionamento e outras informações relevantes. Também pode ser usado para autenticar e autorizar clientes usando o protocolo OpenID Connect.

### OpenIddictAuthorizations

Esta tabela armazena os dados de autorização do OpenID Connect no aplicativo. Também pode ser usado para gerenciar e validar as concessões de autorização emitidas para clientes e usuários.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [OpenIddictApplications](#openiddictapplications) | Id | Vincula a autorização à aplicação correspondente. |

### OpenIddictTokens

Esta tabela pode armazenar informações sobre os tokens OpenID Connect, incluindo o payload do token, expiração, tipo e outras informações relevantes. Também pode ser usado para gerenciar e validar os tokens emitidos para clientes e usuários, como tokens de acesso e tokens de atualização, e controlar o acesso a recursos protegidos.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [OpenIddictApplications](#openiddictapplications) | Id | Vincula o token à aplicação correspondente. |
| [OpenIddictAuthorizations](#openiddictauthorizations) | Id | Vincula o token à autorização correspondente. |

### OpenIddictScopes

Esta tabela pode armazenar informações sobre os escopos OpenID Connect, incluindo o nome e a descrição do escopo. Também pode ser usado para definir as permissões ou direitos de acesso associados aos escopos, que são então usados para controlar o acesso a recursos protegidos.

## [IdentityServer](IdentityServer.md)

### IdentityServerApiResources

Esta tabela pode armazenar informações sobre os recursos da API, incluindo o nome do recurso, nome de exibição, descrição e outras informações relevantes. Também pode ser usado para definir os escopos, reivindicações e propriedades associadas aos recursos da API, que são então usados para controlar o acesso a recursos protegidos.

### IdentityServerIdentityResources

Esta tabela pode armazenar informações sobre os recursos de identidade, incluindo o nome, nome de exibição, descrição e status habilitado.

### IdentityServerClients

Esta tabela pode armazenar informações sobre os clientes, incluindo o ID do cliente, nome do cliente, URI do cliente e outras informações relevantes. Também pode ser usado para definir os escopos, reivindicações e propriedades associadas aos clientes, que são então usados para controlar o acesso a recursos protegidos.

### IdentityServerApiScopes

Esta tabela pode armazenar informações sobre os escopos da API, incluindo o nome do escopo, nome de exibição, descrição e outras informações relevantes. Também pode ser usado para definir as reivindicações e propriedades associadas aos escopos da API, que são então usados para controlar o acesso a recursos protegidos.

### IdentityServerApiResourceClaims

Esta tabela pode armazenar informações sobre as reivindicações de um recurso da API, incluindo o tipo de reivindicação e o ID do recurso da API.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerApiResources](#identityserverapiresources) | Id | Vincula a reivindicação ao recurso da API correspondente. |

### IdentityServerIdentityResourceClaims

Esta tabela pode armazenar informações sobre as reivindicações de um recurso de identidade, incluindo o tipo de reivindicação e o ID do recurso de identidade.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerIdentityResources](#identityserveridentityresources) | Id | Vincula a reivindicação ao recurso de identidade correspondente. |

### IdentityServerClientClaims

Esta tabela pode armazenar informações sobre as reivindicações de um cliente, incluindo o tipo de reivindicação, valor da reivindicação e ID do cliente.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Vincula a reivindicação ao cliente correspondente. |

### IdentityServerApiScopeClaims

Esta tabela pode armazenar informações sobre as reivindicações de um escopo da API, incluindo o tipo de reivindicação e o ID do escopo da API.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerApiScopes](#identityserverapiscopes) | Id | Vincula a reivindicação ao escopo da API correspondente. |

### IdentityServerApiResourceProperties

Esta tabela pode armazenar informações sobre propriedades, incluindo a chave da propriedade e o valor, e o recurso da API associado. Essas propriedades podem armazenar metadados adicionais ou informações de configuração relacionadas aos recursos da API.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerApiResources](#identityserverapiresources) | Id | Vincula a propriedade ao recurso da API correspondente. |

### IdentityServerIdentityResourceProperties

Esta tabela pode armazenar informações sobre propriedades, incluindo a chave da propriedade e o valor, e o recurso de identidade associado. Essas propriedades podem armazenar metadados adicionais ou informações de configuração relacionadas aos recursos de identidade.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerIdentityResources](#identityserveridentityresources) | Id | Vincula a propriedade ao recurso de identidade correspondente. |

### IdentityServerClientProperties

Esta tabela pode armazenar informações sobre as propriedades de um cliente, incluindo a chave, valor e ID do cliente. Essas propriedades podem armazenar metadados adicionais ou informações de configuração relacionadas aos clientes.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Vincula a propriedade ao cliente correspondente. |

### IdentityServerApiScopeProperties

Esta tabela pode armazenar informações sobre as propriedades de um escopo da API, incluindo a chave, valor e ID do escopo da API. Essas propriedades podem armazenar metadados adicionais ou informações de configuração relacionadas aos escopos da API.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerApiScopes](#identityserverapiscopes) | Id | Vincula a propriedade ao escopo da API correspondente. |

### IdentityServerApiResourceScopes

Esta tabela pode armazenar informações sobre os escopos de um recurso da API, incluindo o nome do escopo e o ID do recurso da API.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerApiResources](#identityserverapiresources) | Id | Vincula o escopo ao recurso da API correspondente. |

### IdentityServerClientScopes

Esta tabela pode armazenar informações sobre os escopos de um cliente, incluindo o escopo e o ID do cliente.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Vincula o escopo ao cliente correspondente. |

### IdentityServerApiResourceSecrets

Esta tabela pode armazenar informações sobre os segredos de um recurso da API, incluindo o valor do segredo, data de expiração e ID do recurso da API.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerApiResources](#identityserverapiresources) | Id | Vincula o segredo ao recurso da API correspondente. |

### IdentityServerClientSecrets

Esta tabela pode armazenar informações sobre os segredos de um cliente, incluindo o valor do segredo, data de expiração e ID do cliente.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Vincula o segredo ao cliente correspondente. |

### IdentityServerClientCorsOrigins

Esta tabela pode armazenar informações sobre as origens CORS de um cliente, incluindo a origem e o ID do cliente. Também pode ser usado para gerenciar e validar as origens CORS de um cliente.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Vincula a origem CORS ao cliente correspondente. |

### IdentityServerClientGrantTypes

Esta tabela pode armazenar informações sobre os tipos de concessão de um cliente, incluindo o tipo de concessão e o ID do cliente.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Vincula o tipo de concessão ao cliente correspondente. |

### IdentityServerClientIdPRestrictions

Esta tabela pode armazenar informações sobre as restrições do provedor de identidade de um cliente, incluindo o provedor de identidade e o ID do cliente.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Vincula a restrição do provedor de identidade ao cliente correspondente. |

### IdentityServerClientPostLogoutRedirectUris

Esta tabela pode armazenar informações sobre os URIs de redirecionamento pós logout de um cliente, incluindo o URI de redirecionamento pós logout e o ID do cliente.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Vincula o URI de redirecionamento pós logout ao cliente correspondente. |

### IdentityServerClientRedirectUris

Esta tabela pode armazenar informações sobre os URIs de redirecionamento de um cliente, incluindo o URI de redirecionamento e o ID do cliente.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Vincula o URI de redirecionamento ao cliente correspondente. |

### IdentityServerDeviceFlowCodes

Esta tabela pode armazenar informações sobre os códigos de fluxo de dispositivo, incluindo o código do usuário, código do dispositivo, ID do assunto, ID do cliente, data de criação, expiração, dados e ID da sessão.

### IdentityServerPersistedGrants

Esta tabela pode armazenar informações sobre as concessões persistidas, incluindo a chave, tipo, ID do assunto, ID do cliente, data de criação, expiração e dados.

## Outros

### AbpBlobContainers

Esta tabela é importante para fornecer uma melhor experiência do usuário, permitindo que o aplicativo suporte vários contêineres e forneça recursos específicos de BLOB.

### AbpBlobs

Esta tabela armazena os dados binários de BLOBs (objetos binários grandes) no aplicativo. Cada BLOB está relacionado a um contêiner na tabela [AbpBlobContainers](#abpblobcontainers), onde o nome do contêiner, ID do inquilino e outras propriedades do contêiner podem ser encontrados.

#### Chaves Estrangeiras

| Tabela | Coluna | Descrição |
| --- | --- | --- |
| [AbpBlobContainers](#abpblobcontainers) | Id | Vincula o BLOB ao contêiner correspondente. |

### AbpLocalizationResources

Esta tabela armazena os recursos de localização para o aplicativo. Esta tabela é importante para fornecer uma melhor experiência do usuário, permitindo que o aplicativo suporte vários recursos e forneça texto localizado e outros recursos específicos de localização.

### AbpLocalizationTexts

A tabela contém o nome do recurso, nome da cultura e um valor codificado em JSON que contém o par chave-valor do texto de localização. Ele permite o armazenamento e gerenciamento eficiente de textos de localização e permite a atualização fácil ou adição de novas traduções para recursos e culturas específicas.