import { mapEnumToOptions } from '@abp/ng.core';

export enum PageStatus {
  Draft = 0,
  Publish = 1,
}

export const pageStatusOptions = mapEnumToOptions(PageStatus);
