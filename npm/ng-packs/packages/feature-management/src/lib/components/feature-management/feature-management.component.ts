import { TrackByService } from '@abp/ng.core';
import { LocaleDirection } from '@abp/ng.theme.shared';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { FeatureManagement } from '../../models/feature-management';
import { FeatureDto, FeaturesService, UpdateFeatureDto } from '../../proxy/feature-management';

enum ValueTypes {
  ToggleStringValueType = 'ToggleStringValueType',
  FreeTextStringValueType = 'FreeTextStringValueType',
  SelectionStringValueType = 'SelectionStringValueType',
}

@Component({
  selector: 'abp-feature-management',
  templateUrl: './feature-management.component.html',
  exportAs: 'abpFeatureManagement',
})
export class FeatureManagementComponent
  implements
    FeatureManagement.FeatureManagementComponentInputs,
    FeatureManagement.FeatureManagementComponentOutputs {
  @Input()
  providerKey: string;

  @Input()
  providerName: string;

  selectedGroup: string;

  groups: string[] = [];

  features: {
    [group: string]: Array<FeatureDto & { style?: { [key: string]: number }; initialValue: any }>;
  };

  valueTypes = ValueTypes;

  protected _visible;

  @Input()
  get visible(): boolean {
    return this._visible;
  }

  set visible(value: boolean) {
    if (this._visible === value) return;

    this._visible = value;
    this.visibleChange.emit(value);
    if (value) this.openModal();
  }

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  modalBusy = false;

  constructor(public readonly track: TrackByService, private service: FeaturesService) {}

  openModal() {
    if (!this.providerName) {
      throw new Error('providerName is required.');
    }

    this.getFeatures();
  }

  getFeatures() {
    this.service.get(this.providerName, this.providerKey).subscribe(res => {
      this.groups = res.groups.map(group => group.displayName);
      this.selectedGroup = this.groups[0];
      this.features = res.groups.reduce(
        (acc, val) => ({
          ...acc,
          [val.name]: mapFeatures(val.features, document.body.dir as LocaleDirection),
        }),
        {},
      );
    });
  }

  save() {
    if (this.modalBusy) return;

    const changedFeatures = [] as UpdateFeatureDto[];

    Object.keys(this.features).forEach(key => {
      this.features[key].forEach(feature => {
        if (feature.value !== feature.initialValue)
          changedFeatures.push({ name: feature.name, value: feature.value });
      });
    });

    if (!changedFeatures.length) {
      this.visible = false;
      return;
    }

    this.modalBusy = true;
    this.service
      .update(this.providerName, this.providerKey, { features: changedFeatures })
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(() => {
        this.visible = false;
      });
  }
}

function mapFeatures(features: FeatureDto[], dir: LocaleDirection) {
  const margin = `margin-${dir === 'rtl' ? 'right' : 'left'}.px`;

  return features.map(feature => {
    const value =
      feature.valueType?.name === ValueTypes.ToggleStringValueType
        ? (feature.value || '').toLowerCase() === 'true'
        : feature.value;

    return {
      ...feature,
      value,
      initialValue: value,
      style: { [margin]: feature.depth * 20 },
    };
  });
}
