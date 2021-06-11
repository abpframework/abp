using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class UiMessageAlert : ComponentBase, IDisposable
    {
        protected Modal ModalRef { get; set; }

        protected virtual bool IsConfirmation
            => MessageType == UiMessageType.Confirmation;

        protected virtual bool CenterMessage
           => Options?.CenterMessage ?? true;

        protected virtual bool ShowMessageIcon
           => Options?.ShowMessageIcon ?? true;

        protected virtual object MessageIcon => Options?.MessageIcon ?? MessageType switch
        {
            UiMessageType.Info => IconName.Info,
            UiMessageType.Success => IconName.Check,
            UiMessageType.Warning => IconName.Exclamation,
            UiMessageType.Error => IconName.Times,
            UiMessageType.Confirmation => IconName.QuestionCircle,
            _ => null,
        };

        protected virtual string MessageIconColor => MessageType switch
        {
            // gets the color in the order of importance: Blazorise > Bootstrap > fallback color
            UiMessageType.Info => "var(--b-theme-info, var(--info, #17a2b8))",
            UiMessageType.Success => "var(--b-theme-success, var(--success, #28a745))",
            UiMessageType.Warning => "var(--b-theme-warning, var(--warning, #ffc107))",
            UiMessageType.Error => "var(--b-theme-danger, var(--danger, #dc3545))",
            UiMessageType.Confirmation => "var(--b-theme-secondary, var(--secondary, #6c757d))",
            _ => null,
        };

        protected virtual string MessageIconStyle
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append($"color:{MessageIconColor}");

                return sb.ToString();
            }
        }

        protected virtual string OkButtonText
            => Options?.OkButtonText ?? "OK";

        protected virtual string ConfirmButtonText
            => Options?.ConfirmButtonText ?? "Confirm";

        protected virtual string CancelButtonText
            => Options?.CancelButtonText ?? "Cancel";

        [Parameter] public UiMessageType MessageType { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public string Message { get; set; }

        [Parameter] public TaskCompletionSource<bool> Callback { get; set; }

        [Parameter] public UiMessageOptions Options { get; set; }

        [Parameter] public EventCallback Okayed { get; set; }

        [Parameter] public EventCallback Confirmed { get; set; }

        [Parameter] public EventCallback Canceled { get; set; }

        [Inject] protected BlazoriseUiMessageService UiMessageService { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            UiMessageService.MessageReceived += OnMessageReceived;
        }

        private async void OnMessageReceived(object sender, UiMessageEventArgs e)
        {
            MessageType = e.MessageType;
            Message = e.Message;
            Title = e.Title;
            Options = e.Options;
            Callback = e.Callback;

            await InvokeAsync(ModalRef.Show);
        }

        public void Dispose()
        {
            if (UiMessageService != null)
            {
                UiMessageService.MessageReceived -= OnMessageReceived;
            }
        }

        protected async Task OnOkClicked()
        {
            await InvokeAsync(async () =>
            {
                await Okayed.InvokeAsync(null);

                ModalRef.Hide();
            });
        }

        protected async Task OnConfirmClicked()
        {
            await InvokeAsync(async () =>
            {
                ModalRef.Hide();

                if (IsConfirmation && Callback != null)
                {
                    await InvokeAsync(() => Callback.SetResult(true));
                }

                await Confirmed.InvokeAsync(null);
            });
        }

        protected async Task OnCancelClicked()
        {
            await InvokeAsync(async () =>
            {
                ModalRef.Hide();

                if (IsConfirmation && Callback != null)
                {
                    await InvokeAsync(() => Callback.SetResult(false));
                }

                await Canceled.InvokeAsync(null);
            });
        }

        protected virtual void OnModalClosing(ModalClosingEventArgs eventArgs)
        {
            eventArgs.Cancel = eventArgs.CloseReason == CloseReason.EscapeClosing
                || eventArgs.CloseReason == CloseReason.FocusLostClosing;
        }
    }
}
