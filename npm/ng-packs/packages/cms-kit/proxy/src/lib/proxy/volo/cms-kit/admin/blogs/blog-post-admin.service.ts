import type { BlogPostDto, BlogPostGetListInput, BlogPostListDto, CreateBlogPostDto, UpdateBlogPostDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BlogPostAdminService {
  private restService = inject(RestService);
  apiName = 'CmsKitAdmin';
  

  create = (input: CreateBlogPostDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlogPostDto>({
      method: 'POST',
      url: '/api/cms-kit-admin/blogs/blog-posts',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  createAndPublish = (input: CreateBlogPostDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlogPostDto>({
      method: 'POST',
      url: '/api/cms-kit-admin/blogs/blog-posts/create-and-publish',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  createAndSendToReview = (input: CreateBlogPostDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlogPostDto>({
      method: 'POST',
      url: '/api/cms-kit-admin/blogs/blog-posts/create-and-send-to-review',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/cms-kit-admin/blogs/blog-posts/${id}`,
    },
    { apiName: this.apiName,...config });
  

  draft = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/cms-kit-admin/blogs/blog-posts/${id}/draft`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlogPostDto>({
      method: 'GET',
      url: `/api/cms-kit-admin/blogs/blog-posts/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: BlogPostGetListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<BlogPostListDto>>({
      method: 'GET',
      url: '/api/cms-kit-admin/blogs/blog-posts',
      params: { filter: input.filter, blogId: input.blogId, authorId: input.authorId, tagId: input.tagId, status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  hasBlogPostWaitingForReview = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'GET',
      url: '/api/cms-kit-admin/blogs/blog-posts/has-blogpost-waiting-for-review',
    },
    { apiName: this.apiName,...config });
  

  publish = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/cms-kit-admin/blogs/blog-posts/${id}/publish`,
    },
    { apiName: this.apiName,...config });
  

  sendToReview = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/cms-kit-admin/blogs/blog-posts/${id}/send-to-review`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateBlogPostDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlogPostDto>({
      method: 'PUT',
      url: `/api/cms-kit-admin/blogs/blog-posts/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}