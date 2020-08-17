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
  properties: Property[] | null;
}

export interface Property {
  name: string;
  type: string;
  typeSimple: string;
}

export interface Module {
  rootPath: string;
  remoteServiceName: string;
  controllers: Record<string, Controller>;
}

export interface Controller {
  controllerName: string;
  type: string;
  interfaces: Interface[];
  actions: Record<string, Action>;
}

export interface Interface {
  type: string;
}

export interface Action {
  uniqueName: string;
  name: string;
  httpMethod: string;
  url: string;
  supportedVersions: string[];
  parametersOnMethod: ParameterOnMethod[];
  parameters: Parameter[];
  returnValue: ReturnValue;
}

export interface ParameterOnMethod {
  name: string;
  typeAsString: string;
  type: string;
  typeSimple: string;
  isOptional: boolean;
  defaultValue: any;
}

export interface Parameter {
  nameOnMethod: string;
  name: string;
  type: string;
  typeSimple: string;
  isOptional: boolean;
  defaultValue: any;
  constraintTypes: string[] | null;
  bindingSourceId: string;
  descriptorName: string;
}

export interface ReturnValue {
  type: string;
  typeSimple: string;
}
