````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Yes", "No"]
}
````

## Step 2: Customizing the Configuration of the ABP Application

- To customize the configuration of your ABP application, modify the `ConnectionString` values in every location throughout your project. The `ConnectionString` values are stored in the `appsettings.json` files.

    This includes the following files:
{{ if UI == "MVC" && Tiered == "No" }}
    **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.Web/appsettings.json**
{{else}}
    **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.HttpApi.Host/appsettings.json**
{{end}}
{{if Tiered == "Yes"}}
    **./src/yourapp.AuthServer/appsettings.json**
{{end}}

```json
"ConnectionStrings": {
    "Default": "Server=tcp:yourserver.database.windows.net,1433;Initial Catalog=yourdatabase;Persist Security Info=False;User ID=yourusername;Password=yourpassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}
```

{{ if UI == "MVC" }}

{{if Tiered == "No"}}

- Modify the **yourapp.Web** URL in every location throughout your project, especially within the **./src/yourapp.Web/appsettings.json** and **./src/yourapp.DbMigrator/appsettings.json** files, to match your Azure Web App Service URL.

```json
    "App": {
        "SelfUrl": "https://yourapp.azurewebsites.net"
    }
```

{{else}}

- Modify the **yourapp.Web** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.Web/appsettings.json** , **./src/yourapp.DbMigrator/appsettings.json** , **./src/yourapp.HttpApi.Host/appsettings.json** and **./src/yourapp.AuthServer/appsettings.json**

```json
"App": {
    "SelfUrl": "https://yourapp.azurewebsites.net"
}
```

- Modify the **yourapp.ApiHost** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.HttpApi.Host/appsettings.json** , **./src/yourapp.Web/appsettings.json** , **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.AuthServer/appsettings.json**

```json
"App": {
    "SelfUrl": "https://yourapp-apihost.azurewebsites.net"
}
```

- Modify the **yourapp.AuthServer** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.Web/appsettings.json** , **./src/yourapp.AuthServer/appsettings.json** ,  **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.HttpApi.Host/appsettings.json**

```json
"App": {
    "SelfUrl": "https://yourapp-authserver.azurewebsites.net"
}
```

- Modify the **Redis__Configuration** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.Web/appsettings.json** , **./src/yourapp.AuthServer/appsettings.json** ,  **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.HttpApi.Host/appsettings.json**

```json
"Redis": {
    "Configuration": "redis-abpdemo.redis.cache.windows.net:6380,password={yourpassword},ssl=true,abortConnect=False"
  },
```

{{end}}

{{ else if UI == "NG" }}

- Modify the **`localhost:4200`** in every location throughout your project.

    This includes the following files:

    **./angular/src/environments/environment.prod.ts** , **./aspnet-core/src/yourapp.DbMigrator/appsettings.json** and **./aspnet-core/src/yourapp.HttpApi.Host/appsettings.json**

```typescript
    application: {
        baseUrl: 'https://yourapp.azurestaticapps.net'
    }
```

- Modify the **yourapp.HttpApi.Host** URL in every location throughout your project.

    This includes the following files:

    **./angular/src/environments/environment.prod.ts** , **./aspnet-core/src/yourapp.DbMigrator/appsettings.json** and **./aspnet-core/src/yourapp.HttpApi.Host/appsettings.json**

```json
    "App": {
        "SelfUrl": "https://yourApiHost.azurewebsites.net"
    }
```

{{ else if UI == "Blazor" }}

- Modify the **yourapp.Blazor** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.Blazor/appsettings.json** , **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.HttpApi.Host/appsettings.json**

```json
    "App": {
        "SelfUrl": "https://yourapp.azurewebsites.net"
    }
```

- Modify the **yourapp.HttpApi.Host** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.Blazor/appsettings.json** , **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.HttpApi.Host/appsettings.json**

```json
    "App": {
        "SelfUrl": "https://yourApiHost.azurewebsites.net"
    }
```

{{ else }}

{{if Tiered == "No"}}

- Modify the **yourapp.Web** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.Blazor/appsettings.json** , **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.HttpApi.Host/appsettings.json**

```json
"App": {
    "SelfUrl": "https://yourapp.azurewebsites.net"
}
```

- Modify the **yourapp.ApiHost** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.HttpApi.Host/appsettings.json** , **./src/yourapp.Blazor/appsettings.json** and **./src/yourapp.DbMigrator/appsettings.json**

```json
"App": {
    "SelfUrl": "https://yourapp-apihost.azurewebsites.net"
}
```

{{else}}

- Modify the **yourapp.Web** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.Blazor/appsettings.json** , **./src/yourapp.DbMigrator/appsettings.json** , **./src/yourapp.HttpApi.Host/appsettings.json** and **./src/yourapp.AuthServer/appsettings.json**

```json
"App": {
    "SelfUrl": "https://yourapp.azurewebsites.net"
}
```

- Modify the **yourapp.ApiHost** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.HttpApi.Host/appsettings.json** , **./src/yourapp.Blazor/appsettings.json** , **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.AuthServer/appsettings.json**

```json
"App": {
    "SelfUrl": "https://yourapp-apihost.azurewebsites.net"
}
```

- Modify the **yourapp.AuthServer** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.Blazor/appsettings.json** , **./src/yourapp.AuthServer/appsettings.json** ,  **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.HttpApi.Host/appsettings.json**

```json
"App": {
    "SelfUrl": "https://yourapp-authserver.azurewebsites.net"
}
```

- Modify the **Redis__Configuration** URL in every location throughout your project.

    This includes the following files:

    **./src/yourapp.Blazor/appsettings.json** , **./src/yourapp.AuthServer/appsettings.json** ,  **./src/yourapp.DbMigrator/appsettings.json** and **./src/yourapp.HttpApi.Host/appsettings.json**

```json
"Redis": {
    "Configuration": "redis-abpdemo.redis.cache.windows.net:6380,password={yourpassword},ssl=true,abortConnect=False"
    },
```

{{end}}

{{end}}


## What's next?

- [Deploying Your ABP Application to Azure](step3-deployment-github-action.md)
