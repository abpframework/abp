using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ActionApiDescriptionModel
    {
        public string UniqueName { get; set; }

        public string NameOnClass { get; set; }

        public string HttpMethod { get; set; }

        public string Url { get; set; }

        public IList<MethodParameterApiDescriptionModel> ParametersOnMethod { get; set; }

        public IList<ParameterApiDescriptionModel> Parameters { get; set; }

        public ReturnValueApiDescriptionModel ReturnValue { get; set; }

        private ActionApiDescriptionModel()
        {

        }

        public static ActionApiDescriptionModel Create(MethodInfo method, string uniqueName, string url, string httpMethod)
        {
            return new ActionApiDescriptionModel
            {
                UniqueName = uniqueName,
                NameOnClass = method.Name,
                Url = url,
                HttpMethod = httpMethod,
                ReturnValue = ReturnValueApiDescriptionModel.Create(method.ReturnType),
                Parameters = new List<ParameterApiDescriptionModel>(),
                ParametersOnMethod = method
                    .GetParameters()
                    .Select(MethodParameterApiDescriptionModel.Create)
                    .ToList()
            };
        }

        public ParameterApiDescriptionModel AddParameter(ParameterApiDescriptionModel parameter)
        {
            Parameters.Add(parameter);
            return parameter;
        }

        public HttpMethod GetHttpMethod()
        {
            return HttpMethodHelper.ConvertToHttpMethod(HttpMethod);
        }
    }
}