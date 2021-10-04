## Application Services Best Practices & Conventions

* **Do** create an application service for each **aggregate root**.

### Application Service Interface

* **Do** define an `interface` for each application service in the **application contracts** package.
* **Do** inherit from the `IApplicationService` interface.
* **Do** use the `AppService` postfix for the interface name (ex: `IProductAppService`).
* **Do** create DTOs (Data Transfer Objects) for inputs and outputs of the service.
* **Do not** get/return entities for the service methods.
* **Do** define DTOs based on the [DTO best practices](Data-Transfer-Objects.md).

#### Outputs

* **Avoid** to define too many output DTOs for same or related entities. Instead, define a **basic** and a **detailed** DTO for an entity.

##### Basic DTO

**Do** define a **basic** DTO for an aggregate root.

- Include all the **primitive properties** directly on the aggregate root.
  - Exception: Can **exclude** properties for **security** reasons (like `User.Password`).
- Include all the **sub collections** of the entity where every item in the collection is a simple **relation DTO**.
- Inherit from one of the **extensible entity DTO** classes for aggregate roots (and entities implement the `IHasExtraProperties`).

Example:

```c#
[Serializable]
public class IssueDto : ExtensibleFullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Text { get; set; }
    public Guid? MilestoneId { get; set; }
    public Collection<IssueLabelDto> Labels { get; set; }
}

[Serializable]
public class IssueLabelDto
{
    public Guid IssueId { get; set; }
    public Guid LabelId { get; set; }
}
```

##### Detailed DTO

**Do** define a **detailed** DTO for an entity if it has reference(s) to other aggregate roots.

* Include all the **primitive properties** directly on the entity.
  - Exception-1: Can **exclude** properties for **security** reasons (like `User.Password`).
  - Exception-2: **Do** exclude reference properties (like `MilestoneId` in the example above). Will already add details for the reference properties.
* Include a **basic DTO** property for every reference property.
* Include all the **sub collections** of the entity where every item in the collection is the **basic DTO** of the related entity.

Example:

````C#
[Serializable]
public class IssueWithDetailsDto : ExtensibleFullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Text { get; set; }
    public MilestoneDto Milestone { get; set; }
    public Collection<LabelDto> Labels { get; set; }
}

[Serializable]
public class MilestoneDto : ExtensibleEntityDto<Guid>
{
    public string Name { get; set; }
    public bool IsClosed { get; set; }
}

[Serializable]
public class LabelDto : ExtensibleEntityDto<Guid>
{
    public string Name { get; set; }
    public string Color { get; set; }
}
````

#### Inputs

* **Do not** define any property in an input DTO that is not used in the service class.
* **Do not** share input DTOs between application service methods.
* **Do not** inherit an input DTO class from another one.
  * **May** inherit from an abstract base DTO class and share some properties between different DTOs in that way. However, should be very careful in that case because manipulating the base DTO would effect all related DTOs and service methods. Avoid from that as a good practice.

#### Methods

* **Do** define service methods as asynchronous with **Async** postfix.
* **Do not** repeat the entity name in the method names.
  * Example: Define `GetAsync(...)` instead of `GetProductAsync(...)` in the `IProductAppService`.

##### Getting A Single Entity

* **Do** use the `GetAsync` **method name**.
* **Do** get Id with a **primitive** method parameter.
* Return the **detailed DTO**. Example:

````C#
Task<QuestionWithDetailsDto> GetAsync(Guid id);
````

##### Getting A List Of Entities

* **Do** use the `GetListAsync` **method name**.
* **Do** get a single DTO argument for **filtering**, **sorting** and **paging** if necessary.
  * **Do** implement filters optional where possible.
  * **Do** implement sorting & paging properties as optional and provide default values.
  * **Do** limit maximum page size (for performance reasons).
* **Do** return a list of **detailed DTO**s. Example:

````C#
Task<List<QuestionWithDetailsDto>> GetListAsync(QuestionListQueryDto queryDto);
````

##### Creating A New Entity

