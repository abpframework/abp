# ABP Version 5.3 Migration Guide

This document is a guide for upgrading ABP v5.2 solutions to ABP v5.3. There is a change in this version that may effect your applications, please read it carefully and apply the necessary changes to your application.

## AutoMapper Upgraded to v11.0.1

AutoMapper library upgraded to **v11.0.1** in this version. So, you need to change your project's target SDK that use the **AutoMapper** library (typically your `*.Application` project). You can change it from `netstandard2.0` to `netstandard2.1` or `net6` if needed. Please see [#12189](https://github.com/abpframework/abp/pull/12189) for more info.

## See Also

* [Official blog post for the 5.3 release](https://blog.abp.io/abp/ABP.IO-Platform-5.3-RC-Has-Been-Published)
