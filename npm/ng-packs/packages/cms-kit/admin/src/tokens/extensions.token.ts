import {
  CommentWithAuthorDto,
  TagDto,
  PageDto,
  BlogDto,
  BlogPostListDto,
  MenuItemDto,
  MenuItemCreateInput,
  MenuItemUpdateInput,
  TagCreateDto,
  TagUpdateDto,
  CreateBlogDto,
  CreateBlogPostDto,
  CreatePageInputDto,
  UpdateBlogDto,
  UpdateBlogPostDto,
  UpdatePageInputDto,
} from '@abp/ng.cms-kit/proxy';
import {
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
  CreateFormPropContributorCallback,
  EditFormPropContributorCallback,
} from '@abp/ng.components/extensible';
import { InjectionToken } from '@angular/core';
import { DEFAULT_COMMENT_ENTITY_ACTIONS } from '../defaults/comments/default-comment-entity-actions';
import { DEFAULT_COMMENT_ENTITY_PROPS } from '../defaults/comments/default-comment-entity-props';
import {
  DEFAULT_TAG_ENTITY_ACTIONS,
  DEFAULT_TAG_ENTITY_PROPS,
  DEFAULT_TAG_TOOLBAR_ACTIONS,
  DEFAULT_TAG_CREATE_FORM_PROPS,
  DEFAULT_TAG_EDIT_FORM_PROPS,
} from '../defaults/tags';
import {
  DEFAULT_PAGE_ENTITY_ACTIONS,
  DEFAULT_PAGE_ENTITY_PROPS,
  DEFAULT_PAGE_TOOLBAR_ACTIONS,
  DEFAULT_PAGE_CREATE_FORM_PROPS,
  DEFAULT_PAGE_EDIT_FORM_PROPS,
} from '../defaults/pages';
import {
  DEFAULT_BLOG_ENTITY_ACTIONS,
  DEFAULT_BLOG_ENTITY_PROPS,
  DEFAULT_BLOG_TOOLBAR_ACTIONS,
  DEFAULT_BLOG_CREATE_FORM_PROPS,
  DEFAULT_BLOG_EDIT_FORM_PROPS,
} from '../defaults/blogs';
import {
  DEFAULT_BLOG_POST_ENTITY_ACTIONS,
  DEFAULT_BLOG_POST_ENTITY_PROPS,
  DEFAULT_BLOG_POST_TOOLBAR_ACTIONS,
  DEFAULT_BLOG_POST_CREATE_FORM_PROPS,
  DEFAULT_BLOG_POST_EDIT_FORM_PROPS,
} from '../defaults/blog-posts';
import {
  DEFAULT_MENU_ITEM_CREATE_FORM_PROPS,
  DEFAULT_MENU_ITEM_EDIT_FORM_PROPS,
  DEFAULT_MENU_ITEM_TOOLBAR_ACTIONS,
} from '../defaults/menus';
import { eCmsKitAdminComponents } from '../enums';

export const DEFAULT_CMS_KIT_ADMIN_ENTITY_ACTIONS = {
  [eCmsKitAdminComponents.CommentList]: DEFAULT_COMMENT_ENTITY_ACTIONS,
  [eCmsKitAdminComponents.CommentDetails]: DEFAULT_COMMENT_ENTITY_ACTIONS,
  [eCmsKitAdminComponents.Tags]: DEFAULT_TAG_ENTITY_ACTIONS,
  [eCmsKitAdminComponents.Pages]: DEFAULT_PAGE_ENTITY_ACTIONS,
  [eCmsKitAdminComponents.Blogs]: DEFAULT_BLOG_ENTITY_ACTIONS,
  [eCmsKitAdminComponents.BlogPosts]: DEFAULT_BLOG_POST_ENTITY_ACTIONS,
};

