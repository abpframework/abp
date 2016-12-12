using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.Data
{
    public interface IConnectionInfoResolver
    {
        ConnectionInfo Resolve();
    }

    public class ConnectionInfo
    {
        public string ConnectionString { get; set; }
    }
}
