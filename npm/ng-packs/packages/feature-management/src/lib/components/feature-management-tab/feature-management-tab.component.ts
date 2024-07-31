import { LocalizationModule, ReplaceableTemplateDirective } from '@abp/ng.core';
import { Component } from '@angular/core';
import { FeatureManagementComponent } from '../feature-management/feature-management.component';

@Component({
  standalone: true,
  selector: 'abp-feature-management-tab',
  templateUrl: './feature-management-tab.component.html',
  imports: [
    ReplaceableTemplateDirective,
    LocalizationModule,
    FeatureManagementComponent,
  ],
})
export class FeatureManagementTabComponent {
  visibleFeatures = false;
  providerKey: string;

  openFeaturesModal() {
    this.visibleFeatures = true;
  }

  onVisibleFeaturesChange = (value: boolean) => {
    this.visibleFeatures = value;
  };
}
