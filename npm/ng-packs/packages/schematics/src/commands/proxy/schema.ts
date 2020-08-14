export interface Schema {
  /**
   * Angular project to place the generated code in
   */
  destination?: string;

  /**
   * The name of the backend module to generate code for
   */
  module?: string;

  /**
   * Angular project to resolve API definition URL from
   */
  source?: string;
}
