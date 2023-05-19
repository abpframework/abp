using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Uow;

public class TestUnitOfWorkConfig : ISingletonDependency
{
    public const string ExceptionOnCompleteMessage = "TestUnitOfWork configured for exception";

    public bool ThrowExceptionOnComplete { get; set; }
}
