export interface GenerateProxySchema {
  /**
   * Backend module name
   */
  module?: string;

  /**
   * Backend api name, a.k.a. remoteServiceName
   */
  ['api-name']?: string;

  /**
   * Source Angular project for API definition URL & root namespace resolution
   */
  source?: string;

  /**
   * Target Angular project to place the generated code
   */
  target?: string;
}
