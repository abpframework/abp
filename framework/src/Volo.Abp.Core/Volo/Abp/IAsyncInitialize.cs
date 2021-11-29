using System.Threading.Tasks;

namespace Volo.Abp;

/// <summary>
/// IMPORTANT: THIS IS AN INTERNAL CLASS TO BE USED BY THE ABP FRAMEWORK.
/// IT WILL BE REMOVED IN THE FUTURE VERSIONS. DON'T USE IT!
/// </summary>
public interface IAsyncInitialize //TODO: Remove once we have async module initialization
{
    Task InitializeAsync();
}
