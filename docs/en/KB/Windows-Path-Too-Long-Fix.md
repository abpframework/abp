# How to Fix "Filename too long" Error on Windows

If you encounter the "filename too long" or "unzip" error on Windows, it's probably related to the Windows maximum file path limitation. Windows has a maximum file path limitation of 255 characters.

## Solution 1
Try [enabling the long path option in Windows 10](https://docs.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=cmd#enable-long-paths-in-windows-10-version-1607-and-later).

If you face long path errors related to Git, try the following command to enable long paths in Windows.
```
git config --system core.longpaths true
```

See https://github.com/msysgit/msysgit/wiki/Git-cannot-create-a-file-or-directory-with-a-long-path


## Solution 2

You may encounter a "DirectoryNotFoundException - Could not find a part of the path" exception in Windows while using certain .NET MAUI build tools. This is related to some 32 bit .NET MAUI build tools. To resolve this issue, you can try placing the solution in the root directory of your drive, such as `C:\Projects\`. However, please note that this solution is specific to this particular exception and may not be applicable to all cases of the Windows long path issue.


## Solution 3

You can define an alias for a path in Windows by creating a symbolic link using the `mklink` command in the command prompt. Here's an example:

```
mklink /D C:\MyProject C:\my\long\path\to\solution\
```

> Your **solution (.sln)** file should be in `C:\my\long\path\to\solution\`. Keep in mind that, if you have relative paths in your .csproj file, it will not work!

This command creates a symbolic link named `MyProject` in the root of the `C:` drive that points to the `C:\my\long\path\to\solution\` directory. You can then use `C:\MyProject` to access the contents of the `C:\my\long\path\to\solution\` directory.

> Note that you need to run the command prompt as an administrator to the create symbolic links.

Then you can try building your project with `dotnet build` command.

```
dotnet build C:\MyProject\MyProjectName.sln
```
