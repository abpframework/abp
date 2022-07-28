# Deploy Abp Webapp to Azure App Service

> In this document, you'll learn how to create and deploy your first abp web app to [Azure App Service](https://docs.microsoft.com/en-us/azure/app-service/overview). App Service supports various versions of .NET apps, and provides a highly scalable, self-patching web hosting service. Abp web apps are cross-platform and can be hosted on Linux or Windows.

****Prerequisites****

- An Azure account with an active subscription. [Create an account for free](https://azure.microsoft.com/free/dotnet).
- A GitHub account [Create an account for free](http://github.com/).


## Create a new ABP Framework application

Create repository on [GitHub.com](https://github.com/) to create a new repository (keep all the default settings)

Open a command prompt and clone the repository into a folder on your computer

```bash
git clone https://github.com/your-username/your-repository-name.git
```
Check your dotnet version. Should be at least 3.1.x
```bash
dotnet --version
```
Install or update first the *ABP CLI* using a command line window

```bash
dotnet tool install -g Volo.Abp.Cli || dotnet tool update -g Volo.Abp.Cli
```

Open a comman prompt in the *GitHub repository folder* and create a *new abp Blazor solution* with the command below

```bash
abp new YourAppName -u blazor
```

Open a command prompt in the *[YourAppName].DbMigrator* project and enter the command below to apply the database migrations

```bash
dotnet run
```

Open a command prompt in the *[YourAppName].HttpApi.Host* project to run the API project

```bash
dotnet run
```

Navigate to the *applicationUrl* specified in *the launchSettings.json* file of the *[YourAppName].HttpApi.Host project*. You should get the *Swagger window*

Open a command prompt in the *[YourAppName].Blazor* folder and enter the command below to run the Blazor project

```bash
dotnet run
```

Navigate to the *applicationUrl* specified in the *launchSettings.json* file of the *[YourAppName].Blazor* project. You should get the *ABP Framework Welcome window*

Stop both the *API* and the *Blazor* project by pressing **CTRL+C**

Open a command prompt in the root folder of your project and *add, commit and push* all your changes to your GitHub repository

```bash
git add .
git commit -m initialcommit
git push
```


## Create an SQL Database on Azure and change connection string in appsettings.json files

1. Login into the [Azure Portal](https://portal.azure.com/)

2. Click on **Create a resouce**

3. Search for *SQL Database*

4. Click the **Create** button in the *SQL Database window*

5. Create a new resource group. Name it *rg[YourAppName]*

6. Enter *[YourAppName]Db* as database name

7. Create a new Server and name it *[yourappname]server*

8. Enter a serveradmin login and passwords. Click the **OK** button

9. Select your *Location*

10. Check *Allow Azure services to access server*

11. Click on **Configure database**. Go for the *Basic* version and click the **Apply** button

12. Click on the **Review + create** button. Click **Create**

13. Click on **Go to resource** and click on **SQL server** when the SQL Database is created

14. Click on **Networking** under Security left side menu 

15. Select **Selected networks** and click on **Add your client IP$ address** at Firewall rules

16. Select **Allow Azure and resources to access this seerver** and save

17. Go to your **SQL database**, click on **Connection strings** and copy the connection string

18. Copy/paste the appsettings.json files of the [YourAppName].HttpApi.Host and the [YourAppName].DbMigrator project

19. Do not forget to replace {your_password} with the correct server password you entered in Azure SQL Database

Open a command prompt in the [YourAppName].DbMigrator project again and enter the command below to apply the database migrations

```bash
dotnet run
```
Open a command prompt in the [YourAppName].HttpApi.Host project and enter the command below to check your API is working

```bash
dotnet run
```

Stop the [YourAppName].HttpApi.Host by entering CTRL+C

Open a command prompt in the root folder of your project and add, commit and push all your changes to your GitHub repository
```bash
git add .
git commit -m initialcommit
git push
```


## Set up the Build pipeline in AzureDevops and publish the Build Artifacts

1. Sign in into Azure DevOps

2. Click on **New organization** and follow the steps to create a new organisation. Name it [YourAppName]org

3. Enter [YourAppName]Proj as project name in the Create a project to get started window

4. Select **Public visibility** and click the **Create project** button

5. Click on the Pipelines button to continue

6. Click on the **Create Pipeline** button

7. Select GitHub in the Select your repository window

![azdevops-1](images/azdevops-1)

8. Enter Connection name. [YourAppName]GitHubConnection and click on **Authorize using OAuth**

9. Select your **GitHub** [YourAppName]repo and click Continue

10. Search for **ASP.NET**  in the Select a template window

![azdevops-2](images/azdevops-2)

11. Select the ASP.NET Core template and click the **Apply** button

12. Select **Settings** on the second task(Nugetcommand@2) in the pipeline

13. Select **Feeds in my Nuget.config** and type **Nuget.config** in the text box

![azdevops-3](images/azdevops-3)

14. Add below commands block to end of the pipeline

    ```
    - task: PublishBuildArtifacts@1
      displayName: 'Publish Artifact'
      inputs:
        PathtoPublish: '$(build.artifactstagingdirectory)'
        ArtifactName: '$(Parameters.ArtifactName)'
      condition: succeededOrFailed()
      ```
![azdevops-4](images/azdevops-4)

15. Click on **Save & queue** in the top menu. Click on **Save & queue** again and click **Save an run** to run the Build pipeline

16. When the Build pipeline has finished. Click on **1 published; 1 consumed**



## Create a Web App in the Azure Portal to deploy [YourAppName] project

1. Search for Web App in the Search the Marketplace field

2. Click the **Create** button in the Web App window

3. Select rg[YourAppName] in the Resource Group dropdown

4. Enter [YourAppName]API in the Name input field

5. Select code, .NET Core 3.1 (LTS) and windows as Operating System

6. Enter [YourAppName]API in the Name input field

7. Select .NET Core 3.1 (LTS) in the Runtime stack dropdown

8. Select Windows as Operating System

9. Select the same Region as in tthe SQL server you created in Part 3

![azdevops-5](images/azdevops-5)

10. Click on **Create new** in thet Windows Plan. Name it [YourAppName]ApiWinPlan

11. Click **Change size** in Sku and size. Go for the Dev/Test Free F1 version and click the **Apply** button

![azdevops-6](images/azdevops-6)

12. Click the **Review + creat**e** button. Click **Create** button

13. Click on **Go to resource** when the Web App has been created



## Create a Release pipeline in the AzureDevops and deploy [YourAppName] project

1. Sign in into [Azure DevOps](https://azure.microsoft.com/en-us/services/devops/)

2. Click on [YourAppName]Proj and click on **Releases** in the *Pipelines* menu

3. Click on the **New pipeline** button in the *No release pipelines found* window

4. Select *Azure App Service deployment* and click the **Apply** button

![azdevops-7](images/azdevops-7)

5. Enter *[YourAppName]staging* in the *Stage name* field in the *Stage* winwow. Close window

6. Click **+ Add an artifact** in the *Pipeline* tab

7. Select the **Build** icon as *Source type* in the *Add an artifact* window

8. Select Build pipeline in the *Source (build pipeline)* dropdown and click the **Add** button

![azdevops-8](images/azdevops-8)

9. Click on the **Continuous deployment trigger (thunderbolt icon)**

10. Set the toggle to **Enabled** in the the *Continous deployment trigger* window

11. Click **+ Add** in *No filters added*. Select **Include** in the *Type* dropdown. Select your branch in the *Build* branch dropdown and close the window

![azdevops-9](images/azdevops-9)

12. Click on **the little red circle with the exclamation mark** in the *Tasks* tab menu

13. Select your subscription in the *Azure subscription* dropdown.

![azdevops-10](images/azdevops-10)

14. Click **Authorize** and enter your credentials in the next screens

15. After Authorisation, select the **[YourAppName]API** in the *App service name* dropdown

16. Click on the **Deploy Azure App Service** task

17. Select **[YourAppName].HttpApi.Host.zip** in the *Package or folder* input field

![azdevops-11](images/azdevops-11)

18. Click on the **Save** icon in the top menu and click **OK**

19. Click **Create release** in the top menu. Click **Create**to create a release>

20. Click on the *Pipeline* tab and wait until the Deployment succeeds

![azdevops-12](images/azdevops-12)

21. Open a browser and navigate to the URL of your Web App

```
https://[YourAppName]api.azurewebsites.net
```