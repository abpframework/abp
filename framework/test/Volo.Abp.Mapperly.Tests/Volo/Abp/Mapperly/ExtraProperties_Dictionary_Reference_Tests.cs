using System;
using Microsoft.Extensions.DependencyInjection;
using Riok.Mapperly.Abstractions;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Mapperly.SampleClasses;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Mapperly;

public class ExtraProperties_Dictionary_Reference_Tests : AbpIntegratedTest<MapperlyTestModule>
{
    private readonly IObjectMapper _objectMapper;

    public ExtraProperties_Dictionary_Reference_Tests()
    {
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public void Should_Create_New_ExtraProperties_Dictionary_When_Same_Reference()
    {
        // Arrange: Create a shared ExtraProperties dictionary
        var sharedExtraProperties = new ExtraPropertyDictionary
        {
            {"TestProperty", "TestValue"},
            {"NumberProperty", 42}
        };

        var source = new TestEntityWithExtraProperties
        {
            Id = Guid.NewGuid(),
            Name = "Source Entity"
        };
        
        var destination = new TestEntityDtoWithExtraProperties
        {
            Id = Guid.NewGuid(),
            Name = "Destination DTO"
        };

        // Make both source and destination reference the same ExtraProperties dictionary
        SetExtraPropertiesReference(source, sharedExtraProperties);
        SetExtraPropertiesReference(destination, sharedExtraProperties);

        // Verify they have the same reference before mapping
        ReferenceEquals(source.ExtraProperties, destination.ExtraProperties).ShouldBeTrue();
        source.ExtraProperties.Count.ShouldBe(2);
        destination.ExtraProperties.Count.ShouldBe(2);

        // Act: Perform mapping
        _objectMapper.Map(source, destination);

        // Assert: After mapping, they should have different references
        // This is the key fix: when ExtraProperties references are the same,
        // a new dictionary should be created for the destination
        ReferenceEquals(source.ExtraProperties, destination.ExtraProperties).ShouldBeFalse();
        
        // But content should be preserved
        destination.ExtraProperties["TestProperty"].ShouldBe("TestValue");
        destination.ExtraProperties["NumberProperty"].ShouldBe(42);
        destination.ExtraProperties.Count.ShouldBe(source.ExtraProperties.Count);
    }

    [Fact]
    public void Should_Not_Create_New_Dictionary_When_Different_References()
    {
        // Arrange: Create source and destination with different ExtraProperties references
        var source = new TestEntityWithExtraProperties
        {
            Id = Guid.NewGuid(),
            Name = "Source Entity"
        };
        source.SetProperty("SourceProperty", "SourceValue");

        var destination = new TestEntityDtoWithExtraProperties
        {
            Id = Guid.NewGuid(),
            Name = "Destination DTO"
        };
        destination.SetProperty("DestinationProperty", "DestinationValue");

        var originalSourceReference = source.ExtraProperties;

        // Verify they have different references before mapping
        ReferenceEquals(source.ExtraProperties, destination.ExtraProperties).ShouldBeFalse();

        // Act: Perform mapping
        _objectMapper.Map(source, destination);

        // Assert: Source reference should remain unchanged
        ReferenceEquals(source.ExtraProperties, originalSourceReference).ShouldBeTrue();
        
        // Destination reference may change due to normal mapping process, but should not be same as source
        ReferenceEquals(source.ExtraProperties, destination.ExtraProperties).ShouldBeFalse();
        
        destination.ExtraProperties["SourceProperty"].ShouldBe("SourceValue");
        destination.ExtraProperties["DestinationProperty"].ShouldBe("DestinationValue");
    }

    [Fact]
    public void Should_Handle_Readonly_ExtraProperties_Gracefully()
    {
        // Arrange: Create entities with readonly ExtraProperties
        var source = new TestEntityWithReadonlyExtraProperties
        {
            Id = Guid.NewGuid(),
            Name = "Source Entity"
        };
        source.ExtraProperties.Add("TestProperty", "TestValue");

        var destination = new TestEntityWithReadonlyExtraProperties
        {
            Id = Guid.NewGuid(),
            Name = "Destination Entity"
        };

        // Make them reference the same ExtraProperties
        var sharedExtraProperties = new ExtraPropertyDictionary
        {
            {"SharedProperty", "SharedValue"}
        };
        SetReadonlyExtraPropertiesReference(source, sharedExtraProperties);
        SetReadonlyExtraPropertiesReference(destination, sharedExtraProperties);

        // Verify they have the same reference
        ReferenceEquals(source.ExtraProperties, destination.ExtraProperties).ShouldBeTrue();

        // Act & Assert: Should not throw exception even if setter is not available
        Should.NotThrow(() => _objectMapper.Map(source, destination));
    }

    private static void SetExtraPropertiesReference(TestEntityWithExtraProperties entity, ExtraPropertyDictionary extraProperties)
    {
        // Use reflection to set the protected setter from ExtensibleObject
        var propertyInfo = typeof(ExtensibleObject).GetProperty(nameof(ExtensibleObject.ExtraProperties));
        propertyInfo?.SetValue(entity, extraProperties);
    }

    private static void SetExtraPropertiesReference(TestEntityDtoWithExtraProperties entity, ExtraPropertyDictionary extraProperties)
    {
        // Use reflection to set the protected setter from ExtensibleObject
        var propertyInfo = typeof(ExtensibleObject).GetProperty(nameof(ExtensibleObject.ExtraProperties));
        propertyInfo?.SetValue(entity, extraProperties);
    }

    private static void SetReadonlyExtraPropertiesReference(TestEntityWithReadonlyExtraProperties entity, ExtraPropertyDictionary extraProperties)
    {
        // Use reflection to set the private field
        var fieldInfo = typeof(TestEntityWithReadonlyExtraProperties).GetField("_extraProperties", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fieldInfo?.SetValue(entity, extraProperties);
    }
}

