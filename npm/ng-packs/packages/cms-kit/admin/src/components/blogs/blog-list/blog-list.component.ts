import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ListService, PagedResultDto, LocalizationPipe } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { ExtensibleTableComponent, EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { PageComponent } from '@abp/ng.components/page';
import { BlogAdminService, BlogGetListInput, BlogDto } from '@abp/ng.cms-kit/proxy';
import { eCmsKitAdminComponents } from '../../../enums';
import { BlogModalComponent, BlogModalVisibleChange } from '../blog-modal/blog-modal.component';
import {
  BlogFeaturesModalComponent,
  BlogFeaturesModalVisibleChange,
} from '../blog-features-modal/blog-features-modal.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-blog-list',
  templateUrl: './blog-list.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eCmsKitAdminComponents.Blogs,
    },
  ],
  imports: [
    ExtensibleTableComponent,
    PageComponent,
    LocalizationPipe,
    FormsModule,
    CommonModule,
    BlogModalComponent,
    BlogFeaturesModalComponent,
  ],
})
export class BlogListComponent {
  public readonly list = inject(ListService<BlogGetListInput>);
  private blogService = inject(BlogAdminService);
  private confirmationService = inject(ConfirmationService);

  readonly data = toSignal(
    this.list.hookToQuery(query => {
      let filters: Partial<BlogGetListInput> = {};
      if (this.list.filter) {
        filters.filter = this.list.filter;
      }
      const input: BlogGetListInput = {
        ...query,
        ...filters,
      };
      return this.blogService.getList(input);
    }),
    {
      initialValue: { items: [], totalCount: 0 } as PagedResultDto<BlogDto>,
    },
  );

  filter = '';
  readonly isModalVisible = signal(false);
  readonly selected = signal<BlogDto | undefined>(undefined);
  readonly isFeaturesModalVisible = signal(false);
  readonly selectedBlogId = signal<string | undefined>(undefined);

  onSearch() {
    this.list.filter = this.filter;
    this.list.get();
  }

  add() {
    this.selected.set({} as BlogDto);
    this.isModalVisible.set(true);
  }

  edit(id: string) {
    this.blogService.get(id).subscribe(blog => {
      this.selected.set(blog);
      this.isModalVisible.set(true);
    });
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn('CmsKit::BlogDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.blogService.delete(id).subscribe(() => this.list.get());
        }
      });
  }

  openFeatures(id: string) {
    this.selectedBlogId.set(id);
    this.isFeaturesModalVisible.set(true);
  }

  onVisibleModalChange(visibilityChange: BlogModalVisibleChange) {
    if (visibilityChange.visible) {
      return;
    }
    if (visibilityChange.refresh) {
      this.list.get();
    }
    this.selected.set(undefined);
    this.isModalVisible.set(false);
  }

  onFeaturesModalChange(visibilityChange: BlogFeaturesModalVisibleChange) {
    if (visibilityChange.visible) {
      return;
    }
    if (visibilityChange.refresh) {
      this.list.get();
    }
    this.selectedBlogId.set(undefined);
    this.isFeaturesModalVisible.set(false);
  }
}
