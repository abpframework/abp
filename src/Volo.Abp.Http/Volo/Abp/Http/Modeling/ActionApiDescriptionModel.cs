using System;
using System.Collections.Generic;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ActionApiDescriptionModel
    {
        public string Name { get; }

        public string HttpMethod { get; }

        public string Url { get; }

        public IList<ParameterApiDescriptionModel> Parameters { get; }

        public ReturnValueApiDescriptionModel ReturnValue { get; }

        private ActionApiDescriptionModel()
        {

        }

        public ActionApiDescriptionModel(string name, ReturnValueApiDescriptionModel returnValue, string url, string httpMethod = null)
        {
            Name = name;
            ReturnValue = returnValue;
            Url = url;
            HttpMethod = httpMethod;

            Parameters = new List<ParameterApiDescriptionModel>();
        }

        public ParameterApiDescriptionModel AddParameter(ParameterApiDescriptionModel parameter)
        {
            Parameters.Add(parameter);
            return parameter;
        }
    }
}