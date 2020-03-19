using System;
using Volo.Abp.Threading;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ReturnValueApiDescriptionModel
    {
        public string Type { get; set; }

        public string TypeSimple { get; set; }

        private ReturnValueApiDescriptionModel()
        {

        }

        public static ReturnValueApiDescriptionModel Create(Type type)
        {
            var unwrappedType = AsyncHelper.UnwrapTask(type);

            return new ReturnValueApiDescriptionModel
            {
                Type = ModelingTypeHelper.GetFullNameHandlingNullableAndGenerics(unwrappedType),
                TypeSimple = ModelingTypeHelper.GetSimplifiedName(unwrappedType)
            };
        }
    }
}