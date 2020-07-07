# 切换到EF Core Oracle 提供程序

本文介绍如何将预配置为SqlServer提供程序的 **[应用程序启动模板](Startup-Templates/Application.md)** 切换到 **Oracle** 数据库提供程序

ABP框架提供了两种不同的Oracle包集成. 你可以选择以下其中一个:

* **[Volo.Abp.EntityFrameworkCore.Oracle](Entity-Framework-Core-Oracle-Official.md)** 使用官方 & 免费的oracle驱动 ( **当前处于 beta**).
* **[Volo.Abp.EntityFrameworkCore.Oracle.Devart](Entity-Framework-Core-Oracle-Devart.md)** 使用[Devart](https://www.devart.com/)公司提供的商业(付费)驱动.

> 你可以选择一个你想要的包,如果你不知道它们之间的区别,请在网站上进行搜索. ABP框架仅提供集成,不提供第三库类库的支持.