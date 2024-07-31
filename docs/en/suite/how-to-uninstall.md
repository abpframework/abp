# How to uninstall ABP Suite?

You can simply uninstall ABP Suite with the terminal command:

```bash
abp suite remove
```

## How to do a clean uninstallation of ABP Suite?

To cleanly uninstall ABP Suite, follow these steps:

1. Terminate your ABP Suite application.
2. Uninstall ABP Suite via terminal command:

   ```bash
   dotnet tool uninstall -g volo.abp.suite
   ```

3. If exists, delete the application installation folder:

	*Windows:*
   
   ```bash
   %UserProfile%\.dotnet\tools\.store\volo.abp.suite
   ```
   
   *MAC:*
   
   ```bash
   ~/.dotnet/tools/.store/volo.abp.suite
   ```
   
4. If exists, delete the ABP Suite executable file:

	*Windows:*
   
   ```bash
   %UserProfile%\.dotnet\tools\abp-suite.exe
   ```
   
   *MAC:*
   
   ```bash
   ~/.dotnet/tools/abp-suite
   ```
   
5. If exists, delete license file:

   *Windows:*

   ```bash
   %UserProfile%\AppData\Local\Temp\AbpLicense.bin
   ```

   *MAC:*

   ```bash
   $TMPDIR/AbpLicense.bin
   ```
   
6. Delete the access-token file:

   *Windows:*

   ```bash
    %UserProfile%\.abp\cli\access-token.bin
   ```

   *MAC:*

   ```bash
    ~/.abp/cli/access-token.bin
   ```

You have successfully uninstalled ABP Suite!

## Reinstall ABP Suite

If you want to reinstall ABP Suite:

- Make sure you have already installed the ABP CLI (by running the `abp` command on a terminal).

- Login your account via ABP CLI:

  ```bash
  abp login <username>
  ```

- Then, install the ABP Suite with the following command:

  ```bash
  abp suite install
  ```
  
  > If you want to install the preview version add the parameter `--preview`.
  
