## AbpFeatureGroups

AbpFeatureGroups is used to store feature groups.

### Description

This table stores information about the feature groups in the application. Each record in the table represents a feature group and allows to manage and organize the features effectively. For example, you can group all the features in the [`AbpFeatures`](#abpfeatures) table related to the `Identity` module under the `Identity` group.

### Module

[`Volo.Abp.Features`](../Feature-Management.md)

---

## AbpFeatures

AbpFeatures is used to store features.

### Description

This table stores information about the features in the application. Each record in the table represents a feature and allows to manage and organize the features effectively. For example, you can use the `Name` column to link each feature with its corresponding feature value in the [`AbpFeatureValues`](#abpfeaturevalues) table, so that you can easily manage and organize the features.

### Module

[`Volo.Abp.FeatureManagement`](../Feature-Management.md)

---

## AbpFeatureValues

AbpFeatureValues is used to store feature values for providers.

### Description

This table stores the values of the features for different providers. Each record in the table represents a feature value and allows to manage the application features values effectively. For example, you can use the `Name` column to link each feature value with its corresponding feature in the [`AbpFeatures`](#abpfeatures) table, so that you can easily manage and organize the features.

### Module

[`Volo.Abp.FeatureManagement`](../Feature-Management.md)