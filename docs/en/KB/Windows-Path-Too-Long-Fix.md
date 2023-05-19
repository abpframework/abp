# How to Fix "Filename too long" Error on Windows

If you encounter the "filename too long" or "unzip" error on Windows, it's probably related to the Windows maximum file path limitation. Windows has a maximum file path limitation of 250 characters. To solve this, [enable the long path option in Windows 10](https://docs.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=cmd#enable-long-paths-in-windows-10-version-1607-and-later).

If you face long path errors related to Git, try the following command to enable long paths in Windows.
```
git config --system core.longpaths true
```

See https://github.com/msysgit/msysgit/wiki/Git-cannot-create-a-file-or-directory-with-a-long-path