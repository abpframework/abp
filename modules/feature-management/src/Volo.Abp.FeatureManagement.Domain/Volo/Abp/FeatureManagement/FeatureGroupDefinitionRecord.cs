using System;
using System.Text.Json.Serialization;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.FeatureManagement;

public class FeatureGroupDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    /* Ignoring Id because it is different whenever we create an instance of
     * this class, and we are using Json Serialize, than Hash to understand
     * if feature definitions have changed (in StaticFeatureSaver.CalculateHash()).
     */
    [JsonIgnore] //TODO: TODO: Use JSON modifier to ignore this property
    public override Guid Id { get; protected set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public FeatureGroupDefinitionRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public FeatureGroupDefinitionRecord(
        Guid id,
        string name,
        string displayName)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), FeatureGroupDefinitionRecordConsts.MaxNameLength);
        DisplayName =  Check.NotNullOrWhiteSpace(displayName, nameof(displayName), FeatureGroupDefinitionRecordConsts.MaxDisplayNameLength);;


        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public bool HasSameData(FeatureGroupDefinitionRecord otherRecord)
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

    public void Patch(FeatureGroupDefinitionRecord otherRecord)
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
