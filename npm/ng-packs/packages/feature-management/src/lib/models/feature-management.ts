import { EventEmitter } from '@angular/core';

export namespace FeatureManagement {
  export interface State {
    features: Feature[];
  }

  export interface ValueType {
    name: string;
    properties: object;
    validator: object;
  }

  export interface Feature {
    name: string;
    value: string;
    description?: string;
    valueType?: ValueType;
    depth?: number;
    parentName?: string;
  }

  export interface Features {
    features: Feature[];
  }

  export interface Provider {
    providerName: string;
    providerKey: string;
  }

  export interface FeatureManagementComponentInputs {
    visible: boolean;
    readonly providerName: string;
    readonly providerKey: string;
  }

  export interface FeatureManagementComponentOutputs {
    readonly visibleChange: EventEmitter<boolean>;
  }
}
