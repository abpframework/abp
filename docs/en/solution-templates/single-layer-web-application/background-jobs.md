```json
//[doc-seo]
{
    "Description": "Learn how to implement and manage background jobs in your Single Layer solution using ABP Framework for efficient task handling."
}
```

# Single Layer Solution: Background Jobs

```json
//[doc-nav]
{
  "Previous": {
    "Name": "Swagger integration",
    "Path": "solution-templates/single-layer-web-application/swagger-integration"
  },
  "Next": {
    "Name": "Background Workers",
    "Path": "solution-templates/single-layer-web-application/background-workers"
  }
}
```

Background jobs are long-running, asynchronous tasks that run in the background of your application. They are useful for tasks that are not time-sensitive, such as sending emails, generating reports, or processing data. Background jobs are typically triggered by a user action or a scheduled task. You can learn more about background jobs in the [Background Jobs](../../framework/infrastructure/background-jobs/index.md) document.

In the Single Layer solution template, background jobs are implemented using the [Background Jobs](../../modules/background-jobs.md) module. This module provides a simple and efficient way to create and manage background jobs in your application. It includes features such as job queues, job scheduling. It stores job information in the database, allowing you to track the status of jobs and retry failed jobs.