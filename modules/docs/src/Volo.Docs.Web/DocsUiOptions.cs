using System;

namespace Volo.Docs
{
    public class DocsUiOptions
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
        public bool ShowProjectsCombobox = true;
        
        /// <summary>
        /// Allows user to see a label in user interface for projects combobox
        /// Default value: True;
        /// </summary>
        public bool ShowProjectsComboboxLabel = true;

        /// <summary>
        /// If true, allows to create sections in document and show/hide sections according to user preferences.
        /// Default value: True;
        /// </summary>
        public bool SectionRendering = true;
        
        public bool MultiLanguageMode { get; set; } = true;
        
        public SingleProjectModeOptions SingleProjectMode { get; } = new ();
        
        public bool EnableEnlargeImage { get; set; } = true;

        private string GetFormattedRoutePrefix()
        {
            if (string.IsNullOrWhiteSpace(_routePrefix))
            {
                return "/";
            }

            return _routePrefix.EnsureEndsWith('/').EnsureStartsWith('/');
        }
    }
    
    public class SingleProjectModeOptions
    {
        /// <summary>
        /// Determines whether to enable single project mode by removing the project name from the routing.
        /// When enabled, only a single project is allowed within the module.
        /// </summary>
        public bool Enable { get; set; }
        
        public string ProjectName { get; set; }
    }
}
