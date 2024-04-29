# ABP Version 7.1 Migration Guide

This document is a guide for upgrading ABP v7.0 solutions to ABP v7.1. There are a few changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

> **Note**: Entity Framework developers may need to add a new code-first database migration to their projects since we made some improvements to the existing entities of some application modules.

## Navigation Menu - `CustomData` type changed to `Dictionary<string, object>`

`ApplicationMenu` and `ApplicationMenuItem` classes' `CustomData` property type has been changed to `Dictionary<string, object>`. So, if you use the optional `CustomData` property of these classes, change it accordingly. See [#15608](https://github.com/abpframework/abp/pull/15608) for more information.

*Old usage:*

```csharp
var menu = new ApplicationMenu("Home", L["Home"], "/", customData: new MyCustomData()); 
```

*New usage:*

```csharp
var menu = new ApplicationMenu("Home", L["Home"], "/").WithCustomData("CustomDataKey", new MyCustomData());
```
