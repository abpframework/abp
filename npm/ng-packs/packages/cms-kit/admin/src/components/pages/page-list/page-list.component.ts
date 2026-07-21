import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ListService, PagedResultDto, LocalizationPipe } from '@abp/ng.core';
import { ExtensibleTableComponent, EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { PageComponent } from '@abp/ng.components/page';
import { PageAdminService, GetPagesInputDto, PageDto } from '@abp/ng.cms-kit/proxy';
import { eCmsKitAdminComponents } from '../../../enums';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-page-list',
  templateUrl: './page-list.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eCmsKitAdminComponents.Pages,
    },
  ],
  imports: [ExtensibleTableComponent, PageComponent, LocalizationPipe, FormsModule, CommonModule],
})
export class PageListComponent {
  public readonly list = inject(ListService<GetPagesInputDto>);
  private pageService = inject(PageAdminService);
  private confirmationService = inject(ConfirmationService);
  private toasterService = inject(ToasterService);

  readonly data = toSignal(
    this.list.hookToQuery(query => {
      let filters: Partial<GetPagesInputDto> = {};
      if (this.list.filter) {
        filters.filter = this.list.filter;
      }
      const input: GetPagesInputDto = {
        ...query,
        ...filters,
      };
      return this.pageService.getList(input);
    }),
    {
      initialValue: { items: [], totalCount: 0 } as PagedResultDto<PageDto>,
    },
  );

  filter = '';

  onSearch() {
    this.list.filter = this.filter;
    this.list.get();
  }

  delete(id: string) {
    this.confirmationService
      .warn('CmsKit::PageDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        yesText: 'AbpUi::Yes',
        cancelText: 'AbpUi::Cancel',
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.pageService.delete(id).subscribe(() => this.list.get());
        }
      });
  }

  setAsHomePage(id: string, isHomePage: boolean) {
    this.pageService.setAsHomePage(id).subscribe(() => {
      this.list.get();
      if (isHomePage) {
        this.toasterService.warn('CmsKit::RemovedSettingAsHomePage');
      } else {
        this.toasterService.success('CmsKit::CompletedSettingAsHomePage');
      }
    });
  }
}
