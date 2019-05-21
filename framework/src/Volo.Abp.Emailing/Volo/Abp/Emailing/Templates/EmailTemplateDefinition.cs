using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateDefinition
    {
        public const string DefaultLayoutPlaceHolder = "_";

        public string Name { get; }

        public bool IsLayout { get; }

        public string Layout { get; set; }

        public Type LocalizationResource { get; set; }

        public EmailTemplateContributorList Contributors { get; }

        public string DefaultCultureName { get; set; }

        public EmailTemplateDefinition([NotNull]string name, Type localizationResource = null, bool isLayout = false, string layout = DefaultLayoutPlaceHolder, string defaultCultureName = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            LocalizationResource = localizationResource;
            Contributors = new EmailTemplateContributorList();
            IsLayout = isLayout;
            Layout = layout;
            DefaultCultureName = defaultCultureName;
        }
    }
}