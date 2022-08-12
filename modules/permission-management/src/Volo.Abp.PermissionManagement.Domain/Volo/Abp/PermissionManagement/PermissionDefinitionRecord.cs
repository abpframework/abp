using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement;

public class PermissionDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    public string GroupName { get; set; }
    
    public string Name { get; set; }
    
    public string ParentName { get; set; }
    
    public string DisplayName { get; set; }

    public bool IsEnabled { get; set; }
    
    public MultiTenancySides MultiTenancySide { get; set; }
    
    /// <summary>
    /// Comma separated list of provider names.
    /// </summary>
    public string Providers { get; set; }

    /// <summary>
    /// Serialized string to store info about the state checkers.
    /// </summary>
    public string StateCheckers { get; set; }
    
    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public PermissionDefinitionRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
    
    public PermissionDefinitionRecord(
        Guid id,
        string groupName,
        string name,
        string parentName,
        string displayName,
        bool isEnabled = true,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both,
        string providers = null,
        string stateCheckers = null)
        : base(id)
    {
        GroupName = Check.NotNullOrWhiteSpace(groupName, nameof(groupName), PermissionGroupDefinitionRecordConsts.MaxNameLength);
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), PermissionDefinitionRecordConsts.MaxNameLength);
        ParentName = Check.Length(parentName, nameof(parentName), PermissionDefinitionRecordConsts.MaxNameLength);
        DisplayName =  Check.NotNullOrWhiteSpace(displayName, nameof(displayName), PermissionDefinitionRecordConsts.MaxDisplayNameLength);
        IsEnabled = isEnabled;
        MultiTenancySide = multiTenancySide;
        Providers = providers;
        StateCheckers = stateCheckers;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
}