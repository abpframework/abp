using System;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ReturnValueApiDescriptionModel
    {
        public string TypeAsString { get; set; }

        private ReturnValueApiDescriptionModel()
        {
            
        }

        public static ReturnValueApiDescriptionModel Create(Type type)
        {
            return new ReturnValueApiDescriptionModel
            {
                TypeAsString = type.GetFullNameWithAssemblyName()
            };
        }
    }
}