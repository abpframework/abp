## 领域服务最佳实践 & 约定

### 介绍

在领域驱动设计（DDD）解决方案中，核心业务逻辑通常在聚合（实体）和领域服务中实现。在以下情况中尤其需要创建领域服务：

- 您实现了核心领域逻辑，其取决于某些服务（如存储库或其他外部服务）。

- 您需要实现的逻辑与多于一个聚合/实体相关，因此它无法适合任何聚合。

### ABP Domain Service Infrastructure
领域服务很简单，无状态类。虽然您不必继承任何服务或界面，但ABP框架提供了一些有用的基类和约定。

#### DomainService & IDomainService
要么从`DomainService`基类推出领域服务，要么直接实现`IDomainService`接口。

**例子:从 `DomainService`继承创建领域服务类.**

````C#
using Volo.Abp.Domain.Services;

namespace MyProject.Issues
{
    public class IssueManager : DomainService
    {
        
    }
}
````
当你这样做;

ABP框架自动将类注册到具有瞬态实例的依赖注入系统。

您可以直接使用某些常见服务作为基础属性，而无需手动注入（例如 ILogger和IGuidGenerator）。

**例子：实现为用户分配问题的领域逻辑

````C#
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
Issue 是聚合根，定义如下：

````c#
public class Issue : AggregateRoot<Guid>
{
    public Guid? AssignedUserId { get; internal set; }
    
    //...
}
````
- 设置`Setter`为`internal`，可确保它不能直接在上层中设置，并且始终使用`ISSueManager`为用户分配问题。

 ### 使用 领域服务（·Domain Service·）
 
 领域服务通常用于应用服务（·aplication service·）中。

示例：使用·IssueManager·为用户分配问题

····c#
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
····
由于·IssueAppService·位于应用程序层中，因此它无法直接为用户分配问题。所以，它使用了ISSUeManager。

### 应用服务VS领域服务

虽然两个应用程序服务和领域服务都实现了业务规则，但有基本的逻辑和形式上的差异;

- 应用程序服务实现应用程序的用例（典型Web应用程序中的用户交互），而领域服务实现核心，用例独立域逻辑。

- 应用程序服务获取/返回数据传输对象，领域服务方法通常会得到并返回领域对象（实体，值对象）。

- 领域服务通常由应用程序服务或其他领域服务使用，而展示层或客户端应用程序使用应用程序服务。

### 生命周期

领域服务的生命周期是瞬时的，它们会自动注册到依赖注入系统。
