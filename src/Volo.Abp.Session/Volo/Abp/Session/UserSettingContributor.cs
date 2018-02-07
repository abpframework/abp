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

        public override async Task<string> GetOrNullAsync(string name, bool fallback)
        {
            if (CurrentUser.Id == null)
            {
                return null;
            }

            return await SettingStore.GetOrNullAsync(name, EntityType, CurrentUser.Id.Value.ToString());
        }

        public override async Task<string> GetOrNullAsync(string name, string entityId, bool fallback = true)
        {
            return await SettingStore.GetOrNullAsync(name, EntityType, entityId);
        }
    }
}