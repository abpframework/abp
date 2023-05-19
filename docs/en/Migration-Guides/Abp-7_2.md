# ABP Version 7.2 Migration Guide

This document is a guide for upgrading ABP v7.1 solutions to ABP v7.2. There are a few changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

## `LastPasswordChangeTime` and `ShouldChangePasswordOnNextLogin` Properties Added to the `IdentityUser` Class

In this version, two new properties, which are `LastPasswordChangeTime` and `ShouldChangePasswordOnNextLogin` have been added to the `IdentityUser` class and to the corresponding entity. Therefore, you may need to create a new migration and apply it to your database.

## Renamed `OnRegistered` Method

There was a typo in an extension method, named as `OnRegistred`. In this version, we have fixed the typo and renamed the method as `OnRegistered`. Also, we have updated the related places in our modules that use this method. 

However, if you have used this method in your projects, you need to rename it as `OnRegistered` in your code.