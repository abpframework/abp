# How to install ABP Suite?

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Overall",
    "Path": "suite/index"
  },
  "Next": {
    "Name": "How to start ABP Suite?",
    "Path": "suite/how-to-start"
  }
}
````

If you are using [ABP Studio](../studio/index.md), then you don't need to install the ABP Suite because it should already have been installed, when you first installed the [ABP Studio](../studio/index.md). Otherwise, please apply the following steps to install it properly:

1. First of all, [ABP CLI](../cli/index.md) must be installed on the computer. If it's not already installed, [click here to learn how to install ABP CLI](../cli/index.md#installation). 

2. Make sure you have logged in to abp.io via ABP CLI. If you are not logged in, [click here to see how to login](../cli/index.md#login).

3. To install ABP Suite write the following command to your terminal:

   ```bash
   abp suite install
   ```

Wait for the process to download and install to your computer. It may take about a minute on a 25 Megabit internet to complete the process.

## How to update ABP Suite?

To update your existing ABP Suite, write the following command to your terminal:

```bash
abp suite update
```

## Preview versions

To install/update the latest preview version of ABP Suite, add `--preview` or `-p` parameter to the command arguments:

```bash
abp suite install --preview
abp suite update --p
```


## Older versions

To install a specific version of ABP Suite, add `--version` or `-v` parameter to the command arguments:

```bash
abp suite install --version 3.3.1
abp suite update --v 4.0.0-rc.5
```

You can install older or newer versions. The version of ABP Suite and your solution's ABP package versions must be the same. Otherwise you will get errors due to templates not suitable for your project. 