using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.WebAssembly;

namespace MyCompanyName.MyProjectName.Blazor.Pages
{
    public partial class Index
    {
        private IEnumerable<Claim> _claims;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if ( authState.User.Identity.IsAuthenticated )
            {
                _claims = authState.User.Claims;
            }
        }

        Task OnInfoTestClicked()
        {
            return UiMessageService.InfoAsync( "This is the Info message", "Info", options =>
            {
                options.OkButtonIcon = IconName.InfoCircle;
                options.OkButtonText = "Hello info";
            } );
        }

        Task OnSuccessTestClicked()
        {
            return UiMessageService.SuccessAsync( "This is the Success message", "Success" );
        }

        Task OnWarningTestClicked()
        {
            return UiMessageService.WarnAsync( "This is the Warning message", "Warning" );
        }

        Task OnErrorTestClicked()
        {
            return UiMessageService.ErrorAsync( "This is the Error message", "Error" );
        }

        Task OnConfirmTestClicked()
        {
            return UiMessageService.ConfirmAsync( "Are you sure you want to delete the item?", "Confirm", options =>
            {
                options.CancelButtonText = "Do not delete it";
                options.ConfirmButtonText = "Yes I'm sure";
            } )
                .ContinueWith( result =>
                 {
                     if ( result.Result )
                     {
                         Console.WriteLine( "Confirmed" );
                     }
                     else
                     {
                         Console.WriteLine( "Cancelled" );
                     }
                 } );
        }

        [Inject] IUiMessageService UiMessageService { get; set; }
    }
}
