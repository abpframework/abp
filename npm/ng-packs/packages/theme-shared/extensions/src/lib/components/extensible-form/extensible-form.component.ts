import { TrackByService } from '@abp/ng.core';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Inject,
  Input,
  Optional,
  QueryList,
  SkipSelf,
  ViewChildren,
} from '@angular/core';
import { ControlContainer, UntypedFormGroup } from '@angular/forms';
import { EXTRA_PROPERTIES_KEY } from '../../constants/extra-properties';
import { FormPropList, GroupedFormPropList } from '../../models/form-props';
import { ExtensionsService } from '../../services/extensions.service';
import { EXTENSIONS_IDENTIFIER } from '../../tokens/extensions.token';
import { selfFactory } from '../../utils/factory.util';
import { ExtensibleFormPropComponent } from './extensible-form-prop.component';

@Component({
  exportAs: 'abpExtensibleForm',
  selector: 'abp-extensible-form',
  templateUrl: './extensible-form.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: selfFactory,
      deps: [[new Optional(), new SkipSelf(), ControlContainer]],
    },
  ],
})
export class ExtensibleFormComponent<R = any> {
  @ViewChildren(ExtensibleFormPropComponent)
  formProps!: QueryList<ExtensibleFormPropComponent>;

  @Input()
  set selectedRecord(record: R) {
    const type = !record || JSON.stringify(record) === '{}' ? 'create' : 'edit';
    const propList = this.extensions[`${type}FormProps`].get(this.identifier).props;
    this.groupedPropList = this.createGroupedList(propList);
    this.record = record;
  }

  extraPropertiesKey = EXTRA_PROPERTIES_KEY;
  groupedPropList: GroupedFormPropList;
  record!: R;

  createGroupedList(propList: FormPropList<R>) {
    const groupedFormPropList = new GroupedFormPropList();
    propList.forEach(item => {
      groupedFormPropList.addItem(item.value);
    });
    return groupedFormPropList;
  }

  get form(): UntypedFormGroup {
    return (this.container ? this.container.control : { controls: {} }) as UntypedFormGroup;
  }

  get extraProperties(): UntypedFormGroup {
    return (this.form.controls.extraProperties || { controls: {} }) as UntypedFormGroup;
  }

  constructor(
    public readonly cdRef: ChangeDetectorRef,
    public readonly track: TrackByService,
    private container: ControlContainer,
    private extensions: ExtensionsService,
    @Inject(EXTENSIONS_IDENTIFIER) private identifier: string,
  ) {}
}
