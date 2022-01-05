using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Serializable]
public class CurrentUserDto
{
    public bool IsAuthenticated { get; set; }

    public Guid? Id { get; set; }

    public Guid? TenantId { get; set; }

    public Guid? ImpersonatorUserId { get; set; }

    public Guid? ImpersonatorTenantId { get; set; }

    public string UserName { get; set; }

    public string Name { get; set; }

    public string SurName { get; set; }

    public string Email { get; set; }

    public bool EmailVerified { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberVerified { get; set; }

    public string[] Roles { get; set; }
}
