## Entity Best Practices & Conventions

### Entities

Every aggregate root is also an entity. So, these rules are valid for aggregate roots too unless aggregate root rules override them.

- **Do** define entities in the **domain layer**.

#### Primary Constructor

* **Do** define a **primary constructor** that ensures the validity of the entity on creation. Primary constructors are used to create a new instance of the entity by the application code.

- **Do** define primary constructor as `public`, `internal` or `protected internal` based on the requirements. If it's not public, the entity is expected to be created by a domain service.
- **Do** always initialize sub collections in the primary constructor.
- **Do not** generate `Guid` keys inside the constructor. Get it as a parameter, so the calling code will use `IGuidGenerator` to generate a new `Guid` value.

#### Parameterless Constructor

- **Do** always define a `protected` parameterless constructor to be compatible with ORMs.

#### References

- **Do** always **reference** to other aggregate roots **by Id**. Never add navigation properties to other aggregate roots.

#### Other Class Members

- **Do** always define properties and methods as `virtual` (except `private` methods, obviously). Because some ORMs and dynamic proxy tools require it.
- **Do** keep the entity as always **valid** and **consistent** within its own boundary.
  - **Do** define properties with `private`, `protected`, `internal ` or `protected internal` setter where it is needed to protect the entity consistency and validity.
  - **Do** define `public `, `internal` or `protected internal` (virtual) **methods** to change the properties (with non-public setters) if necessary.

### Aggregate Roots

#### Primary Keys

* **Do** always use a **Id** property for the aggregate root key.
* **Do not** use **composite keys** for aggregate roots.
* **Do** use **Guid** as the **primary key** of all aggregate roots.

#### Base Class

* **Do** inherit from the `AggregateRoot<TKey>` or one of the audited classes  (`CreationAuditedAggregateRoot<TKey>`, `AuditedAggregateRoot<TKey>` or `FullAuditedAggregateRoot<TKey>`) based on requirements.

#### Aggregate Boundary

* **Do** keep aggregates **as small as possible**. Most of the aggregates will only have primitive properties and will not have sub collections. Consider these as design decisions:
  * **Performance** & **memory** cost of loading & saving aggregates (keep in mind that an aggregate is normally loaded & saved as a single unit). Larger aggregates will consume more CPU & memory.
  * **Consistency** & **validity** boundary.

### Example

#### Aggregate Root

````C#
public class Issue : FullAuditedAggregateRoot<Guid> //Using Guid as the key/identifier
{
    public virtual string Title { get; private set; } //Changed using the SetTitle() method
    public virtual string Text { get; set; } //Can be directly changed. null values are allowed
    public virtual Guid? MilestoneId { get; set; } //Reference to another aggregate root
    public virtual bool IsClosed { get; private set; }
    public virtual IssueCloseReason? CloseReason { get; private set; } //Just an enum type
    public virtual Collection<IssueLabel> Labels { get; protected set; } //Sub collection

    protected Issue()
    {
        /* This constructor is for ORMs to be used while getting the entity from database.
         * - No need to initialize the Labels collection
             since it will be overrided from the database.
           - It's protected since proxying and deserialization tools
             may not work with private constructors.
         */
    }

    //Primary constructor
    public Issue(
        Guid id, //Get Guid value from the calling code
        [NotNull] string title, //Indicate that the title can not be null.
        string text = null,
        Guid? milestoneId = null) //Optional argument
    {
        Id = id;
        Title = Check.NotNullOrWhiteSpace(title, nameof(title)); //Validate
        Text = text;
        MilestoneId = milestoneId;
        
        Labels = new Collection<IssueLabel>(); //Always initialize the collection
    }

    public virtual void SetTitle([NotNull] string title)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title)); //Validate
    }
    
    /* AddLabel & RemoveLabel methods manages the Labels collection
     * in a safe way (prevents adding the same label twice) */

    public virtual void AddLabel(Guid labelId)
    {
        if (Labels.Any(l => l.LabelId == labelId))
        {
            return;
        }

        Labels.Add(new IssueLabel(Id, labelId));
    }
    
    public virtual void RemoveLabel(Guid labelId)
    {
        Labels.RemoveAll(l => l.LabelId == labelId);
    }

    /* Close & ReOpen methods protect the consistency
     * of the IsClosed and the CloseReason properties. */
    
    public virtual void Close(IssueCloseReason reason)
    {
        IsClosed = true;
        CloseReason = reason;
    }

    public virtual void ReOpen()
    {
        IsClosed = false;
        CloseReason = null;
    }
}
````

#### The Entity

````C#
public class IssueLabel : Entity
{
    public virtual Guid IssueId { get; private set; }
    public virtual Guid LabelId { get; private set; }

    protected IssueLabel()
    {
        
    }

    public IssueLabel(Guid issueId, Guid labelId)
    {
        IssueId = issueId;
        LabelId = labelId;
    }
}
````

### References

* Effective Aggregate Design by Vaughn Vernon
  http://dddcommunity.org/library/vernon_2011