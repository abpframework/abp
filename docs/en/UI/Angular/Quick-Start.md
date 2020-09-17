# ABP Angular Quick Start

## How to Prepare Development Environment

Please follow the steps below to prepare your development environment for Angular.

1. **Install Node.js:** Please visit [Node.js downloads page](https://nodejs.org/en/download/) and download proper Node.js v12 or v14 installer for your OS. An alternative is to install [NVM](https://github.com/nvm-sh/nvm) and use it to have multiple versions of Node.js in your operating system.
2. **[Optional] Install Yarn:** You may install Yarn v1 (not v2) following the instructions on [the installation page](https://classic.yarnpkg.com/en/docs/install). Yarn v1 delivers an arguably better developer experience compared to npm v6 and below. You may skip this step and work with npm, which is built-in in Node.js, instead.
3. **[Optional] Install VS Code:** [VS Code](https://code.visualstudio.com/) is a free, open-source IDE which works seamlessly with TypeScript. Although you can use any IDE including Visual Studio or Rider, VS Code will most likely deliver the best developer experience when it comes to Angular projects. ABP project templates even contain plugin recommendations for VS Code users, which VS Code will ask you to install when you open the Angular project folder. Here is a list of recommended extensions:
   - [Angular Language Service](https://marketplace.visualstudio.com/items?itemName=angular.ng-template)
   - [Prettier - Code formatter](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode)
   - [TSLint](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-tslint-plugin)
   - [Visual Studio IntelliCode](https://marketplace.visualstudio.com/items?itemName=visualstudioexptteam.vscodeintellicode)
   - [Path Intellisense](https://marketplace.visualstudio.com/items?itemName=christian-kohler.path-intellisense)
   - [npm Intellisense](https://marketplace.visualstudio.com/items?itemName=christian-kohler.npm-intellisense)
   - [Angular 10 Snippets - TypeScript, Html, Angular Material, ngRx, RxJS & Flex Layout](https://marketplace.visualstudio.com/items?itemName=Mikael.Angular-BeastCode)
   - [JavaScript (ES6) code snippets](https://marketplace.visualstudio.com/items?itemName=xabikos.JavaScriptSnippets)
   - [Debugger for Chrome](https://marketplace.visualstudio.com/items?itemName=msjsdiag.debugger-for-chrome)
   - [Git History](https://marketplace.visualstudio.com/items?itemName=donjayamanne.githistory)
   - [indent-rainbow](https://marketplace.visualstudio.com/items?itemName=oderwat.indent-rainbow)

## How to Start a New Angular Project

You have multiple options to initiate a new Angular project that works with ABP:

### 1. Using ABP CLI

ABP CLI is probably the most convenient and flexible way to initiate an Angular project that works with ABP. Simply [install the ABP CLI](/CLI.md) and run the following command in your terminal:

```sh
abp new MyCompanyName.MyProjectName -u angular
```

> To see further options in the CLI, please visit the [CLI manual](/CLI.md).

This command will prepare a workspace with an angular and an .NET Core project. Please visit [Getting Started section](/Getting-Started.md?UI=NG&DB=EF&Tiered=No#abp-cli-commands-options) for further instructions on how to setup the backend of your project.

To continue reading without checking other methods, visit [project structure section](#angular-project-structure).

### 2. Direct Download

You may [download project scaffold directly on ABP.io](https://abp.io/get-started) if you are more comfortable with GUI or simply want to try the project without installing the CLI.

Please do the following:

1. Click on "DIRECT DOWNLOAD" tab.
2. Fill the short form about your project information.
3. Click on "Create now" button.

...and a customized download will start in a few seconds.

## Angular Project Structure
