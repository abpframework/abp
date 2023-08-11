# ABP Commercial - GDPR Module Overview

In this article, I will highlight the ABP Commercial's [GDPR Module](https://commercial.abp.io/modules/Volo.Gdpr) and show you how to provide personal data to the GDPR Module that collected by your application and make it aligned with personal data download result.

## GDPR Module

[GDPR Module](https://docs.abp.io/en/commercial/latest/modules/gdpr) allows users to download and delete their data collected by the application. It's used for Personal Data Management obligating by the GDPR regulation and provides the following features:

* Allows users to request their personal data.
* Lists the personal data requests and users can download their personal data by the listed request times.
* Allows users to delete their personal data and their accounts permanently.

The GDPR module is pre-installed in the [Application](https://docs.abp.io/en/commercial/latest/startup-templates/application/index) and [Application (Single Layer)](https://docs.abp.io/en/commercial/latest/startup-templates/application-single-layer/index) Pro Startup templates for ABP Commercial customers. Therefore, most of the time you don't need to manually install it.

> If you need to install the GDPR Module manually, you can refer to the [GDPR Module documentation](https://docs.abp.io/en/commercial/latest/modules/gdpr#how-to-install).

Let's create an Application template with the following command and see the UI for the GDPR module:

```csharp
abp new GdprDemo -t app-pro
```

After the application is created, we can run the application and login. Then, when we check the user menu, we should be able to see the "Personal Data" menu item:

![](./gdpr-personal-data-menu.png)

We can click this menu item to see the "Personal Data Management" page:

![](./gdpr-personal-data-page.png)

This page is used to manage personal data requests. You can view the past requests, current status of the latest request, create a new personal data download request, download the prepared personal data or delete all your personal data (makes the data anonymized) and account from the application.

### Downloading Personal Data

After a quick overview of the GDPR module, now we can deep dive into the Personal Data Download process.

The GDPR module requests the information from the other modules that reference the `Volo.Abp.Gdpr.Abstractions` package and merges the response data into a single JSON file and the personal data can be downloaded later by the user from the "Personal Data Management" when it's prepared (when the specified preperation time has been passed). 

Currently, only the [Identity Pro module](https://docs.abp.io/en/commercial/latest/modules/identity) provides some personal data to the GDPR Module because it's the only module that can be considered as collecting personal data, such as user's name, surname, email address and so on. Therefore, ABP makes the everything that needed such as getting all data collecting that when the personal data download request has been made or delete/anonymize the personal data when the personal data delete request has been made.

This might be enough for some of your applications and you would not need to collect any other sensitive data from a user. However, most of the time you would probably need to collect some personal data from users for certain reasons. 

In that case, you should provide the personal data that you collected for a certain user in your application/module or provider to the GPDR module. So, when the personal data download request has been made, the GDPR module can request the information from the different part of your application and merges the response data into a single JSON file to be downloaded. 

In the next section, we will see how to provide personal data from a different module/provider within a scenerio.

## Providing Personal Data

//TODO: consider a scenerio!!! (getting address from a user might be used)

## Conclusion

In this article, I've explained the GDPR Module's data collection system and give you a brief overview of the module. 