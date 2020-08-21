export interface Schema {
  /**
   * Solution name
   */
  solution: string;

  /**
   * Angular project to resolve API definition URL from
   */
  source?: string;

  /**
   * Angular project to generate code in
   */
  target?: string;

  /**
   * Backend module to generate code for
   */
  module?: string;
}
