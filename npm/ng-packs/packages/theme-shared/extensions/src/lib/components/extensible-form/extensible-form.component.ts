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
import { ControlContainer, FormGroup } from '@angular/forms';
import { EXTRA_PROPERTIES_KEY } from '../../constants/extra-properties';
import { FormPropList } from '../../models/form-props';
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
    this.propList = this.extensions[`${type}FormProps`].get(this.identifier).props;
    this.record = record;
  }

  extraPropertiesKey = EXTRA_PROPERTIES_KEY;
  propList!: FormPropList<R>;
  record!: R;

  get form(): FormGroup {
    return (this.container ? this.container.control : { controls: {} }) as FormGroup;
  }

  get extraProperties(): FormGroup {
    return (this.form.controls.extraProperties || { controls: {} }) as FormGroup;
  }

  constructor(
    public readonly cdRef: ChangeDetectorRef,
    public readonly track: TrackByService,
    private container: ControlContainer,
    private extensions: ExtensionsService,
    @Inject(EXTENSIONS_IDENTIFIER) private identifier: string,
  ) {}
}
