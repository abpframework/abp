using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

public class AbpIdentityErrorDescriber_Tests : AbpIdentityDomainTestBase
{
    [Fact]
    public void Should_Localize_Error_Messages()
    {
        var describer = GetRequiredService<IdentityErrorDescriber>();

        using (CultureHelper.Use("en"))
        {
            describer.DefaultError().Description.ShouldBe("An unknown failure has occurred.");
            describer.ConcurrencyFailure().Description.ShouldBe("Optimistic concurrency check has been failed. The entity you're working on has modified by another user. Please discard your changes and try again.");
            describer.PasswordMismatch().Description.ShouldBe("Incorrect password.");
            describer.InvalidToken().Description.ShouldBe("Invalid token.");
            describer.RecoveryCodeRedemptionFailed().Description.ShouldBe("Recovery code redemption failed.");
            describer.LoginAlreadyAssociated().Description.ShouldBe("A user with this login already exists.");
            describer.InvalidUserName("john").Description.ShouldBe("Username 'john' is invalid.");
            describer.InvalidEmail("john@abp.io").Description.ShouldBe("Email 'john@abp.io' is invalid.");
            describer.DuplicateUserName("john").Description.ShouldBe("Username 'john' is already taken.");
            describer.DuplicateEmail("john@abp.io").Description.ShouldBe("Email 'john@abp.io' is already taken.");
            describer.InvalidRoleName("admin").Description.ShouldBe("Role name 'admin' is invalid.");
            describer.DuplicateRoleName("admin").Description.ShouldBe("Role name 'admin' is already taken.");
            describer.UserAlreadyHasPassword().Description.ShouldBe("User already has a password set.");
            describer.UserLockoutNotEnabled().Description.ShouldBe("Lockout is not enabled for this user.");
            describer.UserAlreadyInRole("admin").Description.ShouldBe("User already in role 'admin'.");
            describer.UserNotInRole("admin").Description.ShouldBe("User is not in role 'admin'.");
            describer.PasswordTooShort(6).Description.ShouldBe("Password length must be greater than 6 characters.");
            describer.PasswordRequiresUniqueChars(3).Description.ShouldBe("Passwords must use at least 3 different characters.");
            describer.PasswordRequiresNonAlphanumeric().Description.ShouldBe("Password must contain at least one non-alphanumeric character.");
            describer.PasswordRequiresDigit().Description.ShouldBe("Passwords must have at least one digit ('0'-'9').");
            describer.PasswordRequiresLower().Description.ShouldBe("Passwords must have at least one lowercase ('a'-'z').");
            describer.PasswordRequiresUpper().Description.ShouldBe("Passwords must have at least one uppercase ('A'-'Z').");
        }

        using (CultureHelper.Use("tr"))
        {
            describer.DefaultError().Description.ShouldBe("Bilinmeyen bir hata oluştu.");
            describer.ConcurrencyFailure().Description.ShouldBe("İyimser eşzamanlılık denetimi başarısız oldu. Üzerinde çalıştığınız varlık başka bir kullanıcı tarafından değiştirildi. Lütfen değişikliklerinizi geri alın ve tekrar deneyin.");
            describer.PasswordMismatch().Description.ShouldBe("Hatalı şifre.");
            describer.InvalidToken().Description.ShouldBe("Geçersiz token.");
            describer.RecoveryCodeRedemptionFailed().Description.ShouldBe("Kurtarma kodu kullanılamadı.");
            describer.LoginAlreadyAssociated().Description.ShouldBe("Bu giriş bilgilerine sahip bir kullanıcı zaten var.");
            describer.InvalidUserName("john").Description.ShouldBe("'john' kullanıcı adı geçersiz.");
            describer.InvalidEmail("john@abp.io").Description.ShouldBe("'john@abp.io' email adresi hatalı.");
            describer.DuplicateUserName("john").Description.ShouldBe("'john' kullanıcı adı zaten alınmış.");
            describer.DuplicateEmail("john@abp.io").Description.ShouldBe("'john@abp.io' email adresi zaten alınmış.");
            describer.InvalidRoleName("admin").Description.ShouldBe("'admin' rol ismi geçersizdir.");
            describer.DuplicateRoleName("admin").Description.ShouldBe("'admin' rol ismi zaten alınmış.");
            describer.UserAlreadyHasPassword().Description.ShouldBe("Kullanıcının zaten bir şifresi var.");
            describer.UserLockoutNotEnabled().Description.ShouldBe("Bu kullanıcı için hesap kilitleme etkin değil.");
            describer.UserAlreadyInRole("admin").Description.ShouldBe("Kullanıcı zaten 'admin' rolünde.");
            describer.UserNotInRole("admin").Description.ShouldBe("Kullanıcı 'admin' rolünde değil.");
            describer.PasswordTooShort(6).Description.ShouldBe("Şifre uzunluğu 6 karakterden uzun olmalıdır.");
            describer.PasswordRequiresUniqueChars(3).Description.ShouldBe("Şifre en az 3 farklı karakter içermeli.");
            describer.PasswordRequiresNonAlphanumeric().Description.ShouldBe("Parola en az bir alfasayısal olmayan karakter içermelidir.");
            describer.PasswordRequiresDigit().Description.ShouldBe("Şifre en az bir sayı içermeli ('0'-'9').");
            describer.PasswordRequiresLower().Description.ShouldBe("Şifre en az bir küçük harf içermeli ('a'-'z').");
            describer.PasswordRequiresUpper().Description.ShouldBe("Şifre en az bir büyük harf içermeli ('A'-'Z').");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            describer.DefaultError().Description.ShouldBe("发生了一个未知错误。");
            describer.ConcurrencyFailure().Description.ShouldBe("乐观并发检查失败. 你正在处理的对象已被其他用户修改. 请放弃你的更改, 然后重试。");
            describer.PasswordMismatch().Description.ShouldBe("密码错误。");
            describer.InvalidToken().Description.ShouldBe("token无效。");
            describer.RecoveryCodeRedemptionFailed().Description.ShouldBe("恢复代码兑换失败。");
            describer.LoginAlreadyAssociated().Description.ShouldBe("此登录名的用户已存在。");
            describer.InvalidUserName("john").Description.ShouldBe("用户名 'john' 无效。");
            describer.InvalidEmail("john@abp.io").Description.ShouldBe("邮箱 'john@abp.io' 无效。");
            describer.DuplicateUserName("john").Description.ShouldBe("用户名 'john' 已存在。");
            describer.DuplicateEmail("john@abp.io").Description.ShouldBe("邮箱 'john@abp.io' 已存在。");
            describer.InvalidRoleName("admin").Description.ShouldBe("角色名 'admin' 无效。");
            describer.DuplicateRoleName("admin").Description.ShouldBe("角色名 'admin' 已存在。");
            describer.UserAlreadyHasPassword().Description.ShouldBe("用户已设置密码。");
            describer.UserLockoutNotEnabled().Description.ShouldBe("该用户未启用锁定。");
            describer.UserAlreadyInRole("admin").Description.ShouldBe("用户已具有角色 'admin'。");
            describer.UserNotInRole("admin").Description.ShouldBe("用户不具有 'admin' 角色。");
            describer.PasswordTooShort(6).Description.ShouldBe("密码长度必须大于 6 字符。");
            describer.PasswordRequiresUniqueChars(3).Description.ShouldBe("密码至少包含3个唯一字符。");
            describer.PasswordRequiresNonAlphanumeric().Description.ShouldBe("密码必须至少包含一个非字母数字字符。");
            describer.PasswordRequiresDigit().Description.ShouldBe("密码至少包含一位数字 ('0'-'9')。");
            describer.PasswordRequiresLower().Description.ShouldBe("密码至少包含一位小写字母 ('a'-'z')。");
            describer.PasswordRequiresUpper().Description.ShouldBe("密码至少包含一位大写字母 ('A'-'Z')。");
        }
    }

