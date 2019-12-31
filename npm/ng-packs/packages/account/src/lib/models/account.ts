import { TemplateRef } from '@angular/core';

export namespace Account {
  export interface AuthWrapperComponentInputs {
    readonly mainContentRef: TemplateRef<any>;
    readonly cancelContentRef?: TemplateRef<any>;
  }

  // tslint:disable-next-line: no-empty-interface
  export interface AuthWrapperComponentOutputs {}

  // tslint:disable-next-line: no-empty-interface
  export interface TenantBoxComponentInputs {}

  // tslint:disable-next-line: no-empty-interface
  export interface TenantBoxComponentOutputs {}

  // tslint:disable-next-line: no-empty-interface
  export interface PersonalSettingsComponentInputs {}

  // tslint:disable-next-line: no-empty-interface
  export interface PersonalSettingsComponentOutputs {}

  // tslint:disable-next-line: no-empty-interface
  export interface ChangePasswordComponentInputs {}

  // tslint:disable-next-line: no-empty-interface
  export interface ChangePasswordComponentOutputs {}
}
