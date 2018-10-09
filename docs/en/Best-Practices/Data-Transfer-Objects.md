## Data Transfer Objects Best Practices & Conventions

* **Do** define DTOs in the **application contracts** package.
* **Do** inherit from the pre-built **base DTO classes** where possible and necessary (like `EntityDto<TKey>`, `CreationAuditedEntityDto<TKey>`, `AuditedEntityDto<TKey>`, `FullAuditedEntityDto<TKey>` and so on).
* **Do** define DTO members with **public getter and setter**.
* **Do** use **data annotations** for **validation** on the properties of DTOs those are inputs of the service.
* **Do** not add any **logic** into DTOs except implementing `IValidatableObject` when necessary.