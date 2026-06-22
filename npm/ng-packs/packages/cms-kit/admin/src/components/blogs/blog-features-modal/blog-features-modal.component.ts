import { Component, OnInit, inject, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { forkJoin } from 'rxjs';
import { LocalizationPipe } from '@abp/ng.core';
import {
  ModalComponent,
  ModalCloseDirective,
  ButtonComponent,
  ToasterService,
} from '@abp/ng.theme.shared';
import {
  BlogFeatureAdminService,
  BlogFeatureDto,
  BlogFeatureInputDto,
} from '@abp/ng.cms-kit/proxy';

export interface BlogFeaturesModalVisibleChange {
  visible: boolean;
  refresh: boolean;
}

@Component({
  selector: 'abp-blog-features-modal',
  templateUrl: './blog-features-modal.component.html',
  imports: [
    LocalizationPipe,
    ReactiveFormsModule,
    CommonModule,
    NgxValidateCoreModule,
    ModalComponent,
    ModalCloseDirective,
    ButtonComponent,
  ],
})
export class BlogFeaturesModalComponent implements OnInit {
  private blogFeatureService = inject(BlogFeatureAdminService);
  private fb = inject(FormBuilder);
  private toasterService = inject(ToasterService);

  blogId = input<string>();
  visibleChange = output<BlogFeaturesModalVisibleChange>();

  form: FormGroup;
  features: BlogFeatureDto[] = [];
  private initialFeatureStates: Map<string, boolean> = new Map();

  ngOnInit() {
    if (this.blogId()) {
      this.loadFeatures();
    }
  }

  private loadFeatures() {
    this.blogFeatureService.getList(this.blogId()!).subscribe(features => {
      this.features = features.sort((a, b) =>
        (a.featureName || '').localeCompare(b.featureName || ''),
      );
      // Store initial states
      this.initialFeatureStates = new Map(
        this.features.map(f => [f.featureName || '', f.isEnabled || false]),
      );
      this.buildForm();
    });
  }

  private buildForm() {
    const featureControls = this.features.map(feature =>
      this.fb.group({
        featureName: [feature.featureName],
        isEnabled: [feature.isEnabled],
        isAvailable: [(feature as any).isAvailable ?? true],
      }),
    );

    this.form = this.fb.group({
      features: this.fb.array(featureControls),
    });
  }

  get featuresFormArray(): FormArray {
    return this.form.get('features') as FormArray;
  }

  onVisibleChange(visible: boolean, refresh = false) {
    this.visibleChange.emit({ visible, refresh });
  }

  save() {
    if (!this.form.valid || !this.blogId()) {
      return;
    }

    const featuresArray = this.form.get('features') as FormArray;

    // Only save features that have changed
    const changedFeatures: BlogFeatureInputDto[] = featuresArray.controls
      .map(control => {
        const featureName = control.get('featureName')?.value;
        const isEnabled = control.get('isEnabled')?.value;
        const initialIsEnabled = this.initialFeatureStates.get(featureName);

        // Only include if the value has changed
        if (featureName && initialIsEnabled !== isEnabled) {
          return {
            featureName,
            isEnabled,
          };
        }
        return null;
      })
      .filter((input): input is BlogFeatureInputDto => input !== null);

    // If no features changed, just close the modal
    if (changedFeatures.length === 0) {
      this.onVisibleChange(false, false);
      return;
    }

    // Save only changed features
    const saveObservables = changedFeatures.map(input =>
      this.blogFeatureService.set(this.blogId()!, input),
    );

    // Use forkJoin to save all changed features at once
    forkJoin(saveObservables).subscribe({
      next: () => {
        this.onVisibleChange(false, true);
        this.toasterService.success('AbpUi::SavedSuccessfully');
      },
    });
  }
}
