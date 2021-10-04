## 应用服务最佳实践 & 约定

* **推荐** 为每个 **聚合根** 创建一个应用服务.

### 应用服务接口

* **推荐** 在 **application.contracts**层中为每一个应用服务定义一个`接口`.
* **推荐** 继承 `IApplicationService` 接口 .
* **推荐** 接口名称使用`AppService` 后缀 (如: `IProductAppService`).
* **推荐** 为服务创建输入输出DTO(数据传输对象).
* **不推荐** 服务中含有返回实体的方法.
* **推荐** 根据[DTO 最佳实践](Data-Transfer-Objects.md)定义DTO.

#### 输出

* **避免** 为相同或相关实体定义过多的输出DTO. 为实体定义 **基础** 和 **详细** DTO.

##### 基础DTO

**推荐** 为聚合根定义一个**基础**DTO.

- 直接包含实体中所有的**原始属性**.
  - 例外: 出于**安全**原因,可以**排除**某些属性(像 `User.Password`).
- 包含实体中所有**子集合**, 每个集合项都是一个简单的**关系DTO**.

示例:

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

##### 详细DTO

**推荐** 如果实体持有对其他聚合根的引用,那么应该为其定义**详细**DTO.

* 直接包含实体中所有的 **原始属性**.
  - 例外-1: 出于**安全**原因,可以**排除**某些属性(像 `User.Password`).
  - 例外-2: **推荐** 排除引用属性(如上例中的 `MilestoneId`). 为其添加引用属性的详细信息.
* 为每个引用属性添加其**基本DTO** .
* 包含实体的**所有子集合**, 集合中的每项都是相关实体的基本DTO.

示例:

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

#### 输入

* **不推荐** 在输入DTO中定义未在服务类中使用的属性.
* **不推荐** 在应用服务方法之间共享输入DTO.
* **不推荐** 继承另一个输入DTO类.
  * **可以** 继承自抽象基础DTO类, 并以这种方式在不同的DTO之间共享一些属性. 但是在这种情况下需要非常小心, 因为更新基础DTO会影响所有相关的DTO和服务方法. 所以避免这样做是一种好习惯.

#### 方法

* **推荐** 为异步方法使用 **Async** 后缀.
* **不推荐** 在方法名中重复实体的名称.
  * 例如: 在 `IProductAppService` 中定义`GetAsync(...)` 而不是 `GetProductAsync(...)` .

##### 获取单一实体

* **推荐** 使用 `GetAsync` 做为**方法名**.
* **推荐** 使用id做为方法参数.
* 返回 **详细DTO**. 示例:

````C#
Task<QuestionWithDetailsDto> GetAsync(Guid id);
````

##### 获取实体集合

* **推荐** 使用 `GetListAsync` 做为**方法名**.
* **推荐** 如果需要获取单个DTO可以使用参数进行 **过滤**, **排序** 和 **分页**.
  * **推荐** 尽可能让过滤参数可选.
  * **推荐** 将排序与分页属性设置为可选, 并且提供默认值.
  * **推荐** 限制最大页数大小 (基于性能考虑).
* **推荐** 返回 **详细DTO**集合. 示例:

````C#
Task<List<QuestionWithDetailsDto>> GetListAsync(QuestionListQueryDto queryDto);
````

##### 创建一个新实体

* **推荐** 使用 `CreateAsync` 做为**方法名**.
* **推荐** 使用**专门的输入DTO**来创建实体.
* **推荐** DTO类从 `ExtensibleObject` 类继承(或任何实现 `ExtensibleObject`的类) 以允许在需要时传递额外的属性.
* **推荐** 使用 **data annotations** 进行输入验证.
  * 尽可能在**领域**之间共享常量(通过**domain shared** package定义的常量).
* **推荐** 只需要创建实体的**最少**信息, 但是提供了其他可选属性.

示例**方法**:

````C#
Task<QuestionWithDetailsDto> CreateAsync(CreateQuestionDto questionDto);
````

