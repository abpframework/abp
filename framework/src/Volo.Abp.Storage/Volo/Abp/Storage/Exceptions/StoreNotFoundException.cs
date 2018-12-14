using System;

namespace Volo.Abp.Storage.Exceptions
{
    public class StoreNotFoundException : Exception
    {
        public StoreNotFoundException(string storeName)
            : base(
                $"The configured store '{storeName}' was not found. Did you configure it properly in your appsettings.json?")
        {
        }
    }
}