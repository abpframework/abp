# Módulo de Explorador de Arquivos Virtual

## O que é o Módulo de Explorador de Arquivos Virtual?

O Módulo de Explorador de Arquivos Virtual fornece uma interface de usuário simples para visualizar todos os arquivos no [sistema de arquivos virtual](../Virtual-File-System.md).

> O Módulo de Explorador de Arquivos Virtual não está instalado nos [modelos de inicialização](../Startup-Templates/Index.md). Portanto, você precisa adicionar manualmente este módulo à sua aplicação.

### Instalação

#### 1- Usando o ABP CLI

Recomenda-se usar o [ABP CLI](../CLI.md) para instalar o módulo. Abra a janela do CMD no diretório do arquivo de solução (`.sln`) e execute o seguinte comando:

```
abp add-module Volo.VirtualFileExplorer
```

> Se você ainda não o fez, primeiro precisa instalar o [ABP CLI](../CLI.md). Para outras opções de instalação, consulte [a página de descrição do pacote](https://abp.io/package-detail/Volo.Abp.VirtualFileExplorer.Web).

#### 2- Instalação manual

Ou você também pode instalar manualmente o pacote nuget no projeto `Acme.MyProject.Web`:

* Instale o pacote nuget [Volo.Abp.VirtualFileExplorer.Web](https://www.nuget.org/packages/Volo.Abp.VirtualFileExplorer.Web/) no projeto `Acme.MyProject.Web`.

  `Install-Package Volo.Abp.VirtualFileExplorer.Web`

##### 2.1- Adicionando Dependências do Módulo

  * Abra `MyProjectWebModule.cs` e adicione `typeof(AbpVirtualFileExplorerWebModule)` como mostrado abaixo;

  ```csharp
     [DependsOn(
          typeof(AbpVirtualFileExplorerWebModule),
          typeof(MyProjectApplicationModule),
          typeof(MyProjectEntityFrameworkCoreModule),
          typeof(AbpAutofacModule),
          typeof(AbpIdentityWebModule),
          typeof(AbpAccountWebModule),
          typeof(AbpAspNetCoreMvcUiBasicThemeModule)
      )]
      public class MyProjectWebModule : AbpModule
      {
          //...
      }
  ```

##### 2.2- Adicionando Pacote NPM

 * Abra `package.json` e adicione `@abp/virtual-file-explorer": "^2.9.0` como mostrado abaixo:

  ```json
    {
        "version": "1.0.0",
        "name": "my-app",
        "private": true,
        "dependencies": {
            "@abp/aspnetcore.mvc.ui.theme.basic": "^2.9.0",
            "@abp/virtual-file-explorer": "^2.9.0"
        }
    }
  ```

  Em seguida, abra o terminal de linha de comando na pasta do projeto `Acme.MyProject.Web` e execute o seguinte comando:

````bash
abp install-libs
````

Isso é tudo, agora execute a aplicação e navegue até `/VirtualFileExplorer`. Você verá a página do explorador de arquivos virtual:

![Virtual-File-Explorer](../images/virtual-file-explorer.png)

### Opções

Você pode desativar o módulo de explorador de arquivos virtual através das opções `AbpVirtualFileExplorerOptions`:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpVirtualFileExplorerOptions>(options =>
    {
        options.IsEnabled = false;
    });
}
```