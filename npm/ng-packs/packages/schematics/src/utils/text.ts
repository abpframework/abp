import { strings } from '@angular-devkit/core';

export const lower = (text: string) => text.toLowerCase();
export const upper = (text: string) => text.toUpperCase();
export const camel = (text: string) => strings.camelize(_(text));
export const pascal = (text: string) => strings.classify(_(text));
export const kebab = (text: string) => strings.dasherize(_(text));
export const snake = (text: string) => strings.underscore(_(text));
export const macro = (text: string) => upper(snake(text));
export const dir = (text: string) =>
  strings.dasherize(text.replace(/\./g, '/').replace(/\/\//g, '/'));

function _(text: string): string {
  return text.replace(/\./g, '_');
}
