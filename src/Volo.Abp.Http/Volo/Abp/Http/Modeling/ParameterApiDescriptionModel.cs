using System;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ParameterApiDescriptionModel
    {
        public string NameOnMethod { get; }

        public string Name { get; }

        public Type Type { get; }

        public string TypeAsString { get; }

        public bool IsOptional { get;  }

        public object DefaultValue { get;  }

        public string[] ConstraintTypes { get; }

        public string BindingSourceId { get; }

        private ParameterApiDescriptionModel()
        {
            
        }

        public ParameterApiDescriptionModel(string name, string nameOnMethod, Type type, bool isOptional = false, object defaultValue = null, string[] constraintTypes = null, string bindingSourceId = null)
        {
            Name = name;
            NameOnMethod = nameOnMethod;
            Type = type;
            TypeAsString = type.FullName;
            IsOptional = isOptional;
            DefaultValue = defaultValue;
            ConstraintTypes = constraintTypes;
            BindingSourceId = bindingSourceId;
        }
    }
}