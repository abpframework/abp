import { TrackByService } from '@abp/ng.core';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Inject,
  Input,
  OnDestroy,
  Optional,
  QueryList,
  SkipSelf,
  ViewChildren,
} from '@angular/core';
import { ControlContainer, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
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
export class ExtensibleFormComponent<R = any> implements AfterViewInit, OnDestroy {
  @ViewChildren(ExtensibleFormPropComponent)
  formProps: QueryList<ExtensibleFormPropComponent>;

  @Input()
  set selectedRecord(record: R) {
    const type = !record || JSON.stringify(record) === '{}' ? 'create' : 'edit';
    this.propList = this.extensions[`${type}FormProps`].get(this.identifier).props;
    this.record = record;
  }

  private subscription = new Subscription();
  extraPropertiesKey = EXTRA_PROPERTIES_KEY;
  propList: FormPropList<R>;
  record: R;

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

  ngAfterViewInit() {
    this.subscription.add(
      this.form.statusChanges.pipe(debounceTime(0)).subscribe(() => {
        this.formProps.forEach(prop => prop.cdRef.markForCheck());
        this.cdRef.detectChanges();
      }),
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
