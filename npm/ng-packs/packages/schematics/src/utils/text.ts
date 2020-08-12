import { strings } from '@angular-devkit/core';

export const camel = strings.camelize;
export const kebab = strings.dasherize;
export const lower = (text: string) => text.toLowerCase();
export const macro = (text: string) => strings.underscore(text).toUpperCase();
export const pascal = strings.classify;
export const snake = strings.underscore;
export const upper = (text: string) => text.toUpperCase();
