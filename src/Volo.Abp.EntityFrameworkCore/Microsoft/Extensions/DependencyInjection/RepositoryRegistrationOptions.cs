using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class RepositoryRegistrationOptions
    {
        public bool RegisterDefaultRepositories { get; set; }

        public bool IncludeAllEntitiesForDefaultRepositories { get; set; }

        public Dictionary<Type, Type> CustomRepositories { get; set; }

        public RepositoryRegistrationOptions()
        {
            CustomRepositories = new Dictionary<Type, Type>();
        }
    }
}