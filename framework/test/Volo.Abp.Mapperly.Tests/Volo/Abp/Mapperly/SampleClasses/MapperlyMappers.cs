using System;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.Mapperly;
using Volo.Abp.Mapperly.SampleClasses;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.TestObjects;

[Mapper]
public partial class MyEntityMapper : MapperBase<MyEntity, MyEntityDto>
{
    public override partial MyEntityDto Map(MyEntity source);

    public override partial void Map(MyEntity source, MyEntityDto destination);
}

[Mapper]
public partial class MyEnumMapper : MapperBase<MyEnum, MyEnumDto>
{
    public override partial MyEnumDto Map(MyEnum source);

    public override void Map(MyEnum source, MyEnumDto destination)
    {
        destination = Map(source);
    }
}

[Mapper]
[MapExtraProperties(IgnoredProperties = ["CityName"])]
public partial class ExtensibleTestPersonMapper : MapperBase<ExtensibleTestPerson, ExtensibleTestPersonDto>
{
    public override partial ExtensibleTestPersonDto Map(ExtensibleTestPerson source);

    public override partial void Map(ExtensibleTestPerson source, ExtensibleTestPersonDto destination);
}

[Mapper]
[MapExtraProperties(MapToRegularProperties = true)]
public partial class ExtensibleTestPersonWithRegularPropertiesDtoMapper : MapperBase<ExtensibleTestPerson, ExtensibleTestPersonWithRegularPropertiesDto>
{
    [MapperIgnoreTarget(nameof(ExtensibleTestPersonWithRegularPropertiesDto.Name))]
    [MapperIgnoreTarget(nameof(ExtensibleTestPersonWithRegularPropertiesDto.Age))]
    [MapperIgnoreTarget(nameof(ExtensibleTestPersonWithRegularPropertiesDto.IsActive))]
    public override partial ExtensibleTestPersonWithRegularPropertiesDto Map(ExtensibleTestPerson source);

    public override partial void Map(ExtensibleTestPerson source, ExtensibleTestPersonWithRegularPropertiesDto destination);
}

// Test entities for ExtraProperties dictionary reference tests
public class TestEntityWithExtraProperties : ExtensibleObject
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TestEntityDtoWithExtraProperties : ExtensibleObject
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TestEntityWithReadonlyExtraProperties : IHasExtraProperties
{
    private readonly ExtraPropertyDictionary _extraProperties = new();

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ExtraPropertyDictionary ExtraProperties => _extraProperties;
}

[Mapper]
public partial class TestEntityWithExtraPropertiesMapper : MapperBase<TestEntityWithExtraProperties, TestEntityDtoWithExtraProperties>
{
    public override partial TestEntityDtoWithExtraProperties Map(TestEntityWithExtraProperties source);

    public override partial void Map(TestEntityWithExtraProperties source, TestEntityDtoWithExtraProperties destination);
}

[Mapper]
public partial class TestEntityWithReadonlyExtraPropertiesMapper : MapperBase<TestEntityWithReadonlyExtraProperties, TestEntityWithReadonlyExtraProperties>
{
    public override partial TestEntityWithReadonlyExtraProperties Map(TestEntityWithReadonlyExtraProperties source);

    public override partial void Map(TestEntityWithReadonlyExtraProperties source, TestEntityWithReadonlyExtraProperties destination);
}

public class ExtensibleReverseEntity : ExtensibleObject
{
    public Guid Id { get; set; }
}

public class ExtensibleReverseDto : ExtensibleObject
{
    public Guid Id { get; set; }
}

[Mapper]
[MapExtraProperties(IgnoredProperties = ["Secret"])]
public partial class ExtensibleReverseMapper : TwoWayMapperBase<ExtensibleReverseEntity, ExtensibleReverseDto>
{
    public override partial ExtensibleReverseDto Map(ExtensibleReverseEntity source);

    public override partial void Map(ExtensibleReverseEntity source, ExtensibleReverseDto destination);

    public override partial ExtensibleReverseEntity ReverseMap(ExtensibleReverseDto source);

    public override partial void ReverseMap(ExtensibleReverseDto source, ExtensibleReverseEntity destination);
}

public class ExtensibleSeededEntity : ExtensibleObject
{
    public Guid Id { get; set; }
}

public class ExtensibleSeededDto : ExtensibleObject
{
    public Guid Id { get; set; }

    public ExtensibleSeededDto()
    {
        this.SetProperty("CreatedBy", "system");
    }
}

public class ExtensibleNonSeededDto : ExtensibleObject
{
    public Guid Id { get; set; }

    public ExtensibleNonSeededDto()
        : base(false)
    {
    }
}

public class ExtensibleNoAttributeDto : ExtensibleObject
{
    public static int CountedDefaultValueFactoryCalls;

    public Guid Id { get; set; }
}

[Mapper]
[MapExtraProperties]
public partial class ExtensibleSeededMapper : MapperBase<ExtensibleSeededEntity, ExtensibleSeededDto>
{
    public override partial ExtensibleSeededDto Map(ExtensibleSeededEntity source);

    public override partial void Map(ExtensibleSeededEntity source, ExtensibleSeededDto destination);
}

[Mapper]
[MapExtraProperties]
public partial class ExtensibleNonSeededMapper : MapperBase<ExtensibleSeededEntity, ExtensibleNonSeededDto>
{
    public override partial ExtensibleNonSeededDto Map(ExtensibleSeededEntity source);

    public override partial void Map(ExtensibleSeededEntity source, ExtensibleNonSeededDto destination);
}

[Mapper]
public partial class ExtensibleNoAttributeMapper : MapperBase<ExtensibleSeededEntity, ExtensibleNoAttributeDto>
{
    public override partial ExtensibleNoAttributeDto Map(ExtensibleSeededEntity source);

    public override partial void Map(ExtensibleSeededEntity source, ExtensibleNoAttributeDto destination);
}