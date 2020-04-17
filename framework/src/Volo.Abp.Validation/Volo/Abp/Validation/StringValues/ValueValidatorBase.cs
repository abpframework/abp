using System;
using System.Collections.Generic;

namespace Volo.Abp.Validation.StringValues
{
    [Serializable]
    public abstract class ValueValidatorBase : IValueValidator
    {
        public virtual string Name => ValueValidatorAttribute.GetName(GetType());

        public object this[string key]
        {
            get => Properties.GetOrDefault(key);
            set => Properties[key] = value;
        }

        public IDictionary<string, object> Properties { get; }

        protected ValueValidatorBase()
        {
            Properties = new Dictionary<string, object>();
        }

        public abstract bool IsValid(object value);
    }
}