using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Users;

public class UserData : IUserData
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
    
    public ExtraPropertyDictionary ExtraProperties { get; }

    public UserData()
    {

    }

    public UserData(IUserData userData)
    {
        Id = userData.Id;
        UserName = userData.UserName;
        Email = userData.Email;
        Name = userData.Name;
        Surname = userData.Surname;
        IsActive = userData.IsActive;
        EmailConfirmed = userData.EmailConfirmed;
        PhoneNumber = userData.PhoneNumber;
        PhoneNumberConfirmed = userData.PhoneNumberConfirmed;
        TenantId = userData.TenantId;
        ExtraProperties = userData.ExtraProperties;
    }

    public UserData(
        Guid id,
        [NotNull] string userName,
        [CanBeNull] string email = null,
        [CanBeNull] string name = null,
        [CanBeNull] string surname = null,
        bool emailConfirmed = false,
        [CanBeNull] string phoneNumber = null,
        bool phoneNumberConfirmed = false,
        Guid? tenantId = null,
        bool isActive = true,
        ExtraPropertyDictionary extraProperties = null)
    {
        Id = id;
        UserName = userName;
        Email = email;
        Name = name;
        Surname = surname;
        IsActive = isActive;
        EmailConfirmed = emailConfirmed;
        PhoneNumber = phoneNumber;
        PhoneNumberConfirmed = phoneNumberConfirmed;
        TenantId = tenantId;
        ExtraProperties = extraProperties;
    }
}
