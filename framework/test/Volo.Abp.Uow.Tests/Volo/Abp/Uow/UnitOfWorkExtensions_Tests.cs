using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Uow;

public class UnitOfWorkExtensions_Tests : AbpIntegratedTest<AbpUnitOfWorkModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWorkExtensions_Tests()
    {
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public void AddItem()
    {
        var uow = _unitOfWorkManager.Begin();
        uow.AddItem("testKey", "testValue");

        uow.Items.ShouldContainKey("testKey");
        uow.Items.ContainsValue("testValue");

        uow.AddItem("testKey", "testValue2");

        uow.Items.ShouldContainKey("testKey");
        uow.Items.ContainsValue("testValue2");
    }

    [Fact]
    public void GetItemOrDefault()
    {
        var uow = _unitOfWorkManager.Begin();
        uow.Items.Add("testKey", new NameValue("TestKey","TestValue"));

        uow.GetItemOrDefault<NameValue>("testKey").ShouldBeOfType<NameValue>();
        uow.GetItemOrDefault<NameValue>("testKey").Value.ShouldBe("TestValue");
    }

    [Fact]
    public void GetOrAddItem()
    {
        var uow = _unitOfWorkManager.Begin();

        var item = uow.GetOrAddItem("testKey", _ => new NameValue("TestKey", "TestValue"));

        item.Name.ShouldBe("TestKey");
        item.ShouldBeOfType<NameValue>();
        item.Value.ShouldBe("TestValue");
    }

    [Fact]
    public void RemoveItem()
    {
        var uow = _unitOfWorkManager.Begin();
        uow.Items.Add("testKey", "testValue");

        uow.RemoveItem("testKey");

        uow.Items.ShouldNotContainKey("testKey");
    }
}
