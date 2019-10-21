## 实体最佳实践 & 约定

### 实体

每个聚合根也是一个实体, 所以这些规则对聚合根也是有效的, 除非聚合根的某些规则覆盖了它们.

- **推荐** 在 **领域层** 中定义实体.

#### 主构造函数

* **推荐** 定义一个 **主构造函数** 确保实体在创建时的有效性, 在代码中通过主构造函数创建实体的新实例.

- **推荐** 根据需求把主构造函数定义为 `public`,`internal` 或 `protected internal` . 如果它不是public的, 那么应该由领域服务来创建实体.
- **推荐** 总是在主构造函数中初始化子集合.
- **不推荐** 在主构造函数中生成 `Guid` 键, 应该将其做为参数获取, 在调用时推荐使用 `IGuidGenerator` 生成新的 `Guid` 值做为参数.

#### 无参构造函数

- **推荐** 总是定义 `protected` 无参构造函数与ORM兼容.

#### 引用

- **推荐** 总是通过 **id** **引用** 其他聚合根, 不要将导航属性添加到其他聚合根中.

#### 类的其他成员

- **推荐** 总是将属性与方法定义为 `virtual` (除了`私有`方法 ). 因为有些ORM和动态代理工具需要.
- **推荐** 保持实体在自身边界内始终 **有效** 和 **一致**.
  - **推荐** 使用 `private`,`protected`,`internal`或`protected internal` setter定义属性, 保护实体的一致性和有效性.
  - **推荐** 定义 `public`, `internal` 或 `protected internal` (virtual)**方法**在必要时更改属性值(使用非public setters时).

### 聚合根

#### 主键

* **推荐** 总是使用 **Id** 属性做为聚合根主键.
* **不推荐** 在聚合根中使用 **复合主键**.
* **推荐** 所有的聚合根都使用 **Guid** 类型 **主键**.

#### 基类

* **推荐** 根据需求继承 `AggregateRoot<TKey>` 或以下一个审计类  (`CreationAuditedAggregateRoot<TKey>`, `AuditedAggregateRoot<TKey>` 或 `FullAuditedAggregateRoot<TKey>`).

#### 聚合边界

* **推荐** 聚合**尽可能小**. 大多数聚合只有原始属性, 不会有子集合. 把这些视为设计决策:
  * 加载和保存聚合的 **性能** 与 **内存** 成本 (请记住,聚合通常是做为一个单独的单元被加载和保存的). 较大的聚合会消耗更多的CPU和内存.
  * **一致性** & **有效性** 边界.

### 示例

#### 聚合根

````C#
public class Issue : FullAuditedAggregateRoot<Guid> //使用Guid作为键/标识符
{
    public virtual string Title { get; private set; } //使用 SetTitle() 方法set
    public virtual string Text { get; set; } //可以直接set,null值也是允许的
    public virtual Guid? MilestoneId { get; set; } //引用其他聚合根
    public virtual bool IsClosed { get; private set; }
    public virtual IssueCloseReason? CloseReason { get; private set; } //一个枚举类型
    public virtual Collection<IssueLabel> Labels { get; protected set; } //子集合

    protected Issue()
    {
        /* 此构造函数是提供给ORM用来从数据库中获取实体.
         * - 无需初始化Labels集合
             因为它会被来自数据库的值覆盖.
           - It's protected since proxying and deserialization tools
             可能不适用于私有构造函数.
         */
    }

    //主构造函数
    public Issue(
        Guid id, //从调用代码中获取Guid值
        [NotNull] string title, //表示标题不能为空.
        string text = null,
        Guid? milestoneId = null) //可选参数
    {
        Id = id;
        Title = Check.NotNullOrWhiteSpace(title, nameof(title)); //验证
        Text = text;
        MilestoneId = milestoneId;
 
        Labels = new Collection<IssueLabel>(); //总是初始化子集合
    }

    public virtual void SetTitle([NotNull] string title)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title)); //验证
    }
    
    /* AddLabel和RemoveLabel方法管理Labels集合
     * 安全的方式(防止两次添加相同的标签) */

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

    /* Close和ReOpen方法可保护一致性
     * IsClosed 与 CloseReason 属性. */
    
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

#### 实体

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

### 参考文献

* Effective Aggregate Design by Vaughn Vernon
  http://dddcommunity.org/library/vernon_2011
