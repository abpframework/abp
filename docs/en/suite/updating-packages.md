# Updating packages

ABP Suite allows you to update `NPM` and `NuGet` packages in a solution.

![Book list page](../images/suite-update-packages.png)

1. **Update ABP packages**:  Updates both `NPM` and `NuGet` packages in the solution. 

2. **Update NPM packages:** Updates only `NPM` packages.

3. **Update NuGet packages:** Updates only `NuGet` packages.

## How it works?

It scans all the `*.csproj` files in the solution and checks the version of the packages starting with the name  `Volo.*`

- For **nightly packages**, it updates the package from https://www.myget.org.
- For **open-source ABP packages**, it updates the package from https://nuget.org.
- For **commercial ABP packages**, it updates the package from https://nuget.abp.io.
