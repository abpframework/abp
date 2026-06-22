import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { inject, Injector } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { map, tap } from 'rxjs';
import { eCmsKitAdminComponents } from '../enums';
import {
  CMS_KIT_ADMIN_ENTITY_ACTION_CONTRIBUTORS,
  CMS_KIT_ADMIN_TOOLBAR_ACTION_CONTRIBUTORS,
  CMS_KIT_ADMIN_ENTITY_PROP_CONTRIBUTORS,
  CMS_KIT_ADMIN_CREATE_FORM_PROP_CONTRIBUTORS,
  CMS_KIT_ADMIN_EDIT_FORM_PROP_CONTRIBUTORS,
  DEFAULT_CMS_KIT_ADMIN_ENTITY_ACTIONS,
  DEFAULT_CMS_KIT_ADMIN_TOOLBAR_ACTIONS,
  DEFAULT_CMS_KIT_ADMIN_ENTITY_PROPS,
  DEFAULT_CMS_KIT_ADMIN_CREATE_FORM_PROPS,
  DEFAULT_CMS_KIT_ADMIN_EDIT_FORM_PROPS,
} from '../tokens';

export const cmsKitAdminExtensionsResolver: ResolveFn<any> = () => {
  const injector = inject(Injector);
  const extensions = inject(ExtensionsService);

  const config = { optional: true };

  const actionContributors = inject(CMS_KIT_ADMIN_ENTITY_ACTION_CONTRIBUTORS, config) || {};
  const toolbarContributors = inject(CMS_KIT_ADMIN_TOOLBAR_ACTION_CONTRIBUTORS, config) || {};
  const propContributors = inject(CMS_KIT_ADMIN_ENTITY_PROP_CONTRIBUTORS, config) || {};
  const createFormContributors = inject(CMS_KIT_ADMIN_CREATE_FORM_PROP_CONTRIBUTORS, config) || {};
  const editFormContributors = inject(CMS_KIT_ADMIN_EDIT_FORM_PROP_CONTRIBUTORS, config) || {};

  return getObjectExtensionEntitiesFromStore(injector, 'CmsKit').pipe(
    map(entities => ({
      [eCmsKitAdminComponents.CommentList]: entities.Comment,
      [eCmsKitAdminComponents.CommentDetails]: entities.Comment,
      [eCmsKitAdminComponents.Tags]: entities.Tag,
      [eCmsKitAdminComponents.Pages]: entities.Page,
      [eCmsKitAdminComponents.PageForm]: entities.Page,
      [eCmsKitAdminComponents.Blogs]: entities.Blog,
      [eCmsKitAdminComponents.BlogPosts]: entities.BlogPost,
      [eCmsKitAdminComponents.BlogPostForm]: entities.BlogPost,
      [eCmsKitAdminComponents.Menus]: entities.MenuItem,
    })),
    mapEntitiesToContributors(injector, 'CmsKit'),
    tap(objectExtensionContributors => {
      mergeWithDefaultActions(
        extensions.entityActions,
        DEFAULT_CMS_KIT_ADMIN_ENTITY_ACTIONS,
        actionContributors,
      );
      mergeWithDefaultActions(
        extensions.toolbarActions,
        DEFAULT_CMS_KIT_ADMIN_TOOLBAR_ACTIONS,
        toolbarContributors,
      );
      mergeWithDefaultProps(
        extensions.entityProps,
        DEFAULT_CMS_KIT_ADMIN_ENTITY_PROPS,
        objectExtensionContributors.prop,
        propContributors,
      );
      mergeWithDefaultProps(
        extensions.createFormProps,
        DEFAULT_CMS_KIT_ADMIN_CREATE_FORM_PROPS,
        objectExtensionContributors.createForm,
        createFormContributors,
      );
      mergeWithDefaultProps(
        extensions.editFormProps,
        DEFAULT_CMS_KIT_ADMIN_EDIT_FORM_PROPS,
        objectExtensionContributors.editForm,
        editFormContributors,
      );
    }),
  );
};
