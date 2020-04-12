import { TemplateRef, Type } from '@angular/core';

export type InferredInstanceOf<T> = T extends Type<infer U> ? U : never;
export type InferredContextOf<T> = T extends TemplateRef<infer U> ? U : never;
