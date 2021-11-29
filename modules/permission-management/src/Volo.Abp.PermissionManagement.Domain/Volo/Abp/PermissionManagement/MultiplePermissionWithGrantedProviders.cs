using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement
{
    public class MultiplePermissionWithGrantedProviders
    {
        public List<PermissionWithGrantedProviders> Result { get; }

        public MultiplePermissionWithGrantedProviders()
        {
            Result = new List<PermissionWithGrantedProviders>();
        }

        public MultiplePermissionWithGrantedProviders(string[] names)
        {
            Check.NotNull(names, nameof(names));

            Result = new List<PermissionWithGrantedProviders>();

            foreach (var name in names)
            {
                Result.Add(new PermissionWithGrantedProviders(name, false));
            }
        }
    }
}
