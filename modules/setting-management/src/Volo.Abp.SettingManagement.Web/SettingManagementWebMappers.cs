using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using static Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.EmailSettingGroup.EmailSettingGroupViewComponent;
using static Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.EmailSettingGroup.SendTestEmailModal;

namespace Volo.Abp.SettingManagement.Web;

[Mapper]
public partial class EmailSettingsDtoToUpdateEmailSettingsViewModelMapper : MapperBase<EmailSettingsDto, UpdateEmailSettingsViewModel>
{
    public override partial UpdateEmailSettingsViewModel Map(EmailSettingsDto source);

    public override partial void Map(EmailSettingsDto source, UpdateEmailSettingsViewModel destination);
}

[Mapper]
public partial class SendTestEmailViewModelToSendTestEmailInputMapper : MapperBase<SendTestEmailViewModel, SendTestEmailInput>
{
    public override partial SendTestEmailInput Map(SendTestEmailViewModel source);

    public override partial void Map(SendTestEmailViewModel source, SendTestEmailInput destination);
}