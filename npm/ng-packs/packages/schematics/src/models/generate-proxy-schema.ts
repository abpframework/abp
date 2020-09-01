export interface GenerateProxySchema {
  /**
   * Backend module name
   */
  module?: string;

  /**
   * Source Angular project for API definition URL & root namespace resolution
   */
  source?: string;

  /**
   * Target Angular project to place the generated code
   */
  target?: string;
}
