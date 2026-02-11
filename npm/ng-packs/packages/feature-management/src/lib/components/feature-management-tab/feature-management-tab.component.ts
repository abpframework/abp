import { LocalizationPipe, ReplaceableTemplateDirective } from '@abp/ng.core';
import { Component } from '@angular/core';
import { FeatureManagementComponent } from '../feature-management/feature-management.component';

@Component({
  selector: 'abp-feature-management-tab',
  templateUrl: './feature-management-tab.component.html',
  imports: [ReplaceableTemplateDirective, LocalizationPipe, FeatureManagementComponent],
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
