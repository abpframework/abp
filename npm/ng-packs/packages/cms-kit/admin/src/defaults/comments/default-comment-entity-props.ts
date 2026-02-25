import { of } from 'rxjs';
import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { CommentWithAuthorDto } from '@abp/ng.cms-kit/proxy';
import { CommentEntityService } from '../../services';

export const DEFAULT_COMMENT_ENTITY_PROPS = EntityProp.createMany<CommentWithAuthorDto>([
  {
    type: ePropType.String,
    name: 'userName',
    displayName: 'CmsKit::Username',
    sortable: false,
    columnWidth: 150,
    valueResolver: data => {
      const userName = data.record.author.userName;
      return of(userName);
    },
  },
  {
    type: ePropType.String,
    name: 'entityType',
    displayName: 'CmsKit::EntityType',
    sortable: false,
    columnWidth: 200,
  },
  {
    type: ePropType.String,
    name: 'url',
    displayName: 'CmsKit::URL',
    sortable: false,
    valueResolver: data => {
      const url = data.record.url;
      if (url) {
        return of(
          `<a href="${url}#comment-${data.record.id}" target="_blank"><i class="fa fa-location-arrow"></i></a>`,
        );
      }
      return of('');
    },
  },
  {
    type: ePropType.String,
    name: 'text',
    displayName: 'CmsKit::Text',
    sortable: false,
    valueResolver: data => {
      const text = data.record.text || '';
      const maxChars = 64;
      if (text.length > maxChars) {
        return of(text.substring(0, maxChars) + '...');
      }
      return of(text);
    },
  },
  {
    type: ePropType.String,
    name: 'isApproved',
    displayName: 'CmsKit::ApproveState',
    sortable: false,
    columnWidth: 100,
    columnVisible: getInjected => {
      const commentEntityService = getInjected(CommentEntityService);
      return commentEntityService.requireApprovement;
    },
    valueResolver: data => {
      const isApproved = data.record.isApproved;
      if (isApproved || isApproved === null) {
        return of('<div class="text-success"><i class="fa fa-check" aria-hidden="true"></i></div>');
      }
      return of('<div class="text-danger"><i class="fa fa-times" aria-hidden="true"></i></div>');
    },
  },
  {
    type: ePropType.Date,
    name: 'creationTime',
    displayName: 'CmsKit::CreationTime',
    sortable: true,
    columnWidth: 200,
  },
]);
