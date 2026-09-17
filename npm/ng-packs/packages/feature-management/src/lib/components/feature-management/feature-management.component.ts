import {Component, inject, DOCUMENT, input, output, signal, effect, ChangeDetectionStrategy,} from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ConfigStateService, LocalizationPipe, TrackByService } from '@abp/ng.core';
import {
  FeatureDto,
  FeatureGroupDto,
  FeaturesService,
  UpdateFeatureDto,
} from '@abp/ng.feature-management/proxy';
import {
  ButtonComponent,
  Confirmation,
  ConfirmationService,
  LocaleDirection,
  ModalCloseDirective,
  ModalComponent,
  ToasterService,
} from '@abp/ng.theme.shared';
import { Tabs, TabList, Tab, TabPanel, TabContent } from '@angular/aria/tabs';
import { finalize } from 'rxjs/operators';
import { FreeTextInputDirective } from '../../directives';

enum ValueTypes {
  ToggleStringValueType = 'ToggleStringValueType',
  FreeTextStringValueType = 'FreeTextStringValueType',
  SelectionStringValueType = 'SelectionStringValueType',
}

const DEFAULT_PROVIDER_NAME = 'D';

/**
 * FeatureDto.ValueType is typed as IStringValueType in Application.Contracts, but the API
 * serializes concrete value types at runtime (e.g. SelectionStringValueType includes itemSource).
 * generate-proxy only reflects the interface contract, so keep runtime-only shapes here — not in proxy/.
 * See: modules/feature-management (FeatureDto, StringValueTypeJsonConverter) and Blazor/MVC cast pattern.
 */
type FeatureWithStyle = FeatureDto & {
  style?: Record<string, number>;
  initialValue: unknown;
};

type SelectionStringValueType = FeatureDto['valueType'] & {
  itemSource?: {
    items?: Array<{
      value?: string;
      displayText?: { resourceName?: string; name?: string };
    }>;
  };
};

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-feature-management',
  templateUrl: './feature-management.component.html',
  exportAs: 'abpFeatureManagement',
  imports: [
    NgTemplateOutlet,
    ButtonComponent,
    ModalComponent,
    LocalizationPipe,
    FormsModule,
    Tabs,
    TabList,
    Tab,
    TabPanel,
    TabContent,
    FreeTextInputDirective,
    ModalCloseDirective,
  ],
})
export class FeatureManagementComponent {
  protected readonly track = inject(TrackByService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly service = inject(FeaturesService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly confirmationService = inject(ConfirmationService);
  private document = inject(DOCUMENT);

  // Signal inputs
  readonly providerKey = input<string | undefined>(undefined);
  readonly providerName = input<string | undefined>(undefined);
  readonly providerTitle = input<string | undefined>(undefined);
  readonly visibleInput = input(false, { alias: 'visible' });

  // Output signals
  readonly visibleChange = output<boolean>();

  // Internal state
  protected readonly _visible = signal(false);

  protected readonly selectedGroupDisplayName = signal<string | undefined>(undefined);

  protected readonly groups = signal<Pick<FeatureGroupDto, 'name' | 'displayName'>[]>([]);

  protected readonly features = signal<{
    [group: string]: FeatureWithStyle[];
  }>({});

  valueTypes = ValueTypes;

  defaultProviderName = DEFAULT_PROVIDER_NAME;

  protected readonly modalBusy = signal(false);

  // Getter/setter for backward compatibility
  get visible(): boolean {
    return this._visible();
  }

  set visible(value: boolean) {
    if (this._visible() === value) {
      return;
    }

    this._visible.set(value);
    this.visibleChange.emit(value);

    if (value) {
      this.openModal();
    }
  }

  constructor() {
    // Sync visible input to internal signal
    effect(() => {
      const inputValue = this.visibleInput();
      if (this._visible() !== inputValue) {
        this._visible.set(inputValue);
        if (inputValue) {
          this.openModal();
        }
      }
    });
  }

  openModal() {
    if (!this.providerName()) {
      throw new Error('providerName is required.');
    }

    this.getFeatures();
  }

  getFeatures() {
    this.service.get(this.providerName()!, this.providerKey()).subscribe(res => {
      if (!res.groups?.length) return;
      const groups = res.groups.map(({ name, displayName }) => ({ name, displayName }));
      this.groups.set(groups);
      this.selectedGroupDisplayName.set(groups[0].displayName);
      this.features.set(
        res.groups.reduce(
          (acc, val) => ({
            ...acc,
            [val.name]: mapFeatures(val.features, this.document.body?.dir as LocaleDirection),
          }),
          {},
        ),
      );
    });
  }

  save() {
    if (this.modalBusy()) return;

    const changedFeatures = [] as UpdateFeatureDto[];

    Object.keys(this.features()).forEach(key => {
      this.features()[key].forEach(feature => {
        if (feature.value !== feature.initialValue)
          changedFeatures.push({ name: feature.name, value: `${feature.value}` });
      });
    });

    if (!changedFeatures.length) {
      this.visible = false;
      return;
    }

    this.modalBusy.set(true);
    this.service
      .update(this.providerName()!, this.providerKey(), { features: changedFeatures })
      .pipe(finalize(() => this.modalBusy.set(false)))
      .subscribe(() => {
        this.visible = false;

        this.toasterService.success('AbpUi::SavedSuccessfully');
        if (!this.providerKey()) {
          // to refresh host's features
          this.configState.refreshAppState().subscribe();
        }
      });
  }

  resetToDefault() {
    this.confirmationService
      .warn('AbpFeatureManagement::AreYouSureToResetToDefault', 'AbpFeatureManagement::AreYouSure')
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.service.delete(this.providerName()!, this.providerKey()).subscribe(() => {
            this.toasterService.success('AbpFeatureManagement::ResetedToDefault');
            this.visible = false;

            if (!this.providerKey()) {
              // to refresh host's features
              this.configState.refreshAppState().subscribe();
            }
          });
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

  getSelectionItems(feature: FeatureWithStyle) {
    if (feature.valueType?.name !== ValueTypes.SelectionStringValueType) {
      return [];
    }

    return (feature.valueType as SelectionStringValueType).itemSource?.items ?? [];
  }

  isParentDisabled(parentName: string, groupName: string, provider: string): boolean {
    const children = this.features()[groupName]?.filter(f => f.parentName === parentName);
    const providerNameValue = this.providerName();

    if (children?.length) {
      return children.some(child => {
        const childProvider = child.provider?.name;
        return (
          (childProvider !== providerNameValue && childProvider !== this.defaultProviderName) ||
          (provider !== providerNameValue && provider !== this.defaultProviderName)
        );
      });
    } else {
      return provider !== providerNameValue && provider !== this.defaultProviderName;
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
    const selectedGroup = this.selectedGroupDisplayName();
    return selectedGroup ? this.features()[selectedGroup] ?? [] : [];
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
