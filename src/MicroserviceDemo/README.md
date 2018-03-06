# Micro-service Architecture Demo

## Applications

* **MicroserviceDemo.AuthServer**: Identity4 Server used for authentication.
* **MicroserviceDemo.TenancyService**: API service to manage tenants.
* **MicroserviceDemo.Web**: Web UI application to manage users, roles and tenants.

## How To Run

#### Run Database Migrations

TODO:... Also need to seed database!

#### Host File

Add these lines into *C:\Windows\System32\drivers\etc\host* file:

````
127.0.0.1			openidclientdemo.com
127.0.0.1			abp-test-authserver.com
127.0.0.1			abp-test-web.com
127.0.0.1			abp-test-tenancy.com
````

#### IIS Express Configuration

Add the sites into *C:\Users\%USER%\Documents\IISExpress\config\applicationhost.config* file:

````xml
<site name="MultiTenancyDemo.WebSite" id="2" serverAutoStart="true">
    <application path="/">
        <virtualDirectory path="/" physicalPath="D:\Github\abp\src\MicroserviceDemo\MicroserviceDemo.Web" />
    </application>
    <bindings>
        <binding protocol="http" bindingInformation=":61144:abp-test-web.com" />
    </bindings>
</site>
<site name="MultiTenancyDemo.TenancyServiceSite" id="3" serverAutoStart="true">
    <application path="/">
        <virtualDirectory path="/" physicalPath="D:\Github\abp\src\MicroserviceDemo\MicroserviceDemo.TenancyService" />
    </application>
    <bindings>
        <binding protocol="http" bindingInformation=":63877:abp-test-tenancy.com" />
    </bindings>
</site>
            <site name="MultiTenancyDemo.AuthServerSite" id="4" serverAutoStart="true">
    <application path="/">
        <virtualDirectory path="/" physicalPath="D:\Github\abp\src\MicroserviceDemo\MicroserviceDemo.AuthServer" />
    </application>
    <bindings>
        <binding protocol="http" bindingInformation=":54307:abp-test-authserver.com" />
    </bindings>
</site>

````

...