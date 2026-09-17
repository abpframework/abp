using System;
using Riok.Mapperly.Abstractions;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Mapperly;

public class MyDIClass
{
    public string Id { get; set; }

    public DateTime Birthday { get; set; }
}

public class MyDIClassDto
{
    public string Id { get; set; }

    public DateTime Birthday { get; set; }
}

public class BirthdayCalculatorService : ITransientDependency
{
    public DateTime Birthday => DateTime.Parse("2025-01-01");
}

[Mapper]
public partial class MyDIClassMapper : MapperBase<MyDIClass, MyDIClassDto>
{
    private readonly BirthdayCalculatorService _birthdayCalculatorService;

    public MyDIClassMapper(BirthdayCalculatorService birthdayCalculatorService)
    {
        _birthdayCalculatorService = birthdayCalculatorService;
    }

    public override partial MyDIClassDto Map(MyDIClass source);

    public override partial void Map(MyDIClass source, MyDIClassDto destination);

    public override void AfterMap(MyDIClass source, MyDIClassDto destination)
    {
        destination.Birthday = _birthdayCalculatorService.Birthday;
    }
}

public class AbpMapperly_Dependency_Injection_Tests : AbpIntegratedTest<MapperlyTestModule>
{
    private readonly IObjectMapper _objectMapper;
    private readonly BirthdayCalculatorService _birthdayCalculatorService;

    public AbpMapperly_Dependency_Injection_Tests()
    {
        _objectMapper = GetRequiredService<IObjectMapper>();
        _birthdayCalculatorService = GetRequiredService<BirthdayCalculatorService>();
    }

    [Fact]
    public void DI_Test()
    {
        var myClass = new MyDIClass { Id = "1", Birthday = DateTime.Now };
        var myClassDto = _objectMapper.Map<MyDIClass, MyDIClassDto>(myClass);
        myClassDto.Birthday.ShouldBe(_birthdayCalculatorService.Birthday);
    }
}
