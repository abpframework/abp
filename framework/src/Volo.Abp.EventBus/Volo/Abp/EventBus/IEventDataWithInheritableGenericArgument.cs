namespace Volo.Abp.EventBus;

/// <summary>
/// This interface must be implemented by event data classes that
/// has a single generic argument and this argument will be used by inheritance. 
/// 
/// For example;
/// Assume that Student inherits From Person. When trigger an EntityCreatedEventData{Student},
/// EntityCreatedEventData{Person} is also triggered if EntityCreatedEventData implements
/// this interface.
/// </summary>
public interface IEventDataWithInheritableGenericArgument
{
    /// <summary>
    /// Gets arguments to create this class since a new instance of this class is created.
    /// </summary>
    /// <returns>Constructor arguments</returns>
    object[] GetConstructorArgs();
}
