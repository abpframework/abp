import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ListService, PagedResultDto, LocalizationPipe } from '@abp/ng.core';
import { ExtensibleTableComponent, EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { PageComponent } from '@abp/ng.components/page';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { TagAdminService, TagGetListInput, TagDto, TagDefinitionDto } from '@abp/ng.cms-kit/proxy';
import { eCmsKitAdminComponents } from '../../../enums';
import { TagModalComponent, TagModalVisibleChange } from '../tag-modal/tag-modal.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-tag-list',
  templateUrl: './tag-list.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eCmsKitAdminComponents.Tags,
    },
  ],
  imports: [
    ExtensibleTableComponent,
    PageComponent,
    LocalizationPipe,
    FormsModule,
    CommonModule,
    TagModalComponent,
  ],
})
export class TagListComponent {
  public readonly list = inject(ListService<TagGetListInput>);
  private tagService = inject(TagAdminService);
  private confirmationService = inject(ConfirmationService);

  readonly data = toSignal(
    this.list.hookToQuery(query => {
      let filters: Partial<TagGetListInput> = {};
      if (this.list.filter) {
        filters.filter = this.list.filter;
      }
      const input: TagGetListInput = {
        ...query,
        ...filters,
      };
      return this.tagService.getList(input);
    }),
    {
      initialValue: { items: [], totalCount: 0 } as PagedResultDto<TagDto>,
    },
  );

  readonly tagDefinitions = toSignal(this.tagService.getTagDefinitions(), {
    initialValue: [] as TagDefinitionDto[],
  });

  filter = '';
  readonly isModalVisible = signal(false);
  readonly selected = signal<TagDto | undefined>(undefined);

  onSearch() {
    this.list.filter = this.filter;
    this.list.get();
  }

  add() {
    this.selected.set({} as TagDto);
    this.isModalVisible.set(true);
  }

  edit(id: string) {
    this.tagService.get(id).subscribe(tag => {
      this.selected.set(tag);
      this.isModalVisible.set(true);
    });
  }

  onVisibleModalChange(visibilityChange: TagModalVisibleChange) {
    if (visibilityChange.visible) {
      return;
    }
    if (visibilityChange.refresh) {
      this.list.get();
    }
    this.selected.set(undefined);
    this.isModalVisible.set(false);
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn('CmsKit::TagDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.tagService.delete(id).subscribe(() => this.list.get());
        }
      });
  }
}