输入**DTO**:

````C#
[Serializable]
public class CreateQuestionDto : ExtensibleObject
{
    [Required]
    [StringLength(QuestionConsts.MaxTitleLength, MinimumLength = QuestionConsts.MinTitleLength)]
    public string Title { get; set; }
    
    [StringLength(QuestionConsts.MaxTextLength)]
    public string Text { get; set; } //Optional
    
    public Guid? CategoryId { get; set; } //Optional
}
````

##### 更新已存在的实体

- **推荐** 使用 `UpdateAsync` 做为**方法名**.
- **推荐** 使用**专门的输入DTO**来更新实体.
- **推荐** DTO类从 `ExtensibleObject` 类继承(或任何实现 `ExtensibleObject`的类) 以允许在需要时传递额外的属性.
- **推荐** 获取实体的id做为分离的原始参数. 不要包含更新DTO.
- **推荐** 使用 **data annotations** 进行输入验证.
  - 尽可能在**领域**之间共享常量(通过**domain shared** package定义的常量).
- **推荐** 返回更新实体的**详细**DTO.

示例:

````C#
Task<QuestionWithDetailsDto> UpdateAsync(Guid id, UpdateQuestionDto updateQuestionDto);
````

##### 删除已存在的实体

- **推荐** 使用 `DeleteAsync` 做为**方法名**.
- **推荐** 使用原始参数 id. 示例:

````C#
Task DeleteAsync(Guid id);
````

##### 其他方法

* **可以** 定义其他方法以对实体执行操作. 示例:

````C#
Task<int> VoteAsync(Guid id, VoteType type);
````

此方法为试题投票并返回试题的当前分数.

### 应用服务实现

* **推荐** 开发**完全独立于web层**的应用层.
* **推荐** 在**应用层**实现应用服务接口.
  * **推荐** 使用命名约定. 如: 为 `IProductAppService` 接口创建 `ProductAppService` 类.
  * **推荐** 继承自 `ApplicationService` 基类.
* **推荐** 将所有的公开方法定义为 **virtual**, 以便开发人员继承和覆盖它们.
* **不推荐** 定义 **private** 方法. 应该定义为 **protected virtual**, 这样开发人员可以继承和覆盖它们.

#### 使用仓储

* **推荐** 使用专门设计的仓储 (如 `IProductRepository`).
* **不推荐** 使用泛型仓储 (如 `IRepository<Product>`).z`

#### 查询数据

* **不推荐** 在应用服务方法中使用linq/sql查询来自数据库的数据. 让仓储负责从数据源执行linq/sql查询.

#### 额外的属性

* **推荐** 使用 `MapExtraPropertiesTo` 扩展方法 ([参阅](../Object-Extensions.md)) 或配置对象映射 (`MapExtraProperties`) 以允许应用开发人员能够扩展对象和服务.

#### 操作/删除 实体

* **推荐** 总是从数据库中获取所有的相关实体以对他们执行操作.
* **推荐** 更新实体后调用存储的Update/UpdateAsync方法.因为并非所有数据库API都支持更改跟踪和自动更新.

#### 处理文件

* **不推荐** 在应用服务中使用任何web组件, 例如`IFormFile`和`Stream`. 如果你想接收一个文件, 可以使用`byte[]`.
* **推荐** 使用`Controller`来处理文件上传, 然后将文件的`byte[]`传递给应用服务的方法。

#### 使用其他应用服务

* **不推荐** 在同一个模块/应用中使用其他应用服务. 相反;
  * 使用领域层执行所需的任务.
  * 提取新类并在应用服务之间共享, 在必要时代码重用. 但要小心不要结合两个用例. 它们在开始时可能看起来相似, 但可能会随时间演变为不同的方向. 请谨慎使用代码共享.
* **可以** 在以下情况下使用其他应用服务;
  * 它们是另一个模块/微服务的一部分.
  * 当前模块仅引用已使用模块的application contracts.
