import type { CommentApprovalDto, CommentGetListInput, CommentSettingsDto, CommentWithAuthorDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CommentAdminService {
  private restService = inject(RestService);
  apiName = 'CmsKitAdmin';
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/cms-kit-admin/comments/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommentWithAuthorDto>({
      method: 'GET',
      url: `/api/cms-kit-admin/comments/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: CommentGetListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CommentWithAuthorDto>>({
      method: 'GET',
      url: '/api/cms-kit-admin/comments',
      params: { entityType: input.entityType, text: input.text, repliedCommentId: input.repliedCommentId, author: input.author, creationStartDate: input.creationStartDate, creationEndDate: input.creationEndDate, commentApproveState: input.commentApproveState, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getWaitingCount = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: '/api/cms-kit-admin/comments/waiting-count',
    },
    { apiName: this.apiName,...config });
  

  updateApprovalStatus = (id: string, input: CommentApprovalDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/cms-kit-admin/comments/${id}/approval-status`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateSettings = (input: CommentSettingsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/cms-kit-admin/comments/settings',
      body: input,
    },
    { apiName: this.apiName,...config });
}