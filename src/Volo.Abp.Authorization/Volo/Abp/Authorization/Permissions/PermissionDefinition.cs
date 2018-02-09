namespace Volo.Abp.Authorization.Permissions
{
    public class PermissionDefinition
    {
        /// <summary>
        /// Unique name of the permission.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parent of this permission if one exists.
        /// If set, this permission can be granted only if parent is granted.
        /// </summary>
        public PermissionDefinition Parent { get; private set; }

        //TODO: Add Properties dictionary for custom stuff

        public PermissionDefinition(string name)
        {
            Name = name;
        }

        public PermissionDefinition CreateChild(string name)
        {
            return new PermissionDefinition(name) {Parent = this};
        }
    }
}