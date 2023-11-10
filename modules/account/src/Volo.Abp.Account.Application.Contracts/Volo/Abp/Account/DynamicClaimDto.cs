namespace Volo.Abp.Account;

public class DynamicClaimDto
{
    public string Type { get; set; }

    public string Value { get; set; }

    public DynamicClaimDto(string type, string value)
    {
        Type = type;
        Value = value;
    }
}
