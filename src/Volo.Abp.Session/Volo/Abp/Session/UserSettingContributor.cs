using System;
using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace Volo.Abp.Session
{
    //TODO: Optimization: Get all settings and cache it!

    public class UserSettingContributor : SettingContributor
    {
        public const string DefaultEntityType = "User";

        public override string EntityType => DefaultEntityType;

        protected ICurrentUser CurrentUser { get; }

        public UserSettingContributor(ISettingStore settingStore, ICurrentUser currentUser)
            : base(settingStore)
        {
            CurrentUser = currentUser;
        }

        public override async Task<string> GetOrNullAsync(SettingDefinition setting, string entityId)
        {
            if (entityId == null)
            {
                if (CurrentUser.Id == null)
                {
                    return null;
                }

                entityId = CurrentUser.Id.ToString();
            }

            return await SettingStore.GetOrNullAsync(setting.Name, EntityType, entityId);
        }
    }
}