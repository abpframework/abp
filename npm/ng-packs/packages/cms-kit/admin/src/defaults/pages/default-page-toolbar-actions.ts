import { Router } from '@angular/router';
import { PageDto } from '@abp/ng.cms-kit/proxy';
import { ToolbarAction } from '@abp/ng.components/extensible';

export const DEFAULT_PAGE_TOOLBAR_ACTIONS = ToolbarAction.createMany<PageDto[]>([
  {
    text: 'CmsKit::NewPage',
    action: data => {
      const router = data.getInjected(Router);
      router.navigate(['/cms/pages/create']);
    },
    permission: 'CmsKit.Pages.Create',
    icon: 'fa fa-plus',
  },
]);
