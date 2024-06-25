# Unit Tests

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Overall",
    "Path": "testing/overall"
  },
  "Next": {
    "Name": "Integration tests",
    "Path": "testing/integration-tests"
  }
}
````

For Unit Tests, you don't need to much infrastructure. You typically instantiate your class and provide some pre-configured mocked objects to prepare your object to test.

## Classes Without Dependencies

In this simplest case, the class you want to test has no dependencies. In this case, you can directly instantiate your class, call its methods and make your assertions.

### Example: Testing an Entity

Assume that you've an `Issue` [entity](../framework/architecture/domain-driven-design/entities.md) as shown below:

````csharp
using System;
using Volo.Abp.Domain.Entities;

namespace MyProject.Issues
{
    public class Issue : AggregateRoot<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsLocked { get; set; }
        public bool IsClosed { get; private set; }
        public DateTime? CloseDate { get; private set; }

        public void Close()
        {
            IsClosed = true;
            CloseDate = DateTime.UtcNow;
        }

        public void Open()
        {
            if (!IsClosed)
            {
                return;
            }

            if (IsLocked)
            {
                throw new IssueStateException("You can not open a locked issue!");
            }

            IsClosed = false;
            CloseDate = null;
        }
    }
}

````

Notice that the `IsClosed` and `CloseDate` properties have private setters to force some business rules by using the `Open()` and `Close()` methods:

* Whenever you close an issue, the `CloseDate` should be set to the [current time](../framework/infrastructure/virtual-file-system.md).
* An issue can not be re-opened if it is locked. And if it is re-opened, the `CloseDate` should be set to `null`.

Since the `Issue` entity is a part of the Domain Layer, we should test it in the `Domain.Tests` project. Create an `Issue_Tests` class inside the `Domain.Tests` project:

````csharp
using Shouldly;
using Xunit;

namespace MyProject.Issues
{
    public class Issue_Tests
    {
        [Fact]
        public void Should_Set_The_CloseDate_Whenever_Close_An_Issue()
        {
            // Arrange

            var issue = new Issue();
            issue.CloseDate.ShouldBeNull(); // null at the beginning

            // Act

            issue.Close();

            // Assert

            issue.IsClosed.ShouldBeTrue();
            issue.CloseDate.ShouldNotBeNull();
        }
    }
}
````

This test follows the AAA (Arrange-Act-Assert) pattern;

* **Arrange** part creates an `Issue` entity and ensures the `CloseDate` is `null` at the beginning.
* **Act** part executes the method we want to test for this case.
* **Assert** part checks if the `Issue` properties are same as we expect to be.

