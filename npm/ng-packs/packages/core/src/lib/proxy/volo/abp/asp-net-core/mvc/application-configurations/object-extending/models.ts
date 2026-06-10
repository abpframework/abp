
export interface EntityExtensionDto {
  properties: Record<string, ExtensionPropertyDto>;
  configuration: Record<string, object>;
}

export interface ExtensionEnumDto {
  fields: ExtensionEnumFieldDto[];
  localizationResource?: string;
}

export interface ExtensionEnumFieldDto {
  name?: string;
  value: object;
}

export interface ExtensionPropertyApiCreateDto {
  isAvailable: boolean;
}

export interface ExtensionPropertyApiDto {
  onGet: ExtensionPropertyApiGetDto;
  onCreate: ExtensionPropertyApiCreateDto;
  onUpdate: ExtensionPropertyApiUpdateDto;
}

export interface ExtensionPropertyApiGetDto {
  isAvailable: boolean;
}

export interface ExtensionPropertyApiUpdateDto {
  isAvailable: boolean;
}

export interface ExtensionPropertyAttributeDto {
  typeSimple?: string;
  config: Record<string, object>;
}

export interface ExtensionPropertyDto {
  type?: string;
  typeSimple?: string;
  displayName: LocalizableStringDto;
  api: ExtensionPropertyApiDto;
  ui: ExtensionPropertyUiDto;
  attributes: ExtensionPropertyAttributeDto[];
  configuration: Record<string, object>;
  defaultValue: object;
}

export interface ExtensionPropertyUiDto {
  onTable: ExtensionPropertyUiTableDto;
  onCreateForm: ExtensionPropertyUiFormDto;
  onEditForm: ExtensionPropertyUiFormDto;
  lookup: ExtensionPropertyUiLookupDto;
}

export interface ExtensionPropertyUiFormDto {
  isVisible: boolean;
}

export interface ExtensionPropertyUiLookupDto {
  url?: string;
  resultListPropertyName?: string;
  displayPropertyName?: string;
  valuePropertyName?: string;
  filterParamName?: string;
}

export interface ExtensionPropertyUiTableDto {
  isVisible: boolean;
}

export interface LocalizableStringDto {
  name?: string;
  resource?: string;
}

export interface ModuleExtensionDto {
  entities: Record<string, EntityExtensionDto>;
  configuration: Record<string, object>;
}

export interface ObjectExtensionsDto {
  modules: Record<string, ModuleExtensionDto>;
  enums: Record<string, ExtensionEnumDto>;
}
