import type { EntityTagCreateDto, EntityTagRemoveDto, EntityTagSetDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EntityTagAdminService {
  private restService = inject(RestService);
  apiName = 'CmsKitAdmin';
  

  addTagToEntity = (input: EntityTagCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/cms-kit-admin/entity-tags',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  removeTagFromEntity = (input: EntityTagRemoveDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/cms-kit-admin/entity-tags',
      params: { tagId: input.tagId, entityType: input.entityType, entityId: input.entityId },
    },
    { apiName: this.apiName,...config });
  

  setEntityTags = (input: EntityTagSetDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: '/api/cms-kit-admin/entity-tags',
      body: input,
    },
    { apiName: this.apiName,...config });
}