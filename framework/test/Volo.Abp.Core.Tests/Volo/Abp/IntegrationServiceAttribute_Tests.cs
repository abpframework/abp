using Shouldly;
using Xunit;

namespace Volo.Abp;

public class IntegrationServiceAttribute_Tests
{
    [Fact]
    public static void IsDefinedOrInherited()
    {
        // True cases
        
        IntegrationServiceAttribute
            .IsDefinedOrInherited<IMyIntegrationService1>()
            .ShouldBeTrue();   

        IntegrationServiceAttribute
            .IsDefinedOrInherited<MyIntegrationService1>()
            .ShouldBeTrue();   
        
        IntegrationServiceAttribute
            .IsDefinedOrInherited<MyIntegrationService2>()
            .ShouldBeTrue();
        
        // False cases

        IntegrationServiceAttribute
            .IsDefinedOrInherited<IMyIntegrationService2>()
            .ShouldBeFalse();  
        
        IntegrationServiceAttribute
            .IsDefinedOrInherited<MyApplicationService>()
            .ShouldBeFalse(); 
        
    }

    [IntegrationService]
    private interface IMyIntegrationService1 { }
    private class MyIntegrationService1 : IMyIntegrationService1 { }

    private interface IMyIntegrationService2 { }
    [IntegrationService]
    private class MyIntegrationService2 : IMyIntegrationService2 { }

    private interface IMyApplicationService { }
    private class MyApplicationService : IMyApplicationService { }
}

