using System;
using System.Collections.Generic;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ActionApiDescriptionModel
    {
        public string Name { get; }

        public string NameOnClass { get; }

        public string HttpMethod { get; }

        public string Url { get; }

        //TODO: Add actual parameters on method

        public IList<ParameterApiDescriptionModel> Parameters { get; }

        public ReturnValueApiDescriptionModel ReturnValue { get; }

        private ActionApiDescriptionModel()
        {

        }

        public ActionApiDescriptionModel(string name, string nameOnClass, ReturnValueApiDescriptionModel returnValue, string url, string httpMethod = null)
        {
            Name = name;
            NameOnClass = nameOnClass;
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