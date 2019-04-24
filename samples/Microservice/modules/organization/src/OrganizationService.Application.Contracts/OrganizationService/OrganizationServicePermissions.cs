namespace OrganizationService
{
    public class OrganizationServicePermissions
    {
        public const string GroupName = "OrganizationService";

        public static class AbpOrganizations
        {
            public const string Default = GroupName + ".AbpOrganizations";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";

        }

     
        public static string[] GetAll()
        {
            return new[]
            {
                GroupName,
                AbpOrganizations.Default,
                AbpOrganizations.Delete,
                AbpOrganizations.Update,
                AbpOrganizations.Create
            };
        }
    }
}