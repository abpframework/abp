import { Component, OnInit, inject } from '@angular/core';
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
export class TagListComponent implements OnInit {
  data: PagedResultDto<TagDto> = { items: [], totalCount: 0 };

  public readonly list = inject(ListService<TagGetListInput>);
  private tagService = inject(TagAdminService);
  private confirmationService = inject(ConfirmationService);

  filter = '';
  isModalVisible = false;
  selected?: TagDto;
  tagDefinitions: TagDefinitionDto[] = [];

  ngOnInit() {
    this.loadTagDefinitions();
    this.hookToQuery();
  }

  private loadTagDefinitions() {
    this.tagService.getTagDefinitions().subscribe(definitions => {
      this.tagDefinitions = definitions;
    });
  }

  onSearch() {
    this.list.filter = this.filter;
    this.list.get();
  }

  add() {
    this.selected = {} as TagDto;
    this.isModalVisible = true;
  }

  edit(id: string) {
    this.tagService.get(id).subscribe(tag => {
      this.selected = tag;
      this.isModalVisible = true;
    });
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => {
        let filters: Partial<TagGetListInput> = {};
        if (this.list.filter) {
          filters.filter = this.list.filter;
        }
        const input: TagGetListInput = {
          ...query,
          ...filters,
        };
        return this.tagService.getList(input);
      })
      .subscribe(res => {
        this.data = res;
      });
  }

  onVisibleModalChange(visibilityChange: TagModalVisibleChange) {
    if (visibilityChange.visible) {
      return;
    }
    if (visibilityChange.refresh) {
      this.list.get();
    }
    this.selected = null;
    this.isModalVisible = false;
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn('CmsKit::TagDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.tagService.delete(id).subscribe(() => {
            this.list.get();
          });
        }
      });
  }
}
