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
            [key: string]: any;
        };
    }
}
