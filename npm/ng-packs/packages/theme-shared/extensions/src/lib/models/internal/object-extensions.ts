import { ePropType } from '../../enums/props.enum';
import { EntityPropList } from '../entity-props';
import { FormPropList } from '../form-props';
import { PropContributorCallbacks } from '../props';

export type DisplayNameGeneratorFn = (
  displayName: LocalizableStringDto,
  fallback: LocalizableStringDto,
) => string;

export type EntityExtensions = Record<string, EntityExtensionDto>;

export interface EntityExtensionDto {
  properties: EntityExtensionProperties;
  configuration: Record<string, object>;
}

export type EntityExtensionProperties = Record<string, ExtensionPropertyDto>;

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
  lookup?: ExtensionPropertyUiLookupDto;
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
