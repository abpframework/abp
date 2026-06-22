import { Component, OnInit, inject, Injector, input, output, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { finalize } from 'rxjs/operators';
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
import { BlogAdminService, BlogDto, CreateBlogDto, UpdateBlogDto } from '@abp/ng.cms-kit/proxy';
import { prepareSlugFromControl } from '@abp/ng.cms-kit';

export interface BlogModalVisibleChange {
  visible: boolean;
  refresh: boolean;
}

@Component({
  selector: 'abp-blog-modal',
  templateUrl: './blog-modal.component.html',
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
export class BlogModalComponent implements OnInit {
  private blogService = inject(BlogAdminService);
  private injector = inject(Injector);
  private toasterService = inject(ToasterService);
  private destroyRef = inject(DestroyRef);

  selected = input<BlogDto>();
  visibleChange = output<BlogModalVisibleChange>();

  modalBusy = false;

  form: FormGroup;

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    const data = new FormPropData(this.injector, this.selected());
    this.form = generateFormFromProps(data);
    prepareSlugFromControl(this.form, 'name', 'slug', this.destroyRef);
  }

  onVisibleChange(visible: boolean, refresh = false) {
    this.visibleChange.emit({ visible, refresh });
  }

  save() {
    if (this.modalBusy) {
      return;
    }
    this.modalBusy = true;

    if (!this.form.valid) {
      return;
    }

    let observable$ = this.blogService.create(this.form.value as CreateBlogDto);

    const selectedBlog = this.selected();
    const { id } = selectedBlog || {};

    if (id) {
      observable$ = this.blogService.update(id, {
        ...selectedBlog,
        ...this.form.value,
      } as UpdateBlogDto);
    }

    observable$.pipe(finalize(() => (this.modalBusy = false))).subscribe(() => {
      this.onVisibleChange(false, true);
      this.toasterService.success('AbpUi::SavedSuccessfully');
    });
  }
}
