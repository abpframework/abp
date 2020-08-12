export interface Schema {
  /**
   * The URL to get API configuration from
   */
  apiUrl?: string;

  /**
   * The name of the module to generate code for
   */
  module?: string;

  /**
   * The path to place the generated code at
   */
  out?: string;
}
