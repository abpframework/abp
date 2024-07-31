# Event Bus

An event bus is a mediator that transfers a message from a sender to a receiver. In this way, it provides a loosely coupled communication way between objects, services and applications.

## Event Bus Types

ABP provides two type of event buses;

* **[Local Event Bus](local)** is suitable for in-process messaging.
* **[Distributed Event Bus](distributed)** is suitable for inter-process messaging, like microservices publishing and subscribing to distributed events.