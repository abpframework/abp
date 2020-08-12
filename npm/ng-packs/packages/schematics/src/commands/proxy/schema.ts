export interface Schema {
  /**
   * The name of the module to generate code for
   */
  module?: string;

  /**
   * The project to place the generated code in
   */
  project?: string;

  /**
   * The path to place the generated code at
   */
  path?: string;

  /**
   * The URL to get API configuration from
   */
  source?: string;
}
