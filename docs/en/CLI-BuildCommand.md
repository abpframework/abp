# Build Command

Building a .NET project is hard when the project references a project reference outside of the solution or even from a different GIT repository. This command builds a GIT repository and it's depending repositories or a single .NET solution File. In order ```build``` command to work, its **executing directory** or passed ```--working-directory``` parameter's directory must contain one of;

* A .NET solution file (*.sln)
* abp-build-config.json

When the executing directory  (or ```--working-directory``` parameter's directory) contains a .NET solution file, ```build``` command builds all the projects in the related solution file and all project references on building project files recursively. 

When the executing directory  (or ```--working-directory``` parameter's directory) contains a ```abp-build-config.json```, ```build``` command builds all changed projects form its last build and all project references on building project files recursively. 

# Build Command Config

```abp-build-config.json``` contains properties below;

* ```Name```: Name of the GIT repository. This can be friendly name of your GIT repository or any other unique string for the repository.
* ```RootPath```: Root path of the repository which contains ```.git``` folder.
* ```DependingRepositories```: Depending repository list of a repository. Each depending repository item contains same fields as a repository.
* ```IgnoredDirectories```: Relative directory paths to ignore while building a GIT repository.

A sample ```abp-build-config.json``` looks like for a Windows OS;

````json
{
    "Name": "main-repository",
    "RootPath": "D:\\GitHub\\main-repository",
    "DependingRepositories": [{
        "Name": "module-repository",
        "RootPath": "D:\\GitHub\\module-repository"
    }],
    "IgnoredDirectories": [
        "utils"
    ]
}
````

# Build Status

ABP CLI stores a build status file for builds using the repository friendly names and current branch names in the;

* ```%USERPROFILE%\.abp\build\``` for Windows.
* ```$HOME/.abp/build/``` for Linux/macOS.

and uses this file when building same repository next time and only builds affected projects and decreases the total build time. A sample build status file content looks like;

````json
{
    "RepositoryName": "main-repository",
    "BranchName": "dev",
    "CommitId": "84ecde8ba275aeeb14d24a87ad46a1e941adf8ba",
    "SucceedProjects": [{
        "CsProjPath": "D:\\GitHub\\main-repository\\BookStore\\BookStore.Web.csproj",
        "CommitId": "84ecde8ba275aeeb14d24a87ad46a1e941adf8ba"
    }],
    "DependingRepositories": [{
        "RepositoryName": "module-repository",
        "BranchName": "dev",
        "CommitId": "0598b8e45af9507fc9ba8abf304e78fc7d434e04",
        "SucceedProjects": [{
            "CsProjPath": "D:\\GitHub\\module-repository\\identity-module\Identity\\Identity.Web.csproj",
            "CommitId": "0598b8e45af9507fc9ba8abf304e78fc7d434e04"
        }],
        "DependingRepositories": []
    }]
}
````

