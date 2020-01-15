# Audit Logging

[Wikipedia](https://en.wikipedia.org/wiki/Audit_trail): "*An audit trail (also called **audit log**) is a security-relevant chronological record, set of records, and/or destination and source of records that provide documentary evidence of the sequence of activities that have affected at any time a specific operation, procedure, or event*".

ABP Framework automates the audit logging by convention and provides configuration points to control the level of the audit logs.

## IAuditingStore

`IAuditingStore` is an interface that is used to save the audit log objects (explained below) by the ABP Framework.

`SimpleLogAuditingStore` is used if no audit store was registered. It simply writes the audit object to the standard [logging system](Logging.md).

However, [the audit logging module](Modules/Audit-Logging.md) has been configured in [the startup templates](Startup-Templates/Index.md) which writes audit log objects to a database (it supports multiple database providers). So, most of the times you don't care about how `IAuditingStore` was implemented and used.

If you need to save the audit log objects to a custom data store, you can implement the `IAuditingStore` in your own application and replace using the [dependency injection system](Dependency-Injection.md).

## Audit Log Object

An **audit log object** is created for each **web request** is by default. An audit log object can be represented by the following relation diagram:

![**auditlog-object-diagram**](D:\Github\abp\docs\en\images\auditlog-object-diagram.png)

* **AuditLogInfo**: The root object with the following properties:
  * `ApplicationName`: When you save audit logs of different applications to the same database, this property is used to distinguish the logs of the applications.
  * `UserId`: Id of the current user, if the user has logged in.
  * `UserName`: User name of the current user, if the user has logged in (this value is here to not depend on the identity module/system for lookup).
  * `TenantId`: Id of the current tenant, for a multi-tenant application.
  * `TenantName`: Name of the current tenant, for a multi-tenant application.
  * `ExecutionTime`: The time when this audit log object has been created.
  * `ExecutionDuration`: Total execution duration of the request, in milliseconds. This can be used to observe the performance of the application.
  * `ClientId`: Id of the current client, if the client has been authenticated. A client is generally a 3rd-party application using the system over an HTTP API.
  * `ClientName`: Name of the current client, if available.
  * `ClientIpAddress`: IP address of the client/user device.
  * `CorrelationId`: Current [Correlation Id]((CorrelationId.md)). Correlation Id is used to relate the audit logs written by different applications (or microservices) in a single logical operation.
  * `BrowserInfo`: Browser name/version info of the current user, if available.
  * `HttpMethod`: HTTP method of the current request (GET, POST, PUT, DELETE... etc.).
  * `HttpStatusCode`: HTTP response status code for this request.
  * `Url`: URL of the request.
* **AuditLogActionInfo**: An audit log action is typically a controller action or an [application service](Application-Services.md) method call during the web request. One audit log may contain multiple actions. An action object has the following properties:
  * `ServiceName`: Name of the executed controller/service.
  * `MethodName`: Name of the executed method of the controller/service.
  * `Parameters`: A JSON formatted text representing the parameters passed to the method.
  * `ExecutionTime`: The time when this method was executed.
  * `ExecutionDuration`: Duration of the method execution, in milliseconds. This can be used to observe the performance of the method.
* **EntityChangeInfo**: Represents a change of an entity in this web request. An audit log may contain zero or more entity changes. An entity change has the following properties:
  * `ChangeTime`: The time when the entity was changed.
  * `ChangeType`: An enum with the following fields: `Created` (0), `Updated` (1) and `Deleted` (2).
  * `EntityId`: Id of the entity that was changed.
  * `EntityTenantId`: Id of the tenant this entity belongs to.
  * `EntityTypeFullName`: Type (class) name of the entity with full namespace (like *Acme.BookStore.Book* for the Book entity).
* **EntityPropertyChangeInfo**: Represents a change of a property of an entity. An entity change info (explained above) may contain one or more property change with the following properties:
  * `NewValue`: New value of the property. It is `null` if the entity was deleted.
  * `OriginalValue`: Old/original value before the change. It is `null` if the entity was newly created.
  * `PropertyName`: The name of the property on the entity class.
  * `PropertyTypeFullName`: Type (class) name of the property with full namespace.
* **Exception**: An audit log object may contain zero or more exception. In this way, you can get a report of the failed requests.
* **Comment**: An arbitrary string value to add custom messages to the audit log entry. An audit log object may contain zero or more comments.

In addition to the standard properties explained above, `AuditLogInfo`, `AuditLogActionInfo` and `EntityChangeInfo` objects implement the `IHasExtraProperties` interface, so you can add custom properties to these objects.

## Audit Logging Module

...