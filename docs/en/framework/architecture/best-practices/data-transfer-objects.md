```json
//[doc-seo]
{
    "Description": "Explore best practices for implementing Data Transfer Objects in your applications, guided by Domain-Driven Design principles."
}
```

# Data Transfer Objects Best Practices & Conventions

> This document offers best practices for implementing Data Transfer Object classes in your modules and applications based on Domain-Driven-Design principles.
>
> **Ensure you've read the [*Data Transfer Objects*](../domain-driven-design/data-transfer-objects.md) document first.**

## General

* **Do** define DTOs in the **application contracts** package.
* **Do** inherit from the pre-built **base DTO classes** where possible and necessary (like `EntityDto<TKey>`, `CreationAuditedEntityDto<TKey>`, `AuditedEntityDto<TKey>`, `FullAuditedEntityDto<TKey>` and so on).
  * **Do** inherit from the **extensible DTO** classes for the **aggregate roots** (like `ExtensibleAuditedEntityDto<TKey>`), because aggregate roots are extensible objects and extra properties are mapped to DTOs in this way.
* **Do** define DTO members with **public getter and setter**.
* **Do** use **data annotations** for **validation** on the properties of DTOs those are inputs of the service.
* **Do** not add any **logic** into DTOs except implementing `IValidatableObject` when necessary.
* **Do** mark all DTOs as **[Serializable]** since they are already serializable and developers may want to binary serialize them.

## See Also

* [Video tutorial](https://abp.io/video-courses/essentials/data-transfer-objects)