namespace Volo.Abp.AspNetCore.Components.Messages;

/// <summary>
/// Options to override message dialog appearance.
/// </summary>
public class UiMessageOptions
{
    /// <summary>
    /// If true, the message dialogue will be centered on the screen.
    /// </summary>
    public bool CenterMessage { get; set; }

    /// <summary>
    /// If true, the message dialogue will show the large icon for the current message type.
    /// </summary>
    public bool ShowMessageIcon { get; set; }

    /// <summary>
    /// Overrides the build-in message icon.
    /// </summary>
    public object MessageIcon { get; set; }

    /// <summary>
    /// Custom text for the Ok button.
    /// </summary>
    public string OkButtonText { get; set; }

    /// <summary>
    /// Custom icon for the Ok button.
    /// </summary>
    public object OkButtonIcon { get; set; }

    /// <summary>
    /// Custom text for the Confirmation button.
    /// </summary>
    public string ConfirmButtonText { get; set; }

    /// <summary>
    /// Custom icon for the Confirmation button.
    /// </summary>
    public object ConfirmButtonIcon { get; set; }

    /// <summary>
    /// Custom text for the Cancel button.
    /// </summary>
    public string CancelButtonText { get; set; }

    /// <summary>
    /// Custom icon for the Cancel button.
    /// </summary>
    public object CancelButtonIcon { get; set; }
}
