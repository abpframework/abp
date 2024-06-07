## Gerenciamento de Pacotes do Lado do Cliente no ASP.NET Core MVC

O framework ABP pode funcionar com qualquer tipo de sistema de gerenciamento de pacotes do lado do cliente. Você até pode decidir não usar nenhum sistema de gerenciamento de pacotes e gerenciar suas dependências manualmente.

No entanto, o framework ABP funciona melhor com o **NPM/Yarn**. Por padrão, os módulos integrados são configurados para funcionar com o NPM/Yarn.

Por fim, sugerimos o [**Yarn**](https://classic.yarnpkg.com/) em vez do NPM, pois ele é mais rápido, estável e também compatível com o NPM.

### Pacotes NPM do ABP

O ABP é uma plataforma modular. Todo desenvolvedor pode criar módulos e os módulos devem funcionar juntos em um estado **compatível** e **estável**.

Um desafio são as **versões dos pacotes NPM dependentes**. E se dois módulos diferentes usarem a mesma biblioteca JavaScript, mas em versões diferentes (e potencialmente incompatíveis)?

Para resolver o problema de versionamento, criamos um **conjunto padrão de pacotes** que dependem de algumas bibliotecas de terceiros comuns. Alguns exemplos de pacotes são [@abp/jquery](https://www.npmjs.com/package/@abp/jquery), [@abp/bootstrap](https://www.npmjs.com/package/@abp/bootstrap) e [@abp/font-awesome](https://www.npmjs.com/package/@abp/font-awesome). Você pode ver a **lista de pacotes** no [repositório do GitHub](https://github.com/volosoft/abp/tree/master/npm/packs).

A vantagem de um **pacote padrão** é:

* Ele depende de uma **versão padrão** de um pacote. Depender desse pacote é **seguro** porque todos os módulos dependem da mesma versão.
* Ele contém os mapeamentos para copiar os recursos da biblioteca (arquivos js, css, img...) da pasta `node_modules` para a pasta `wwwroot/libs`. Consulte a seção *Mapeando os Recursos da Biblioteca* para mais informações.

Depender de um pacote padrão é fácil. Basta adicioná-lo ao seu arquivo **package.json** como você normalmente faria. Exemplo:

```json
{
  ...
  "dependencies": {
    "@abp/bootstrap": "^1.0.0"
  }
}
```

É sugerido depender de um pacote padrão em vez de depender diretamente de um pacote de terceiros.

#### Instalação do Pacote

Depois de depender de um pacote NPM, tudo o que você precisa fazer é executar o comando **yarn** no terminal para instalar todos os pacotes e suas dependências:

```bash
yarn
```

Alternativamente, você pode usar `npm install`, mas o [Yarn](https://classic.yarnpkg.com/) é sugerido como mencionado anteriormente.

#### Contribuição de Pacotes

Se você precisar de um pacote NPM de terceiros que não esteja no conjunto padrão de pacotes, você pode criar uma Pull Request no repositório do Github [repositório](https://github.com/volosoft/abp). Uma Pull Request que segue essas regras é aceita:

* O nome do pacote deve ser `@abp/nome-do-pacote` para um `nome-do-pacote` no NPM (exemplo: `@abp/bootstrap` para o pacote `bootstrap`).
* Deve ser a versão **mais recente estável** do pacote.
* Deve depender apenas de um pacote de terceiros. Pode depender de vários pacotes `@abp/*`.
* O pacote deve incluir um arquivo `abp.resourcemapping.js` formatado como definido na seção *Mapeando os Recursos da Biblioteca*. Este arquivo deve mapear apenas os recursos do pacote dependente.
* Você também precisa criar [contribuidor(es) de pacote](Bundling-Minification.md) para o pacote que você criou.

Veja os pacotes padrão atuais para exemplos.

### Mapeando os Recursos da Biblioteca

O uso de pacotes NPM e da ferramenta NPM/Yarn é o padrão de fato para bibliotecas do lado do cliente. A ferramenta NPM/Yarn cria uma pasta **node_modules** na pasta raiz do seu projeto da web.

O próximo desafio é copiar os recursos necessários (arquivos js, css, img...) da pasta `node_modules` para uma pasta dentro da pasta **wwwroot** para torná-los acessíveis aos clientes/navegadores.

O comando `abp install-libs` da CLI do ABP **copia os recursos** da pasta **node_modules** para a pasta **wwwroot/libs**. Cada **pacote padrão** (consulte a seção *@ABP NPM Packages*) define o mapeamento para seus próprios arquivos. Portanto, na maioria das vezes, você só precisa configurar as dependências.

Os modelos de inicialização já estão configurados para funcionar com tudo isso. Esta seção explicará as opções de configuração.

#### Arquivo de Definição de Mapeamento de Recursos

Um módulo deve definir um arquivo JavaScript chamado `abp.resourcemapping.js` formatado como no exemplo abaixo:

```json
module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs",
        "!@libs/**/foo.txt"
    ],
    mappings: {
        
    }
}
```

* A seção **aliases** define aliases padrão (placeholders) que podem ser usados nos caminhos de mapeamento. **@node_modules** e **@libs** são obrigatórios (pelos pacotes padrão), você pode definir seus próprios aliases para reduzir a duplicação.
* A seção **clean** é uma lista de pastas a serem limpas antes de copiar os arquivos. A correspondência de glob e a negação estão habilitadas, então você pode ajustar o que excluir e manter. O exemplo acima limpará tudo dentro de `./wwwroot/libs`, mas manterá quaisquer arquivos `foo.txt`.
* A seção **mappings** é uma lista de mapeamentos de arquivos/pastas a serem copiados. Este exemplo não copia nenhum recurso em si, mas depende de um pacote padrão.

Um exemplo de configuração de mapeamento é mostrado abaixo:

```json
mappings: {
    "@node_modules/bootstrap/dist/css/bootstrap.css": "@libs/bootstrap/css/",
    "@node_modules/bootstrap/dist/js/bootstrap.bundle.js": "@libs/bootstrap/js/",
    "@node_modules/bootstrap-datepicker/dist/locales/*.*": "@libs/bootstrap-datepicker/locales/",
    "@node_modules/bootstrap-v4-rtl/dist/**/*": "@libs/bootstrap-v4-rtl/dist/"
}
```

#### Comando install-libs

Depois de configurar corretamente o arquivo `abp.resourcemapping.js`, você pode executar o seguinte comando da CLI do ABP no terminal:

````bash
abp install-libs
````

Quando você executar este comando, todos os pacotes copiarão seus próprios recursos para a pasta `wwwroot/libs`. Executar `abp install-libs` é necessário apenas se você fizer uma alteração em suas dependências no arquivo **package.json**.

#### Veja também

* [Empacotamento e Minificação](Bundling-Minification.md)
* [Tematização](Theming.md)