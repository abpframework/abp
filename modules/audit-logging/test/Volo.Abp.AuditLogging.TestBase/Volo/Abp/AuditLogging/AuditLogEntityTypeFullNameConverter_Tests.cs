using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.AuditLogging;

public abstract class AuditLogEntityTypeFullNameConverter_Tests<TStartupModule> : AuditLoggingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly AuditLogEntityTypeFullNameConverter _typeFullNameConverter;

    protected AuditLogEntityTypeFullNameConverter_Tests()
    {
        _typeFullNameConverter = GetRequiredService<AuditLogEntityTypeFullNameConverter>();
    }

    [Fact]
    public void AuditLogEntityTypeFullNameConverter_Test()
    {
        _typeFullNameConverter.Convert("MyType").ShouldBe("MyType");

        _typeFullNameConverter.Convert(typeof(string).FullName!).ShouldBe("System.String");
        _typeFullNameConverter.Convert(typeof(Guid).FullName!).ShouldBe("System.Guid");
        _typeFullNameConverter.Convert(typeof(Guid?).FullName!).ShouldBe("System.Guid?");
        _typeFullNameConverter.Convert(typeof(int).FullName!).ShouldBe("System.Int32");
        _typeFullNameConverter.Convert(typeof(long?).FullName!).ShouldBe("System.Int64?");
        _typeFullNameConverter.Convert(typeof(MyClass).FullName!).ShouldBe("Volo.Abp.AuditLogging.AuditLogEntityTypeFullNameConverter_Tests.MyClass");

        _typeFullNameConverter.Convert(typeof(ICollection<string>).FullName!).ShouldBe($"System.Collections.Generic.ICollection<System.String>");
        _typeFullNameConverter.Convert(typeof(Collection<int>).FullName!).ShouldBe($"System.Collections.ObjectModel.Collection<System.Int32>");
        _typeFullNameConverter.Convert(typeof(List<Guid>).FullName!).ShouldBe($"System.Collections.Generic.List<System.Guid>");
        _typeFullNameConverter.Convert(typeof(List<MyClass>).FullName!).ShouldBe($"System.Collections.Generic.List<Volo.Abp.AuditLogging.AuditLogEntityTypeFullNameConverter_Tests.MyClass>");

        _typeFullNameConverter.Convert(typeof(ICollection<long?>).FullName!).ShouldBe($"System.Collections.Generic.ICollection<System.Int64?>");
        _typeFullNameConverter.Convert(typeof(Collection<int?>).FullName!).ShouldBe($"System.Collections.ObjectModel.Collection<System.Int32?>");
        _typeFullNameConverter.Convert(typeof(List<Guid?>).FullName!).ShouldBe($"System.Collections.Generic.List<System.Guid?>");
    }

    public class MyClass
    {

    }
}
