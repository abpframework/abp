export interface GenerateLibSchema {
  /**
   * Angular package name will create
   */
  packageName: string;

  /**
   * İs the package a library or a library module
   */
  isSecondaryEntrypoint: boolean;

  isModuleTemplate: boolean;

  override: boolean;

  target: string;
}
