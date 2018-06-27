# ABP

### Documentation

See the <a href="docs\Index.md" target="_blank">documentation</a>.

### How to Build

- Run the `build-all.ps1`. It will build all the solutions in this repository.

### Development

#### Pre Requirements

- Visual Studio 2017 15.7.0+

#### Framework

Framework solution is located under the `framework` folder. It has no external dependency. Just open `Volo.Abp.sln` by Visual Studio and start the development.

#### Modules/Templates

[Modules](modules/) and [Templates](templates/) have their own solutions and have **local references** to the framework. Unfortunately, Visual Studio has some problems with local references to projects those are out of the solution. As a workaround, you should follow the steps below in order to start developing a module/template:

- Disable "*Automatically check for missing packages during build in Visual Studio*" in the Visual Studio options.

![disable-package-restore-visual-studio](docs/images/disable-package-restore-visual-studio.png)

- When you open a solution, first run `dotnet restore` in the root folder of the solution.
- When you change a dependency of a project (or any of the dependencies of your projects change their dependencies), run `dotnet restore` again.

### Contribution

ABP is an open source platform.

* Open a [new issue](https://github.com/volosoft/volo/issues/new) if you found a bug or if you have a new feature/enhancement idea.
* Open a pull request if you want to make a development. Please create an issue before a development, so we can discuss it.
* Contribute to the [documentation](docs/Index.md).
