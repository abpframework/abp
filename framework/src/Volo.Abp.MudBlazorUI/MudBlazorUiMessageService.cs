using System;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MudBlazorUI;

[Dependency(ReplaceServices = true)]
public class MudBlazorUiMessageService : IUiMessageService, IScopedDependency
{
    /// <summary>
    /// An event raised after the message is received. Used to notify the message dialog.
    /// </summary>
    public event EventHandler<UiMessageEventArgs>? MessageReceived;

    private readonly IStringLocalizer<AbpUiResource> _localizer;

    public ILogger<MudBlazorUiMessageService> Logger { get; set; }

    public MudBlazorUiMessageService(IStringLocalizer<AbpUiResource> localizer)
    {
        _localizer = localizer;
        Logger = NullLogger<MudBlazorUiMessageService>.Instance;
    }

    public Task Info(string message, string? title = null, Action<UiMessageOptions>? options = null)
    {
        var uiMessageOptions = CreateDefaultOptions();
        options?.Invoke(uiMessageOptions);

        MessageReceived?.Invoke(this, new UiMessageEventArgs(UiMessageType.Info, message, title, uiMessageOptions));

        return Task.CompletedTask;
    }

    public Task Success(string message, string? title = null, Action<UiMessageOptions>? options = null)
    {
        var uiMessageOptions = CreateDefaultOptions();
        options?.Invoke(uiMessageOptions);

        MessageReceived?.Invoke(this, new UiMessageEventArgs(UiMessageType.Success, message, title, uiMessageOptions));

        return Task.CompletedTask;
    }

    public Task Warn(string message, string? title = null, Action<UiMessageOptions>? options = null)
    {
        var uiMessageOptions = CreateDefaultOptions();
        options?.Invoke(uiMessageOptions);

        MessageReceived?.Invoke(this, new UiMessageEventArgs(UiMessageType.Warning, message, title, uiMessageOptions));

        return Task.CompletedTask;
    }

    public Task Error(string message, string? title = null, Action<UiMessageOptions>? options = null)
    {
        var uiMessageOptions = CreateDefaultOptions();
        options?.Invoke(uiMessageOptions);

        MessageReceived?.Invoke(this, new UiMessageEventArgs(UiMessageType.Error, message, title, uiMessageOptions));

        return Task.CompletedTask;
    }

    public Task<bool> Confirm(string message, string? title = null, Action<UiMessageOptions>? options = null)
    {
        var uiMessageOptions = CreateDefaultOptions();
        options?.Invoke(uiMessageOptions);

        var callback = new TaskCompletionSource<bool>();

        MessageReceived?.Invoke(this, new UiMessageEventArgs(UiMessageType.Confirmation, message, title, uiMessageOptions, callback));

        return callback.Task;
    }

    protected virtual UiMessageOptions CreateDefaultOptions()
    {
        return new UiMessageOptions
        {
            CenterMessage = true,
            ShowMessageIcon = true,
            OkButtonText = _localizer["Ok"],
            CancelButtonText = _localizer["Cancel"],
            ConfirmButtonText = _localizer["Yes"],
        };
    }
}
