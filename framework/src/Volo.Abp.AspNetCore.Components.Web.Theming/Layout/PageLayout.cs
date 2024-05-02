using System.Collections.ObjectModel;
using System.ComponentModel;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Layout;

public class PageLayout : IScopedDependency, INotifyPropertyChanged
{
    private string title;

    // TODO: Consider using this property for setting Page Title too.
    public virtual string Title
    {
        get => title;
        set
        {
            title = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
        }
    }

    public virtual ObservableCollection<BreadcrumbItem> BreadcrumbItems { get; set; } = new();

    public virtual ObservableCollection<PageToolbarItem> ToolbarItems { get; set; } = new();

    public event PropertyChangedEventHandler PropertyChanged;
}