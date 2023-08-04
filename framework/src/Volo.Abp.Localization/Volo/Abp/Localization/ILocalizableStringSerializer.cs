namespace Volo.Abp.Localization;

public interface ILocalizableStringSerializer
{
    string? Serialize(ILocalizableString localizableString);
    
    ILocalizableString Deserialize(string value);
}