export const DEFAULT_CMS_KIT_ADMIN_ENTITY_PROPS = {
  [eCmsKitAdminComponents.CommentList]: DEFAULT_COMMENT_ENTITY_PROPS,
  [eCmsKitAdminComponents.CommentDetails]: DEFAULT_COMMENT_ENTITY_PROPS,
  [eCmsKitAdminComponents.Tags]: DEFAULT_TAG_ENTITY_PROPS,
  [eCmsKitAdminComponents.Pages]: DEFAULT_PAGE_ENTITY_PROPS,
  [eCmsKitAdminComponents.Blogs]: DEFAULT_BLOG_ENTITY_PROPS,
  [eCmsKitAdminComponents.BlogPosts]: DEFAULT_BLOG_POST_ENTITY_PROPS,
};

export const DEFAULT_CMS_KIT_ADMIN_TOOLBAR_ACTIONS = {
  [eCmsKitAdminComponents.Tags]: DEFAULT_TAG_TOOLBAR_ACTIONS,
  [eCmsKitAdminComponents.Pages]: DEFAULT_PAGE_TOOLBAR_ACTIONS,
  [eCmsKitAdminComponents.Blogs]: DEFAULT_BLOG_TOOLBAR_ACTIONS,
  [eCmsKitAdminComponents.BlogPosts]: DEFAULT_BLOG_POST_TOOLBAR_ACTIONS,
  [eCmsKitAdminComponents.Menus]: DEFAULT_MENU_ITEM_TOOLBAR_ACTIONS,
};

export const DEFAULT_CMS_KIT_ADMIN_CREATE_FORM_PROPS = {
  [eCmsKitAdminComponents.Tags]: DEFAULT_TAG_CREATE_FORM_PROPS,
  [eCmsKitAdminComponents.Pages]: DEFAULT_PAGE_CREATE_FORM_PROPS,
  [eCmsKitAdminComponents.Blogs]: DEFAULT_BLOG_CREATE_FORM_PROPS,
  [eCmsKitAdminComponents.PageForm]: DEFAULT_PAGE_CREATE_FORM_PROPS,
  [eCmsKitAdminComponents.BlogPostForm]: DEFAULT_BLOG_POST_CREATE_FORM_PROPS,
  [eCmsKitAdminComponents.Menus]: DEFAULT_MENU_ITEM_CREATE_FORM_PROPS,
};

export const DEFAULT_CMS_KIT_ADMIN_EDIT_FORM_PROPS = {
  [eCmsKitAdminComponents.Tags]: DEFAULT_TAG_EDIT_FORM_PROPS,
  [eCmsKitAdminComponents.Pages]: DEFAULT_PAGE_EDIT_FORM_PROPS,
  [eCmsKitAdminComponents.Blogs]: DEFAULT_BLOG_EDIT_FORM_PROPS,
  [eCmsKitAdminComponents.PageForm]: DEFAULT_PAGE_EDIT_FORM_PROPS,
  [eCmsKitAdminComponents.BlogPostForm]: DEFAULT_BLOG_POST_EDIT_FORM_PROPS,
  [eCmsKitAdminComponents.Menus]: DEFAULT_MENU_ITEM_EDIT_FORM_PROPS,
};

export const CMS_KIT_ADMIN_ENTITY_ACTION_CONTRIBUTORS =
  new InjectionToken<EntityActionContributors>('CMS_KIT_ADMIN_ENTITY_ACTION_CONTRIBUTORS');

export const CMS_KIT_ADMIN_ENTITY_PROP_CONTRIBUTORS = new InjectionToken<EntityPropContributors>(
  'CMS_KIT_ADMIN_ENTITY_PROP_CONTRIBUTORS',
);

export const CMS_KIT_ADMIN_TOOLBAR_ACTION_CONTRIBUTORS =
  new InjectionToken<ToolbarActionContributors>('CMS_KIT_ADMIN_TOOLBAR_ACTION_CONTRIBUTORS');

