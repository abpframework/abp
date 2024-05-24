# Microservice Solution: Background Jobs

Background jobs are long-running tasks executed in the background. They handle tasks that don't need to be performed immediately and can be delayed. The ABP Framework includes a system for creating and running these tasks in your application. You can learn more in the [Background Jobs](../../framework/infrastructure/background-jobs/index.md) document.

In the microservice solution template, RabbitMQ is used as the message broker to manage these tasks. The functionality is implemented in the `Volo.Abp.BackgroundJobs.RabbitMq` package, which provides the necessary implementations to create and execute jobs using RabbitMQ. This setup is integrated into the microservice solution template and is used in microservice projects. You can change the RabbitMQ configuration in the `appsettings.json` file. The default configuration is as follows:

```json
"RabbitMQ": {
  "Connections": {
    "Default": {
      "HostName": "localhost"
    }
  },
  "EventBus": {
    "ClientName": "Acme_MicroserviceName",
    "ExchangeName": "Acme"
  }
}
```

Additionally, there is docker-compose configuration for RabbitMQ in the `docker-compose.yml` file and helm chart configuration in the `etc/helm` folder.