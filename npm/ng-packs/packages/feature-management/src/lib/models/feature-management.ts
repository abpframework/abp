import { EventEmitter } from '@angular/core';

export namespace FeatureManagement {
  export interface FeatureManagementComponentInputs {
    visible: boolean;
    readonly providerName: string;
    readonly providerKey: string;
  }

  export interface FeatureManagementComponentOutputs {
    readonly visibleChange: EventEmitter<boolean>;
  }
}
