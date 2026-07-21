import { Router } from '@angular/router';
import { CommentGetListInput, CommentWithAuthorDto } from '@abp/ng.cms-kit/proxy';
import { ListService } from '@abp/ng.core';
import { EntityAction } from '@abp/ng.components/extensible';
import { CommentEntityService } from '../../services';

export const DEFAULT_COMMENT_ENTITY_ACTIONS = EntityAction.createMany<CommentWithAuthorDto>([
  {
    text: 'CmsKit::Details',
    action: data => {
      const router = data.getInjected(Router);
      router.navigate(['/cms/comments', data.record.id]);
    },
  },
  {
    text: 'CmsKit::Delete',
    action: data => {
      const commentEntityService = data.getInjected(CommentEntityService);
      const list = data.getInjected(ListService<CommentGetListInput>);
      commentEntityService.delete(data.record.id!, list);
    },
    permission: 'CmsKit.Comments.Delete',
  },
  {
    text: 'CmsKit::Approve',
    action: data => {
      const commentEntityService = data.getInjected(CommentEntityService);
      const list = data.getInjected(ListService<CommentGetListInput>);
      commentEntityService.updateApprovalStatus(data.record.id!, true, list);
    },
    visible: data => {
      const commentEntityService = data.getInjected(CommentEntityService);
      return commentEntityService.requireApprovement && data.record.isApproved === false;
    },
  },
  {
    text: 'CmsKit::Disapproved',
    action: data => {
      const commentEntityService = data.getInjected(CommentEntityService);
      const list = data.getInjected(ListService<CommentGetListInput>);
      commentEntityService.updateApprovalStatus(data.record.id!, false, list);
    },
    visible: data => {
      const commentEntityService = data.getInjected(CommentEntityService);
      return (
        commentEntityService.requireApprovement &&
        (data.record.isApproved || data.record.isApproved === null)
      );
    },
  },
]);
