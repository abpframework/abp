# Microservice Solution: Communication

> You must have an ABP Business or a higher license to be able to create a microservice solution.

Microservice solution template provides multiple ways to communicate between services, applications and API gateways. This documents explains the communication methods in the solution template.

## Synchronous Communication

In this pattern, a service calls an API that another service exposes, using a protocol such as HTTP or gRPC. This option is a synchronous messaging pattern because the caller waits for a response from the receiver.

* [HTTP API Calls](http-api-calls.md)
* [gRPC Calls](grpc-calls.md)

## Asynchronous Communication

In this pattern, a service sends message without waiting for a response, and one or more services process the message asynchronously. This is done by using a message broker or event bus. Further information [check out](https://docs.microsoft.com/en-us/azure/architecture/microservices/design/interservice-communication).

* [Distributed Events](distributed-events.md)
