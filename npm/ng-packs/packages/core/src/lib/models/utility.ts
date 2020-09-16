import { TemplateRef, Type } from '@angular/core';

export type DeepPartial<T> = {
  [P in keyof T]?: T[P] extends Serializable ? DeepPartial<T[P]> : T[P];
};

type Serializable = Record<
  string | number | symbol,
  string | number | boolean | Record<string | number | symbol, any>
>;

export type InferredInstanceOf<T> = T extends Type<infer U> ? U : never;
export type InferredContextOf<T> = T extends TemplateRef<infer U> ? U : never;
