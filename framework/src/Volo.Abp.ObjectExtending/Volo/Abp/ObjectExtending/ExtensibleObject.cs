using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    [Serializable]
    public class ExtensibleObject : IHasExtraProperties
    {
        public Dictionary<string, object> ExtraProperties { get; protected set; }

        public ExtensibleObject()
        {
            ExtraProperties = new Dictionary<string, object>();
        }
    }
}
