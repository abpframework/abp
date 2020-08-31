# How to add the user entity as a navigation property?

In this post, I'll show you how to add the user as a navigation property in your new entity.

To do this, open the ABP Suite. Create a new entity called `Note`.

![create-note-entity](create-note-entity.jpg)

Then add a string property called `Title`.

![add-simple-property](add-simple-property.jpg)

To be able to add a user navigation, we need to create a user DTO to map from entity. To do this, create a new folder called "Users" in `*.Application.Contracts`  then add a new class called  `AppUserDto` inherited from `IdentityUserDto`.

![create-appuserdto](create-appuserdto.jpg)

Create the mapping for `AppUserDto`.  To do this, open `YourProjectApplicationAutoMapperProfile.cs` and add the below line:

```csharp
CreateMap<AppUser, AppUserDto>().Ignore(x => x.ExtraProperties);
```

![create-mapping](create-mapping.jpg)

Get back to ABP Suite, go to **Navigation Properties** tab. Click **Add Navigation Property** button. Browse  `AppUser.cs` in `*.Domain\Users` folder. Then choose the `Name` item as display property. Browse `AppUserDto.cs` in `*.Contracts\Users` folder. Choose `Users` from Collection Names dropdown.

![add-user-navigation](add-user-navigation.jpg)

That's it! Click **Save and generate** button to create your page. You'll see the following page if there's everything goes well. 

![final-page](final-page.jpg)



Note this example is implemented with ABP Commercial 3.1.0-rc.3. This is a RC version. If you want to install the CLI and Suite RC version follow the next steps:

1- Uninstall the current version of the CLI and install the specific RC version:

```bash
dotnet tool uninstall --global Volo.Abp.Cli && dotnet tool install --global Volo.Abp.Cli --version 3.1.0-rc.3
```

2- Uninstall the current version of the Suite and install the specific RC version:

```bash
dotnet tool uninstall --global Volo.Abp.Suite && dotnet tool install -g Volo.Abp.Suite --version 3.1.0-rc.3 --add-source https://nuget.abp.io/<YOUR-API-KEY>/v3/index.json
```

Don't forget to replace the `<YOUR-API-KEY>` with your own key!