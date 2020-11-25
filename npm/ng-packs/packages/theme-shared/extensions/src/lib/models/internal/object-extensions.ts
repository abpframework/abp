import { ePropType } from '../../enums/props.enum';
import { EntityPropList } from '../entity-props';
import { FormPropList } from '../form-props';
import { PropContributorCallbacks } from '../props';

export interface EntityExtensionDto {
  properties: Record<string, ExtensionPropertyDto>;
  configuration: Record<string, object>;
}

export interface ExtensionEnumDto {
  fields: ExtensionEnumFieldDto[];
  localizationResource?: string;
  transformed?: any;
}

export interface ExtensionEnumFieldDto {
  name?: string;
  value: any;
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
  config: Record<string, any>;
}

export interface ExtensionPropertyDto {
  type?: string;
  typeSimple?: ePropType;
  displayName: LocalizableStringDto;
  api: ExtensionPropertyApiDto;
  ui: ExtensionPropertyUiDto;
  attributes: ExtensionPropertyAttributeDto[];
  configuration: Record<string, any>;
  defaultValue: any;
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
  isSortable?: boolean;
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

export interface PropContributors<T = any> {
  prop: PropContributorCallbacks<EntityPropList<T>>;
  createForm: PropContributorCallbacks<FormPropList<T>>;
  editForm: PropContributorCallbacks<FormPropList<T>>;
}

/**
 * @deprecated To be deleted in v4.2.
 */
export interface Config {
  objectExtensions: Item;
}

/**
 * @deprecated Use ObjectExtensionsDto. To be deleted in v4.2.
 */
export interface Item {
  modules: Modules;
  enums: Enums;
}

/**
 * @deprecated Use Record<string, ModuleExtensionDto>. To be deleted in v4.2.
 */
export type Modules = Configuration<Module>;

/**
 * @deprecated Use ModuleExtensionDto. To be deleted in v4.2.
 */
export interface Module {
  configuration: Configuration;
  entities: Entities;
}

/**
 * @deprecated Use Record<string, EntityExtensionDto>. To be deleted in v4.2.
 */
export type Entities = Configuration<Entity>;

/**
 * @deprecated Use EntityExtensionDto. To be deleted in v4.2.
 */
export interface Entity {
  configuration: Configuration;
  properties: Properties;
}

/**
 * @deprecated Use Record<string, ExtensionPropertyDto>. To be deleted in v4.2.
 */
export type Properties = Configuration<Property>;

/**
 * @deprecated Use ExtensionPropertyDto. To be deleted in v4.2.
 */
export interface Property {
  type: string;
  typeSimple: ePropType;
  displayName: DisplayName | null;
  api: Api;
  ui: Ui;
  attributes: Attribute[];
  configuration: Configuration;
  defaultValue?: any;
}

/**
 * @deprecated Use LocalizableStringDto. To be deleted in v4.2.
 */
export interface DisplayName {
  name: string;
  resource: string;
}

/**
 * @deprecated Use ExtensionPropertyApiDto. To be deleted in v4.2.
 */
export interface Api {
  onGet: ApiConfig;
  onCreate: ApiConfig;
  onUpdate: ApiConfig;
}

/**
 * @deprecated Use ExtensionPropertyApiCreateDto. To be deleted in v4.2.
 */
export interface ApiConfig {
  isAvailable: boolean;
}

/**
 * @deprecated Use ExtensionPropertyUiDto. To be deleted in v4.2.
 */
export interface Ui {
  onTable: UiPropConfig;
  onCreateForm: UiFormConfig;
  onEditForm: UiFormConfig;
}

/**
 * @deprecated Use ExtensionPropertyUiTableDto. To be deleted in v4.2.
 */
export interface UiPropConfig {
  isSortable: boolean;
  isVisible: boolean;
}

/**
 * @deprecated Use ExtensionPropertyUiFormDto. To be deleted in v4.2.
 */
export interface UiFormConfig {
  isVisible: boolean;
}

/**
 * @deprecated Use ExtensionPropertyAttributeDto. To be deleted in v4.2.
 */
export interface Attribute {
  typeSimple: string;
  config: Configuration;
}

/**
 * @deprecated To be deleted in v4.2.
 */
export type Configuration<T = any> = Record<string, T>;

/**
 * @deprecated Use Record<string, ExtensionEnumDto>. To be deleted in v4.2.
 */
export type Enums = Record<string, Enum>;

/**
 * @deprecated Use ExtensionEnumDto. To be deleted in v4.2.
 */
export interface Enum {
  fields: EnumMember[];
  localizationResource?: string;
  transformed?: any;
}

/**
 * @deprecated Use ExtensionEnumFieldDto. To be deleted in v4.2.
 */
export interface EnumMember {
  name: string;
  value: any;
}
