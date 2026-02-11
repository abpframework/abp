# Installation Notes for Feature Management Module

The Feature Management module provides a way to define and manage features in an ABP application. Features are used to enable or disable specific functionalities of an application based on different conditions, such as tenant subscription levels or user preferences.

Key capabilities of the Feature Management module:
- Define features with different value types (boolean, numeric, etc.)
- Group features by providers (tenant, edition, etc.)
- Manage feature values through a user interface
- Check feature status in your application code

## NuGet Packages

The following NuGet packages are required for the Feature Management module:
- `Volo.Abp.FeatureManagement.Application`
- `Volo.Abp.FeatureManagement.HttpApi`
- `Volo.Abp.FeatureManagement.EntityFrameworkCore` (for EF Core)
- `Volo.Abp.FeatureManagement.MongoDB` (for MongoDB)
- `Volo.Abp.FeatureManagement.Web` (for MVC UI)
- `Volo.Abp.FeatureManagement.Blazor.Server` (for Blazor Server UI)
- `Volo.Abp.FeatureManagement.Blazor.WebAssembly` (for Blazor WebAssembly UI)

## Documentation

For detailed information and usage instructions, please visit the [Feature Management Module documentation](https://abp.io/docs/latest/Modules/Feature-Management). 