
export interface EntityExtensionDto {
  properties?: Record<string, ExtensionPropertyDto>;
  configuration?: Record<string, object>;
}

export interface ExtensionEnumDto {
  fields?: ExtensionEnumFieldDto[];
  localizationResource?: string | null;
}

export interface ExtensionEnumFieldDto {
  name?: string | null;
  value?: object | null;
}

export interface ExtensionPropertyApiCreateDto {
  isAvailable?: boolean;
}

export interface ExtensionPropertyApiDto {
  onGet?: ExtensionPropertyApiGetDto;
  onCreate?: ExtensionPropertyApiCreateDto;
  onUpdate?: ExtensionPropertyApiUpdateDto;
}

export interface ExtensionPropertyApiGetDto {
  isAvailable?: boolean;
}

export interface ExtensionPropertyApiUpdateDto {
  isAvailable?: boolean;
}

export interface ExtensionPropertyAttributeDto {
  typeSimple?: string;
  config?: Record<string, object>;
}

export interface ExtensionPropertyDto {
  type?: string;
  typeSimple?: string;
  displayName?: LocalizableStringDto | null;
  api?: ExtensionPropertyApiDto;
  ui?: ExtensionPropertyUiDto;
  policy?: ExtensionPropertyPolicyDto;
  attributes?: ExtensionPropertyAttributeDto[];
  configuration?: Record<string, object>;
  defaultValue?: object | null;
}

export interface ExtensionPropertyFeaturePolicyDto {
  features?: string[];
  requiresAll?: boolean;
}

export interface ExtensionPropertyGlobalFeaturePolicyDto {
  features?: string[];
  requiresAll?: boolean;
}

export interface ExtensionPropertyPermissionPolicyDto {
  permissionNames?: string[];
  requiresAll?: boolean;
}

export interface ExtensionPropertyPolicyDto {
  globalFeatures?: ExtensionPropertyGlobalFeaturePolicyDto;
  features?: ExtensionPropertyFeaturePolicyDto;
  permissions?: ExtensionPropertyPermissionPolicyDto;
}

export interface ExtensionPropertyUiDto {
  onTable?: ExtensionPropertyUiTableDto;
  onCreateForm?: ExtensionPropertyUiFormDto;
  onEditForm?: ExtensionPropertyUiFormDto;
  lookup?: ExtensionPropertyUiLookupDto;
}

export interface ExtensionPropertyUiFormDto {
  isVisible?: boolean;
}

export interface ExtensionPropertyUiLookupDto {
  url?: string;
  resultListPropertyName?: string;
  displayPropertyName?: string;
  valuePropertyName?: string;
  filterParamName?: string;
}

export interface ExtensionPropertyUiTableDto {
  isVisible?: boolean;
}

export interface LocalizableStringDto {
  name?: string;
  resource?: string | null;
}

export interface ModuleExtensionDto {
  entities?: Record<string, EntityExtensionDto>;
  configuration?: Record<string, object>;
}

export interface ObjectExtensionsDto {
  modules?: Record<string, ModuleExtensionDto>;
  enums?: Record<string, ExtensionEnumDto>;
}
