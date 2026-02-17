import { mapEnumToOptions } from '@abp/ng.core';

export enum CommentApproveState {
  All = 0,
  Approved = 1,
  Disapproved = 2,
  Waiting = 4,
}

export const commentApproveStateOptions = mapEnumToOptions(CommentApproveState);
