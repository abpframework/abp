import { eBindingSourceId } from '../enums';

export interface ApiDefinition {
  modules: Record<string, Module>;
  types: Record<string, Type>;
}

export interface Type {
  baseType: string | null;
  isEnum: boolean;
  enumNames: string[] | null;
  enumValues: number[] | null;
  genericArguments: string[] | null;
  properties: PropertyDef[] | null;
}

export interface PropertyDef {
  name: string;
  jsonName: string | null;
  type: string;
  typeSimple: string;
  isRequired: boolean;
}

export interface Module {
  rootPath: string;
  remoteServiceName: string;
  controllers: Record<string, Controller>;
}

export interface Controller {
  controllerName: string;
  type: string;
  interfaces: InterfaceDef[];
  actions: Record<string, Action>;
}

export interface InterfaceDef {
  type: string;
}

export interface Action {
  uniqueName: string;
  name: string;
  httpMethod: string;
  url: string;
  supportedVersions: string[];
  parametersOnMethod: ParameterInSignature[];
  parameters: ParameterInBody[];
  returnValue: TypeDef;
}

export interface ParameterInSignature {
  name: string;
  typeAsString: string;
  type: string;
  typeSimple: string;
  isOptional: boolean;
  defaultValue: any;
}

export interface ParameterInBody {
  nameOnMethod: string;
  name: string;
  jsonName: string | null;
  type: string;
  typeSimple: string;
  isOptional: boolean;
  defaultValue: any;
  constraintTypes: string[] | null;
  bindingSourceId: eBindingSourceId;
  descriptorName: string;
}

export interface TypeDef {
  type: string;
  typeSimple: string;
}

export interface TypeWithEnum {
  isEnum: boolean;
  type: string;
}
