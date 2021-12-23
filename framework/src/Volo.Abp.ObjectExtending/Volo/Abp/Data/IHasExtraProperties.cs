namespace Volo.Abp.Data;

//TODO: Move to Volo.Abp.Data.ObjectExtending namespace at 4.0?

public interface IHasExtraProperties
{
    ExtraPropertyDictionary ExtraProperties { get; }
}
