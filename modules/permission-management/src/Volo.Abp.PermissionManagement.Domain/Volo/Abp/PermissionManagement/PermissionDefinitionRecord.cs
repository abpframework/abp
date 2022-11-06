using System;
using System.Text.Json.Serialization;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement;

public class PermissionDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    /* Ignoring Id because it is different whenever we create an instance of
     * this class, and we are using Json Serialize, than Hash to understand
     * if permission definitions have changed (in StaticPermissionSaver.CalculateHash()).
     */
    [JsonIgnore]
    public override Guid Id { get; protected set; }
    
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

    public bool HasSameData(PermissionDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            return false;
        }
        
        if (GroupName != otherRecord.GroupName)
        {
            return false;
        }
        
        if (ParentName != otherRecord.ParentName)
        {
            return false;
        }
        
        if (DisplayName != otherRecord.DisplayName)
        {
            return false;
        }
        
        if (IsEnabled != otherRecord.IsEnabled)
        {
            return false;
        }
        
        if (MultiTenancySide != otherRecord.MultiTenancySide)
        {
            return false;
        }
                
        if (Providers != otherRecord.Providers)
        {
            return false;
        }
                
        if (StateCheckers != otherRecord.StateCheckers)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            return false;
        }

        return true;
    }

    public void Patch(PermissionDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            Name = otherRecord.Name;
        }
        
        if (GroupName != otherRecord.GroupName)
        {
            GroupName = otherRecord.GroupName;
        }
        
        if (ParentName != otherRecord.ParentName)
        {
            ParentName = otherRecord.ParentName;
        }
        
        if (DisplayName != otherRecord.DisplayName)
        {
            DisplayName = otherRecord.DisplayName;
        }
        
        if (IsEnabled != otherRecord.IsEnabled)
        {
            IsEnabled = otherRecord.IsEnabled;
        }
        
        if (MultiTenancySide != otherRecord.MultiTenancySide)
        {
            MultiTenancySide = otherRecord.MultiTenancySide;
        }
                
        if (Providers != otherRecord.Providers)
        {
            Providers = otherRecord.Providers;
        }
                
        if (StateCheckers != otherRecord.StateCheckers)
        {
            StateCheckers = otherRecord.StateCheckers;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            this.ExtraProperties.Clear();
            
            foreach (var property in otherRecord.ExtraProperties)
            {
                this.ExtraProperties.Add(property.Key, property.Value);
            }
        }
    }
}