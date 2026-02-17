import { Component, OnInit, inject, Injector, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { LocalizationPipe } from '@abp/ng.core';
import {
  ExtensibleFormComponent,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import {
  ModalComponent,
  ModalCloseDirective,
  ButtonComponent,
  ToasterService,
} from '@abp/ng.theme.shared';
import { TagAdminService, TagDto, TagCreateDto, TagUpdateDto } from '@abp/ng.cms-kit/proxy';

export interface TagModalVisibleChange {
  visible: boolean;
  refresh: boolean;
}

@Component({
  selector: 'abp-tag-modal',
  templateUrl: './tag-modal.component.html',
  imports: [
    ExtensibleFormComponent,
    LocalizationPipe,
    ReactiveFormsModule,
    CommonModule,
    NgxValidateCoreModule,
    ModalComponent,
    ModalCloseDirective,
    ButtonComponent,
  ],
})
export class TagModalComponent implements OnInit {
  private tagService = inject(TagAdminService);
  private injector = inject(Injector);
  private toasterService = inject(ToasterService);

  selected = input<TagDto>();
  sectionId = input<string>();
  visibleChange = output<TagModalVisibleChange>();

  form: FormGroup;

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    const data = new FormPropData(this.injector, this.selected());
    this.form = generateFormFromProps(data);
  }

  onVisibleChange(visible: boolean, refresh = false) {
    this.visibleChange.emit({ visible, refresh });
  }

  save() {
    if (!this.form.valid) {
      return;
    }

    let observable$ = this.tagService.create(this.form.value as TagCreateDto);

    const selectedTag = this.selected();
    const { id } = selectedTag || {};

    if (id) {
      observable$ = this.tagService.update(id, {
        ...selectedTag,
        ...this.form.value,
      } as TagUpdateDto);
    }

    observable$.subscribe(() => {
      this.onVisibleChange(false, true);
      this.toasterService.success('AbpUi::SavedSuccessfully');
    });
  }
}
