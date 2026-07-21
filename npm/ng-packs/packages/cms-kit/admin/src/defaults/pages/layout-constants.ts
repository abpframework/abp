import { eLayoutType } from '@abp/ng.core';

export const LAYOUT_CONSTANTS = {
  Account: eLayoutType.account,
  Public: 'public',
  Empty: eLayoutType.empty,
  Application: eLayoutType.application,
} as const;
