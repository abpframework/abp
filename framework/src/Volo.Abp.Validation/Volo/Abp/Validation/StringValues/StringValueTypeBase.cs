using System;
using System.Collections.Generic;

namespace Volo.Abp.Validation.StringValues
{
    [Serializable]
    public abstract class StringValueTypeBase : IStringValueType
    {
        public virtual string Name => ValueValidatorAttribute.GetName(GetType());

        public object this[string key]
        {
            get => Properties.GetOrDefault(key);
            set => Properties[key] = value;
        }

        public Dictionary<string, object> Properties { get; }

        public IValueValidator Validator { get; set; }

        protected StringValueTypeBase()
            : this(new AlwaysValidValueValidator())
        {

        }

        protected StringValueTypeBase(IValueValidator validator)
        {
            Validator = validator;
            Properties = new Dictionary<string, object>();
        }
    }
}