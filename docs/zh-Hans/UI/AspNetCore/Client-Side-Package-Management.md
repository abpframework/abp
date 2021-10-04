
## ASP.NET Core MVC 客户端包管理

ABP框架可以与任何类型的客户端包管理系统一起使用. 甚至你可以决定不使用包管理系统并手动管理依赖项.

但是, ABP框架最适用于**NPM/Yarn**. 默认情况下,内置模块配置为与NPM/Yarn一起使用.

最后, 我们建议[**Yarn**](https://classic.yarnpkg.com/)而不是NPM,因为它更快,更稳定并且与NPM兼容.

### @ABP NPM Packages

ABP是一个模块化平台. 每个开发人员都可以创建模块, 模块应该在**兼容**和**稳定**状态下协同工作.

一个挑战是依赖NPM包的**版本**. 如果两个不同的模块使用相同的JavaScript库但其不同(并且可能不兼容)的版本会怎样.

为了解决版本问题, 我们创建了一套**标准包**, 这取决于一些常见的第三方库. 一些示例包是[@abp/jquery](https://www.npmjs.com/package/@abp/jquery), [@ abp/bootstrap](https://www.npmjs.com/package/@abp/bootstrap)和[@abp/font-awesome](https://www.npmjs.com/package/@abp/font-awesome). 你可以从[Github存储库](https://github.com/volosoft/abp/tree/master/npm/packs)中查看**列表**.

**标准包**的好处是:

* 它取决于包装的**标准版本**. 取决于此包是**安全**,因为所有模块都依赖于相同的版本.
* 它包含将库资源(js,css,img...文件)从**node_modules**文件夹复制到**wwwroot/libs**文件夹. 有关更多信息, 请参阅 *映射库资源* 部分.

依赖标准包装很容易. 只需像往常一样将它添加到**package.json**文件中. 例如:

````
    {
      ...
      "dependencies": {
        "@abp/bootstrap": "^1.0.0"
      }
    }
````

建议依赖于标准软件包, 而不是直接依赖于第三方软件包.

#### 安装包

依赖于NPM包后, 你应该做的就是从命令行运行**yarn**命令来安装所有包及其依赖项:

````
yarn
````

虽然你可以使用`npm install`,但如前所述,建议使用[Yarn](https://classic.yarnpkg.com/).

#### 贡献包

如果你需要不在标准软件包中的第三方NPM软件包,你可以在Github[repository](https://github.com/volosoft/abp)上创建Pull请求. 接受遵循这些规则的拉取请求:

* 对于NPM上的`package-name`, 包名称应该命名为`@abp/package-name`(例如:`bootstrap`包的`@abp/bootstrap`).
* 它应该是**最新的稳定**版本的包.
* 它应该只依赖于**单个**第三方包. 它可以依赖于多个`@abp/*`包.
* 包应包含一个`abp.resourcemapping.js`文件格式,如*映射库资源*部分中所定义. 此文件应仅映射所依赖包的资源.
* 你还需要为你创建的包创建[bundle贡献者](Bundling-Minification.md).

有关示例, 请参阅当前标准包.

### 映射库资源

使用NPM包和NPM/Yarn工具是客户端库的事实标准.  NPM/Yarn工具在Web项目的根文件夹中创建一个**node_modules**文件夹.

下一个挑战是将所需的资源(js,css,img ...文件)从`node_modules`复制到**wwwroot**文件夹内的文件夹中,以使其可供客户端/浏览器访问.

ABP的 `install-libs` 命令**将资源**从**node_modules**复制到**wwwroot/libs**文件夹. 每个**标准包**(参见*@ABP NPM Packages*部分)定义了自己文件的映射. 因此, 大多数情况你只配置依赖项.

**启动模板**已经配置为开箱即用的所有这些. 本节将介绍配置选项.

#### 资源映射定义文件

模块应该定义一个名为`abp.resourcemapping.js`的JavaScript文件,其格式如下例所示:

````js
module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs"
    ],
    mappings: {
        
    }
}
````

* **aliases**部分定义了可在映射路径中使用的标准别名(占位符). **@node_modules**和 **@libs**是必需的(通过标准包), 你可以定义自己的别名以减少重复.
* **clean**部分是在复制文件之前要清理的文件夹列表.
* **mappings**部分是要复制的文件/文件夹的映射列表.此示例不会复制任何资源本身,但取决于标准包.

示例映射配置如下所示:

````js
mappings: {
    "@node_modules/bootstrap/dist/css/bootstrap.css": "@libs/bootstrap/css/",
    "@node_modules/bootstrap/dist/js/bootstrap.bundle.js": "@libs/bootstrap/js/"
}
````

#### install-libs 命令

正确配置`abp.resourcemapping.js`文件后, 可以从命令行运行ABP CLI命令:

````bash
abp install-libs
````

当你运行这个命令时,所有包都会将自己的资源复制到**wwwroot/libs**文件夹中. 只有在**package.json**文件中对依赖项进行更改时, 才需要运行`abp install-libs`.

#### 参见

* [捆绑 & 压缩](Bundling-Minification.md)
* [主题](Theming.md)
