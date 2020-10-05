using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.WebAssembly;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class UiMessageAlert : ComponentBase, IDisposable
    {
        protected override void OnInitialized()
        {
            UiMessageNotifierService.MessageReceived += UiMessageNotifierService_MessageReceived;

            base.OnInitialized();
        }

        private void UiMessageNotifierService_MessageReceived(object sender, UiMessageEventArgs e)
        {
            Message = e.Message;
            Title = e.Title;

            ModalRef.Show();
        }

        public void Dispose()
        {
            if (UiMessageNotifierService != null)
            {
                UiMessageNotifierService.MessageReceived -= UiMessageNotifierService_MessageReceived;
            }
        }

        protected Task OnCancelClicked()
        {
            ModalRef.Hide();

            return Task.CompletedTask;
        }

        protected Task OnOKClicked()
        {
            ModalRef.Hide();

            return Task.CompletedTask;
        }

        protected Modal ModalRef { get; set; }

        [Inject] IUiMessageNotifierService UiMessageNotifierService { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public string Message { get; set; }
    }
}
