import { ePropType } from '../../enums/props.enum';
import { EntityPropList } from '../entity-props';
import { FormPropList } from '../form-props';
import { PropContributorCallbacks } from '../props';

export interface PropContributors<T = any> {
  prop: PropContributorCallbacks<EntityPropList<T>>;
  createForm: PropContributorCallbacks<FormPropList<T>>;
  editForm: PropContributorCallbacks<FormPropList<T>>;
}

export interface Config {
  objectExtensions: Item;
}

export interface Item {
  modules: Modules;
  enums: Enums;
}

export type Modules = Configuration<Module>;

export interface Module {
  configuration: Configuration;
  entities: Entities;
}

export type Entities = Configuration<Entity>;

export interface Entity {
  configuration: Configuration;
  properties: Properties;
}

export type Properties = Configuration<Property>;

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

export interface DisplayName {
  name: string;
  resource: string;
}

export interface Api {
  onGet: ApiConfig;
  onCreate: ApiConfig;
  onUpdate: ApiConfig;
}

export interface ApiConfig {
  isAvailable: boolean;
}

export interface Ui {
  onTable: UiPropConfig;
  onCreateForm: UiFormConfig;
  onEditForm: UiFormConfig;
}

export interface UiPropConfig {
  isSortable: boolean;
  isVisible: boolean;
}

export interface UiFormConfig {
  isVisible: boolean;
}

export interface Attribute {
  typeSimple: string;
  config: Configuration;
}

export type Configuration<T = any> = Record<string, T>;

export type Enums = Record<string, Enum>;

export interface Enum {
  fields: EnumMember[];
  localizationResource?: string;
  transformed?: any;
}

export interface EnumMember {
  name: string;
  value: any;
}
