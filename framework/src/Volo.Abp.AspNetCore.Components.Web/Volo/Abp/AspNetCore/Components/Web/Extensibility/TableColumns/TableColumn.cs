using System;
using System.Collections.Generic;
using System.Globalization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;

namespace Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;

public class TableColumn
{
    public string Title { get; set; } = default!;

    public string Data { get; set; } = default!;

    public string Width { get; set; } = default!;

    public string PropertyName { get; set; } = default!;

    public string? DisplayFormat { get; set; }

    public IFormatProvider DisplayFormatProvider { get; set; } = CultureInfo.CurrentCulture;

    public Type? Component { get; set; }

    public List<EntityAction> Actions { get; set; }

    public Func<object, string>? ValueConverter { get; set; }

    public bool Sortable { get; set; }

    public bool Visible { get; set; } = true;

    public TableColumn()
    {
        Actions = new List<EntityAction>();
    }
}
