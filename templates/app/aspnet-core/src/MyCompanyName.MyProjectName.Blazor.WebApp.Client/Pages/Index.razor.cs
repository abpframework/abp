using System.Threading.Tasks;

namespace MyCompanyName.MyProjectName.Blazor.WebApp.Client.Pages;

public partial class Index
{
    protected override void OnInitialized()
    {
        Alerts.Warning(
                "We will have a service interruption between 02:00 AM and 04:00 AM at October 23, 2023!",
                "Service Interruption");
    }

    //   protected override void OnAfterRender(bool firstRender)
    // {
    //     if (firstRender)
    //     {
            
    //     }
    //     base.OnAfterRender(firstRender);
    // }
}
