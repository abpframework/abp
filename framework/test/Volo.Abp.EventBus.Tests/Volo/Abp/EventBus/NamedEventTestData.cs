namespace Volo.Abp.EventBus;

[EventName("MyNamedEvent")]
public class NamedEventTestData
{
    public int Value { get; set; }

    public string? Name { get; set; }
}
