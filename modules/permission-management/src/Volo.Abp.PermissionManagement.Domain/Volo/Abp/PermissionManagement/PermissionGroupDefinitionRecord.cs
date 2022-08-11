using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.PermissionManagement;

public class PermissionGroupDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
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
}