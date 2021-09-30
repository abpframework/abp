import { EventEmitter, Type } from '@angular/core';
import { Subject } from 'rxjs';
import { ABP } from './common';

export namespace ReplaceableComponents {
  export interface State {
    replaceableComponents: ReplaceableComponent[];
  }

  export interface ReplaceableComponent {
    component: Type<any>;
    key: string;
  }

  export interface ReplaceableTemplateDirectiveInput<
    I,
    O extends { [K in keyof O]: EventEmitter<any> | Subject<any> },
  > {
    inputs: { -readonly [K in keyof I]: { value: I[K]; twoWay?: boolean } };
    outputs: { -readonly [K in keyof O]: (value: ABP.ExtractFromOutput<O[K]>) => void };
    componentKey: string;
  }

  export interface ReplaceableTemplateData<
    I,
    O extends { [K in keyof O]: EventEmitter<any> | Subject<any> },
  > {
    inputs: ReplaceableTemplateInputs<I>;
    outputs: ReplaceableTemplateOutputs<O>;
    componentKey: string;
  }

  export type ReplaceableTemplateInputs<T> = {
    [K in keyof T]: T[K];
  };

  export type ReplaceableTemplateOutputs<
    T extends { [K in keyof T]: EventEmitter<any> | Subject<any> },
  > = {
    [K in keyof T]: (value: ABP.ExtractFromOutput<T[K]>) => void;
  };

  export interface RouteData<T = any> {
    key: string;
    defaultComponent: Type<T>;
  }
}
