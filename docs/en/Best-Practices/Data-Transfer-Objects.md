## Data Transfer Objects Best Practices & Conventions

* **Do** define DTOs in the **application contracts** package.
* **Do** inherit from the pre-built **base DTO classes** where possible and necessary (like `EntityDto<TKey>`, `CreationAuditedEntityDto<TKey>`, `AuditedEntityDto<TKey>`, `FullAuditedEntityDto<TKey>` and so on).
  * **Do** inherit from the **extensible DTO** classes for the **aggregate roots** (like `ExtensibleAuditedEntityDto<TKey>`), because aggregate roots are extensible objects and extra properties are mapped to DTOs in this way.
* **Do** define DTO members with **public getter and setter**.
* **Do** use **data annotations** for **validation** on the properties of DTOs those are inputs of the service.
* **Do** not add any **logic** into DTOs except implementing `IValidatableObject` when necessary.
* **Do** mark all DTOs as **[Serializable]** since they are already serializable and developers may want to binary serialize them.