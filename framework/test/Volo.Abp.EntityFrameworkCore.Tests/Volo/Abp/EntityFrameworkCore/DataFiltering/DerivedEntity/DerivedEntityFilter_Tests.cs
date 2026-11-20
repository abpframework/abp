namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

public class DerivedEntityFilter_Tests : DerivedEntityFilterTestBase<DerivedEntityFilterTestModule>
{
    protected override bool UseDbFunction => true;
}
