import { ConfigStateService, TrackByService } from '@abp/ng.core';
import { LocaleDirection } from '@abp/ng.theme.shared';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { finalize } from 'rxjs/operators';
import { FeatureManagement } from '../../models/feature-management';
import { FeaturesService } from '../../proxy/feature-management/features.service';
import {
  FeatureDto,
  FeatureGroupDto,
  UpdateFeatureDto,
} from '../../proxy/feature-management/models';

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
    FeatureManagement.FeatureManagementComponentOutputs
{
  @Input()
  providerKey: string;

  @Input()
  providerName: string;

  selectedGroupDisplayName: string;

  groups: Pick<FeatureGroupDto, 'name' | 'displayName'>[] = [];

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

  constructor(
    public readonly track: TrackByService,
    protected service: FeaturesService,
    protected store: Store,
    protected configState: ConfigStateService,
  ) {}

  openModal() {
    if (!this.providerName) {
      throw new Error('providerName is required.');
    }

    this.getFeatures();
  }

  getFeatures() {
    this.service.get(this.providerName, this.providerKey).subscribe(res => {
      if (!res.groups?.length) return;
      this.groups = res.groups.map(({ name, displayName }) => ({ name, displayName }));
      this.selectedGroupDisplayName = this.groups[0].displayName;
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
          changedFeatures.push({ name: feature.name, value: `${feature.value}` });
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

        if (!this.providerKey) {
          // to refresh host's features
          this.configState.refreshAppState().subscribe();
        }
      });
  }

  onCheckboxClick(val: boolean, feature: FeatureDto) {
    if (val) {
      this.checkToggleAncestors(feature);
    } else {
      this.uncheckToggleDescendants(feature);
    }
  }

  private uncheckToggleDescendants(feature: FeatureDto) {
    this.findAllDescendantsOfByType(feature, ValueTypes.ToggleStringValueType).forEach(node =>
      this.setFeatureValue(node, false),
    );
  }

  private checkToggleAncestors(feature: FeatureDto) {
    this.findAllAncestorsOfByType(feature, ValueTypes.ToggleStringValueType).forEach(node =>
      this.setFeatureValue(node, true),
    );
  }

  private findAllAncestorsOfByType(feature: FeatureDto, type: ValueTypes) {
    let parent = this.findParentByType(feature, type);
    const ancestors = [];
    while (parent) {
      ancestors.push(parent);
      parent = this.findParentByType(parent, type);
    }
    return ancestors;
  }

  private findAllDescendantsOfByType(feature: FeatureDto, type: ValueTypes) {
    const descendants = [];
    const queue = [feature];

    while (queue.length) {
      const node = queue.pop();
      const newDescendants = this.findChildrenByType(node, type);
      descendants.push(...newDescendants);
      queue.push(...newDescendants);
    }

    return descendants;
  }

  private findParentByType(feature: FeatureDto, type: ValueTypes) {
    return this.getCurrentGroup().find(
      f => f.valueType.name === type && f.name === feature.parentName,
    );
  }

  private findChildrenByType(feature: FeatureDto, type: ValueTypes) {
    return this.getCurrentGroup().filter(
      f => f.valueType.name === type && f.parentName === feature.name,
    );
  }

  private getCurrentGroup() {
    return this.features[this.selectedGroupDisplayName] ?? [];
  }

  private setFeatureValue(feature: FeatureDto, val: boolean) {
    feature.value = val as any;
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