* **Do** use the `CreateAsync` **method name**.
* **Do** get a **specialized input** DTO to create the entity.
* **Do** inherit the DTO class from the `ExtensibleObject` (or any other class implements the `IHasExtraProperties`) to allow to pass extra properties if needed.
* **Do** use **data annotations** for input validation.
  * Share constants between domain wherever possible (via constants defined in the **domain shared** package).
* **Do** return **the detailed** DTO for new created entity.
* **Do** only require the **minimum** info to create the entity but provide possibility to set others as optional properties.

Example **method**:

````C#
Task<QuestionWithDetailsDto> CreateAsync(CreateQuestionDto questionDto);
````

The related **DTO**:

````C#
[Serializable]
public class CreateQuestionDto : ExtensibleObject
{
    [Required]
    [StringLength(QuestionConsts.MaxTitleLength,
                  MinimumLength = QuestionConsts.MinTitleLength)]
    public string Title { get; set; }
    
    [StringLength(QuestionConsts.MaxTextLength)]
    public string Text { get; set; } //Optional
    
    public Guid? CategoryId { get; set; } //Optional
}
````

##### Updating An Existing Entity

- **Do** use the `UpdateAsync` **method name**.
- **Do** get a **specialized input** DTO to update the entity.
- **Do** inherit the DTO class from the `ExtensibleObject` (or any other class implements the `IHasExtraProperties`) to allow to pass extra properties if needed.
- **Do** get the Id of the entity as a separated primitive parameter. Do not include to the update DTO.
- **Do** use **data annotations** for input validation.
  - Share constants between domain wherever possible (via constants defined in the **domain shared** package).
- **Do** return **the detailed** DTO for the updated entity.

Example:

````C#
Task<QuestionWithDetailsDto> UpdateAsync(Guid id, UpdateQuestionDto updateQuestionDto);
````

##### Deleting An Existing Entity

- **Do** use the `DeleteAsync` **method name**.
- **Do** get Id with a **primitive** method parameter. Example:

````C#
Task DeleteAsync(Guid id);
````

##### Other Methods

* **Can** define additional methods to perform operations on the entity. Example:

````C#
Task<int> VoteAsync(Guid id, VoteType type);
````

This method votes a question and returns the current score of the question.

### Application Service Implementation

* **Do** develop the application layer **completely independent from the web layer**.
* **Do** implement application service interfaces in the **application layer**.
  * **Do** use the naming convention. Ex: Create `ProductAppService` class for the `IProductAppService` interface.
  * **Do** inherit from the `ApplicationService` base class.
* **Do** make all public methods **virtual**, so developers may inherit and override them.
* **Do not** make **private** methods. Instead make them **protected virtual**, so developers may inherit and override them.

#### Using Repositories

* **Do** use the specifically designed repositories (like `IProductRepository`).
* **Do not** use generic repositories (like `IRepository<Product>`).

#### Querying Data

* **Do not** use LINQ/SQL for querying data from database inside the application service methods. It's repository's responsibility to perform LINQ/SQL queries from the data source.

#### Extra Properties

* **Do** use either `MapExtraPropertiesTo` extension method ([see](../Object-Extensions.md)) or configure the object mapper (`MapExtraProperties`) to allow application developers to be able to extend the objects and services.

#### Manipulating / Deleting Entities

* **Do** always get all the related entities from repositories to perform the operations on them.
* **Do** call repository's Update/UpdateAsync method after updating an entity. Because, not all database APIs support change tracking & auto update.

#### Handle files

* **Do not** use any web components like `IFormFile` or `Stream` in the application services. If you want to serve a file you can use `byte[]`.
* **Do** use a `Controller` to handle file uploading then pass the `byte[]` of the file to the application service method.

#### Using Other Application Services

* **Do not** use other application services of the same module/application. Instead;
  * Use domain layer to perform the required task.
  * Extract a new class and share between the application services to accomplish the code reuse when necessary. But be careful to don't couple two use cases. They may seem similar at the beginning, but may evolve to different directions by time. So, use code sharing carefully.
* **Can** use application services of others only if;
  * They are parts of another module / microservice.
  * The current module has only reference to the application contracts of the used module.



