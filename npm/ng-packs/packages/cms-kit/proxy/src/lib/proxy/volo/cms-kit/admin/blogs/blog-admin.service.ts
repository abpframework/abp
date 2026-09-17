import type { BlogDto, BlogGetListInput, CreateBlogDto, UpdateBlogDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BlogAdminService {
  private restService = inject(RestService);
  apiName = 'CmsKitAdmin';

  create = (input: CreateBlogDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlogDto>(
      {
        method: 'POST',
        url: '/api/cms-kit-admin/blogs',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/cms-kit-admin/blogs/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlogDto>(
      {
        method: 'GET',
        url: `/api/cms-kit-admin/blogs/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getAllList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<BlogDto>>(
      {
        method: 'GET',
        url: '/api/cms-kit-admin/blogs/all',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: BlogGetListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<BlogDto>>(
      {
        method: 'GET',
        url: '/api/cms-kit-admin/blogs',
        params: {
          filter: input.filter,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  moveAllBlogPosts = (blogId: string, assignToBlogId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/cms-kit-admin/blogs/${blogId}/move-all-blog-posts`,
        params: { blogId, assignToBlogId },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: UpdateBlogDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlogDto>(
      {
        method: 'PUT',
        url: `/api/cms-kit-admin/blogs/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );
}
