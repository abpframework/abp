# OLD vs NEW ABP CLI

ABP CLI (Command Line Interface) is a command line tool to perform some common operations for ABP based solutions or ABP Studio features. With **v8.2+**, old/legacy ABP CLI has been replaced with a new [CLI](index.md) system to align with the new templating system and [ABP Studio](../studio/index.md). Also, some superior features/commands have been introduced with the new CLI, such as `kube-connect` and `kube-intercept`.

In this guide, you will learn the motivation behind this change, the common questions that you may wonder, how to use the old/legacy CLI, its features and more...

## Reason for the change

With v8.2+, ABP introduces a new templating system, which is fully compatible with the ABP Studio. Since, the new templating system have a different versioning structure and ABP Studio provides additional and superior features (such as allows you to run, browse, monitor, trace and deploy your applications), we wanted to introduce a new CLI with extending the current features and introduce even more features those are compatible with the new templating system.

This change allows you to create your application with new templating system in either by running the cross-platform ABP Studio application or ABP CLI. Also, allows you to create automated pipelines with the power of the new CLI.

## Using the new ABP CLI

> If you installed the [ABP Studio](../studio/index.md) recently, then you may skip this section, because ABP Studio automatically uninstall the old CLI and replate it with the new CLI. Therefore, you don't need to manually switch to the new ABP CLI.

> 🛈 New ABP CLI is in the beta version now. If you have any issues, you can always use the old ABP CLI by following the instructions in the _Using the old ABP CLI_ section below.

ABP CLI has two variations, which are `Volo.Abp.Studio.Cli` (new) and `Volo.Abp.Cli` (old). If you are using ABP earlier than v8.2+, then you are probably using the old ABP CLI and easily switch to the new CLI by simply uninstalling the old one and installing the new CLI by executing the commands below:

```bash
# uninstalling the old CLI
dotnet tool uninstall -g Volo.Abp.Cli

# installing the new CLI
dotnet tool install -g Volo.Abp.Studio.Cli
```

> **Note:** If you try to install the new CLI before uninstalling the old one, you will get an error such as **`Failed to create shell shim for tool 'volo.abp.studio.cli': Command 'abp' conflicts with an existing command from another tool.`**. This is caused because the both old and new CLI's use the same `abp` command as the executing command, therefore it's required to use only one of them.

If you start using ABP from v8.2+, then you can directly install the new CLI with the following command:

```bash
dotnet tool install -g Volo.Abp.Studio.Cli
```

After installing the new CLI, you will be able use the CLI commands described [in the documentation](index.md) (with the `abp` command) or if you prefer GUI, you can always use the [ABP Studio](../studio/index.md).

## Using the old ABP CLI

If you have an older version of ABP solutions and need to use old ABP CLI, you can easily put the `--old` command end of you command and execute the related CLI command. This allows you to use the old CLI commands with the new CLI without need to uninstall the new CLI.

For example, if you want to create a new ABP v8.1.4 solution, you can use the following command:

```bash
abp new Acme.BookStore --version 8.1.4 --old
```

> **Note:** `--old` parameter **must be at the end of your command** for running the command with the old CLI. Otherwise, it will be ignored!

Note that, when you use the `--old` parameter the latest version of ABP will be used as the CLI version. If you want to use a specific version of old ABP CLI, you can use the `install-old-cli` command to install/update an old CLI and then use it with the `--old` parameter as always. For example, to use the old CLI in v8.0 and create a v8.0 template you can run the following commands one after another:

```bash
# installing the old ABP CLI with v8.0
abp install-old-cli --version 8.0.0

# creating a new solution with v8.0 template and cli version
abp new Acme.BookStore --version 8.0 --old # or you can use `abp-old new Acme.BookStore` command
```

Alternatively, you can uninstall the new CLI and install the old CLI to use the old ABP CLI. However, with the `--old` parameter, you don't need to do that and instead you can easily use the old CLI by just passing the `--old` parameter to end of your command.

> Using the `--old` parameter is recommended to use old ABP CLI. However, if you had any problem, you can always remove the new CLI and install the old CLI and use the old CLI.

## Common questions

In this section, you can see the common questions with the new CLI changes and their answers:

1. **Can I create a new project with an older ABP version with the new ABP CLI?**

Yes, you can. You just need to type the command and put the `--old` command in the end of your command and execute it. For example, if you want to create an application in v8.0, you can use the following command and pass the template version:

```bash
abp new Acme.BookStore --version 8.0.0 --old
```

2. **How to use the old ABP CLI with a specific version?**

To use the old ABP CLI within a specific version, you first need to install the old-cli with the new CLI's `install-old-cli` command by passing the version and then either put the `--old` parameter to end of to your command or use the `abp-old` command. For example, to use the old CLI in v7.4 and create a v7.4 template you can run the following commands one after another:

```bash
# installing the old ABP CLI with v7.4
abp install-old-cli --version 7.4.0

# creating a new solution with v8.0 template and cli version
abp-old new Acme.BookStore
```

3. **Does the new CLI provide all commands that is already supported by the old CLI?**

The new CLI extends the old CLI and provide most of the commands those are already supported by the old CLI. You can check the [CLI documentation](index.md) to see all available commands.

4. **What happens if I have multiple old CLI versions installed with the `install-old-cli` commands? Which version would be used?**

You can see the all available `abp-old` command versions under the **%UserProfile%\.abp\studio\cli\old** folder and when you execute the `abp-old` command, you can learn which old CLI version will be used when you execute an ABP command with the old CLI (by either running the `abp-old` command or passing the `--old` parameter as the last argument for your command):

```bash
C:\Users\user-name>abp-old
ABP CLI 8.1.4

Usage:

    abp <command> <target> [options]

Command List:
    //...
```

You can update old CLI version with the `install-old-cli` command or safely delete the **%UserProfile%\.abp\studio\cli\old** folder and install any old CLI version you want to use.

5. **What are the new commands came with new ABP CLI?**

New ABP CLI extends the existing features or old ABP CLI and also introduces new commands. Here are some of them:

* `kube-connect`: Connects to kubernetes environment. (Available for **Business** or higher licenses)
* `kube-intercept`: Intercepts a service running in Kubernetes environment. (Available for **Business** or higher licenses)
* `list-module-sources`: Lists the remote module sources.
* ...

Please refer to the [CLI documentation](index.md) for all available commands and their usages.

## See Also

- [CLI](index.md)
