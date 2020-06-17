# Distributed Event Bus

Distributed Event bus system allows to publish and subscribe to events that can be transferred across application/service boundaries. You can use the distributed event bus to asynchronously send and receive message between microservices or applications.

## Providers

Distributed event bus system provides an abstraction that can be implemented by any vendor/provider. There are two providers implemented out of the box:

* `LocalDistributedEventBus` is the default implementation that implements the distributed event bus to work as in-process. Yes! The **default implementation works just like the [local event bus](Local-Event-Bus.md)**, if you don't configure a real distributed provider.
* `RabbitMqDistributedEventBus` implements the distributed event bus with the [RabbitMQ](https://www.rabbitmq.com/). See the [RabbitMQ integration document](Distributed-Event-Bus-RabbitMQ-Integration.md) to learn how to configure it.

## Publishing Events

TODO