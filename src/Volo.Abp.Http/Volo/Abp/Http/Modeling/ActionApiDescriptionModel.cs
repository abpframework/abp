using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ActionApiDescriptionModel
    {
        public string Name { get; set; }

        public string HttpMethod { get; set; }

        public string Url { get; set; }

        public IList<string> SupportedVersions { get; set; }

        public IList<MethodParameterApiDescriptionModel> ParametersOnMethod { get; set; }

        public IList<ParameterApiDescriptionModel> Parameters { get; set; }

        public ReturnValueApiDescriptionModel ReturnValue { get; set; }

        private ActionApiDescriptionModel()
        {

        }

        public static ActionApiDescriptionModel Create([NotNull] MethodInfo method, [NotNull] string url, [NotNull] string httpMethod, [NotNull] IList<string> supportedVersions)
        {
            Check.NotNull(method, nameof(method));
            Check.NotNull(url, nameof(url));
            Check.NotNull(httpMethod, nameof(httpMethod));
            Check.NotNull(supportedVersions, nameof(supportedVersions));

            return new ActionApiDescriptionModel
            {
                Name = method.Name,
                Url = url,
                HttpMethod = httpMethod,
                ReturnValue = ReturnValueApiDescriptionModel.Create(method.ReturnType),
                Parameters = new List<ParameterApiDescriptionModel>(),
                ParametersOnMethod = method
                    .GetParameters()
                    .Select(MethodParameterApiDescriptionModel.Create)
                    .ToList(),
                SupportedVersions = supportedVersions
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

        public override string ToString()
        {
            return $"[ActionApiDescriptionModel {Name}]";
        }
    }
}