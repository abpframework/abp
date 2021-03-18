using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Data
{
    public class AbpDatabaseInfoDictionary : Dictionary<string, AbpDatabaseInfo>
    {
        private Dictionary<string, AbpDatabaseInfo> ConnectionIndex { get; set; }

        public AbpDatabaseInfoDictionary()
        {
            ConnectionIndex = new Dictionary<string, AbpDatabaseInfo>();
        }
        
        [CanBeNull]
        public AbpDatabaseInfo GetMappedDatabaseOrNull(string connectionStringName)
        {
            return ConnectionIndex.GetOrDefault(connectionStringName);
        }

        public AbpDatabaseInfoDictionary Configure(string databaseName, Action<AbpDatabaseInfo> configureAction)
        {
            var databaseInfo = this.GetOrAdd(
                databaseName,
                () => new AbpDatabaseInfo(databaseName)
            );
            
            configureAction(databaseInfo);
            
            return this;
        }

        /// <summary>
        /// This method should be called if this dictionary changes.
        /// It refreshes indexes for quick access to the connection informations.
        /// </summary>
        public void RefreshIndexes()
        {
            ConnectionIndex = new Dictionary<string, AbpDatabaseInfo>();
            
            foreach (var databaseInfo in Values)
            {
                foreach (var mappedConnection in databaseInfo.MappedConnections)
                {
                    if (ConnectionIndex.ContainsKey(mappedConnection))
                    {
                        throw new AbpException(
                            $"A connection name can not map to multiple databases: {mappedConnection}."
                        );
                    }

                    ConnectionIndex[mappedConnection] = databaseInfo;
                }
            }
        }
    }
}