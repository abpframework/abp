﻿using System.Threading.Tasks;
using Volo.Abp.Users;

namespace Volo.Abp.Settings
{
    public class UserSettingValueProvider : SettingValueProvider
    {
        public const string ProviderName = "U";

        public override string Name => ProviderName;

        protected ICurrentUser CurrentUser { get; }

        public UserSettingValueProvider(ISettingStore settingStore, ICurrentUser currentUser)
            : base(settingStore)
        {
            CurrentUser = currentUser;
        }

        public override async Task<string> GetOrNullAsync(SettingDefinition setting)
        {
            if (CurrentUser.Id == null)
            {
                return null;
            }

            return await SettingStore.GetOrNullAsync(setting.Name, Name, CurrentUser.Id.ToString()).ConfigureAwait(false);
        }
    }
}