export const CMS_KIT_ADMIN_CREATE_FORM_PROP_CONTRIBUTORS =
  new InjectionToken<CreateFormPropContributors>('CMS_KIT_ADMIN_CREATE_FORM_PROP_CONTRIBUTORS');

export const CMS_KIT_ADMIN_EDIT_FORM_PROP_CONTRIBUTORS =
  new InjectionToken<EditFormPropContributors>('CMS_KIT_ADMIN_EDIT_FORM_PROP_CONTRIBUTORS');

// Fix for TS4023 -> https://github.com/microsoft/TypeScript/issues/9944#issuecomment-254693497
type EntityActionContributors = Partial<{
  [eCmsKitAdminComponents.CommentList]: EntityActionContributorCallback<CommentWithAuthorDto>[];
  [eCmsKitAdminComponents.CommentDetails]: EntityActionContributorCallback<CommentWithAuthorDto>[];
  [eCmsKitAdminComponents.Tags]: EntityActionContributorCallback<TagDto>[];
  [eCmsKitAdminComponents.Pages]: EntityActionContributorCallback<PageDto>[];
  [eCmsKitAdminComponents.Blogs]: EntityActionContributorCallback<BlogDto>[];
  [eCmsKitAdminComponents.BlogPosts]: EntityActionContributorCallback<BlogPostListDto>[];
}>;

type EntityPropContributors = Partial<{
  [eCmsKitAdminComponents.CommentList]: EntityPropContributorCallback<CommentWithAuthorDto>[];
  [eCmsKitAdminComponents.CommentDetails]: EntityPropContributorCallback<CommentWithAuthorDto>[];
  [eCmsKitAdminComponents.Tags]: EntityPropContributorCallback<TagDto>[];
  [eCmsKitAdminComponents.Pages]: EntityPropContributorCallback<PageDto>[];
  [eCmsKitAdminComponents.Blogs]: EntityPropContributorCallback<BlogDto>[];
  [eCmsKitAdminComponents.BlogPosts]: EntityPropContributorCallback<BlogPostListDto>[];
}>;

type ToolbarActionContributors = Partial<{
  [eCmsKitAdminComponents.Tags]: ToolbarActionContributorCallback<TagDto[]>[];
  [eCmsKitAdminComponents.Pages]: ToolbarActionContributorCallback<PageDto[]>[];
  [eCmsKitAdminComponents.Blogs]: ToolbarActionContributorCallback<BlogDto[]>[];
  [eCmsKitAdminComponents.BlogPosts]: ToolbarActionContributorCallback<BlogPostListDto[]>[];
  [eCmsKitAdminComponents.Menus]: ToolbarActionContributorCallback<MenuItemDto[]>[];
}>;

type CreateFormPropContributors = Partial<{
  [eCmsKitAdminComponents.Tags]: CreateFormPropContributorCallback<TagCreateDto>[];
  [eCmsKitAdminComponents.PageForm]: CreateFormPropContributorCallback<CreatePageInputDto>[];
  [eCmsKitAdminComponents.Blogs]: CreateFormPropContributorCallback<CreateBlogDto>[];
  [eCmsKitAdminComponents.BlogPostForm]: CreateFormPropContributorCallback<CreateBlogPostDto>[];
  [eCmsKitAdminComponents.Menus]: CreateFormPropContributorCallback<MenuItemCreateInput>[];
}>;

type EditFormPropContributors = Partial<{
  [eCmsKitAdminComponents.Tags]: EditFormPropContributorCallback<TagUpdateDto>[];
  [eCmsKitAdminComponents.PageForm]: EditFormPropContributorCallback<UpdatePageInputDto>[];
  [eCmsKitAdminComponents.Blogs]: EditFormPropContributorCallback<UpdateBlogDto>[];
  [eCmsKitAdminComponents.BlogPostForm]: EditFormPropContributorCallback<UpdateBlogPostDto>[];
  [eCmsKitAdminComponents.Menus]: EditFormPropContributorCallback<MenuItemUpdateInput>[];
}>;
