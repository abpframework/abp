import { TemplateRef, Type } from '@angular/core';

export type DeepPartial<T> = Partible<T> extends never
  ? T
  : {
      [K in keyof T]?: DeepPartial<T[K]>;
    };

type Partible<T> = T extends Primitive | Array<any> | Node
  ? never
  : {
      // eslint-disable-next-line @typescript-eslint/ban-types
      [K in keyof T]: T[K] extends Function ? never : T[K];
    } extends T
  ? T
  : never;

export type Primitive = undefined | null | boolean | string | number | bigint | symbol;

export type InferredInstanceOf<T> = T extends Type<infer U> ? U : never;
export type InferredContextOf<T> = T extends TemplateRef<infer U> ? U : never;

export type Strict<Class, Contract> = Class extends Contract
  ? { [K in keyof Class]: K extends keyof Contract ? Contract[K] : never }
  : Contract;
