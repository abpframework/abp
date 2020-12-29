import { TemplateRef, Type } from '@angular/core';

export type DeepPartial<T> = {
  [P in keyof T]?: T[P] extends Record<string | number | symbol, any> ? DeepPartial<T[P]> : T[P];
};

export type InferredInstanceOf<T> = T extends Type<infer U> ? U : never;
export type InferredContextOf<T> = T extends TemplateRef<infer U> ? U : never;

export type Strict<Class, Contract> = Class extends Contract
  ? { [K in keyof Class]: K extends keyof Contract ? Contract[K] : never }
  : Contract;
