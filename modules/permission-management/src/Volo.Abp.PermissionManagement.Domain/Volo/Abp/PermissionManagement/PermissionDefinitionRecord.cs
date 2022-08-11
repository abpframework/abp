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
    
    public bool IsEnabled { get; set; }
    
    public MultiTenancySides MultiTenancySide { get; set; }
    
    /// <summary>
    /// Comma separated list of provider names.
    /// </summary>
    public string Providers { get; set; }

    public string DisplayName { get; set; }

    /// <summary>
    /// A JSON array containing feature checkers data.
    ///
    /// Example:
    ///   [
    ///    {
    ///      "type": "RequireFeatures",
    ///      "featureNames": [ "FeatureA", "FeatureB" ]
    ///    },
    ///    {
    ///      "type": "RequireGlobalFeatures",
    ///      "featureNames": [ "GlobalFeatureA", "GlobalFeatureB" ]
    ///    }
    ///   ]
    /// </summary>
    public string StateCheckers { get; set; }
    
    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public PermissionDefinitionRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
}