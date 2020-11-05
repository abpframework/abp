using System;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ReturnValueApiDescriptionModel
    {
        public string Type { get; set; }

        public string TypeSimple { get; set; }

        public ReturnValueApiDescriptionModel()
        {

        }

        public static ReturnValueApiDescriptionModel Create(Type type)
        {
            var unwrappedType = AsyncHelper.UnwrapTask(type);

            return new ReturnValueApiDescriptionModel
            {
                Type = TypeHelper.GetFullNameHandlingNullableAndGenerics(unwrappedType),
                TypeSimple = ApiTypeNameHelper.GetSimpleTypeName(unwrappedType)
            };
        }
    }
}
