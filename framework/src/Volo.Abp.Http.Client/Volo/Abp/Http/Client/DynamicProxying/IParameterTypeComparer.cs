using System.Reflection;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public interface IParameterTypeComparer
    {
        /// <summary>
        /// Compares the given parameters.
        /// </summary>
        /// <param name="actionParameter">The parameter description retrieved from the server (depends on the server's runtime)</param>
        /// <param name="methodParameter">The local parameter info (depends on the client's runtime)</param>
        /// <returns></returns>
        bool TypeMatches(MethodParameterApiDescriptionModel actionParameter, ParameterInfo methodParameter);
    }
}
