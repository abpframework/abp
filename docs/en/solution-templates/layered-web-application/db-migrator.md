```json
//[doc-seo]
{
    "Description": "Learn how to use the Db Migrator for seamless database migrations and data seeding in your applications with ABP Framework."
}
```

# Layered Solution: Db Migrator

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Web Applications",
    "Path": "solution-templates/layered-web-application/web-applications"
  },
  "Next": {
    "Name": "Mobile Applications",
    "Path": "solution-templates/layered-web-application/mobile-applications"
  }
}
````

## Db Migrator Project

The Db Migrator project is a console application designed to handle database schema migrations and seed data population. It operates as a standalone application that can be executed on-demand or integrated into a CI/CD pipeline.

### Usage

You can run the Db Migrator application:
- From the command line.  
- Directly from Visual Studio.  

### Configuration

The Db Migrator project maintains its own configuration, separate from the main application. If you need to update the database connection string or any related settings, ensure that changes are applied consistently to both the main application and the Db Migrator to avoid discrepancies.

## Folder Structure

In the `*.DbMigrator` project, you will find the `DbMigratorHostedService` class, which is responsible for executing database migrations and seeding data. This class is registered in the `Program` class and starts running when the application is launched.

### Layers and Responsibilities

- **`*.Domain` Layer**:  
  Contains the `Data` folder, which holds the necessary classes for managing database migrations and seed data. However, since the `*.Domain` layer does not reference the `EntityFrameworkCore` package, it only defines the abstraction for data migration.

- **`*.EntityFrameworkCore` Layer**:  
  This layer is responsible for implementing database schema migrations. It includes the `EntityFrameworkCore[ProjectName]DbSchemaMigrator` class, which handles the actual migration logic using the `EntityFrameworkCore` package.
