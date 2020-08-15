export interface Schema {
  /**
   * The name of the backend module to generate code for
   */
  module?: string;

  /**
   * Angular project to resolve API definition URL from
   */
  source?: string;

  /**
   * Angular project to place the generated code in
   */
  target?: string;
}
