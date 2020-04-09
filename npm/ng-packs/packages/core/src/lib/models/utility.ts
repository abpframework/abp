import { TemplateRef, Type } from '@angular/core';

export type InferedInstanceOf<T> = T extends Type<infer U> ? U : never;
export type InferedContextOf<T> = T extends TemplateRef<infer U> ? U : never;
