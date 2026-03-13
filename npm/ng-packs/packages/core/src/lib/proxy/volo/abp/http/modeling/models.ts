
export interface ActionApiDescriptionModel {
  uniqueName?: string;
  name?: string;
  httpMethod?: string | null;
  url?: string;
  supportedVersions?: string[] | null;
  parametersOnMethod?: MethodParameterApiDescriptionModel[];
  parameters?: ParameterApiDescriptionModel[];
  returnValue?: ReturnValueApiDescriptionModel;
  allowAnonymous?: boolean | null;
  authorizeDatas?: AuthorizeDataApiDescriptionModel[];
  implementFrom?: string | null;
}

export interface ApplicationApiDescriptionModel {
  modules?: Record<string, ModuleApiDescriptionModel>;
  types?: Record<string, TypeApiDescriptionModel>;
}

export interface ApplicationApiDescriptionModelRequestDto {
  includeTypes?: boolean;
}

export interface AuthorizeDataApiDescriptionModel {
  policy?: string | null;
  roles?: string | null;
}

export interface ControllerApiDescriptionModel {
  controllerName?: string;
  controllerGroupName?: string | null;
  isRemoteService?: boolean;
  isIntegrationService?: boolean;
  apiVersion?: string | null;
  type?: string;
  interfaces?: ControllerInterfaceApiDescriptionModel[];
  actions?: Record<string, ActionApiDescriptionModel>;
}

export interface ControllerInterfaceApiDescriptionModel {
  type?: string;
  name?: string;
  methods?: InterfaceMethodApiDescriptionModel[];
}

export interface InterfaceMethodApiDescriptionModel {
  name?: string;
  parametersOnMethod?: MethodParameterApiDescriptionModel[];
  returnValue?: ReturnValueApiDescriptionModel;
}

export interface MethodParameterApiDescriptionModel {
  name?: string;
  typeAsString?: string;
  type?: string;
  typeSimple?: string;
  isOptional?: boolean;
  defaultValue?: object | null;
}

export interface ModuleApiDescriptionModel {
  rootPath?: string;
  remoteServiceName?: string;
  controllers?: Record<string, ControllerApiDescriptionModel>;
}

export interface ParameterApiDescriptionModel {
  nameOnMethod?: string;
  name?: string;
  jsonName?: string | null;
  type?: string | null;
  typeSimple?: string | null;
  isOptional?: boolean;
  defaultValue?: object | null;
  constraintTypes?: string[] | null;
  bindingSourceId?: string | null;
  descriptorName?: string | null;
}

export interface PropertyApiDescriptionModel {
  name?: string;
  jsonName?: string | null;
  type?: string;
  typeSimple?: string;
  isRequired?: boolean;
  minLength?: number | null;
  maxLength?: number | null;
  minimum?: string | null;
  maximum?: string | null;
  regex?: string | null;
  isNullable?: boolean;
}

export interface ReturnValueApiDescriptionModel {
  type?: string;
  typeSimple?: string;
}

export interface TypeApiDescriptionModel {
  baseType?: string | null;
  isEnum?: boolean;
  enumNames?: string[] | null;
  enumValues?: object[] | null;
  genericArguments?: string[] | null;
  properties?: PropertyApiDescriptionModel[] | null;
}
