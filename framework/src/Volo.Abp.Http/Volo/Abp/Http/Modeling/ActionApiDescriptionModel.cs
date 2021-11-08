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
        public string UniqueName { get; set; }

        public string Name { get; set; }

        public string HttpMethod { get; set; }

        public string Url { get; set; }

        public IList<string> SupportedVersions { get; set; }

        public IList<MethodParameterApiDescriptionModel> ParametersOnMethod { get; set; }

        public IList<ParameterApiDescriptionModel> Parameters { get; set; }

        public ReturnValueApiDescriptionModel ReturnValue { get; set; }

        public bool? AllowAnonymous { get; set; }

        public string ImplementFrom { get; set; }

        public ActionApiDescriptionModel()
        {

        }

        public static ActionApiDescriptionModel Create([NotNull] string uniqueName, [NotNull] MethodInfo method, [NotNull] string url, [CanBeNull] string httpMethod, [NotNull] IList<string> supportedVersions, bool? allowAnonymous = null, string implementFrom = null)
        {
            Check.NotNull(uniqueName, nameof(uniqueName));
            Check.NotNull(method, nameof(method));
            Check.NotNull(url, nameof(url));
            Check.NotNull(supportedVersions, nameof(supportedVersions));

            return new ActionApiDescriptionModel
            {
                UniqueName = uniqueName,
                Name = method.Name,
                Url = url,
                HttpMethod = httpMethod,
                ReturnValue = ReturnValueApiDescriptionModel.Create(method.ReturnType),
                Parameters = new List<ParameterApiDescriptionModel>(),
                ParametersOnMethod = method
                    .GetParameters()
                    .Select(MethodParameterApiDescriptionModel.Create)
                    .ToList(),
                SupportedVersions = supportedVersions,
                AllowAnonymous = allowAnonymous,
                ImplementFrom = implementFrom
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
