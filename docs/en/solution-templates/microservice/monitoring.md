# Microservice Solution: Monitoring

In a distributed system it is important to monitor the health of the system and the services. Monitoring helps to detect issues before they become problems and helps to understand the system's behavior. All the services, applications and gateways are configured to use the [Prometheus](https://prometheus.io/) and [Grafana](https://grafana.com/) libraries for monitoring. They are configured in a common way for monitoring. This document explains that common monitoring structure.

## Configuration

The monitoring configuration is done in the each *Module* class of the project. The `OnApplicationInitialization` method of the module class is used to set the monitoring system. We're adding the `prometheus-net.AspNetCore` package to the project to use Prometheus. The `prometheus-net.AspNetCore` package is a library that provides a middleware to expose metrics for Prometheus. The `app.UseHttpMetrics();` line is collect the HTTP request metrics. The `endpoints.MapMetrics();` line is used to expose the metrics to the `/metrics` endpoint. Existing templates includes the prometheus and grafana configurations in the `docker-compose` file. So, you can visit the `http://localhost:9090` to see the [Prometheus](https://prometheus.io/) dashboard and `http://localhost:3001` to see the [Grafana](https://grafana.com/) dashboard. 

> Default username and password for Grafana is `admin` and `admin`. You can change the password after the first login.
