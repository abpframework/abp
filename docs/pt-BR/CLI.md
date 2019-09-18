# ABP CLI

O ABP CLI (Command Line Interface) é uma ferramenta de linha de comando para executar algumas operações comuns para soluções baseadas em ABP.

## Instalação

O ABP CLI é uma [ferramenta global dotnet](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools) . Instale-o usando uma janela de linha de comando:

````bash
dotnet tool install -g Volo.Abp.Cli
````

Para atualizar uma instalação existente:

````bash
dotnet tool update -g Volo.Abp.Cli
````

## Comandos

### Novo

Gera uma nova solução baseada nos [modelos de inicialização](Startup-Templates/Index.md) do ABP .

Uso básico:

````bash
abp new <solution-name> [options]
````

Examplo:

````bash
abp new Acme.BookStore
````

* `Acme.BookStore` é o nome da solução aqui.
* A convenção comum é nomear uma solução como *YourCompany.YourProject* . No entanto, você pode usar nomes diferentes, como *YourProject* (namespacing de nível único) ou *YourCompany.YourProduct.YourModule* (namespacing de três níveis).

#### Opções

* `--template`ou `-t`: especifica o nome do modelo. O nome do modelo padrão é `app`, que gera um aplicativo da web. Modelos disponíveis:
  * `app`(padrão): [modelo de aplicativo](https://docs.abp.io/en/abp/latest/Startup-Templates/Application) . Opções adicionais:
    * `--ui`ou `-u`: Especifica a UI framework. Framework padrão é `mvc`. Framework disponíveis:
      * `mvc`: ASP.NET Core MVC. Existem algumas opções adicionais para este modelo:
        * `--tiered`: Cria uma solução em camadas em que as camadas da Web e da API HTTP são fisicamente separadas. Se não especificado, ele cria uma solução em camadas que é menos complexa e adequada para a maioria dos cenários.
      * `angular`: Angular. Existem algumas opções adicionais para este modelo:
        * `--separate-identity-server`: Separa o aplicativo do servidor de identidade do aplicativo host da API. Se não especificado, você terá um único ponto de extremidade no lado do servidor.
    * `--database-provider` Ou `-d`: especifica o provedor de banco de dados. O provedor padrão é `ef`. Fornecedores disponíveis:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
  *  `module`: [Exemplo de Módulo](Startup-Templates/Module.md). Opções adicionais:
    * `--no-ui`: Especifica para não incluir a UI. Isso possibilita a criação de módulos somente de serviço (também conhecidos como microsserviços - sem interface do usuário).
* `--output-folder` ou `-o`: especifica a pasta de saída. O valor padrão é o diretório atual.
* `--version` ou `-v`: Especifica a ABP & versão de exemplo . Pode ser uma [release tag](https://github.com/abpframework/abp/releases) ou um [branch name](https://github.com/abpframework/abp/branches). Usa a versão mais recente, se não especificado. Na maioria das vezes, você desejará usar a versão mais recente.

### add-package

Adiciona um pacote ABP a um projeto por,

- Adicionando pacote de nuget relacionado como uma dependência ao projeto.
- Adicionando `[DependsOn(...)]`atributo à classe de módulo no projeto (consulte o [documento de desenvolvimento](https://docs.abp.io/en/abp/latest/Module-Development-Basics) do [módulo](https://docs.abp.io/en/abp/latest/Module-Development-Basics) ).

> Observe que o módulo adicionado pode exigir uma configuração adicional, geralmente indicada na documentação do pacote relacionado.

Uso básico:

```bash
abp add-package <package-name> [options]
```

Bater

cópia de

Exemplo:

```
abp add-package Volo.Abp.MongoDB
```

- Este exemplo adiciona o pacote Volo.Abp.MongoDB ao projeto.

#### Opções

- `--project`ou `-p`: especifica o caminho do arquivo do projeto (.csproj). Se não especificado, a CLI tenta encontrar um arquivo .csproj no diretório atual.

### add-module

Adiciona um [módulo de aplicativo com vários pacotes](Modules/Index.md) a uma solução, localizando todos os pacotes do módulo, localizando projetos relacionados na solução e adicionando cada pacote ao projeto correspondente na solução.

> Um módulo de negócios geralmente consiste em vários pacotes (devido a camadas, diferentes opções de provedor de banco de dados ou outros motivos). O uso do `add-module`comando simplifica drasticamente a adição de um módulo a uma solução. No entanto, cada módulo pode exigir algumas configurações adicionais, geralmente indicadas na documentação do módulo relacionado.

Uso básico:

```bash
abp add-module <module-name> [options]
```

Exemplo:

```bash
abp add-module Volo.Blogging
```

- Este exemplo adiciona o módulo Volo.Blogging à solução.

#### Opções

- `--solution`ou `-s`: especifica o caminho do arquivo da solução (.sln). Se não especificado, a CLI tenta encontrar um arquivo .sln no diretório atual.
- `--skip-db-migrations`: Para o provedor de banco de dados EF Core, ele adiciona automaticamente um novo código à primeira migração ( `Add-Migration`) e atualiza o banco de dados ( `Update-Database`), se necessário. Especifique esta opção para pular esta operação.
- `-sp`ou `--startup-project`: caminho relativo para a pasta do projeto de inicialização. O valor padrão é a pasta atual.

### atualizar

A atualização de todos os pacotes relacionados ao ABP pode ser entediante, pois existem muitos pacotes da estrutura e dos módulos. Este comando atualiza automaticamente todos os pacotes NuGet e NPM relacionados ao ABP em uma solução ou projeto para as versões mais recentes.

Uso:

```bash
abp update [options]
```

- Se você executar em um diretório com um arquivo .sln, ele atualizará todos os pacotes relacionados ao ABP de todos os projetos da solução para as versões mais recentes.
- Se você executar em um diretório com um arquivo .csproj, ele atualizará todos os pacotes relacionados ao ABP do projeto para as versões mais recentes.

#### Opções

- `--include-previews`ou `-p`: inclui pacotes de visualização, beta e rc enquanto verifica as versões mais recentes.

### Socorro

Grava informações básicas de uso da CLI.

Uso:

```bash
abp help [command-name]
```

Exemplos:

```bash
abp help        # Shows a general help.
abp help new    # Shows help about the "new" command.
```


  