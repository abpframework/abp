using System;

namespace Volo.Docs
{
    public class DocsOptions
    {
        private string _routePrefix = "documents";

        /// <summary>
        /// Default value: "documents";
        /// </summary>
        public string RoutePrefix
        {
            get => GetFormattedRoutePrefix();
            set => _routePrefix = value;
        }

        /// <summary>
        /// Allows user to see a combobox in user interface for swapping across projects
        /// Default value: True;
        /// </summary>
        public bool ShowProjectsComboboxInUi = true;

        private string GetFormattedRoutePrefix()
        {
            if (string.IsNullOrWhiteSpace(_routePrefix))
            {
                return "/";
            }

            return _routePrefix.EnsureEndsWith('/').EnsureStartsWith('/');
        }
    }
}
