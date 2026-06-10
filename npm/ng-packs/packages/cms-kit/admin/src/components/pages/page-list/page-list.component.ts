import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ListService, PagedResultDto, LocalizationPipe } from '@abp/ng.core';
import { ExtensibleTableComponent, EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { PageComponent } from '@abp/ng.components/page';
import { PageAdminService, GetPagesInputDto, PageDto } from '@abp/ng.cms-kit/proxy';
import { eCmsKitAdminComponents } from '../../../enums';

@Component({
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
export class PageListComponent implements OnInit {
  data: PagedResultDto<PageDto> = { items: [], totalCount: 0 };

  public readonly list = inject(ListService<GetPagesInputDto>);
  private pageService = inject(PageAdminService);
  private confirmationService = inject(ConfirmationService);
  private toasterService = inject(ToasterService);

  filter = '';

  ngOnInit() {
    this.hookToQuery();
  }

  onSearch() {
    this.list.filter = this.filter;
    this.list.get();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => {
        let filters: Partial<GetPagesInputDto> = {};
        if (this.list.filter) {
          filters.filter = this.list.filter;
        }
        const input: GetPagesInputDto = {
          ...query,
          ...filters,
        };
        return this.pageService.getList(input);
      })
      .subscribe(res => {
        this.data = res;
      });
  }

  delete(id: string) {
    this.confirmationService
      .warn('CmsKit::PageDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        yesText: 'AbpUi::Yes',
        cancelText: 'AbpUi::Cancel',
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.pageService.delete(id).subscribe(() => {
            this.list.get();
          });
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