`[Fact]` attribute is defined by the [xUnit](https://xunit.net/) library and marks a method as a test method. `Should...` extension methods are provided by the [Shouldly](https://github.com/shouldly/shouldly) library. You can directly use the `Assert` class of the xUnit, but Shouldly makes it much comfortable and straightforward.

When you execute the tests, you will see that it passes successfully:

![issue-first-test](../images/issue-first-test.png)

Let's add two more test methods:

````csharp
[Fact]
public void Should_Allow_To_ReOpen_An_Issue()
{
    // Arrange

    var issue = new Issue();
    issue.Close();

    // Act

    issue.Open();

    // Assert

    issue.IsClosed.ShouldBeFalse();
    issue.CloseDate.ShouldBeNull();
}

[Fact]
public void Should_Not_Allow_To_ReOpen_A_Locked_Issue()
{
    // Arrange

    var issue = new Issue();
    issue.Close();
    issue.IsLocked = true;

    // Act & Assert

    Assert.Throws<IssueStateException>(() =>
    {
        issue.Open();
    });
}
````

`Assert.Throws` checks if the executed code throws a matching exception.

> See the [xUnit](https://xunit.net/#documentation) & [Shoudly](https://docs.shouldly.org/) documentation to learn more about these libraries.

## Classes With Dependencies

If your service has dependencies and if you want to unit test this service, you need to mock the dependencies.

### Example: Testing a Domain Service

Assume that you've an `IssueManager` [Domain Service](../framework/architecture/domain-driven-design/domain-services.md) that is defined as below:

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace MyProject.Issues
{
    public class IssueManager : DomainService
    {
        public const int MaxAllowedOpenIssueCountForAUser = 3;

        private readonly IIssueRepository _issueRepository;

        public IssueManager(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task AssignToUserAsync(Issue issue, Guid userId)
        {
            var issueCount = await _issueRepository.GetIssueCountOfUserAsync(userId);

            if (issueCount >= MaxAllowedOpenIssueCountForAUser)
            {
                throw new BusinessException(
                    code: "IM:00392",
                    message: $"You can not assign more" +
                             $"than {MaxAllowedOpenIssueCountForAUser} issues to a user!"
                );
            }

            issue.AssignedUserId = userId;
        }
    }
}
````

`IssueManager` depends on the `IssueRepository` service, that will be mocked in this example.

**Business Rule**: The example `AssignToUserAsync` doesn't allow to assign more than 3 (`MaxAllowedOpenIssueCountForAUser` constant) issues to a user. If you want to assign an issue in this case, you first need to unassign an existing issue.

The test case below tries to make a valid assignment:

````csharp
using System;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace MyProject.Issues
{
    public class IssueManager_Tests
    {
        [Fact]
        public async Task Should_Assign_An_Issue_To_A_User()
        {
            // Arrange

            var userId = Guid.NewGuid();

            var fakeRepo = Substitute.For<IIssueRepository>();
            fakeRepo.GetIssueCountOfUserAsync(userId).Returns(1);

            var issueManager = new IssueManager(fakeRepo);

            var issue = new Issue();

            // Act

            await issueManager.AssignToUserAsync(issue, userId);

            //Assert

            issue.AssignedUserId.ShouldBe(userId);
            await fakeRepo.Received(1).GetIssueCountOfUserAsync(userId);
        }
    }
}
````

* `Substitute.For<IIssueRepository>` creates a mock (fake) object that is passed into the `IssueManager` constructor.
* `fakeRepo.GetIssueCountOfUserAsync(userId).Returns(1)` ensures that the `GetIssueCountOfUserAsync` method of the repository returns `1`.
* `issueManager.AssignToUserAsync` doesn't throw any exception since the repository returns `1` for the currently assigned issue count.
* `issue.AssignedUserId.ShouldBe(userId);` line checks if the `AssignedUserId` has the correct value.
* `await fakeRepo.Received(1).GetIssueCountOfUserAsync(userId);` checks if the `IssueManager` called the `GetIssueCountOfUserAsync` method exactly one time.

Let's add a second test to see if it prevents to assign issues to a user more than the allowed count:

````csharp
[Fact]
public async Task Should_Not_Allow_To_Assign_Issues_Over_The_Limit()
{
    // Arrange

    var userId = Guid.NewGuid();

    var fakeRepo = Substitute.For<IIssueRepository>();
    fakeRepo
        .GetIssueCountOfUserAsync(userId)
        .Returns(IssueManager.MaxAllowedOpenIssueCountForAUser);

    var issueManager = new IssueManager(fakeRepo);

    // Act & Assert

    var issue = new Issue();

    await Assert.ThrowsAsync<BusinessException>(async () =>
    {
        await issueManager.AssignToUserAsync(issue, userId);
    });

    issue.AssignedUserId.ShouldBeNull();
    await fakeRepo.Received(1).GetIssueCountOfUserAsync(userId);
}
````

> For more information on the mocking, see the [NSubstitute](https://nsubstitute.github.io/) documentation.

It is relatively easy to mock a single dependency. But, when your dependencies grow, it gets harder to setup the test objects and mock all the dependencies. See the *Integration Tests* section that doesn't require mocking the dependencies.

## Tip: Share the Test Class Constructor

[xUnit](https://xunit.net/) creates a **new test class instance** (`IssueManager_Tests` for this example) for each test method. So, you can move some *Arrange* code into the constructor to reduce the code duplication. The constructor will be executed for each test case and doesn't affect each other, even if they work in parallel.

**Example: Refactor the `IssueManager_Tests` to reduce the code duplication**

````csharp
using System;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace MyProject.Issues
{
    public class IssueManager_Tests
    {
        private readonly Guid _userId;
        private readonly IIssueRepository _fakeRepo;
        private readonly IssueManager _issueManager;
        private readonly Issue _issue;

        public IssueManager_Tests()
        {
            _userId = Guid.NewGuid();
            _fakeRepo = Substitute.For<IIssueRepository>();
            _issueManager = new IssueManager(_fakeRepo);
            _issue = new Issue();
        }

        [Fact]
        public async Task Should_Assign_An_Issue_To_A_User()
        {
            // Arrange            
            _fakeRepo.GetIssueCountOfUserAsync(_userId).Returns(1);

            // Act
            await _issueManager.AssignToUserAsync(_issue, _userId);

            //Assert
            _issue.AssignedUserId.ShouldBe(_userId);
            await _fakeRepo.Received(1).GetIssueCountOfUserAsync(_userId);
        }

        [Fact]
        public async Task Should_Not_Allow_To_Assign_Issues_Over_The_Limit()
        {
            // Arrange
            _fakeRepo
                .GetIssueCountOfUserAsync(_userId)
                .Returns(IssueManager.MaxAllowedOpenIssueCountForAUser);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _issueManager.AssignToUserAsync(_issue, _userId);
            });

            _issue.AssignedUserId.ShouldBeNull();
            await _fakeRepo.Received(1).GetIssueCountOfUserAsync(_userId);
        }
    }
}
````

> Keep your test code clean to create a maintainable test suite.
