export declare namespace APIDefination {
    interface Response {
        modules: Modules;
    }
    interface Modules {
        [key: string]: Module;
    }
    interface Module {
        rootPath: string;
        controllers: {
            [key: string]: Controller;
        };
    }
    interface Controller {
        controllerName: string;
        interfaces: {
            typeAsString: string;
        }[];
        typeAsString: string;
        actions: {
            [key: string]: Action;
        };
    }
    interface Action {
        uniqueName: string;
        name: string;
        httpMethod: string;
        url: string;
        supportedVersions: any[];
        parametersOnMethod: ParametersOnMethod[];
        parameters: Parameter[];
        returnValue: ReturnValue;
    }
    interface Parameter {
        nameOnMethod: string;
        name: string;
        typeAsString: string;
        isOptional: boolean;
        defaultValue: null;
        constraintTypes: null;
        bindingSourceId: string;
    }
    interface ParametersOnMethod {
        name: string;
        typeAsString: string;
        isOptional: boolean;
        defaultValue: null;
    }
    interface ReturnValue {
        typeAsString: string;
    }
}
