# Microservice Solution: Swagger Integration

````json
//[doc-nav]
{
  "Next": {
    "Name": "Permission management in the Microservice solution",
    "Path": "solution-templates/microservice/permission-management"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

In a microservice system, it is important to have a well-documented API. [Swagger](https://swagger.io/) is a tool that helps to create, document, and consume RESTful web services. It provides a user interface to interact with the APIs and also a way to generate client SDKs for the APIs.

In the [Swagger Integration](../../framework/api-development/swagger.md) document, you can find general information about Swagger integration with ABP Framework.

For the microservice side, the API Gateway project is the entry point of the microservice system. It is the main place where the Swagger UI is hosted. Each microservice has its own Swagger endpoint, and the API Gateway project collects them all and hosts them under a single URL. The [API Gateways](./api-gateways.md#the-swagger-configuration) document explains how this is done in detail.