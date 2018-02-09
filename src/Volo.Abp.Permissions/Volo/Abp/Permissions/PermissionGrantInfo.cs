using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public class PermissionGrantInfo
    {
        public string Name { get; }

        public bool IsGranted { get; }

        public PermissionGrantInfo([NotNull] string name, bool isGranted)
        {
            Check.NotNull(name, nameof(name));

            Name = name;
            IsGranted = isGranted;
        }
    }
}