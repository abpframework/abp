using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Mapperly;

public class MapExtraPropertiesDefaultSeed_Tests : AbpIntegratedTest<MapperlyTestModule>
{
    private readonly IObjectMapper _objectMapper;

    public MapExtraPropertiesDefaultSeed_Tests()
    {
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public void Single_Parameter_Map_Should_Preserve_Constructor_Seeded_Properties()
    {
        var entity = new ExtensibleSeededEntity { Id = Guid.NewGuid() }
            .SetProperty("Tag", "ok");

        var dto = _objectMapper.Map<ExtensibleSeededEntity, ExtensibleSeededDto>(entity);

        dto.GetProperty<string>("Tag").ShouldBe("ok"); //Defined in both classes
        dto.GetProperty<string>("CreatedBy").ShouldBe("system"); //Set by the ExtensibleSeededDto constructor
        dto.HasProperty("DtoOnly").ShouldBeTrue(); //Not defined in the source, but was set to the default value by the ExtensibleObject constructor
        dto.GetProperty("DtoOnly").ShouldBeNull();
    }

    [Fact]
    public void Single_Parameter_Map_Should_Not_Change_The_Source_Extra_Properties()
    {
        var entity = new ExtensibleSeededEntity { Id = Guid.NewGuid() }
            .SetProperty("Tag", "ok")
            .SetProperty("CreatedBy", "leaked");
        var originalReference = entity.ExtraProperties;

        _objectMapper.Map<ExtensibleSeededEntity, ExtensibleSeededDto>(entity);

        ReferenceEquals(entity.ExtraProperties, originalReference).ShouldBeTrue();
        entity.GetProperty<string>("Tag").ShouldBe("ok");
        entity.GetProperty<string>("CreatedBy").ShouldBe("leaked");
    }

    [Fact]
    public void Single_Parameter_Map_Should_Not_Seed_Defaults_When_Destination_Disables_Them()
    {
        var entity = new ExtensibleSeededEntity { Id = Guid.NewGuid() }
            .SetProperty("Tag", "ok");

        var dto = _objectMapper.Map<ExtensibleSeededEntity, ExtensibleNonSeededDto>(entity);

        dto.GetProperty<string>("Tag").ShouldBe("ok"); //Defined in both classes
        dto.HasProperty("DtoOnly").ShouldBeFalse(); //ExtensibleNonSeededDto constructor disables the default values seeding
    }

    [Fact]
    public void Single_Parameter_Map_Should_Not_Leak_Filtered_Source_Values_Into_Registered_Keys()
    {
        var entity = new ExtensibleSeededEntity { Id = Guid.NewGuid() }
            .SetProperty("Tag", "ok")
            .SetProperty("DtoOnly", "leaked");

        var dto = _objectMapper.Map<ExtensibleSeededEntity, ExtensibleNonSeededDto>(entity);

        dto.GetProperty<string>("Tag").ShouldBe("ok"); //Defined in both classes
        dto.GetProperty("DtoOnly").ShouldBeNull(); //Not defined in the source, the source value must not leak
    }

    [Fact]
    public void Single_Parameter_Map_Should_Not_Invoke_Default_Value_Factories_Without_MapExtraProperties_Attribute()
    {
        var entity = new ExtensibleSeededEntity { Id = Guid.NewGuid() }
            .SetProperty("Tag", "ok");

        var callsBefore = ExtensibleNoAttributeDto.CountedDefaultValueFactoryCalls;

        var dto = _objectMapper.Map<ExtensibleSeededEntity, ExtensibleNoAttributeDto>(entity);

        (ExtensibleNoAttributeDto.CountedDefaultValueFactoryCalls - callsBefore).ShouldBe(1); //Only the ExtensibleNoAttributeDto constructor seeding
        dto.HasProperty("Counted").ShouldBeTrue();
    }
}
