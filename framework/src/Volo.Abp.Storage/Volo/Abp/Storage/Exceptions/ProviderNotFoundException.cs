using System;

namespace Volo.Abp.Storage.Exceptions
{
    public class ProviderNotFoundException : Exception
    {
        public ProviderNotFoundException(string providerName)
            : base(
                $"The configured provider '{providerName}' was not found. Did you forget to register providers in your Startup.ConfigureServices?")
        {
        }
    }
}