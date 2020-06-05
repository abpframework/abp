using System.Collections.Generic;

namespace Volo.Abp.UI.Navigation.Urls
{
    public class ApplicationUrlDictionary
    {
        private readonly IDictionary<string, ApplicationUrlInfo> _applications;

        public ApplicationUrlInfo this[string appName]
        {
            get
            {
                if (!_applications.ContainsKey(appName))
                {
                    _applications[appName] = new ApplicationUrlInfo();
                }

                return _applications[appName];
            }
        }

        public ApplicationUrlDictionary()
        {
            _applications = new Dictionary<string, ApplicationUrlInfo>();
        }
    }
}
