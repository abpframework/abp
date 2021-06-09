
export interface ActionApiDescriptionModel {
  uniqueName?: string;
  name?: string;
  httpMethod?: string;
  url?: string;
  supportedVersions: string[];
  parametersOnMethod: MethodParameterApiDescriptionModel[];
  parameters: ParameterApiDescriptionModel[];
  returnValue: ReturnValueApiDescriptionModel;
}

export interface ApplicationApiDescriptionModel {
  modules: Record<string, ModuleApiDescriptionModel>;
  types: Record<string, TypeApiDescriptionModel>;
}

export interface ApplicationApiDescriptionModelRequestDto {
  includeTypes: boolean;
}

export interface ControllerApiDescriptionModel {
  controllerName?: string;
  type?: string;
  interfaces: ControllerInterfaceApiDescriptionModel[];
  actions: Record<string, ActionApiDescriptionModel>;
}

export interface ControllerInterfaceApiDescriptionModel {
  type?: string;
}

export interface MethodParameterApiDescriptionModel {
  name?: string;
  typeAsString?: string;
  type?: string;
  typeSimple?: string;
  isOptional: boolean;
  defaultValue: object;
}

export interface ModuleApiDescriptionModel {
  rootPath?: string;
  remoteServiceName?: string;
  controllers: Record<string, ControllerApiDescriptionModel>;
}

export interface ParameterApiDescriptionModel {
  nameOnMethod?: string;
  name?: string;
  jsonName?: string;
  type?: string;
  typeSimple?: string;
  isOptional: boolean;
  defaultValue: object;
  constraintTypes: string[];
  bindingSourceId?: string;
  descriptorName?: string;
}

export interface PropertyApiDescriptionModel {
  name?: string;
  jsonName?: string;
  type?: string;
  typeSimple?: string;
  isRequired: boolean;
}

export interface ReturnValueApiDescriptionModel {
  type?: string;
  typeSimple?: string;
}

export interface TypeApiDescriptionModel {
  baseType?: string;
  isEnum: boolean;
  enumNames: string[];
  enumValues: object[];
  genericArguments: string[];
  properties: PropertyApiDescriptionModel[];
}
