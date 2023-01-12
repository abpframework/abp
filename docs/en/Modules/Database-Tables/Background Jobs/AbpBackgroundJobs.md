## AbpBackgroundJobs

AbpBackgroundJobs is used to store background jobs.

### Description

This table stores information about the background jobs in the application. Each record in the table represents a background job and allows to manage and track the background jobs effectively. For example, you can use the `JobName`, `JobArgs`, `TryCount`, `NextTryTime`, `LastTryTime`, `IsAbandoned`, `Priority` columns to filter the background jobs by job name, job arguments, try count, next try time, last try time, abandoned status, and priority respectively, so that you can easily manage and track the background jobs in the application.

### Module

[`Volo.Abp.BackgroundJobs`](../../Background-Jobs.md)

