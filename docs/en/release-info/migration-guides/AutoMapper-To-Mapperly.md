# Migrating from AutoMapper to Mapperly

## Introduction

The AutoMapper library is **no longer free for commercial use**. For more details, you can refer to [this announcement post](https://www.jimmybogard.com/automapper-and-mediatr-going-commercial/).

ABP Framework provides both AutoMapper and Mapperly integrations. If your project currently uses AutoMapper and you don't have a commercial license, you can switch to Mapperly by following the steps outlined below.

## Migration Steps

Please open your project in an IDE(`Visual Studio`, `VS Code` or `JetBrains Rider`), then perform the following global search and replace operations:

1. Replace `Volo.Abp.AutoMapper` with `Volo.Abp.Mapperly` in all `*.csproj` files.
2. Replace `using Volo.Abp.AutoMapper;` with `using Volo.Abp.Mapperly;` in all `*.cs` files.
3. Replace `AbpAutoMapperModule` with `AbpMapperlyModule` in all `*.cs` files.
4. Replace `AddAutoMapperObjectMapper` with `AddMapperlyObjectMapper` in all `*.cs` files.
5. Remove any code sections that configure `Configure<AbpAutoMapperOptions>`.
6. Review any existing AutoMapper `Profile` classes and ensure all newly created Mapperly mapping classes are registered appropriately. (You can refer to the example below for guidance)

**Example:**

Here is an AutoMapper's `Profile` class:

```csharp
public class ExampleAutoMapper : Profile
{
    public AbpIdentityApplicationModuleAutoMapperProfile()
    {
        CreateMap<IdentityUser, IdentityUserDto>()
            .MapExtraProperties()
            .Ignore(x => x.IsLockedOut)
            .Ignore(x => x.SupportTwoFactor)
            .Ignore(x => x.RoleNames);

        CreateMap<IdentityUserClaim, IdentityUserClaimDto>();

        CreateMap<OrganizationUnit, OrganizationUnitDto>()
            .MapExtraProperties();

		CreateMap<OrganizationUnitRole, OrganizationUnitRoleDto>()
			.ReverseMap();

        CreateMap<IdentityRole, OrganizationUnitRoleDto>()
            .ForMember(dest => dest.RoleId, src => src.MapFrom(r => r.Id));

        CreateMap<IdentityUser, IdentityUserExportDto>()
            .ForMember(dest => dest.Active, src => src.MapFrom(r => r.IsActive ? "Yes" : "No"))
            .ForMember(dest => dest.EmailConfirmed, src => src.MapFrom(r => r.EmailConfirmed ? "Yes" : "No"))
            .ForMember(dest => dest.TwoFactorEnabled, src => src.MapFrom(r => r.TwoFactorEnabled ? "Yes" : "No"))
            .ForMember(dest => dest.AccountLookout, src => src.MapFrom(r => r.LockoutEnd != null && r.LockoutEnd > DateTime.UtcNow ? "Yes" : "No"))
            .Ignore(x => x.Roles);
    }
}
```

And the Mapperly mapping class:

```csharp
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentityUserToIdentityUserDtoMapper : MapperBase<IdentityUser, IdentityUserDto>
{
    [MapperIgnoreTarget(nameof(IdentityUserDto.IsLockedOut))]
    [MapperIgnoreTarget(nameof(IdentityUserDto.SupportTwoFactor))]
    [MapperIgnoreTarget(nameof(IdentityUserDto.RoleNames))]
    public override partial IdentityUserDto Map(IdentityUser source);

    [MapperIgnoreTarget(nameof(IdentityUserDto.IsLockedOut))]
    [MapperIgnoreTarget(nameof(IdentityUserDto.SupportTwoFactor))]
    [MapperIgnoreTarget(nameof(IdentityUserDto.RoleNames))]
    public override partial void Map(IdentityUser source, IdentityUserDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityUserClaimToIdentityUserClaimDtoMapper : MapperBase<IdentityUserClaim, IdentityUserClaimDto>
{
    public override partial IdentityUserClaimDto Map(IdentityUserClaim source);

    public override partial void Map(IdentityUserClaim source, IdentityUserClaimDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class OrganizationUnitToOrganizationUnitDtoMapper : MapperBase<OrganizationUnit, OrganizationUnitDto>
{
    public override partial OrganizationUnitDto Map(OrganizationUnit source);
    public override partial void Map(OrganizationUnit source, OrganizationUnitDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OrganizationUnitRoleToOrganizationUnitRoleDtoMapper : TwoWayMapperBase<OrganizationUnitRole, OrganizationUnitRoleDto>
{
    public override partial OrganizationUnitRoleDto Map(OrganizationUnitRole source);
    public override partial void Map(OrganizationUnitRole source, OrganizationUnitRoleDto destination);

    public override partial OrganizationUnitRole ReverseMap(OrganizationUnitRoleDto destination);
    public override partial void ReverseMap(OrganizationUnitRoleDto destination, OrganizationUnitRole source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class OrganizationUnitToOrganizationUnitWithDetailsDtoMapper : MapperBase<OrganizationUnit, OrganizationUnitWithDetailsDto>
{
    [MapperIgnoreTarget(nameof(OrganizationUnitWithDetailsDto.Roles))]
    [MapperIgnoreTarget(nameof(OrganizationUnitWithDetailsDto.UserCount))]
    public override partial OrganizationUnitWithDetailsDto Map(OrganizationUnit source);

    [MapperIgnoreTarget(nameof(OrganizationUnitWithDetailsDto.Roles))]
    [MapperIgnoreTarget(nameof(OrganizationUnitWithDetailsDto.UserCount))]
    public override partial void Map(OrganizationUnit source, OrganizationUnitWithDetailsDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityRoleToOrganizationUnitRoleDtoMapper : MapperBase<IdentityRole, OrganizationUnitRoleDto>
{
    [MapProperty(nameof(IdentityRole.Id), nameof(OrganizationUnitRoleDto.RoleId))]
    public override partial OrganizationUnitRoleDto Map(IdentityRole source);

    [MapProperty(nameof(IdentityRole.Id), nameof(OrganizationUnitRoleDto.RoleId))]
    public override partial void Map(IdentityRole source, OrganizationUnitRoleDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityUserToIdentityUserExportDtoMapper : MapperBase<IdentityUser, IdentityUserExportDto>
{
    [MapperIgnoreTarget(nameof(IdentityUserExportDto.Roles))]
    public override partial IdentityUserExportDto Map(IdentityUser source);

    [MapperIgnoreTarget(nameof(IdentityUserExportDto.Roles))]
    public override partial void Map(IdentityUser source, IdentityUserExportDto destination);

    public override void AfterMap(IdentityUser source, IdentityUserExportDto destination)
    {
        destination.Active = source.IsActive ? "Yes" : "No";
        destination.EmailConfirmed = source.EmailConfirmed ? "Yes" : "No";
        destination.TwoFactorEnabled = source.TwoFactorEnabled ? "Yes" : "No";
        destination.AccountLookout = source.LockoutEnd != null && source.LockoutEnd > DateTime.UtcNow ? "Yes" : "No";
    }
}
```

## Mapperly Mapping Class

To use Mapperly, you'll need to create a dedicated mapping class for each source and destination types.

* Use the `[Mapper]` attribute to designate the class as a Mapperly mapper. The `RequiredMappingStrategy` is set to `Target` by default.
* Replace AutoMapper's `Ignore()` method with the `[MapperIgnoreTarget]` attribute.
* Replace the `MapExtraProperties()` method with the `[MapExtraProperties]` attribute.
* Use the `TwoWayMapperBase` class as an alternative to AutoMapper’s `ReverseMap()` functionality.
* Implement the `AfterMap()` method to execute logic after the mapping is completed.

### Dependency Injection in Mapper Class

All Mapperly mapping classes automatically registered in the [dependency injection (DI)](../../framework/fundamentals/dependency-injection.md) container. To use a service within a Mapper class, simply add it to the constructor; Mapperly will inject it automatically.

**Example:**

```csharp
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityUserToIdentityUserDtoMapper : MapperBase<IdentityUser, IdentityUserDto>
{
    public IdentityUserToIdentityUserDtoMapper(MyService myService)
    {
        _myService = myService;
    }

    public override partial IdentityUserDto Map(IdentityUser source);
    public override partial void Map(IdentityUser source, IdentityUserDto destination);

    public override void AfterMap(IdentityUser source, IdentityUserDto destination)
    {
        destination.MyProperty = _myService.GetMyProperty(source.MyProperty);
    }
}
```

## AI Prompt for Migrating AutoMapper to Mapperly

If you have AI tools like Cursor, you can use the following prompt to migrate your AutoMapper mappings to Mapperly automatically:

> AI may generate some code that is not correct. Please check the code carefully.

```
Please help me migrate AutoMapper Profile classes to Mapperly. I have AutoMapper Profile files in my current workspace/context that need to be converted. 

**Conversion Requirements:**

1. **Convert AutoMapper Profile to Mapperly Mappers**: Transform each `CreateMap` into a separate Mapperly mapper class
2. **Rename the file**: Change from `XXXAutoMapperProfile.cs` to `XXXMappers.cs`
3. **Use proper Mapperly attributes**: 
   - `[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]` for each mapper class
   - `[MapExtraProperties]` for classes that need extra properties mapping
   - `[MapperIgnoreTarget]` for ignored properties
   - `[MapProperty]` for custom property mappings
4. **Inherit from appropriate base classes**:
   - `MapperBase<TSource, TDestination>` for one-way mapping
   - `TwoWayMapperBase<TSource, TDestination>` for reverse mapping
5. **Handle complex mappings**: Use `AfterMap` method for complex transformations

**Note:** The code below contains two parts - both are reference examples for you to understand the conversion pattern:
1. **AutoMapper Profile example** - shows the original AutoMapper code structure
2. **Mapperly Mappers example** - shows the expected converted Mapperly code structure

Please convert the actual AutoMapper Profile files that exist in your current context/workspace, following the same conversion pattern as shown in these examples.

**Reference Examples:**

**1. AutoMapper Profile (original code):**

using System;
using AutoMapper;
using System.Linq;
using Volo.Abp.AutoMapper;

namespace Volo.Abp.Identity;

public class ExampleAutoMapperProfile : Profile
{
    public ExampleAutoMapperProfile()
    {
        CreateMap<IdentityUser, IdentityUserDto>()
            .MapExtraProperties()
            .Ignore(x => x.IsLockedOut)
            .Ignore(x => x.SupportTwoFactor)
            .Ignore(x => x.RoleNames);

        CreateMap<IdentityUserClaim, IdentityUserClaimDto>();

        CreateMap<OrganizationUnit, OrganizationUnitDto>()
            .MapExtraProperties();

        CreateMap<OrganizationUnitRole, OrganizationUnitRoleDto>()
            .ReverseMap();

        CreateMap<IdentityRole, OrganizationUnitRoleDto>()
            .ForMember(dest => dest.RoleId, src => src.MapFrom(r => r.Id));

        CreateMap<IdentityUser, IdentityUserExportDto>()
            .ForMember(dest => dest.Active, src => src.MapFrom(r => r.IsActive ? "Yes" : "No"))
            .ForMember(dest => dest.EmailConfirmed, src => src.MapFrom(r => r.EmailConfirmed ? "Yes" : "No"))
            .ForMember(dest => dest.TwoFactorEnabled, src => src.MapFrom(r => r.TwoFactorEnabled ? "Yes" : "No"))
            .ForMember(dest => dest.AccountLookout, src => src.MapFrom(r => r.LockoutEnd != null && r.LockoutEnd > DateTime.UtcNow ? "Yes" : "No"))
            .Ignore(x => x.Roles);
    }
}

---

**2. Mapperly Mappers (converted code):**

using System;
using System.Linq;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Volo.Abp.Identity;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentityUserToIdentityUserDtoMapper : MapperBase<IdentityUser, IdentityUserDto>
{
    [MapperIgnoreTarget(nameof(IdentityUserDto.IsLockedOut))]
    [MapperIgnoreTarget(nameof(IdentityUserDto.SupportTwoFactor))]
    [MapperIgnoreTarget(nameof(IdentityUserDto.RoleNames))]
    public override partial IdentityUserDto Map(IdentityUser source);

    [MapperIgnoreTarget(nameof(IdentityUserDto.IsLockedOut))]
    [MapperIgnoreTarget(nameof(IdentityUserDto.SupportTwoFactor))]
    [MapperIgnoreTarget(nameof(IdentityUserDto.RoleNames))]
    public override partial void Map(IdentityUser source, IdentityUserDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityUserClaimToIdentityUserClaimDtoMapper : MapperBase<IdentityUserClaim, IdentityUserClaimDto>
{
    public override partial IdentityUserClaimDto Map(IdentityUserClaim source);

    public override partial void Map(IdentityUserClaim source, IdentityUserClaimDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class OrganizationUnitToOrganizationUnitDtoMapper : MapperBase<OrganizationUnit, OrganizationUnitDto>
{
    public override partial OrganizationUnitDto Map(OrganizationUnit source);
    public override partial void Map(OrganizationUnit source, OrganizationUnitDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OrganizationUnitRoleToOrganizationUnitRoleDtoMapper : TwoWayMapperBase<OrganizationUnitRole, OrganizationUnitRoleDto>
{
    public override partial OrganizationUnitRoleDto Map(OrganizationUnitRole source);
    public override partial void Map(OrganizationUnitRole source, OrganizationUnitRoleDto destination);

    public override partial OrganizationUnitRole ReverseMap(OrganizationUnitRoleDto destination);
    public override partial void ReverseMap(OrganizationUnitRoleDto destination, OrganizationUnitRole source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class OrganizationUnitToOrganizationUnitWithDetailsDtoMapper : MapperBase<OrganizationUnit, OrganizationUnitWithDetailsDto>
{
    [MapperIgnoreTarget(nameof(OrganizationUnitWithDetailsDto.Roles))]
    [MapperIgnoreTarget(nameof(OrganizationUnitWithDetailsDto.UserCount))]
    public override partial OrganizationUnitWithDetailsDto Map(OrganizationUnit source);

    [MapperIgnoreTarget(nameof(OrganizationUnitWithDetailsDto.Roles))]
    [MapperIgnoreTarget(nameof(OrganizationUnitWithDetailsDto.UserCount))]
    public override partial void Map(OrganizationUnit source, OrganizationUnitWithDetailsDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityRoleToOrganizationUnitRoleDtoMapper : MapperBase<IdentityRole, OrganizationUnitRoleDto>
{
    [MapProperty(nameof(IdentityRole.Id), nameof(OrganizationUnitRoleDto.RoleId))]
    public override partial OrganizationUnitRoleDto Map(IdentityRole source);

    [MapProperty(nameof(IdentityRole.Id), nameof(OrganizationUnitRoleDto.RoleId))]
    public override partial void Map(IdentityRole source, OrganizationUnitRoleDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityUserToIdentityUserExportDtoMapper : MapperBase<IdentityUser, IdentityUserExportDto>
{
    [MapperIgnoreTarget(nameof(IdentityUserExportDto.Roles))]
    public override partial IdentityUserExportDto Map(IdentityUser source);

    [MapperIgnoreTarget(nameof(IdentityUserExportDto.Roles))]
    public override partial void Map(IdentityUser source, IdentityUserExportDto destination);

    public override void AfterMap(IdentityUser source, IdentityUserExportDto destination)
    {
        destination.Active = source.IsActive ? "Yes" : "No";
        destination.EmailConfirmed = source.EmailConfirmed ? "Yes" : "No";
        destination.TwoFactorEnabled = source.TwoFactorEnabled ? "Yes" : "No";
        destination.AccountLookout = source.LockoutEnd != null && source.LockoutEnd > DateTime.UtcNow ? "Yes" : "No";
    }
}
```

## Mapperly Documentation

Please refer to the [Mapperly documentation](https://mapperly.riok.app/docs/intro/) for more details.

**Key points:**

- [Mapperly Configuration](https://mapperly.riok.app/docs/configuration/mapper/)
- [Mapperly Enums](https://mapperly.riok.app/docs/configuration/enum/)
- [Mapperly Flattening and unflattening](https://mapperly.riok.app/docs/configuration/flattening/)


## Set Default Mapping Provider

When your project contains modules using both AutoMapper and Mapperly, you may need to explicitly set the default `IAutoObjectMappingProvider` to ensure consistent behavior across your application.

If your application uses `AutoMapper`:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAutoMapperObjectMapper();
}
```

If your application uses `Mapperly`:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddMapperlyObjectMapper();
}
```

### Why Set Default Mapping Provider?

When your project contains modules using both AutoMapper and Mapperly, both `AbpAutoMapperModule` and `AbpMapperlyModule` will be loaded. Their dependency order may vary based on your project's module structure, and the last loaded module will override the `IAutoObjectMappingProvider` implementation. This could lead to unexpected behavior. Setting an explicit default ensures predictable mapping behavior throughout your application.
