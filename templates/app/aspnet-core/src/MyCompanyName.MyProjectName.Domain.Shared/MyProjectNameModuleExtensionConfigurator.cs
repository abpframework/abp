using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace MyCompanyName.MyProjectName
{
    public static class MyProjectNameModuleExtensionConfigurator
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                ConfigureExistingProperties();
                ConfigureExtraProperties();
            });
        }

        private static void ConfigureExistingProperties()
        {
            /* You can change max lengths for properties of the
             * entities defined in the modules used by your application.
             *
             * Example: Change user and role name max lengths

               IdentityUserConsts.MaxNameLength = 99;
               IdentityRoleConsts.MaxNameLength = 99;

             * Notice: It is not suggested to change property lengths
             * unless you really need it. Go with the standard values wherever possible.
             *
             * If you are using EF Core, you will need to run the add-migration command after your changes.
             */
        }

        public static void ConfigureExtraProperties()
        {
            OneTimeRunner.Run(() =>
            {
                ObjectExtensionManager.Instance.Modules()
                    .ConfigureIdentity(identity =>
                    {
                        identity.ConfigureUser(user =>
                        {
                            user.AddOrUpdateProperty<string>( //property type: string
                                "SocialSecurityNumber", //property name
                                property =>
                                {
                                    //validation rules
                                    property.Attributes.Add(new RequiredAttribute());
                                    property.Attributes.Add(
                                        new StringLengthAttribute(64) {
                                            MinimumLength = 4
                                        }
                                    );

                                    property.UI.OnTable.IsVisible = false;

                                    property.Validators.Add(context =>
                                    {
                                        if (((string) context.Value).StartsWith("B"))
                                        {
                                            context.ValidationErrors.Add(
                                                new ValidationResult(
                                                    "Social security number can not start with the letter 'B', sorry!",
                                                    new[] {"extraProperties.SocialSecurityNumber"}
                                                )
                                            );
                                        }
                                    });
                                }
                            );
                        });
                    });
            });
        }
    }
}
