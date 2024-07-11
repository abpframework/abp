# ABP Version 7.2 Migration Guide

This document is a guide for upgrading ABP v7.1 solutions to ABP v7.2. There are a few changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

## Open-Source (Framework)

If you are using one of the open-source startup templates, then you can check the following sections to apply the related breaking changes:

### `LastPasswordChangeTime` and `ShouldChangePasswordOnNextLogin` Properties Added to the `IdentityUser` Class

In this version, two new properties, which are `LastPasswordChangeTime` and `ShouldChangePasswordOnNextLogin` have been added to the `IdentityUser` class and to the corresponding entity. Therefore, you may need to create a new migration and apply it to your database.

### Renamed `OnRegistered` Method

There was a typo in an extension method, named as `OnRegistred`. In this version, we have fixed the typo and renamed the method as `OnRegistered`. Also, we have updated the related places in our modules that use this method. 

However, if you have used this method in your projects, you need to rename it as `OnRegistered` in your code.

## PRO

> Please check the **Open-Source (Framework)** section before reading this section. The listed topics might affect your application and you might need to take care of them.

If you are a paid-license owner and using the ABP's paid version, then please follow the following sections to get informed about the breaking changes and apply the necessary ones:

### Payment Plan Management Permissions Moved to the Host Side

In this version, the payment plan management permissions (payment plan, gateway, and payment request permissions) have been moved to the host side. These permissions could have been visible on both sides prior to this version (tenants and hosts). However, a tenant can't configure its own payment gateway configurations by default, therefore it was not beneficial and logical. 

As a result, we decided to move these permissions to the host side and it will be only visible on the host side furthermore. 

> You don't need to take any action for this breaking change, we have wanted to inform you about the change and its motive.