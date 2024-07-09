# Communications Between Microservices

There are two basic messaging patterns for communication between two microservices:

## Synchronous Communication

In this pattern, a service calls an API that another service exposes,  using a protocol such as HTTP or gRPC. This option is a synchronous  messaging pattern because the caller waits for a response from the  receiver.

## Asynchronous Communication

In this pattern, a service sends message without waiting for a response, and one or more services process the message asynchronously. This is done by using a message broker or event bus.

Further information check out https://docs.microsoft.com/en-us/azure/architecture/microservices/design/interservice-communication

## See Also

- [Synchronous Communication](synchronous-interservice-communication.md)
- [Asynchronous Communication](asynchronous-interservice-communication.md)
