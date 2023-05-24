# How to Fix "Filename too long" Error on Windows

If you encounter the "filename too long" or "unzip" error on Windows, it's probably related to the Windows maximum file path limitation. Windows has a maximum file path limitation of 250 characters.

## Solution 1
Try [enabling the long path option in Windows 10](https://docs.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=cmd#enable-long-paths-in-windows-10-version-1607-and-later).

If you face long path errors related to Git, try the following command to enable long paths in Windows.
```
git config --system core.longpaths true
```

See https://github.com/msysgit/msysgit/wiki/Git-cannot-create-a-file-or-directory-with-a-long-path


## Solution 2

Some of the .NET MAUI build tools need to run in 32-bit. This is not an ABP issue but .NET MAUI problem.
If you get "DirectoryNotFoundException - Could not find a part of the path" exception, put the solution in the root directory of your drive. For example `C:\Samples\`

## Solution 3
You can define an alias for a path in Windows by creating a symbolic link using the mklink command in the Command Prompt. Here's an example:

```
mklink /D C:\myprojectname D:\my\long\path\to\solution\
```
> Your **solution (.sln)** file should be in `D:\my\long\path\to\solution\`. If you link to an appliction, relative paths won't work.

This command creates a symbolic link named `myprojectname` in the root of the `C:` drive that points to the `D:\my\long\path\to\solution\` directory. You can then use `C:\myprojectname` to access the contents of the `D:\my\long\path\to\solution\` directory.

> Note that you need to run the Command Prompt as an administrator to create symbolic links.

Then you can try building your app.

```
dotnet build C:\myprojectname\MyProjectName.sln
```