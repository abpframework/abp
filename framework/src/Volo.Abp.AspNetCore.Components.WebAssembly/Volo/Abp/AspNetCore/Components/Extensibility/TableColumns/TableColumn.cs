using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Components.Extensibility.EntityActions;

namespace Volo.Abp.AspNetCore.Components.Extensibility.TableColumns
{
    public class TableColumn
    {
        public string Title { get; set; }
        public string Data { get; set; }
        [CanBeNull]
        public Func<object, RenderFragment> Render { get; set; }
        public List<EntityAction> Actions { get; set; }

        public TableColumn()
        {
            Actions = new List<EntityAction>();
        }
    }
}