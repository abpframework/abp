using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using static Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement.EmailSettingGroup.EmailSettingGroupViewComponent;

namespace Volo.Abp.SettingManagement.Blazor;

[Mapper]
public partial class UpdateEmailSettingsViewModelToUpdateEmailSettingsDtoMapper : MapperBase<UpdateEmailSettingsViewModel, UpdateEmailSettingsDto>
{
    public override partial UpdateEmailSettingsDto Map(UpdateEmailSettingsViewModel source);

    public override partial void Map(UpdateEmailSettingsViewModel source, UpdateEmailSettingsDto destination);
}


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

