import { EventEmitter } from '@angular/core';

export namespace FeatureManagement {
  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface State {
    features: Feature[];
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface ValueType {
    name: string;
    properties: object;
    validator: object;
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface Feature {
    name: string;
    displayName: string;
    value: string;
    description?: string;
    valueType?: ValueType;
    depth?: number;
    parentName?: string;
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface Features {
    features: Feature[];
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
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
