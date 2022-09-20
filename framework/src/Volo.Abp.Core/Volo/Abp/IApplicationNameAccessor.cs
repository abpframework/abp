namespace Volo.Abp;

public interface IApplicationNameAccessor
{
    /// <summary>
    /// Name of the application.
    /// This is useful for systems with multiple applications, to distinguish
    /// resources of the applications located together.
    /// </summary>
    string ApplicationName { get; }
}