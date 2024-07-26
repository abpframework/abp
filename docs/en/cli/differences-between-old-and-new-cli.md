# Old ABP CLI vs New ABP CLI

ABP CLI (Command Line Interface) is a command line tool to perform some common operations for ABP based solutions or ABP Studio features. With **v8.2+**, the old/legacy ABP CLI has been replaced with a new [CLI](index.md) system to align with the new templating system and [ABP Studio](../studio/index.md). Also, some superior features/commands have been introduced with the new CLI, such as `kube-connect` and `kube-intercept` commands.

In this guide, you will learn the motivation behind this change, some questions that you may have, how to use the old/legacy CLI, its features, and more...

## Reason For The Change

With v8.2+, ABP introduces a new templating system, which is fully compatible with the ABP Studio. 

Since [ABP Studio](../studio/index.md) offers more and better features (like running, browsing, monitoring, tracing, and deploying your applications), and the new templating system has a different versioning structure, we wanted to introduce a new CLI by expanding the current features and adding even more features that are compatible with the new templating system.

This change allows you to create your application with the new templating system either by running the cross-platform ABP Studio application or ABP CLI. Also, allows you to create automated pipelines with the power of the new CLI.

## Using The New ABP CLI

> If you installed [ABP Studio](../studio/index.md) recently, then you may skip this section because ABP Studio automatically uninstalls the old CLI and replaces it with the new CLI. Therefore, you don't need to manually switch to the new ABP CLI.

> 🛈 The new ABP CLI is in the beta version for now. If you have any issues, you can always use the old ABP CLI by following the instructions in the _Using the old ABP CLI_ section below.

ABP CLI has two variations, which are `Volo.Abp.Studio.Cli` (new) and `Volo.Abp.Cli` (old). If you are using ABP earlier than v8.2+, then you are probably using the old ABP CLI and can easily switch to the new CLI by simply uninstalling the old one and installing the new CLI by executing the commands below:

```bash
# uninstalling the old CLI
dotnet tool uninstall -g Volo.Abp.Cli

# installing the new CLI
dotnet tool install -g Volo.Abp.Studio.Cli
```

> **Note:** If you try to install the new CLI before uninstalling the old one, you will get an error such as **`Failed to create shell shim for tool 'volo.abp.studio.cli': Command 'abp' conflicts with an existing command from another tool.`**. This is because both old and new CLI binary names use the same `abp` command as the executing command.

If you start using ABP from v8.2+, then you can directly install the new CLI with the following command:

```bash
dotnet tool install -g Volo.Abp.Studio.Cli
```

After installing the new CLI, you will be able to use the CLI commands described [in the documentation](index.md) (with the `abp` command), or if you prefer GUI, you can always use the [ABP Studio](../studio/index.md).

## Using The Old ABP CLI

If you have an older version of ABP solutions and need to use old ABP CLI, you can easily put the `--old` command at the end of your command and execute the related CLI command. This allows you to use the old CLI commands with the new CLI without need to uninstall the new CLI.

For example, if you want to create a new ABP v8.1.4 solution, you can use the following command:

```bash
abp new Acme.BookStore --version 8.1.4 --old
```

> **Note:** `--old` parameter **must be at the end of your command** for running the command with the old CLI. Otherwise, it will be ignored!

Note that, when you use the `--old` parameter the latest version of ABP will be used as the CLI version. If you want to use a specific version of the old ABP CLI, you can use the `install-old-cli` command to install/update an old CLI and then use it with the `--old` parameter as always. For example, to use the old CLI in v8.0 and create a v8.0 template you can run the following commands one after another:

```bash
# installing the old ABP CLI with v8.0
abp install-old-cli --version 8.0.0

# creating a new solution with v8.0 template and cli version
abp new Acme.BookStore --version 8.0 --old # or you can use `abp-old new Acme.BookStore` command
```

Alternatively, you can uninstall the new CLI and install the old CLI to use the old ABP CLI. However, with the `--old` parameter, you don't need to do that and instead you can easily use the old CLI by just passing the `--old` parameter to the end of your command.

> It is recommended that you use the `--old` parameter to use the old ABP CLI. However, if you have any problems, you can always remove the new CLI and install the old CLI as before.

## Common Questions

In this section, you can see the common questions with the new CLI changes and their answers:

1. **Can I create a new project with an older ABP version with the new ABP CLI?**

Yes, you can. You just need to type the command, put the `--old` command at the end of your command, and execute it. For example, if you want to create an application in v8.0, you can use the following command and pass the template version:

```bash
abp new Acme.BookStore --version 8.0.0 --old
```

2. **How to use the old ABP CLI with a specific version?**

To use the old ABP CLI within a specific version, you first need to install the old cli with the new CLI's `install-old-cli` command by passing the version and then either put the `--old` parameter to the end of your command or use the `abp-old` command. For example, to use the old CLI in v7.4 and create a v7.4 template you can run the following commands one after another:

```bash
# installing the old ABP CLI with v7.4
abp install-old-cli --version 7.4.0

# creating a new solution with v8.0 template and cli version
abp-old new Acme.BookStore
```

3. **Does the new CLI provide all commands that are already supported by the old CLI?**

The new CLI extends the old CLI and provides most of the commands that are already supported by the old CLI. You can check the [CLI documentation](index.md) to see all available commands.

4. **What happens if I have multiple old CLI versions installed with the `install-old-cli` commands? Which version would be used?**

You can see all available `abp-old` command versions under the **%UserProfile%\\.abp\studio\cli\old** folder and when you execute the `abp-old` command, you can learn which old CLI version will be used when you execute an ABP command with the old CLI (by either running the `abp-old` command or passing the `--old` parameter as the last argument for your command):

```bash
C:\Users\user-name>abp-old
ABP CLI 8.1.4 # -> v8.1.4

Usage:

    abp <command> <target> [options]

Command List:
    //...
```

You can update the old CLI version with the `install-old-cli` command or safely delete the **%UserProfile%\\.abp\studio\cli\old** folder and install any old CLI version you want to use.

5. **What are the new commands that came with the new ABP CLI?**

New ABP CLI extends the existing features of old ABP CLI and also introduces new commands. Here are some of them:

* `kube-connect`: Connects to Kubernetes environment. (Available for **Business** or higher licenses)
* `kube-intercept`: Intercepts a service running in the Kubernetes environment. (Available for **Business** or higher licenses)
* `list-module-sources`: Lists the remote module sources.
* ...

Please refer to the [CLI documentation](index.md) for all available commands and their usages.

## See Also

- [CLI](index.md)
