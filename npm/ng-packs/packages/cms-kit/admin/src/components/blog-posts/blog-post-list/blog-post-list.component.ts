import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ListService, PagedResultDto, LocalizationPipe } from '@abp/ng.core';
import { ExtensibleTableComponent, EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { PageComponent } from '@abp/ng.components/page';
import {
  BlogPostAdminService,
  BlogPostGetListInput,
  BlogPostListDto,
  BlogPostStatus,
} from '@abp/ng.cms-kit/proxy';
import { eCmsKitAdminComponents } from '../../../enums';

@Component({
  selector: 'abp-blog-post-list',
  templateUrl: './blog-post-list.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eCmsKitAdminComponents.BlogPosts,
    },
  ],
  imports: [ExtensibleTableComponent, PageComponent, LocalizationPipe, FormsModule, CommonModule],
})
export class BlogPostListComponent implements OnInit {
  data: PagedResultDto<BlogPostListDto> = { items: [], totalCount: 0 };

  public readonly list = inject(ListService<BlogPostGetListInput>);
  private blogPostService = inject(BlogPostAdminService);
  private confirmationService = inject(ConfirmationService);

  filter = '';
  statusFilter: BlogPostStatus | null = null;
  BlogPostStatus = BlogPostStatus;

  ngOnInit() {
    this.hookToQuery();
  }

  onSearch() {
    this.list.filter = this.filter;
    this.list.get();
  }

  onStatusChange() {
    this.list.get();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => {
        let filters: Partial<BlogPostGetListInput> = {};
        if (this.list.filter) {
          filters.filter = this.list.filter;
        }
        if (this.statusFilter !== null) {
          filters.status = this.statusFilter;
        }
        const input: BlogPostGetListInput = {
          ...query,
          ...filters,
        };
        return this.blogPostService.getList(input);
      })
      .subscribe(res => (this.data = res));
  }

  delete(id: string, title: string) {
    this.confirmationService
      .warn('CmsKit::BlogPostDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        yesText: 'AbpUi::Yes',
        cancelText: 'AbpUi::Cancel',
        messageLocalizationParams: [title],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.blogPostService.delete(id).subscribe(() => {
            this.list.get();
          });
        }
      });
  }
}
