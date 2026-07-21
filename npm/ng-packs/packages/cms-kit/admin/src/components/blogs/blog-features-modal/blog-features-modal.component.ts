import {
  ChangeDetectionStrategy,
  Component,
  inject,
  input,
  OnInit,
  output,
  signal,
} from '@angular/core';
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
  changeDetection: ChangeDetectionStrategy.OnPush,
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

  readonly form = signal<FormGroup | undefined>(undefined);
  readonly features = signal<BlogFeatureDto[]>([]);
  private initialFeatureStates: Map<string, boolean> = new Map();

  ngOnInit() {
    if (this.blogId()) {
      this.loadFeatures();
    }
  }

  private loadFeatures() {
    this.blogFeatureService.getList(this.blogId()!).subscribe(features => {
      const sorted = features.sort((a, b) =>
        (a.featureName || '').localeCompare(b.featureName || ''),
      );
      this.features.set(sorted);
      this.initialFeatureStates = new Map(
        sorted.map(f => [f.featureName || '', f.isEnabled || false]),
      );
      this.buildForm(sorted);
    });
  }

  private buildForm(features: BlogFeatureDto[]) {
    const featureControls = features.map(feature =>
      this.fb.group({
        featureName: [feature.featureName],
        isEnabled: [feature.isEnabled],
        isAvailable: [(feature as any).isAvailable ?? true],
      }),
    );

    this.form.set(
      this.fb.group({
        features: this.fb.array(featureControls),
      }),
    );
  }

  get featuresFormArray(): FormArray {
    return this.form()!.get('features') as FormArray;
  }

  onVisibleChange(visible: boolean, refresh = false) {
    this.visibleChange.emit({ visible, refresh });
  }

  save() {
    const currentForm = this.form();
    if (!currentForm?.valid || !this.blogId()) {
      return;
    }

    const featuresArray = currentForm.get('features') as FormArray;

    const changedFeatures: BlogFeatureInputDto[] = featuresArray.controls
      .map(control => {
        const featureName = control.get('featureName')?.value;
        const isEnabled = control.get('isEnabled')?.value;
        const initialIsEnabled = this.initialFeatureStates.get(featureName);

        if (featureName && initialIsEnabled !== isEnabled) {
          return {
            featureName,
            isEnabled,
          };
        }
        return null;
      })
      .filter((input): input is BlogFeatureInputDto => input !== null);

    if (changedFeatures.length === 0) {
      this.onVisibleChange(false, false);
      return;
    }

    const saveObservables = changedFeatures.map(input =>
      this.blogFeatureService.set(this.blogId()!, input),
    );

    forkJoin(saveObservables).subscribe(() => {
      this.onVisibleChange(false, true);
      this.toasterService.success('AbpUi::SavedSuccessfully');
    });
  }
}
