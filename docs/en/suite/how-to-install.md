# How to install ABP Suite?

1. First of all, [ABP CLI](https://docs.abp.io/{{Document_Language_Code}}/abp/{{Document_Version}}/CLI) must be installed on the computer. If it's not already installed, [click here to learn how to install ABP CLI](https://docs.abp.io/{{Document_Language_Code}}/abp/{{Document_Version}}/CLI#installation). 

2. Make sure you have logged in to abp.io via ABP CLI. If you are not logged in, [click here to see how to login](https://docs.abp.io/{{Document_Language_Code}}/abp/{{Document_Version}}/CLI#login).

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

To install a specific version of ABP Suite, add `--version` or `-v` parameter to the command arguments. You can install older versions of the ABP Suite. The version of ABP Suite and your solution's ABP package versions must be the same. Otherwise you will get errors due to templates not suitable for your project. 

```bash
abp suite install --version 3.3.1
abp suite update --v 4.0.0-rc.5
```

## What's next?

[How to start ABP Suite?](how-to-start.md)