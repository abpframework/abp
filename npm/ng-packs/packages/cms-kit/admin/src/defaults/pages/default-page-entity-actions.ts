import { Router } from '@angular/router';
import { EntityAction } from '@abp/ng.components/extensible';
import { PageDto } from '@abp/ng.cms-kit/proxy';
import { PageListComponent } from '../../components/pages/page-list/page-list.component';

export const DEFAULT_PAGE_ENTITY_ACTIONS = EntityAction.createMany<PageDto>([
  {
    text: 'AbpUi::Edit',
    action: data => {
      const router = data.getInjected(Router);
      router.navigate(['/cms/pages/update', data.record.id]);
    },
    permission: 'CmsKit.Pages.Update',
  },
  {
    text: 'AbpUi::Delete',
    action: data => {
      const component = data.getInjected(PageListComponent);
      component.delete(data.record.id!);
    },
    permission: 'CmsKit.Pages.Delete',
  },
  {
    text: 'CmsKit::SetAsHomePage',
    action: data => {
      const component = data.getInjected(PageListComponent);
      const { id, isHomePage } = data.record;
      component.setAsHomePage(id!, isHomePage!);
    },
    permission: 'CmsKit.Pages.SetAsHomePage',
  },
]);
