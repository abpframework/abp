using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Users;

[EventName("Volo.Abp.Users.User")]
public class UserEto : IEntityEto<Guid>, IUserData, IMultiTenant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string UserName { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public bool IsActive { get; set; }

    public string Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public int EntityVersion { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; }

    public UserEto()
    {
    }

    public UserEto(Guid id, Guid? tenantId, string userName, string name, string surname, bool isActive, string email,
        bool emailConfirmed, string phoneNumber, bool phoneNumberConfirmed, int entityVersion,
        ExtraPropertyDictionary extraProperties)
    {
        Id = id;
        TenantId = tenantId;
        UserName = userName;
        Name = name;
        Surname = surname;
        IsActive = isActive;
        Email = email;
        EmailConfirmed = emailConfirmed;
        PhoneNumber = phoneNumber;
        PhoneNumberConfirmed = phoneNumberConfirmed;
        EntityVersion = entityVersion;
        ExtraProperties = extraProperties;
    }
}