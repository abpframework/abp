# Event Bus

An event bus is a mediator that transfers a message from a sender to a receiver. In this way, it provides a loosely coupled communication way between objects, services and applications.

## Event Bus Types

ABP Framework provides two type of event buses;

* **[Local Event Bus](Local-Event-Bus.md)** is suitable for in-process messaging.
* **[Distributed Event Bus](Distributed-Event-Bus.md)** is suitable for inter-process messaging, like microservices publishing and subscribing to distributed events.