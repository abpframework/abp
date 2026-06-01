import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { PageComponent } from '@abp/ng.components/page';
import { LocalizationPipe } from '@abp/ng.core';
import { ButtonComponent, ToasterService } from '@abp/ng.theme.shared';
import { CodeMirrorEditorComponent } from '@abp/ng.cms-kit';
import {
  GlobalResourceAdminService,
  GlobalResourcesDto,
  GlobalResourcesUpdateDto,
} from '@abp/ng.cms-kit/proxy';

@Component({
  selector: 'abp-global-resources',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgxValidateCoreModule,
    NgbNavModule,
    CodeMirrorEditorComponent,
    LocalizationPipe,
    PageComponent,
    ButtonComponent,
  ],
  templateUrl: './global-resources.component.html',
})
export class GlobalResourcesComponent implements OnInit {
  private globalResourceService = inject(GlobalResourceAdminService);
  private toasterService = inject(ToasterService);
  private destroyRef = inject(DestroyRef);

  form: FormGroup;
  activeTab: string = 'script';

  ngOnInit() {
    this.buildForm();
    this.loadGlobalResources();
  }

  private buildForm() {
    this.form = new FormGroup({
      script: new FormControl(''),
      style: new FormControl(''),
    });
  }

  private loadGlobalResources() {
    this.globalResourceService
      .get()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (result: GlobalResourcesDto) => {
          this.form.patchValue({
            script: result.scriptContent || '',
            style: result.styleContent || '',
          });
        },
        error: () => {
          this.toasterService.error('AbpUi::ErrorMessage');
        },
      });
  }

  onTabChange(activeId: string) {
    this.activeTab = activeId;
  }

  save() {
    if (!this.form.valid) {
      return;
    }

    const formValue = this.form.value;
    const input: GlobalResourcesUpdateDto = {
      script: formValue.script || '',
      style: formValue.style || '',
    };

    this.globalResourceService
      .setGlobalResources(input)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.toasterService.success('AbpUi::SavedSuccessfully');
        },
        error: () => {
          this.toasterService.error('AbpUi::ErrorMessage');
        },
      });
  }
}
