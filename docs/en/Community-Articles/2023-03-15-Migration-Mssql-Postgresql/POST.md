
# Migration from Mssql to Postgresql

![sql-server-to-postgres](images/sql-server-to-postgres.jpg)

## Introduction

Database migration is a common practice for organizations that want to move from one database system to another. This can be for a variety of reasons, including cost, performance, and features. In this article, we will discuss the process of migrating a database from MSSQL to PostgreSQL, the challenges that may arise during the migration, and how to overcome them.

In our case, we decided to switch our database from Microsoft SQL Server (MSSQL) to PostgreSQL because we wanted to move our on-premise platform to Azure. We’ve also found out that the cost of the license for MSSQL on Azure was significantly higher than PostgreSQL. After conducting a cost-benefit analysis, we decided to migrate our database to PostgreSQL to save costs.

Before migrating to Azure, we decided to switch our database from MSSQL to PostgreSQL on-premise first. This gave us the opportunity to test and fine-tune the migration process before making the final switch to Azure.

## Challenges

Despite using a third-party tool(DBConvert for MySQL & PostgreSQL) for the migration, we faced three main problems when exporting data to PostgreSQL. Firstly, some tables with plain text had utf-8 encoding problems. We overcame this problem by dump-restoring these tables.

![db-converter](images/db-converter.jpg)


Secondly, our database had to be case-insensitive, but PostgreSQL does not have this as a default configuration. We handled it using “citext” with the abp migration service.

![citext-1](images/citext-1.jpg)
![citext-2](images/citext-2.jpg)
![citext-3](images/citext-3.jpg)


Finally, we had problems with importing binary data, such as the content of the NuGet packages.It was hard to understand that the binaries of the NuGet packages were different. The part that bothered us the most was the transfer of the NuGet packages. Fortunately, we overcame the binary error, thanks to the tool made by talented developers in a very short time.

## Conclusion

One of the benefits of using PostgreSQL is that there is no need to make any changes to the code except for the migration service. This means that the applications that were previously using MSSQL can seamlessly switch to PostgreSQL without any modifications.

Thanks to the flexibility of ABP, it became compatible with PostgreSQL with only a few changes in the migration service. This means that we did not need to modify any of the application's core functionality, and the migration process was smooth and seamless.

In conclusion, migrating a database from MSSQL to PostgreSQL can be challenging, but it can bring significant cost savings in the long run. By testing and fine-tuning the migration process before making the final switch, we were able to overcome the challenges we’d faced during the migration process. Thanks to the flexibility of ABP, we were able to make the transition with minimal code changes.

