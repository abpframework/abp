import { TemplateRef } from '@angular/core';

export namespace Account {
  export interface AuthWrapperComponentInputs {
    readonly mainContentRef: TemplateRef<any>;
    readonly cancelContentRef?: TemplateRef<any>;
  }

  //tslint:disable
  export interface AuthWrapperComponentOutputs {}
  export interface TenantBoxComponentInputs {}
  export interface TenantBoxComponentOutputs {}
  export interface PersonalSettingsComponentInputs {}
  export interface PersonalSettingsComponentOutputs {}
  export interface ChangePasswordComponentInputs {}
  export interface ChangePasswordComponentOutputs {}
  // tslint:enable
}
