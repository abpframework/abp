import {
  ComponentFactoryResolver,
  Directive,
  Injector,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
  TemplateRef,
  Type,
  ViewContainerRef,
} from '@angular/core';
import compare from 'just-compare';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import snq from 'snq';
import { ABP } from '../models/common';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';
import { SubscriptionService } from '../services/subscription.service';

@Directive({
  selector: '[abpReplaceableTemplate]',
  providers: [SubscriptionService],
})
export class ReplaceableTemplateDirective implements OnInit, OnChanges {
  @Input('abpReplaceableTemplate')
  data: ReplaceableComponents.ReplaceableTemplateDirectiveInput<any, any>;

  providedData = {
    inputs: {},
    outputs: {},
  } as ReplaceableComponents.ReplaceableTemplateData<any, any>;

  context = {} as any;

  externalComponent: Type<any>;

  defaultComponentRef: any;

  defaultComponentSubscriptions = {} as ABP.Dictionary<Subscription>;

  initialized = false;

  constructor(
    private injector: Injector,
    private templateRef: TemplateRef<any>,
    private cfRes: ComponentFactoryResolver,
    private vcRef: ViewContainerRef,
    private replaceableComponents: ReplaceableComponentsService,
    private subscription: SubscriptionService,
  ) {
    this.context = {
      initTemplate: ref => {
        this.resetDefaultComponent();
        this.defaultComponentRef = ref;
        this.setDefaultComponentInputs();
      },
    };
  }

  ngOnInit() {
    const component$ = this.replaceableComponents
      .get$(this.data.componentKey)
      .pipe(
        filter(
          (res = {} as ReplaceableComponents.ReplaceableComponent) =>
            !this.initialized || !compare(res.component, this.externalComponent),
        ),
      );

    this.subscription.addOne(
      component$,
      (res = {} as ReplaceableComponents.ReplaceableComponent) => {
        this.vcRef.clear();
        this.externalComponent = res.component;
        if (this.defaultComponentRef) {
          this.resetDefaultComponent();
        }

        if (res.component) {
          this.setProvidedData();
          const customInjector = Injector.create({
            providers: [{ provide: 'REPLACEABLE_DATA', useValue: this.providedData }],
            parent: this.injector,
          });
          this.vcRef.createComponent(
            this.cfRes.resolveComponentFactory(res.component),
            0,
            customInjector,
          );
        } else {
          this.vcRef.createEmbeddedView(this.templateRef, this.context);
        }

        this.initialized = true;
      },
    );
  }

  ngOnChanges(changes: SimpleChanges) {
    if (snq(() => changes.data.currentValue.inputs) && this.defaultComponentRef) {
      this.setDefaultComponentInputs();
    }
  }

  setDefaultComponentInputs() {
    if (!this.defaultComponentRef || (!this.data.inputs && !this.data.outputs)) return;

    if (this.data.inputs) {
      for (const key in this.data.inputs) {
        if (Object.prototype.hasOwnProperty.call(this.data.inputs, key)) {
          if (!compare(this.defaultComponentRef[key], this.data.inputs[key].value)) {
            this.defaultComponentRef[key] = this.data.inputs[key].value;
          }
        }
      }
    }

    if (this.data.outputs) {
      for (const key in this.data.outputs) {
        if (Object.prototype.hasOwnProperty.call(this.data.outputs, key)) {
          if (!this.defaultComponentSubscriptions[key]) {
            this.defaultComponentSubscriptions[key] = this.defaultComponentRef[key].subscribe(
              value => {
                this.data.outputs[key](value);
              },
            );
          }
        }
      }
    }
  }

  setProvidedData() {
    this.providedData = { ...this.data, inputs: {} };

    if (!this.data.inputs) return;
    Object.defineProperties(this.providedData.inputs, {
      ...Object.keys(this.data.inputs).reduce(
        (acc, key) => ({
          ...acc,
          [key]: {
            enumerable: true,
            configurable: true,
            get: () => this.data.inputs[key].value,
            ...(this.data.inputs[key].twoWay && {
              set: newValue => {
                this.data.inputs[key].value = newValue;
                this.data.outputs[`${key}Change`](newValue);
              },
            }),
          },
        }),
        {},
      ),
    });
  }

  resetDefaultComponent() {
    Object.keys(this.defaultComponentSubscriptions).forEach(key => {
      this.defaultComponentSubscriptions[key].unsubscribe();
    });
    this.defaultComponentSubscriptions = {} as ABP.Dictionary<Subscription>;
    this.defaultComponentRef = null;
  }
}
