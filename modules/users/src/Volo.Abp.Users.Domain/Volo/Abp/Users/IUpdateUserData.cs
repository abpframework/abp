using JetBrains.Annotations;

namespace Volo.Abp.Users
{
    public interface IUpdateUserData
    {
        bool Update([NotNull] IUserData user);
    }
}