import { Component, OnInit, inject, Injector, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { LocalizationPipe } from '@abp/ng.core';
import {
  ExtensibleFormComponent,
  FormPropData,
  generateFormFromProps,
  EXTENSIONS_IDENTIFIER,
} from '@abp/ng.components/extensible';
import { PageComponent } from '@abp/ng.components/page';
import { ButtonComponent } from '@abp/ng.theme.shared';
import {
  ToastuiEditorComponent,
  CodeMirrorEditorComponent,
  prepareSlugFromControl,
} from '@abp/ng.cms-kit';
import { PageAdminService, PageDto } from '@abp/ng.cms-kit/proxy';
import { eCmsKitAdminComponents } from '../../../enums';
import { PageFormService } from '../../../services';

@Component({
  selector: 'abp-page-form',
  templateUrl: './page-form.component.html',
  providers: [
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eCmsKitAdminComponents.PageForm,
    },
  ],
  imports: [
    ButtonComponent,
    CodeMirrorEditorComponent,
    ExtensibleFormComponent,
    PageComponent,
    ToastuiEditorComponent,
    LocalizationPipe,
    ReactiveFormsModule,
    CommonModule,
    NgxValidateCoreModule,
    NgbNavModule,
  ],
})
export class PageFormComponent implements OnInit {
  private pageService = inject(PageAdminService);
  private injector = inject(Injector);
  private pageFormService = inject(PageFormService);
  private route = inject(ActivatedRoute);
  private destroyRef = inject(DestroyRef);

  form: FormGroup;
  page: PageDto | null = null;
  pageId: string | null = null;
  isEditMode = false;

  ngOnInit() {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.isEditMode = true;
      this.pageId = id;
      this.loadPage(id);
    } else {
      this.isEditMode = false;
      this.buildForm();
    }
  }

  private loadPage(id: string) {
    this.pageService.get(id).subscribe(page => {
      this.page = page;
      this.buildForm();
    });
  }

  private buildForm() {
    const data = new FormPropData(this.injector, this.page || {});
    const baseForm = generateFormFromProps(data);
    this.form = new FormGroup({
      ...baseForm.controls,
      content: new FormControl(this.page?.content || ''),
      script: new FormControl(this.page?.script || ''),
      style: new FormControl(this.page?.style || ''),
    });
    prepareSlugFromControl(this.form, 'title', 'slug', this.destroyRef);
  }

  private executeSaveOperation(operation: 'save' | 'draft' | 'publish') {
    if (this.isEditMode) {
      if (!this.page || !this.pageId) {
        return;
      }

      switch (operation) {
        case 'save':
          this.pageFormService.update(this.pageId, this.form, this.page).subscribe();
          break;
        case 'draft':
          this.pageFormService.updateAsDraft(this.pageId, this.form, this.page).subscribe();
          break;
        case 'publish':
          this.pageFormService.updateAndPublish(this.pageId, this.form, this.page).subscribe();
          break;
      }
      return;
    }

    switch (operation) {
      case 'save':
        this.pageFormService.create(this.form).subscribe();
        break;
      case 'draft':
        this.pageFormService.createAsDraft(this.form).subscribe();
        break;
      case 'publish':
        this.pageFormService.publish(this.form).subscribe();
        break;
    }
  }

  save() {
    this.executeSaveOperation('save');
  }

  saveAsDraft() {
    this.executeSaveOperation('draft');
  }

  publish() {
    this.executeSaveOperation('publish');
  }
}
