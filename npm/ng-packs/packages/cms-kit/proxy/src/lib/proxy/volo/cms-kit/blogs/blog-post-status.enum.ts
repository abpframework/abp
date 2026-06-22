import { mapEnumToOptions } from '@abp/ng.core';

export enum BlogPostStatus {
  Draft = 0,
  Published = 1,
  WaitingForReview = 2,
}

export const blogPostStatusOptions = mapEnumToOptions(BlogPostStatus);
