export namespace APIDefination {
  export interface Response {
    modules: Modules;
  }

  export interface Modules {
    [key: string]: Module;
  }

  export interface Module {
    rootPath: string;
    controllers: { [key: string]: Controller };
  }

  export interface Controller {
    controllerName: string;
    interfaces: { typeAsString: string }[];
    typeAsString: string;
    actions: { [key: string]: Action };
  }

  export interface Action {
    uniqueName: string;
    name: string;
    httpMethod: string;
    url: string;
    supportedVersions: any[];
    parametersOnMethod: ParametersOnMethod[];
    parameters: Parameter[];
    returnValue: ReturnValue;
  }

  export interface Parameter {
    nameOnMethod: string;
    name: string;
    typeAsString: string;
    isOptional: boolean;
    defaultValue: null;
    constraintTypes: null;
    bindingSourceId: string;
  }

  export interface ParametersOnMethod {
    name: string;
    typeAsString: string;
    isOptional: boolean;
    defaultValue: null;
  }

  export interface ReturnValue {
    typeAsString: string;
  }
}
