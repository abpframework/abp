import type { MenuItemCreateInput, MenuItemMoveInput, MenuItemUpdateInput, MenuItemWithDetailsDto, PageLookupDto, PageLookupInputDto, PermissionLookupDto, PermissionLookupInputDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { MenuItemDto } from '../../menus/models';

@Injectable({
  providedIn: 'root',
})
export class MenuItemAdminService {
  private restService = inject(RestService);
  apiName = 'CmsKitAdmin';
  

  create = (input: MenuItemCreateInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MenuItemDto>({
      method: 'POST',
      url: '/api/cms-kit-admin/menu-items',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/cms-kit-admin/menu-items/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MenuItemWithDetailsDto>({
      method: 'GET',
      url: `/api/cms-kit-admin/menu-items/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAvailableMenuOrder = (parentId?: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: '/api/cms-kit-admin/menu-items/available-order',
      params: { parentId },
    },
    { apiName: this.apiName,...config });
  

  getList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<MenuItemDto>>({
      method: 'GET',
      url: '/api/cms-kit-admin/menu-items',
    },
    { apiName: this.apiName,...config });
  

  getPageLookup = (input: PageLookupInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PageLookupDto>>({
      method: 'GET',
      url: '/api/cms-kit-admin/menu-items/lookup/pages',
      params: { filter: input.filter, status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPermissionLookup = (inputDto: PermissionLookupInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<PermissionLookupDto>>({
      method: 'GET',
      url: '/api/cms-kit-admin/menu-items/lookup/permissions',
      params: { filter: inputDto.filter },
    },
    { apiName: this.apiName,...config });
  

  moveMenuItem = (id: string, input: MenuItemMoveInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/cms-kit-admin/menu-items/${id}/move`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: MenuItemUpdateInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MenuItemDto>({
      method: 'PUT',
      url: `/api/cms-kit-admin/menu-items/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}