using Microsoft.AspNetCore.Mvc;
using System;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Contents.Components.EntityContentEditor
{
    public class ContentViewModel
    {
        public ContentViewModel(string entityType, string entityId, string value = null, Guid? id = null)
        {
            EntityType = entityType;
            EntityId = entityId;
            Value = value;
            Id = id;
        }

        [HiddenInput]
        public string EntityType { get; set; }

        [HiddenInput]
        public string EntityId { get; set; }

        [HiddenInput]
        public string Value { get; set; }

        [HiddenInput]
        public Guid? Id { get; set; }
    }
}
