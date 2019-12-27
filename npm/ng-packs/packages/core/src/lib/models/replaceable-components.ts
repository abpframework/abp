import { Type, EventEmitter } from '@angular/core';
import { ABP } from './common';
import { Subject, BehaviorSubject } from 'rxjs';

export namespace ReplaceableComponents {
  export interface State {
    replaceableComponents: ReplaceableComponent[];
  }

  export interface ReplaceableComponent {
    component: Type<any>;
    key: string;
  }

  export interface ReplaceableTemplateData<
    I,
    O extends { [K in keyof O]: EventEmitter<any> | Subject<any> }
  > {
    inputs: ReplaceableTemplateInputs<I>;
    outputs: ReplaceableTemplateOutputs<O>;
    componentKey: string;
  }

  export type ReplaceableTemplateInputs<T> = {
    [K in keyof T]: T[K];
  };

  export type ReplaceableTemplateOutputs<
    T extends { [K in keyof T]: EventEmitter<any> | Subject<any> }
  > = {
    [K in keyof T]: (value: ABP.ExtractFromOutput<T[K]>) => void;
  };

  export interface RouteData<T = any> {
    key: string;
    defaultComponent: Type<T>;
  }
}
