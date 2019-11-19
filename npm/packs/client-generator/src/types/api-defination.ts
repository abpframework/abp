export namespace APIDefination {
  export interface Response {
    modules: Modules;
  }

  export interface Modules {
    [key: string]: Module;
  }

  export interface Module {
    rootPath: string;
    controllers: { [key: string]: any };
  }
}