    [Fact]
    public async Task Should_Localize_UserManager_Errors()
    {
        using (GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var user = new IdentityUser(Guid.NewGuid(), "um_localize_user", "um_localize_user@abp.io");

            using (CultureHelper.Use("en"))
            {
                var userManager = GetRequiredService<IdentityUserManager>();
                var result = await userManager.CreateAsync(user, "abc", validatePassword: true);

                result.Succeeded.ShouldBeFalse();
                var message = string.Join("; ", result.Errors.Select(e => e.Description));
                message.ShouldContain("Password length must be greater than 6 characters.");
                message.ShouldContain("Password must contain at least one non-alphanumeric character.");
                message.ShouldContain("Passwords must have at least one digit ('0'-'9').");
                message.ShouldContain("Passwords must have at least one uppercase ('A'-'Z').");

                var invalidUser = new IdentityUser(Guid.NewGuid(), "invalid user?", "not-an-email");
                var invalidResult = await userManager.CreateAsync(invalidUser, "Abp123!");

                invalidResult.Succeeded.ShouldBeFalse();
                var invalidMessage = string.Join("; ", invalidResult.Errors.Select(e => e.Description));
                invalidMessage.ShouldContain("Username 'invalid user?' is invalid.");
                invalidMessage.ShouldContain("Email 'not-an-email' is invalid.");

                var firstUser = new IdentityUser(Guid.NewGuid(), "dup_user_en", "dup_user_en@abp.io");
                (await userManager.CreateAsync(firstUser, "Abp123!")).Succeeded.ShouldBeTrue();

                var duplicateUser = new IdentityUser(Guid.NewGuid(), "dup_user_en", "dup_user_en@abp.io");
                var duplicateResult = await userManager.CreateAsync(duplicateUser, "Abp123!");

                duplicateResult.Succeeded.ShouldBeFalse();
                var duplicateMessage = string.Join("; ", duplicateResult.Errors.Select(e => e.Description));
                duplicateMessage.ShouldContain("Username 'dup_user_en' is already taken.");
                duplicateMessage.ShouldContain("Email 'dup_user_en@abp.io' is already taken.");

                var persistedUser = await userManager.FindByIdAsync(firstUser.Id.ToString());
                var addPasswordResult = await userManager.AddPasswordAsync(persistedUser, "Another123!");

                addPasswordResult.Succeeded.ShouldBeFalse();
                addPasswordResult.Errors.ShouldContain(e => e.Description == "User already has a password set.");

                var roleManager = GetRequiredService<IdentityRoleManager>();
                var roleName = "localized_role_en";
                (await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), roleName))).Succeeded.ShouldBeTrue();

