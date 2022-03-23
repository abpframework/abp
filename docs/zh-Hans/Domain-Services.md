# 领域服务

## 介绍

在 [领域驱动设计](Domain-Driven-Design.md) (DDD) 解决方案中,核心业务逻辑通常在聚合 ([实体](Entities.md)) 和领域服务中实现. 在以下情况下特别需要创建领域服务

* 你实现了依赖于某些服务（如存储库或其他外部服务）的核心域逻辑.
* 你需要实现的逻辑与多个聚合/实体相关,因此它不适合任何聚合.

## ABP 领域服务基础设施

领域服务是简单的无状态类. 虽然你不必从任何服务或接口派生,但 ABP 框架提供了一些有用的基类和约定.

### DomainService 和 IDomainService

从 `DomainService` 基类派生领域服务或直接实现 `IDomainService` 接口.

**示例: 创建从 `DomainService` 基类派生的领域服务.**

````csharp
using Volo.Abp.Domain.Services;
namespace MyProject.Issues
{
    public class IssueManager : DomainService
    {
        
    }
}
````

当你这样做时:

* ABP 框架自动将类注册为瞬态生命周期到依赖注入系统.
* 你可以直接使用一些常用服务作为基础属性,而无需手动注入 (例如 [ILogger](Logging.md) and [IGuidGenerator](Guid-Generation.md)).

> 建议使用 `Manager` 或 `Service` 后缀命名领域服务. 我们通常使用如上面示例中的 `Manager` 后缀.
**示例: 实现将问题分配给用户的领域逻辑**

````csharp
public class IssueManager : DomainService
{
    private readonly IRepository<Issue, Guid> _issueRepository;
    public IssueManager(IRepository<Issue, Guid> issueRepository)
    {
        _issueRepository = issueRepository;
    }
    
    public async Task AssignAsync(Issue issue, AppUser user)
    {
        var currentIssueCount = await _issueRepository
            .CountAsync(i => i.AssignedUserId == user.Id);
        
        //Implementing a core business validation
        if (currentIssueCount >= 3)
        {
            throw new IssueAssignmentException(user.UserName);
        }
        issue.AssignedUserId = user.Id;
    }    
}
````

问题是定义如下所示的 [聚合根](Entities.md):

````csharp
public class Issue : AggregateRoot<Guid>
{
    public Guid? AssignedUserId { get; internal set; }
    
    //...
}
````

* 使用 `internal` 的 set 确保外层调用者不能直接在调用 set ,并强制始终使用 `IssueManager` 为 `User` 分配 `Issue`.

### 使用领域服务

领域服务通常用于 [应用程序服务](Application-Services.md).

**示例: 使用 `IssueManager` 将问题分配给用户**

````csharp
using System;
using System.Threading.Tasks;
using MyProject.Users;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
namespace MyProject.Issues
{
    public class IssueAppService : ApplicationService, IIssueAppService
    {
        private readonly IssueManager _issueManager;
        private readonly IRepository<AppUser, Guid> _userRepository;
        private readonly IRepository<Issue, Guid> _issueRepository;
        public IssueAppService(
            IssueManager issueManager,
            IRepository<AppUser, Guid> userRepository,
            IRepository<Issue, Guid> issueRepository)
        {
            _issueManager = issueManager;
            _userRepository = userRepository;
            _issueRepository = issueRepository;
        }
        public async Task AssignAsync(Guid id, Guid userId)
        {
            var issue = await _issueRepository.GetAsync(id);
            var user = await _userRepository.GetAsync(userId);
            await _issueManager.AssignAsync(issue, user);
            await _issueRepository.UpdateAsync(issue);
        }
    }
}
````

由于 `IssueAppService` 在应用层, 它不能直接将问题分配给用户.因此,它使用 `IssueManager`.

## 应用程序服务与领域服务

虽然应用服务和领域服务都实现了业务规则,但存在根本的逻辑和形式差异；
虽然 [应用服务](Application-Services.md) 和领域服务都实现了业务规则,但存在根本的逻辑和形式差异:

* 应用程序服务实现应用程序的 **用例** (典型 Web 应用程序中的用户交互), 而领域服务实现 **核心的、用例独立的领域逻辑**.
* 应用程序服务获取/返回 [数据传输对象](Data-Transfer-Objects.md), 领域服务方法通常获取和返回 **领域对象** ([实体](Entities.md), [值对象](Value-Objects.md)).
* 领域服务通常由应用程序服务或其他领域服务使用,而应用程序服务由表示层或客户端应用程序使用.

## 生命周期

领域服务的生命周期是 [瞬态](https://docs.abp.io/en/abp/latest/Dependency-Injection) 的,它们会自动注册到依赖注入服务.
