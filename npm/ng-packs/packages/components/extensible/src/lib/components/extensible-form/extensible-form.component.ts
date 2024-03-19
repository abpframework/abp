import { TrackByService } from '@abp/ng.core';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  inject,
  Input,
  Optional,
  QueryList,
  SkipSelf,
  ViewChildren,
} from '@angular/core';
import { ControlContainer, ReactiveFormsModule, UntypedFormGroup } from '@angular/forms';
import { EXTRA_PROPERTIES_KEY } from '../../constants/extra-properties';
import { FormProp, FormPropList, GroupedFormPropList } from '../../models/form-props';
import { ExtensionsService } from '../../services/extensions.service';
import { EXTENSIONS_IDENTIFIER } from '../../tokens/extensions.token';
import { selfFactory } from '../../utils/factory.util';
import { ExtensibleFormPropComponent } from './extensible-form-prop.component';
import { CommonModule } from '@angular/common';
import { PropDataDirective } from '../../directives/prop-data.directive';

@Component({
  standalone: true,
  exportAs: 'abpExtensibleForm',
  selector: 'abp-extensible-form',
  templateUrl: './extensible-form.component.html',
  imports: [CommonModule, PropDataDirective, ReactiveFormsModule, ExtensibleFormPropComponent],
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
  public readonly cdRef = inject(ChangeDetectorRef);
  public readonly track = inject(TrackByService);
  private readonly container = inject(ControlContainer);
  private readonly extensions = inject(ExtensionsService);
  private readonly identifier = inject(EXTENSIONS_IDENTIFIER);

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
  groupedPropList!: GroupedFormPropList;
  groupedPropListOfArray: FormProp<any>[][];
  record!: R;

  get form(): UntypedFormGroup {
    return (this.container ? this.container.control : { controls: {} }) as UntypedFormGroup;
  }

  get extraProperties(): UntypedFormGroup {
    return (this.form.controls.extraProperties || { controls: {} }) as UntypedFormGroup;
  }

  createGroupedList(propList: FormPropList<R>) {
    const groupedFormPropList = new GroupedFormPropList();
    propList.forEach(item => {
      groupedFormPropList.addItem(item.value);
    });

    return groupedFormPropList;
  }

  //TODO: Reactor this method
  isAnyGroupMemberVisible(index: number, data) {
    const { items } = this.groupedPropList;
    const formPropList = items[index].formPropList.toArray();
    return formPropList.some(prop => prop.visible(data));
  }
}
