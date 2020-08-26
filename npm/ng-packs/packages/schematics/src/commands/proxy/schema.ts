export interface Schema {
  /**
   * Backend module to generate code for
   */
  module?: string;

  /**
   * Angular project to resolve root namespace & API definition URL from
   */
  source?: string;

  /**
   * Angular project to generate code in
   */
  target?: string;
}
