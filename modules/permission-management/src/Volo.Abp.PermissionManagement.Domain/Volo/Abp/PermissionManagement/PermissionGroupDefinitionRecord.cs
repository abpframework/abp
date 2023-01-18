using System;
using System.Text.Json.Serialization;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.PermissionManagement;

public class PermissionGroupDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    /* Ignoring Id because it is different whenever we create an instance of
     * this class, and we are using Json Serialize, than Hash to understand
     * if permission definitions have changed (in StaticPermissionSaver.CalculateHash()).
     */
    [JsonIgnore]
    public override Guid Id { get; protected set; }

    public string Name { get; set; }
    
    public string DisplayName { get; set; }
    
    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public PermissionGroupDefinitionRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
    
    public PermissionGroupDefinitionRecord(
        Guid id,
        string name,
        string displayName)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), PermissionGroupDefinitionRecordConsts.MaxNameLength);
        DisplayName =  Check.NotNullOrWhiteSpace(displayName, nameof(displayName), PermissionGroupDefinitionRecordConsts.MaxDisplayNameLength);;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public bool HasSameData(PermissionGroupDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            return false;
        }
        
        if (DisplayName != otherRecord.DisplayName)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            return false;
        }

        return true;
    }

    public void Patch(PermissionGroupDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            Name = otherRecord.Name;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            DisplayName = otherRecord.DisplayName;
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