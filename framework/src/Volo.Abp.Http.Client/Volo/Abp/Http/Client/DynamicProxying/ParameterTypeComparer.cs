using System;
using System.Reflection;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class ParameterTypeComparer : IParameterTypeComparer
    {
        public virtual bool TypeMatches(MethodParameterApiDescriptionModel actionParameter, ParameterInfo methodParameter)
        {
            return actionParameter.TypeAsString == methodParameter.ParameterType.GetFullNameWithAssemblyName();
        }
    }
}