                var roleUser = new IdentityUser(Guid.NewGuid(), "role_user_en", "role_user_en@abp.io");
                (await userManager.CreateAsync(roleUser, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                (await userManager.AddToRoleAsync(roleUser, roleName)).Succeeded.ShouldBeTrue();
                var alreadyInRoleResult = await userManager.AddToRoleAsync(roleUser, roleName);
                alreadyInRoleResult.Succeeded.ShouldBeFalse();
                alreadyInRoleResult.Errors.ShouldContain(e => e.Description == $"User already in role '{roleName}'.");

                var notInRoleUser = new IdentityUser(Guid.NewGuid(), "notinrole_user_en", "notinrole_user_en@abp.io");
                (await userManager.CreateAsync(notInRoleUser, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                var notInRoleResult = await userManager.RemoveFromRoleAsync(notInRoleUser, roleName);
                notInRoleResult.Succeeded.ShouldBeFalse();
                notInRoleResult.Errors.ShouldContain(e => e.Description == $"User is not in role '{roleName}'.");

                var loginUser1 = new IdentityUser(Guid.NewGuid(), "login_user1_en", "login_user1_en@abp.io");
                (await userManager.CreateAsync(loginUser1, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                var loginInfo = new UserLoginInfo("Google", "assoc_key_en", "Google");
                (await userManager.AddLoginAsync(loginUser1, loginInfo)).Succeeded.ShouldBeTrue();

                var loginUser2 = new IdentityUser(Guid.NewGuid(), "login_user2_en", "login_user2_en@abp.io");
                (await userManager.CreateAsync(loginUser2, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                var loginConflict = await userManager.AddLoginAsync(loginUser2, loginInfo);
                loginConflict.Succeeded.ShouldBeFalse();
                loginConflict.Errors.ShouldContain(e => e.Description == "A user with this login already exists.");

                var noLowerUser = new IdentityUser(Guid.NewGuid(), "nolower_user_en", "nolower_user_en@abp.io");
                var noLowerResult = await userManager.CreateAsync(noLowerUser, "ABC123!");

                noLowerResult.Succeeded.ShouldBeFalse();
                var noLowerMessage = string.Join("; ", noLowerResult.Errors.Select(e => e.Description));
                noLowerMessage.ShouldContain("Passwords must have at least one lowercase ('a'-'z').");

                var options = GetRequiredService<IOptions<IdentityOptions>>().Value;
                var originalUniqueChars = options.Password.RequiredUniqueChars;
                options.Password.RequiredUniqueChars = 3;

                var uniqueUser = new IdentityUser(Guid.NewGuid(), "unique_user_en", "unique_user_en@abp.io");
                var uniqueResult = await userManager.CreateAsync(uniqueUser, "aaaaaa!");

                uniqueResult.Succeeded.ShouldBeFalse();
                var uniqueMessage = string.Join("; ", uniqueResult.Errors.Select(e => e.Description));
                uniqueMessage.ShouldContain("Passwords must use at least 3 different characters.");

                options.Password.RequiredUniqueChars = originalUniqueChars;

                var mismatchUser = new IdentityUser(Guid.NewGuid(), "mismatch_user_en", "mismatch_user_en@abp.io");
                (await userManager.CreateAsync(mismatchUser, "Abp123!")).Succeeded.ShouldBeTrue();

                var identityException = await Assert.ThrowsAsync<AbpIdentityResultException>(async () =>
                {
                    await userManager.ChangePasswordAsync(mismatchUser, "WrongOld123!", "NewAbp123!");
                });
                identityException.IdentityResult.Succeeded.ShouldBeFalse();
                identityException.IdentityResult.Errors.ShouldContain(e => e.Description == "Incorrect password.");

                var recoveryUser = new IdentityUser(Guid.NewGuid(), "recovery_user_en", "recovery_user_en@abp.io");
                ObjectHelper.TrySetProperty(recoveryUser, x => x.TwoFactorEnabled, () => true);

                (await userManager.CreateAsync(recoveryUser, "Abp123!")).Succeeded.ShouldBeTrue();
                var recoveryResult = await userManager.RedeemTwoFactorRecoveryCodeAsync(recoveryUser, "invalid-code");
                recoveryResult.Succeeded.ShouldBeFalse();
                recoveryResult.Errors.ShouldContain(e => e.Description == "Recovery code redemption failed.");

                var invalidRoleResult = await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), ""));
                invalidRoleResult.Succeeded.ShouldBeFalse();
                invalidRoleResult.Errors.ShouldContain(e => e.Description == "Role name '' is invalid.");

                var duplicateRoleName = "duplicate_role_en";
                (await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), duplicateRoleName))).Succeeded.ShouldBeTrue();
                var duplicateRoleResult = await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), duplicateRoleName));
                duplicateRoleResult.Succeeded.ShouldBeFalse();
                duplicateRoleResult.Errors.ShouldContain(e => e.Description == $"Role name '{duplicateRoleName}' is already taken.");
            }

            // Recreate a fresh user per culture to avoid state issues
            user = new IdentityUser(Guid.NewGuid(), "um_localize_user_tr", "um_localize_user_tr@abp.io");
            using (CultureHelper.Use("tr"))
            {
                var userManager = GetRequiredService<IdentityUserManager>();
                var result = await userManager.CreateAsync(user, "abc", validatePassword: true);

                result.Succeeded.ShouldBeFalse();
                var message = string.Join("; ", result.Errors.Select(e => e.Description));
                message.ShouldContain("Şifre uzunluğu 6 karakterden uzun olmalıdır.");
                message.ShouldContain("Parola en az bir alfasayısal olmayan karakter içermelidir.");
                message.ShouldContain("Şifre en az bir sayı içermeli ('0'-'9').");
                message.ShouldContain("Şifre en az bir büyük harf içermeli ('A'-'Z').");

                var invalidUser = new IdentityUser(Guid.NewGuid(), "invalid user?", "not-an-email");
                var invalidResult = await userManager.CreateAsync(invalidUser, "Abp123!");

                invalidResult.Succeeded.ShouldBeFalse();
                var invalidMessage = string.Join("; ", invalidResult.Errors.Select(e => e.Description));
                invalidMessage.ShouldContain("'invalid user?' kullanıcı adı geçersiz.");
                invalidMessage.ShouldContain("'not-an-email' email adresi hatalı.");

                var firstUser = new IdentityUser(Guid.NewGuid(), "dup_user_tr", "dup_user_tr@abp.io");
                (await userManager.CreateAsync(firstUser, "Abp123!")).Succeeded.ShouldBeTrue();

                var duplicateUser = new IdentityUser(Guid.NewGuid(), "dup_user_tr", "dup_user_tr@abp.io");
                var duplicateResult = await userManager.CreateAsync(duplicateUser, "Abp123!");

                duplicateResult.Succeeded.ShouldBeFalse();
                var duplicateMessage = string.Join("; ", duplicateResult.Errors.Select(e => e.Description));
                duplicateMessage.ShouldContain("'dup_user_tr' kullanıcı adı zaten alınmış.");
                duplicateMessage.ShouldContain("'dup_user_tr@abp.io' email adresi zaten alınmış.");

                var persistedUser = await userManager.FindByIdAsync(firstUser.Id.ToString());
                var addPasswordResult = await userManager.AddPasswordAsync(persistedUser, "Another123!");

                addPasswordResult.Succeeded.ShouldBeFalse();
                addPasswordResult.Errors.ShouldContain(e => e.Description == "Kullanıcının zaten bir şifresi var.");

                var roleManager = GetRequiredService<IdentityRoleManager>();
                var roleName = "localized_role_tr";
                (await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), roleName))).Succeeded.ShouldBeTrue();

                var roleUser = new IdentityUser(Guid.NewGuid(), "role_user_tr", "role_user_tr@abp.io");
                (await userManager.CreateAsync(roleUser, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                (await userManager.AddToRoleAsync(roleUser, roleName)).Succeeded.ShouldBeTrue();
                var alreadyInRoleResult = await userManager.AddToRoleAsync(roleUser, roleName);
                alreadyInRoleResult.Succeeded.ShouldBeFalse();
                alreadyInRoleResult.Errors.ShouldContain(e => e.Description == $"Kullanıcı zaten '{roleName}' rolünde.");

                var notInRoleUser = new IdentityUser(Guid.NewGuid(), "notinrole_user_tr", "notinrole_user_tr@abp.io");
                (await userManager.CreateAsync(notInRoleUser, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                var notInRoleResult = await userManager.RemoveFromRoleAsync(notInRoleUser, roleName);
                notInRoleResult.Succeeded.ShouldBeFalse();
                notInRoleResult.Errors.ShouldContain(e => e.Description == $"Kullanıcı '{roleName}' rolünde değil.");

                var loginUser1 = new IdentityUser(Guid.NewGuid(), "login_user1_tr", "login_user1_tr@abp.io");
                (await userManager.CreateAsync(loginUser1, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                var loginInfo = new UserLoginInfo("Google", "assoc_key_tr", "Google");
                (await userManager.AddLoginAsync(loginUser1, loginInfo)).Succeeded.ShouldBeTrue();

                var loginUser2 = new IdentityUser(Guid.NewGuid(), "login_user2_tr", "login_user2_tr@abp.io");
                (await userManager.CreateAsync(loginUser2, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                var loginConflict = await userManager.AddLoginAsync(loginUser2, loginInfo);
                loginConflict.Succeeded.ShouldBeFalse();
                loginConflict.Errors.ShouldContain(e => e.Description == "Bu giriş bilgilerine sahip bir kullanıcı zaten var.");

                var noLowerUser = new IdentityUser(Guid.NewGuid(), "nolower_user_tr", "nolower_user_tr@abp.io");
                var noLowerResult = await userManager.CreateAsync(noLowerUser, "ABC123!");

                noLowerResult.Succeeded.ShouldBeFalse();
                var noLowerMessage = string.Join("; ", noLowerResult.Errors.Select(e => e.Description));
                noLowerMessage.ShouldContain("Şifre en az bir küçük harf içermeli ('a'-'z').");

                var options = GetRequiredService<IOptions<IdentityOptions>>().Value;
                var originalUniqueChars = options.Password.RequiredUniqueChars;
                options.Password.RequiredUniqueChars = 3;

                var uniqueUser = new IdentityUser(Guid.NewGuid(), "unique_user_tr", "unique_user_tr@abp.io");
                var uniqueResult = await userManager.CreateAsync(uniqueUser, "aaaaaa!");

                uniqueResult.Succeeded.ShouldBeFalse();
                var uniqueMessage = string.Join("; ", uniqueResult.Errors.Select(e => e.Description));
                uniqueMessage.ShouldContain("Şifre en az 3 farklı karakter içermeli.");

                options.Password.RequiredUniqueChars = originalUniqueChars;

                var mismatchUser = new IdentityUser(Guid.NewGuid(), "mismatch_user_tr", "mismatch_user_tr@abp.io");
                (await userManager.CreateAsync(mismatchUser, "Abp123!")).Succeeded.ShouldBeTrue();

                var identityException = await Assert.ThrowsAsync<AbpIdentityResultException>(async () =>
                {
                    await userManager.ChangePasswordAsync(mismatchUser, "WrongOld123!", "NewAbp123!");
                });
                identityException.IdentityResult.Succeeded.ShouldBeFalse();
                identityException.IdentityResult.Errors.ShouldContain(e => e.Description == "Hatalı şifre.");

                var recoveryUser = new IdentityUser(Guid.NewGuid(), "recovery_user_tr", "recovery_user_tr@abp.io");
                ObjectHelper.TrySetProperty(recoveryUser, x => x.TwoFactorEnabled, () => true);

                (await userManager.CreateAsync(recoveryUser, "Abp123!")).Succeeded.ShouldBeTrue();
                var recoveryResult = await userManager.RedeemTwoFactorRecoveryCodeAsync(recoveryUser, "invalid-code");
                recoveryResult.Succeeded.ShouldBeFalse();
                recoveryResult.Errors.ShouldContain(e => e.Description == "Kurtarma kodu kullanılamadı.");

                var invalidRoleResult = await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), ""));
                invalidRoleResult.Succeeded.ShouldBeFalse();
                invalidRoleResult.Errors.ShouldContain(e => e.Description == "'' rol ismi geçersizdir.");

                var duplicateRoleName = "duplicate_role_tr";
                (await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), duplicateRoleName))).Succeeded.ShouldBeTrue();
                var duplicateRoleResult = await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), duplicateRoleName));
                duplicateRoleResult.Succeeded.ShouldBeFalse();
                duplicateRoleResult.Errors.ShouldContain(e => e.Description == $"'{duplicateRoleName}' rol ismi zaten alınmış.");
            }

            user = new IdentityUser(Guid.NewGuid(), "um_localize_user_zh", "um_localize_user_zh@abp.io");
            using (CultureHelper.Use("zh-Hans"))
            {
                var userManager = GetRequiredService<IdentityUserManager>();
                var result = await userManager.CreateAsync(user, "abc", validatePassword: true);

                result.Succeeded.ShouldBeFalse();
                var message = string.Join("; ", result.Errors.Select(e => e.Description));
                message.ShouldContain("密码长度必须大于 6 字符。");
                message.ShouldContain("密码必须至少包含一个非字母数字字符。");
                message.ShouldContain("密码至少包含一位数字 ('0'-'9')。");
                message.ShouldContain("密码至少包含一位大写字母 ('A'-'Z')。");

                var invalidUser = new IdentityUser(Guid.NewGuid(), "invalid user?", "not-an-email");
                var invalidResult = await userManager.CreateAsync(invalidUser, "Abp123!");

                invalidResult.Succeeded.ShouldBeFalse();
                var invalidMessage = string.Join("; ", invalidResult.Errors.Select(e => e.Description));
                invalidMessage.ShouldContain("用户名 'invalid user?' 无效。");
                invalidMessage.ShouldContain("邮箱 'not-an-email' 无效。");

                var firstUser = new IdentityUser(Guid.NewGuid(), "dup_user_zh", "dup_user_zh@abp.io");
                (await userManager.CreateAsync(firstUser, "Abp123!")).Succeeded.ShouldBeTrue();

                var duplicateUser = new IdentityUser(Guid.NewGuid(), "dup_user_zh", "dup_user_zh@abp.io");
                var duplicateResult = await userManager.CreateAsync(duplicateUser, "Abp123!");

                duplicateResult.Succeeded.ShouldBeFalse();
                var duplicateMessage = string.Join("; ", duplicateResult.Errors.Select(e => e.Description));
                duplicateMessage.ShouldContain("用户名 'dup_user_zh' 已存在。");
                duplicateMessage.ShouldContain("邮箱 'dup_user_zh@abp.io' 已存在。");

                var persistedUser = await userManager.FindByIdAsync(firstUser.Id.ToString());
                var addPasswordResult = await userManager.AddPasswordAsync(persistedUser, "Another123!");

                addPasswordResult.Succeeded.ShouldBeFalse();
                addPasswordResult.Errors.ShouldContain(e => e.Description == "用户已设置密码。");

                var roleManager = GetRequiredService<IdentityRoleManager>();
                var roleName = "localized_role_zh";
                (await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), roleName))).Succeeded.ShouldBeTrue();

                var roleUser = new IdentityUser(Guid.NewGuid(), "role_user_zh", "role_user_zh@abp.io");
                (await userManager.CreateAsync(roleUser, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                (await userManager.AddToRoleAsync(roleUser, roleName)).Succeeded.ShouldBeTrue();
                var alreadyInRoleResult = await userManager.AddToRoleAsync(roleUser, roleName);
                alreadyInRoleResult.Succeeded.ShouldBeFalse();
                alreadyInRoleResult.Errors.ShouldContain(e => e.Description == $"用户已具有角色 '{roleName}'。");

                var notInRoleUser = new IdentityUser(Guid.NewGuid(), "notinrole_user_zh", "notinrole_user_zh@abp.io");
                (await userManager.CreateAsync(notInRoleUser, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                var notInRoleResult = await userManager.RemoveFromRoleAsync(notInRoleUser, roleName);
                notInRoleResult.Succeeded.ShouldBeFalse();
                notInRoleResult.Errors.ShouldContain(e => e.Description == $"用户不具有 '{roleName}' 角色。");

                var loginUser1 = new IdentityUser(Guid.NewGuid(), "login_user1_zh", "login_user1_zh@abp.io");
                (await userManager.CreateAsync(loginUser1, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                var loginInfo = new UserLoginInfo("Google", "assoc_key_zh", "Google");
                (await userManager.AddLoginAsync(loginUser1, loginInfo)).Succeeded.ShouldBeTrue();

                var loginUser2 = new IdentityUser(Guid.NewGuid(), "login_user2_zh", "login_user2_zh@abp.io");
                (await userManager.CreateAsync(loginUser2, "Abp123!"))
                    .Succeeded.ShouldBeTrue();

                var loginConflict = await userManager.AddLoginAsync(loginUser2, loginInfo);
                loginConflict.Succeeded.ShouldBeFalse();
                loginConflict.Errors.ShouldContain(e => e.Description == "此登录名的用户已存在。");

                var noLowerUser = new IdentityUser(Guid.NewGuid(), "nolower_user_zh", "nolower_user_zh@abp.io");
                var noLowerResult = await userManager.CreateAsync(noLowerUser, "ABC123!");

                noLowerResult.Succeeded.ShouldBeFalse();
                var noLowerMessage = string.Join("; ", noLowerResult.Errors.Select(e => e.Description));
                noLowerMessage.ShouldContain("密码至少包含一位小写字母 ('a'-'z')。");

                var options = GetRequiredService<IOptions<IdentityOptions>>().Value;
                var originalUniqueChars = options.Password.RequiredUniqueChars;
                options.Password.RequiredUniqueChars = 3;

                var uniqueUser = new IdentityUser(Guid.NewGuid(), "unique_user_zh", "unique_user_zh@abp.io");
                var uniqueResult = await userManager.CreateAsync(uniqueUser, "aaaaaa!");

                uniqueResult.Succeeded.ShouldBeFalse();
                var uniqueMessage = string.Join("; ", uniqueResult.Errors.Select(e => e.Description));
                uniqueMessage.ShouldContain("密码至少包含3个唯一字符。");

                options.Password.RequiredUniqueChars = originalUniqueChars;

                var mismatchUser = new IdentityUser(Guid.NewGuid(), "mismatch_user_zh", "mismatch_user_zh@abp.io");
                (await userManager.CreateAsync(mismatchUser, "Abp123!")).Succeeded.ShouldBeTrue();

                var identityException = await Assert.ThrowsAsync<AbpIdentityResultException>(async () =>
                {
                    await userManager.ChangePasswordAsync(mismatchUser, "WrongOld123!", "NewAbp123!");
                });
                identityException.IdentityResult.Succeeded.ShouldBeFalse();
                identityException.IdentityResult.Errors.ShouldContain(e => e.Description == "密码错误。");

                var recoveryUser = new IdentityUser(Guid.NewGuid(), "recovery_user_zh", "recovery_user_zh@abp.io");
                ObjectHelper.TrySetProperty(recoveryUser, x => x.TwoFactorEnabled, () => true);

                (await userManager.CreateAsync(recoveryUser, "Abp123!")).Succeeded.ShouldBeTrue();
                var recoveryResult = await userManager.RedeemTwoFactorRecoveryCodeAsync(recoveryUser, "invalid-code");
                recoveryResult.Succeeded.ShouldBeFalse();
                recoveryResult.Errors.ShouldContain(e => e.Description == "恢复代码兑换失败。");

                var invalidRoleResult = await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), ""));
                invalidRoleResult.Succeeded.ShouldBeFalse();
                invalidRoleResult.Errors.ShouldContain(e => e.Description == "角色名 '' 无效。");

                var duplicateRoleName = "duplicate_role_zh";
                (await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), duplicateRoleName))).Succeeded.ShouldBeTrue();
                var duplicateRoleResult = await roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), duplicateRoleName));
                duplicateRoleResult.Succeeded.ShouldBeFalse();
                duplicateRoleResult.Errors.ShouldContain(e => e.Description == $"角色名 '{duplicateRoleName}' 已存在。");
            }
        }
    }
}
