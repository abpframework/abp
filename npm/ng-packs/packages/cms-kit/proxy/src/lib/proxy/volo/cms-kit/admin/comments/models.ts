import type { ExtensibleObject, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { CommentApproveState } from '../../comments/comment-approve-state.enum';

export interface CmsUserDto extends ExtensibleObject {
  id?: string;
  userName?: string;
  name?: string;
  surname?: string;
}

export interface CommentApprovalDto {
  isApproved: boolean;
}

export interface CommentGetListInput extends PagedAndSortedResultRequestDto {
  entityType?: string;
  text?: string;
  repliedCommentId?: string;
  author?: string;
  creationStartDate?: string;
  creationEndDate?: string;
  commentApproveState?: CommentApproveState;
}

export interface CommentSettingsDto {
  commentRequireApprovement: boolean;
}

export interface CommentWithAuthorDto extends ExtensibleObject {
  id?: string;
  entityType?: string;
  entityId?: string;
  text?: string;
  repliedCommentId?: string;
  creatorId?: string;
  creationTime?: string;
  author: CmsUserDto;
  url?: string;
  isApproved?: boolean;
}
