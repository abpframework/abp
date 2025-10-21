```json
//[doc-seo]
{
    "Description": "Learn how to customize your ABP application’s configuration by modifying ConnectionString values across key appsettings.json files."
}
```

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "BlazorWebApp", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Yes", "No"]
}
````

## Step 2: Customizing the Configuration of the ABP Application

#### To customize the configuration of your ABP application

- Modify the `ConnectionString` values in every location throughout your project. The `ConnectionString` values are stored in the `appsettings.json` files.

    * This includes the following files:

    **./src/yourapp.DbMigrator/appsettings.json**
{{ if Tiered == "No" }}
{{ if UI == "MVC" }}
    **./src/yourapp.Web/appsettings.json**
{{ else if UI == "Blazor" || UI == "BlazorServer" || UI == "BlazorWebApp" }}
    and **./src/yourapp.Blazor/appsettings.json**
{{ end }}
{{ if UI == "Blazor"|| UI == "NG" }}
    **./src/yourapp.HttpApi.Host/appsettings.json**
{{ end }}
{{ else }}
    **./src/yourapp.HttpApi.Host/appsettings.json**
    **./src/yourapp.AuthServer/appsettings.json**
{{ end }}

```json
"ConnectionStrings": {
    "Default": "Server=tcp:yourserver.database.windows.net,1433;Initial Catalog=yourdatabase;Persist Security Info=False;User ID=yourusername;Password=yourpassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}
```


- Modify the **yourapp.Web** URL in every location throughout your project, especially within the **./src/yourapp.Web/appsettings.json** and **./src/yourapp.DbMigrator/appsettings.json** files, to match your Azure Web App Service URL.

    * This includes the following files:
    **./src/yourapp.DbMigrator/appsettings.json**
{{ if Tiered == "No" }}
{{ if UI == "MVC" }}
    **./src/yourapp.Web/appsettings.json**
{{ else if UI == "Blazor" || UI == "BlazorServer" || UI == "BlazorWebApp" }}
    and **./src/yourapp.Blazor/appsettings.json**
{{ else }}
    Modify the **`localhost:4200`** in every location throughout your project.
    **./angular/src/environments/environment.prod.ts** 
{{ end }}
{{ if UI == "Blazor"|| UI == "NG" }}
    **./src/yourapp.HttpApi.Host/appsettings.json**
{{ end }}
{{ else }}
{{ if UI == "MVC" }}
    **./src/yourapp.Web/appsettings.json**
{{ else if UI == "Blazor" || UI == "BlazorServer" || UI == "BlazorWebApp" }}
    **./src/yourapp.Blazor/appsettings.json**
{{ else }}
    Modify the **`localhost:4200`** in every location throughout your project.
    **./angular/src/environments/environment.prod.ts** 
{{ end }}    
    **./src/yourapp.AuthServer/appsettings.json**

```json
    "App": {
        "SelfUrl": "https://yourapp.azurewebsites.net"
    }
```
- Modify the **yourapp.ApiHost** URL in every location throughout your project.

    * This includes the following files:

    **./src/yourapp.HttpApi.Host/appsettings.json** , **./src/yourapp.Web/appsettings.json** , **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.AuthServer/appsettings.json**

```json
"App": {
    "SelfUrl": "https://yourapp-apihost.azurewebsites.net"
}
```

- Modify the **yourapp.AuthServer** URL in every location throughout your project.

    * This includes the following files:

    **./src/yourapp.Web/appsettings.json** , **./src/yourapp.AuthServer/appsettings.json** ,  **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.HttpApi.Host/appsettings.json**

```json
"App": {
    "SelfUrl": "https://yourapp-authserver.azurewebsites.net"
}
```

- Modify the **Redis__Configuration** URL in every location throughout your project.

    * This includes the following files:

    **./src/yourapp.Blazor/appsettings.json** , **./src/yourapp.AuthServer/appsettings.json** ,  **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.HttpApi.Host/appsettings.json**

```json
"Redis": {
    "Configuration": "redis-abpdemo.redis.cache.windows.net:6380,password={yourpassword},ssl=true,abortConnect=False"
    },
```
{{ end }}

## What's next?

- [Deploying Your ABP Application to Azure](step3-deployment-github-action.md)
