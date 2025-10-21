```json
//[doc-seo]
{
    "Description": "Learn about ABP Framework's Event Bus, its types, and guidance for choosing between local and distributed messaging in your applications."
}
```

# Event Bus

An event bus is a mediator that transfers a message from a sender to a receiver. In this way, it provides a loosely coupled communication way between objects, services and applications.

## Event Bus Types

ABP provides two type of event buses;

* **[Local Event Bus](local/index.md)** is suitable for in-process messaging.
* **[Distributed Event Bus](distributed/index.md)** is suitable for inter-process messaging, like publishing and subscribing to distributed events in a distributed/microservice system.

## Event Bus Guiding

You may confuse which event bus to use in your application. Here, a few example scenarios:

* If you are building a microservice system, use the distributed event bus for **inter-microservice communication**. If you are publishing an event and always handling it in the same microservice, you can use the local event bus for that event.
* If you are building a modular monolith application, use the distributed event bus for **inter-module communication**. Since the distributed event bus works in-process by default (unless you configure a real distributed event bus provider). In that way, if you migrate to a microservice system later, these inter-module communication can become a inter-microservice communication without any code change (with just installing a distributed event bus provide package). If you are publishing an event and always handling it in the same module, you can use the local event bus for that event.
* If you are building a monolith (non-modular) application, you can always use the local event bus.

The distributed event bus works in-process by default. Unless you don't configure a real distributed event bus provider, it works just like the local event bus. Once you configure a real distributed event bus provider, all your events become distributed.

So, you use the local event bus only if you an event should always remain in-process in any scenario. For other type of events, use the distributed event bus.

### Technical Differences

There are some technical differences between the local and distributed event bus:

* You can send entities or other non-serializable objects using the local event bus since these objects are not serialized/deserialized on event publishing. On the other hand, distributed event bus serializes/deserializes objects once you use a real distributed event bus provider.
* The local event bus is always transactional in the database level. For distributed event bus, you should enable inbox/outbox pattern to ensure data consistency once you use a real distributed event bus provider.

These differences are because of the distributed event bus supports distributed scenarios while the local event bus is only used for in-process messaging. When you don't set up a real distributed event bus provider, these differences are not valid.