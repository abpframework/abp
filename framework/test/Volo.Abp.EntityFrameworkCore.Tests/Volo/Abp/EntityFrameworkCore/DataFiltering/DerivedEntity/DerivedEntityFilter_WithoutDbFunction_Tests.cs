namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

public class DerivedEntityFilter_WithoutDbFunction_Tests : DerivedEntityFilterTestBase<DerivedEntityFilterTestModuleWithoutDbFunction>
{
    protected override bool UseDbFunction => false;
}
