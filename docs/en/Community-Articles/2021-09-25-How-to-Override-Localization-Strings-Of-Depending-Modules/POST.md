# How to override localization strings of depending modules

## Source Code

You can find the source of the example solution used in this article [here](https://github.com/abpframework/abp-samples/tree/master/DocumentationSamples/ExtendLocalizationResource).

## Getting Started

This example is based on the following document
https://docs.abp.io/en/abp/latest/Localization#extending-existing-resource

We will change the default `DisplayName:Abp.Timing.Timezone` and `Description:Abp.Timing.Timezone` of [`AbpTimingResource`](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.Timing/Volo/Abp/Timing/Localization/AbpTimingResource.cs) and add localized text in [Russian language(`ru`)](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.Timing/Volo/Abp/Timing/Localization/en.json).

I created the `AbpTiming` folder in the `Localization` directory of the `ExtendLocalizationResource.Domain.Shared` project.

Create `en.json` and `ru.json` in its directory.

`en.json`
```json
{
  "culture": "en",
  "texts": {
    "DisplayName:Abp.Timing.Timezone": "My Time zone",
    "Description:Abp.Timing.Timezone": "My Application time zone"
  }
}
```

`ru.json`
```json
{
  "culture": "ru",
  "texts": {
    "DisplayName:Abp.Timing.Timezone": "Часовой пояс",
    "Description:Abp.Timing.Timezone": "Часовой пояс приложения"
  }
}
```

![](1.png)

We have below content in `ExtendLocalizationResource.Domain.Shared.csproj` file, See [Virtual-File-System](https://docs.abp.io/en/abp/latest/Virtual-File-System#working-with-the-embedded-files) understand how it works.

```xml
<ItemGroup>
    <EmbeddedResource Include="Localization\ExtendLocalizationResource\*.json" />
    <Content Remove="Localization\ExtendLocalizationResource\*.json" />
</ItemGroup>

<ItemGroup>
    <PackageReference Include="Microsoft.Extensions.FileProviders.Embedded" Version="5.0.*" />
</ItemGroup>
```

Change the code of the `ConfigureServices` method in `ExtendLocalizationResourceDomainSharedModule`.

```cs
Configure<AbpLocalizationOptions>(options =>
{
    options.Resources
        .Add<ExtendLocalizationResourceResource>("en")
        .AddBaseTypes(typeof(AbpValidationResource))
        .AddVirtualJson("/Localization/ExtendLocalizationResource");

    //add following code
    options.Resources
        .Get<AbpTimingResource>()
        .AddVirtualJson("/Localization/AbpTiming");

    options.DefaultResourceType = typeof(ExtendLocalizationResourceResource);
});
```

Execute `ExtendLocalizationResource.DbMigrator` to migrate the database and run `ExtendLocalizationResource.Web`.

We have changed the English localization text and added Russian localization.

### Index page

```cs
<p>@AbpTimingResource["DisplayName:Abp.Timing.Timezone"]</p>
@using(CultureHelper.Use("ru"))
{
    <p>@AbpTimingResource["DisplayName:Abp.Timing.Timezone"]</p>
}
```

![](2.png)