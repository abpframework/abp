import {
  Component,
  EventEmitter,
  Inject,
  Input,
  OnDestroy,
  OnInit,
  Output,
  TemplateRef,
  Type,
  Attribute,
  Renderer2,
} from '@angular/core';
import { Store } from '@ngxs/store';
import { distinctUntilChanged, filter } from 'rxjs/operators';
import { ABP } from '../models/common';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ReplaceableComponentsState } from '../states/replaceable-components.state';
import { takeUntilDestroy } from '../utils/rxjs-utils';
import compare from 'just-compare';
import snq from 'snq';

@Component({
  selector: 'abp-replaceable-template-container',
  template: `
    <ng-container *ngTemplateOutlet="externalComponent ? external : defaultTemplate"></ng-container>
    <ng-template #external
      ><ng-container *ngComponentOutlet="externalComponent"></ng-container
    ></ng-template>
  `,
  exportAs: 'abpReplaceableTemplateContainer',
  providers: [{ provide: 'INPUTS', useValue: {} }],
})
export class ReplaceableTemplateContainerComponent implements OnInit, OnDestroy {
  @Input()
  defaultTemplate: TemplateRef<any>;

  @Input()
  inputs: ReplaceableComponents.ReplaceableTemplateInput[] = [];

  @Output()
  readonly writableInputsChange = new EventEmitter<ABP.Dictionary>();

  @Output()
  readonly externalComponentInit = new EventEmitter<void>();

  @Output()
  readonly defaultTemplateInit = new EventEmitter<void>();

  externalComponent: Type<any> = null; // externalComponent must equal to null

  initDefaultTemplate = (ref: any) => {
    console.warn(ref);
  };

  constructor(
    @Attribute('componentKey') private componentKey: string,
    @Inject('INPUTS') public inputsProvider: ABP.Dictionary,
    private store: Store,
    private renderer: Renderer2,
  ) {}

  ngOnInit() {
    this.store
      .select(ReplaceableComponentsState.getComponent(this.componentKey))
      .pipe(
        takeUntilDestroy(this),
        filter(
          (res = {} as ReplaceableComponents.ReplaceableComponent) =>
            !compare(this.externalComponent, res.component),
        ),
      )
      .subscribe(({ component } = {} as ReplaceableComponents.ReplaceableComponent) => {
        console.warn('STOREDAN DATA GELDI!!');
        this.externalComponent = component;
        if (component) {
          this.externalComponentInit.emit();
        } else {
          this.defaultTemplateInit.emit();
        }
      });

    this.defineProperties();
  }

  defineProperties() {
    const writableInputs = this.inputs
      .filter(input => input.writable)
      .reduce((acc, val) => ({ ...acc, [val.key]: val.value }), {});

    this.inputs.forEach((input, index) => {
      Object.defineProperty(this.inputsProvider, this.inputs[index].key, {
        get: () => this.inputs[index].value,
        ...(this.inputs[index].writable && {
          set: newValue => {
            writableInputs[input.key] = newValue;
            this.writableInputsChange.emit(writableInputs);
            this.inputs[index] = { ...this.inputs[index], value: newValue };
          },
        }),
        enumerable: true,
        configurable: true,
      });
    });
  }

  ngOnDestroy() {}
}
