using System;
using System.Text.Json.Serialization;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.FeatureManagement;

public class FeatureDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    /* Ignoring Id because it is different whenever we create an instance of
     * this class, and we are using Json Serialize, than Hash to understand
     * if feature definitions have changed (in StaticFeatureSaver.CalculateHash()).
     */
    [JsonIgnore] //TODO: TODO: Use JSON modifier to ignore this property
    public override Guid Id { get; protected set; }

    public string GroupName { get; set; }

    public string Name  { get; set; }

    public string ParentName { get; set; }

    public string DisplayName  { get; set; }

    public string Description { get; set; }

    public string DefaultValue { get; set; }

    public bool IsVisibleToClients { get; set; }

    public bool IsAvailableToHost { get; set; }

    /// <summary>
    /// Comma separated list of provider names.
    /// </summary>
    public string AllowedProviders { get; set; }

    /// <summary>
    /// Serialized string to store info about the ValueType.
    /// </summary>
    public string ValueType { get; set; } // ToggleStringValueType

    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public FeatureDefinitionRecord()
    {
        IsVisibleToClients = true;
        IsAvailableToHost = true;
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public FeatureDefinitionRecord(
        Guid id,
        string groupName,
        string name,
        string parentName,
        string displayName = null,
        string description = null,
        string defaultValue = null,
        bool isVisibleToClients = true,
        bool isAvailableToHost = true,
        string allowedProviders = null,
        string valueType = null)
        : base(id)
    {
        GroupName = Check.NotNullOrWhiteSpace(groupName, nameof(groupName), FeatureDefinitionRecordConsts.MaxNameLength);
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), FeatureDefinitionRecordConsts.MaxNameLength);
        ParentName = Check.Length(parentName, nameof(parentName), FeatureDefinitionRecordConsts.MaxNameLength);
        DisplayName =  Check.NotNullOrWhiteSpace(displayName, nameof(displayName), FeatureDefinitionRecordConsts.MaxDisplayNameLength);

        Description = Check.Length(description, nameof(description), FeatureDefinitionRecordConsts.MaxDescriptionLength);
        DefaultValue =  Check.NotNullOrWhiteSpace(defaultValue, nameof(defaultValue), FeatureDefinitionRecordConsts.MaxDefaultValueLength);

        IsVisibleToClients = isVisibleToClients;
        IsAvailableToHost = isAvailableToHost;

        AllowedProviders = Check.Length(allowedProviders, nameof(allowedProviders), FeatureDefinitionRecordConsts.MaxAllowedProvidersLength);
        ValueType =  Check.NotNullOrWhiteSpace(valueType, nameof(valueType), FeatureDefinitionRecordConsts.MaxValueTypeLength);

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
    public bool HasSameData(FeatureDefinitionRecord otherRecord)
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

        if (Description != otherRecord.Description)
        {
            return false;
        }

        if (DefaultValue != otherRecord.DefaultValue)
        {
            return false;
        }

        if (IsVisibleToClients != otherRecord.IsVisibleToClients)
        {
            return false;
        }

        if (IsAvailableToHost != otherRecord.IsAvailableToHost)
        {
            return false;
        }
        if (AllowedProviders != otherRecord.AllowedProviders)
        {
            return false;
        }

        if (ValueType != otherRecord.ValueType)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            return false;
        }

        return true;
    }

    public void Patch(FeatureDefinitionRecord otherRecord)
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

        if (Description != otherRecord.Description)
        {
            Description = otherRecord.Description;
        }

        if (DefaultValue != otherRecord.DefaultValue)
        {
            DefaultValue = otherRecord.DefaultValue;
        }

        if (IsVisibleToClients != otherRecord.IsVisibleToClients)
        {
            IsVisibleToClients = otherRecord.IsVisibleToClients;
        }

        if (IsAvailableToHost != otherRecord.IsAvailableToHost)
        {
            IsAvailableToHost = otherRecord.IsAvailableToHost;
        }

        if (AllowedProviders != otherRecord.AllowedProviders)
        {
            AllowedProviders = otherRecord.AllowedProviders;
        }

        if (ValueType != otherRecord.ValueType)
        {
            ValueType = otherRecord.ValueType;
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
