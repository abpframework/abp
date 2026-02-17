import { eCmsKitAdminComponents } from '../enums';
import {
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
  CreateFormPropContributorCallback,
  EditFormPropContributorCallback,
} from '@abp/ng.components/extensible';
import {
  CommentWithAuthorDto,
  TagDto,
  PageDto,
  BlogDto,
  BlogPostListDto,
  MenuItemDto,
  MenuItemCreateInput,
  MenuItemUpdateInput,
  CreatePageInputDto,
  UpdatePageInputDto,
  CreateBlogDto,
  CreateBlogPostDto,
  UpdateBlogDto,
  UpdateBlogPostDto,
  TagCreateDto,
  TagUpdateDto,
} from '@abp/ng.cms-kit/proxy';

export type CmsKitAdminEntityActionContributors = Partial<{
  [eCmsKitAdminComponents.CommentList]: EntityActionContributorCallback<CommentWithAuthorDto>[];
  [eCmsKitAdminComponents.CommentDetails]: EntityActionContributorCallback<CommentWithAuthorDto>[];
  [eCmsKitAdminComponents.Tags]: EntityActionContributorCallback<TagDto>[];
  [eCmsKitAdminComponents.Pages]: EntityActionContributorCallback<PageDto>[];
  [eCmsKitAdminComponents.Blogs]: EntityActionContributorCallback<BlogDto>[];
  [eCmsKitAdminComponents.BlogPosts]: EntityActionContributorCallback<BlogPostListDto>[];
}>;

export type CmsKitAdminEntityPropContributors = Partial<{
  [eCmsKitAdminComponents.CommentList]: EntityPropContributorCallback<CommentWithAuthorDto>[];
  [eCmsKitAdminComponents.CommentDetails]: EntityPropContributorCallback<CommentWithAuthorDto>[];
  [eCmsKitAdminComponents.Tags]: EntityPropContributorCallback<TagDto>[];
  [eCmsKitAdminComponents.Pages]: EntityPropContributorCallback<PageDto>[];
  [eCmsKitAdminComponents.Blogs]: EntityPropContributorCallback<BlogDto>[];
  [eCmsKitAdminComponents.BlogPosts]: EntityPropContributorCallback<BlogPostListDto>[];
}>;

export type CmsKitAdminToolbarActionContributors = Partial<{
  [eCmsKitAdminComponents.Tags]: ToolbarActionContributorCallback<TagDto[]>[];
  [eCmsKitAdminComponents.Pages]: ToolbarActionContributorCallback<PageDto[]>[];
  [eCmsKitAdminComponents.Blogs]: ToolbarActionContributorCallback<BlogDto[]>[];
  [eCmsKitAdminComponents.BlogPosts]: ToolbarActionContributorCallback<BlogPostListDto[]>[];
  [eCmsKitAdminComponents.Menus]: ToolbarActionContributorCallback<MenuItemDto[]>[];
}>;

export type CmsKitAdminCreateFormPropContributors = Partial<{
  [eCmsKitAdminComponents.Tags]: CreateFormPropContributorCallback<TagCreateDto>[];
  [eCmsKitAdminComponents.PageForm]: CreateFormPropContributorCallback<CreatePageInputDto>[];
  [eCmsKitAdminComponents.Blogs]: CreateFormPropContributorCallback<CreateBlogDto>[];
  [eCmsKitAdminComponents.BlogPostForm]: CreateFormPropContributorCallback<CreateBlogPostDto>[];
  [eCmsKitAdminComponents.Menus]: CreateFormPropContributorCallback<MenuItemCreateInput>[];
}>;

export type CmsKitAdminEditFormPropContributors = Partial<{
  [eCmsKitAdminComponents.Tags]: EditFormPropContributorCallback<TagUpdateDto>[];
  [eCmsKitAdminComponents.PageForm]: EditFormPropContributorCallback<UpdatePageInputDto>[];
  [eCmsKitAdminComponents.Blogs]: EditFormPropContributorCallback<UpdateBlogDto>[];
  [eCmsKitAdminComponents.BlogPostForm]: EditFormPropContributorCallback<UpdateBlogPostDto>[];
  [eCmsKitAdminComponents.Menus]: EditFormPropContributorCallback<MenuItemUpdateInput>[];
}>;

export interface CmsKitAdminConfigOptions {
  entityActionContributors?: CmsKitAdminEntityActionContributors;
  entityPropContributors?: CmsKitAdminEntityPropContributors;
  toolbarActionContributors?: CmsKitAdminToolbarActionContributors;
  createFormPropContributors?: CmsKitAdminCreateFormPropContributors;
  editFormPropContributors?: CmsKitAdminEditFormPropContributors;
}
