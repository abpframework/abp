import type { ExtensibleEntityDto, ExtensibleObject, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { BlogPostStatus } from '../../blogs/blog-post-status.enum';

export interface BlogDto extends ExtensibleEntityDto<string> {
  name?: string;
  slug?: string;
  concurrencyStamp?: string;
  blogPostCount: number;
}

export interface BlogFeatureInputDto {
  featureName: string;
  isEnabled: boolean;
}

export interface BlogGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface BlogPostDto extends ExtensibleEntityDto<string> {
  blogId?: string;
  title?: string;
  slug?: string;
  shortDescription?: string;
  content?: string;
  coverImageMediaId?: string;
  creationTime?: string;
  lastModificationTime?: string;
  concurrencyStamp?: string;
  status?: BlogPostStatus;
}

export interface BlogPostGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  blogId?: string;
  authorId?: string;
  tagId?: string;
  status?: BlogPostStatus;
}

export interface BlogPostListDto extends ExtensibleEntityDto<string> {
  blogId?: string;
  blogName?: string;
  title?: string;
  slug?: string;
  shortDescription?: string;
  content?: string;
  coverImageMediaId?: string;
  creationTime?: string;
  lastModificationTime?: string;
  status?: BlogPostStatus;
}

export interface CreateBlogDto extends ExtensibleObject {
  name: string;
  slug: string;
}

export interface CreateBlogPostDto extends ExtensibleObject {
  blogId: string;
  title: string;
  slug: string;
  shortDescription?: string;
  content?: string;
  coverImageMediaId?: string;
}

export interface UpdateBlogDto extends ExtensibleObject {
  name: string;
  slug: string;
  concurrencyStamp?: string;
}

export interface UpdateBlogPostDto extends ExtensibleObject {
  title: string;
  slug: string;
  shortDescription?: string;
  content?: string;
  coverImageMediaId?: string;
  concurrencyStamp?: string;
}
