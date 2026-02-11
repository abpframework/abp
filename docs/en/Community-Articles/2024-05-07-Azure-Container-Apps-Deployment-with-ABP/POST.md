# Deploy Your ABP Framework MVC Project to Azure Container Apps

![](azure-container-abp.png)

In this article, we will show the seamless deployment of an ABP Framework MVC project to Azure Container Apps. enabling you to deploy and run containerized applications without the hassle of managing the infrastructure underneath. It provides an uncomplicated and cost-effective method for deploying and scaling your applications.

### Getting Started with ABP Framework MVC and Azure Container Apps

To get started, you will need an ABP Framework MVC project that you want to deploy. If you don't have one, you can [create a new project using the ABP CLI](https://docs.abp.io/en/abp/latest/Startup-Templates/Application). You will also need [an Azure subscription](https://azure.microsoft.com) and [an Azure SQL database](https://azure.microsoft.com/en-gb/products/azure-sql).

Before creating Azure container apps resources and deploying the ABP Framework MVC project, I show you how you can effortlessly create Docker images and push them to Docker Hub, leveraging the pre-configured Docker file and scripts that come with the ABP MVC framework.

### Creating a Docker Image for ABP Framework MVC

To create a Docker image for your ABP Framework MVC project, navigate to `etc/build/build-images-locally.ps1` and fix the script to match your Docker Hub username and image name. Then, run the script to build the Docker image locally.

![Build Docker Image](build-docker-image.png)

Next, check the Docker Hub repository to confirm that the image has been pushed successfully.

![Docker Hub Repository](docker-hub-repository.png)

### Deploying to Azure Container Apps

Now that you have Docker images for your ABP Framework MVC project, you can proceed to deploy it to Azure Container Apps. To do this, navigate to the Azure portal and create a new Azure Container Apps resource. Ypu will not need just an Azure Container Apps resource, but also an Azure Container Apps Job resource to migrate the database schema and seed data for your ABP Framework MVC project.

![Create Azure Container Apps](create-azure-container-apps.png)

#### Step 1: Deploy the Docker Image

Firstly, create a new Azure Container Apps resource without environment variables. You will need web url so that you can set it as an environment variable in the next step. Then, check the deployment status to confirm that the deployment was successful.

![Deploy Docker Image](deploy-docker-image.png)

#### Step 2: Migrate Database Schema and Seed Data

Secondly, create a new Azure Container Apps Job resource to migrate the database schema and seed data. You can do this by creating a new job with the following environment variables:

```text
ConnectionStrings__Default - Server=tcp:demoabpapp.database.windows.net,1433;Initial Catalog=mvcapppro;Persist Security Info=False;User ID=demoapppro;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;

OpenIddict__Applications__mvcapppro_Web__RootUrl - https://mvcwebapp.victoriousgrass-8b06438d.northeurope.azurecontainerapps.io
```

To get ConnectionStrings of Sql Database and url of the web app, you can navigate to the Azure portal and check the properties of the Azure SQL database and Azure Container Apps resource.


![Azure SQL Database Connection Strings](azure-sql-database-connection-strings.png)

![Create Azure Container Apps Job](create-azure-container-apps-job.png)

Finally, check the job status to confirm that the database migration and seeding were successful. You can connect to the Azure SQL database to verify that the schema and seed data have been applied.

![Check Job Status](check-job-status.png)

#### Step 3: Edit the Azure Container Apps Resource

After completing these steps, you have to edit the Azure Container Apps resource to add the required environment variables for your ABP Framework MVC project. You can do this by adding the following environment variables:

```text
App__SelfUrl - https://mvcwebapp.victoriousgrass-8b06438d.northeurope.azurecontainerapps.io

ASPNETCORE_URLS - http://+:80

AuthServer__Authority - https://mvcwebapp.victoriousgrass-8b06438d.northeurope.azurecontainerapps.io

ConnectionStrings__Default - Server=tcp:demoabpapp.database.windows.net,1433;Initial Catalog=mvcapppro;Persist Security Info=False;User ID=demoapppro;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

![Add Environment Variables](add-environment-variables.png)

#### Step 4: Create a New Deployment

Once you have added the environment variables, save and create a new deployment to apply the changes. You can now access your ABP Framework MVC project running on Azure Container Apps by navigating to the URL provided in the environment variables.

![Access ABP Framework MVC Project](access-abp-framework-mvc-project.png)

You can see the Azure resources created for the ABP Framework MVC project deployment that includes the Azure Container Apps resource, Azure Container Apps Job resource, and Azure SQL database.

![Azure Resources](azure-resources.png)

### Conclusion

Azure Container Apps provides a simple and cost-effective way to deploy and scale your ABP Framework MVC projects without managing the underlying infrastructure. By following the steps outlined in this article, you can seamlessly deploy your ABP Framework MVC projects to Azure Container Apps and enjoy the benefits it offers.

I hope you found this article helpful. If you have any questions or feedback, please feel free to leave a comment below.
