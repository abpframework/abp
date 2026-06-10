```json
//[doc-seo]
{
    "Description": "Trouble logging in as admin? Follow these steps to reset access and ensure your database is properly configured."
}
```

# KB#0003: Cannot login with the admin user

## Use the Correct Username and Password

You may have entered the wrong password. The username is `admin`, and the password is `1q2w3E*`. Note that the password is case-sensitive.

## Forgot to Seed Initial Data

You may need to add migrations and update the database using the EF Core CLI. If your solution includes a `DbMigrator` application, you must run the `DbMigrator` application to seed the initial data.

If your project does not include a `DbMigrator` application, there might be a `migrate-database.ps1` script available. You can use it to migrate and seed the initial data.

> The no-layer application typically support a `--migrate-database` option for migrating and seeding initial data.

> Example:
> ```bash
> dotnet run --migrate-database
> ```

## Tenant Admin User

If you cannot log in as a tenant admin user, ensure the tenant database is created and seeded, Use the password that was set during tenant creation.

> The tenant seeding process is handled by the template project. If it is not completed, please check the `Logs` file for any error logs.

## Check the `AbpUsers` Table

If you have performed migration and seeded the initial data, check the `AbpUsers` table in the database. Ensure that the user record exists. If your tenant has a separate database, check the tenant database as well.

Passwords are stored in hashed format, not plain text. If you suspect the password is incorrect, you can delete the user record and re-seed the initial data using the `DbMigrator` application or the `migrate-database.ps1` script.

## Other Issues

If the issue persists, refer to the `README.MD` file in your solution or consult the [Getting Started](https://abp.io/docs/latest/get-started) documentation.

Feel free to create an issue in the [ABP GitHub repository](https://github.com/abpframework/abp/issues/new/choose) or contact [ABP Commercial Support](https://abp.io/support/questions/New) for assistance.
