import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
  Injector,
  OnInit,
  signal,
} from '@angular/core';
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
  changeDetection: ChangeDetectionStrategy.OnPush,
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
  readonly page = signal<PageDto | null>(null);
  pageId: string | null = null;
  readonly isEditMode = signal(false);

  ngOnInit() {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.isEditMode.set(true);
      this.pageId = id;
      this.loadPage(id);
    } else {
      this.isEditMode.set(false);
      this.buildForm();
    }
  }

  private loadPage(id: string) {
    this.pageService.get(id).subscribe(page => {
      this.page.set(page);
      this.buildForm();
    });
  }

  private buildForm() {
    const currentPage = this.page();
    const data = new FormPropData(this.injector, currentPage || {});
    const baseForm = generateFormFromProps(data);
    this.form = new FormGroup({
      ...baseForm.controls,
      content: new FormControl(currentPage?.content || ''),
      script: new FormControl(currentPage?.script || ''),
      style: new FormControl(currentPage?.style || ''),
    });
    prepareSlugFromControl(this.form, 'title', 'slug', this.destroyRef);
  }

  private executeSaveOperation(operation: 'save' | 'draft' | 'publish') {
    if (this.isEditMode()) {
      const currentPage = this.page();
      if (!currentPage || !this.pageId) {
        return;
      }

      switch (operation) {
        case 'save':
          this.pageFormService.update(this.pageId, this.form, currentPage).subscribe();
          break;
        case 'draft':
          this.pageFormService.updateAsDraft(this.pageId, this.form, currentPage).subscribe();
          break;
        case 'publish':
          this.pageFormService.updateAndPublish(this.pageId, this.form, currentPage).subscribe();
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
