# Módulo de Conta

O módulo de conta implementa recursos básicos de autenticação, como **login**, **registro**, **recuperação de senha** e **gerenciamento de conta**.

Este módulo é baseado na biblioteca de identidade da Microsoft e no módulo de identidade. Ele possui integração com o IdentityServer (com base no módulo IdentityServer) e com o OpenIddict (com base no módulo OpenIddict) para fornecer autenticação avançada, controle de acesso e outros recursos avançados de autenticação.

## Como instalar

Este módulo já vem pré-instalado (como pacotes NuGet/NPM). Você pode continuar a usá-lo como pacote e obter atualizações facilmente, ou pode incluir seu código-fonte em sua solução (consulte o comando `get-source` da CLI) para desenvolver seu próprio módulo personalizado.

### O código-fonte

O código-fonte deste módulo pode ser acessado [aqui](https://github.com/abpframework/abp/tree/dev/modules/account). O código-fonte está licenciado sob a licença MIT, portanto, você pode usá-lo e personalizá-lo livremente.

## Interface do usuário

Esta seção apresenta as principais páginas fornecidas por este módulo.

### Login

A página `/Account/Login` fornece a funcionalidade de login.

![account-module-login](../images/account-module-login.png)

Os botões de login social/externo ficam visíveis se você configurá-los. Consulte a seção *Logins Sociais/Externos* abaixo. Os links de registro e recuperação de senha redirecionam para as páginas explicadas nas próximas seções.

### Registro

A página `/Account/Register` fornece a funcionalidade de registro de novo usuário.

![account-module-register](../images/account-module-register.png)

### Recuperação de senha e redefinição de senha

A página `/Account/ForgotPassword` fornece uma maneira de enviar um link de redefinição de senha para o endereço de e-mail do usuário. O usuário então clica no link e define uma nova senha.

![account-module-forgot-password](../images/account-module-forgot-password.png)

### Gerenciamento de conta

A página `/Account/Manage` é usada para alterar a senha e as informações pessoais do usuário.

![account-module-manage-account](../images/account-module-manage-account.png)

## Integração com o OpenIddict

O pacote [Volo.Abp.Account.Web.OpenIddict](https://www.nuget.org/packages/Volo.Abp.Account.Web.OpenIddict) fornece integração com o OpenIddict. Este pacote já vem instalado com o modelo de inicialização do aplicativo. Consulte a documentação do módulo OpenIddict.

## Integração com o IdentityServer

O pacote [Volo.Abp.Account.Web.IdentityServer](https://www.nuget.org/packages/Volo.Abp.Account.Web.IdentityServer) fornece integração com o IdentityServer. Este pacote já vem instalado com o modelo de inicialização do aplicativo. Consulte a documentação do módulo IdentityServer.

## Logins Sociais/Externos

O módulo de conta já está configurado para lidar com logins sociais ou externos prontamente. Você pode seguir a documentação do ASP.NET Core para adicionar um provedor de login social/externo à sua aplicação.

### Exemplo: Autenticação do Facebook

Siga o documento de integração do Facebook do ASP.NET Core para oferecer suporte ao login do Facebook em sua aplicação.

#### Adicionar o pacote NuGet

Adicione o pacote [Microsoft.AspNetCore.Authentication.Facebook](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.Facebook) ao seu projeto. Com base na sua arquitetura, isso pode ser feito no projeto `.Web`, `.IdentityServer` (para configuração em camadas) ou `.Host`.

#### Configurar o provedor

Use o método de extensão `.AddFacebook(...)` no método `ConfigureServices` do seu [módulo](../Module-Development-Basics.md) para configurar o cliente:

````csharp
context.Services.AddAuthentication()
    .AddFacebook(facebook =>
    {
        facebook.AppId = "...";
        facebook.AppSecret = "...";
        facebook.Scope.Add("email");
        facebook.Scope.Add("public_profile");
    });
````

> Seria uma prática melhor usar o `appsettings.json` ou o sistema de segredos do usuário do ASP.NET Core para armazenar suas credenciais, em vez de um valor codificado como esse. Siga o documento da Microsoft para aprender a usar os segredos do usuário.