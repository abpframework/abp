import { Type } from '@angular/core';
import { ABP } from './common';

export namespace ReplaceableComponents {
  export interface State {
    replaceableComponents: ReplaceableComponent[];
  }

  export interface ReplaceableComponent {
    component: Type<any>;
    key: string;
  }

  export interface ReplaceableTemplateData<I, O> {
    inputs: ReplaceableTemplateInputs<I>;
    outputs: ReplaceableTemplateOutputs<O>;
    componentKey: string;
  }

  export type ReplaceableTemplateInputs<T> = {
    [K in keyof T]: T[K];
  };

  export type ReplaceableTemplateOutputs<T> = {
    [K in keyof T]: (value: ABP.ExtractFromGeneric<T[K]>) => void;
  };

  export interface RouteData<T = any> {
    key: string;
    defaultComponent: Type<T>;
  }
}
