```json
//[doc-seo]
{
    "Description": "Learn how to efficiently implement and manage background jobs in your layered solution using the ABP Framework for asynchronous tasks."
}
```

# Layered Solution: Background Jobs

```json
//[doc-nav]
{
  "Previous": {
    "Name": "Swagger integration",
    "Path": "solution-templates/layered-web-application/swagger-integration"
  },
  "Next": {
    "Name": "Background Workers",
    "Path": "solution-templates/layered-web-application/background-workers"
  }
}
```

Background jobs are long-running, asynchronous tasks that operate in the background of your application. They are ideal for non-time-sensitive tasks, such as sending emails, generating reports, or processing data. These jobs are usually triggered by a user action or a scheduled task. For more information, refer to the [Background Jobs](../../framework/infrastructure/background-jobs/index.md) document.

In the layered solution template, background jobs are implemented using the [Background Jobs](../../modules/background-jobs.md) module. This module offers a simple and efficient way to create and manage background jobs in your application. It provides features like job queues and job scheduling. Job information is stored in the database, enabling you to track job statuses and retry failed jobs.