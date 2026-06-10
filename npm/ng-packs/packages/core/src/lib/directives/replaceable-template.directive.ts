import {
  Directive,
  effect,
  Injector,
  OnChanges,
  OnInit,
  SimpleChanges,
  TemplateRef,
  Type,
  ViewContainerRef,
  inject,
  input
} from '@angular/core';
import compare from 'just-compare';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { ABP } from '../models/common';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';
import { SubscriptionService } from '../services/subscription.service';

@Directive({
  selector: '[abpReplaceableTemplate]',
  providers: [SubscriptionService],
})
export class ReplaceableTemplateDirective implements OnInit, OnChanges {
  private injector = inject(Injector);
  private templateRef = inject<TemplateRef<any>>(TemplateRef);
  private vcRef = inject(ViewContainerRef);
  private replaceableComponents = inject(ReplaceableComponentsService);
  private subscription = inject(SubscriptionService);

  readonly data = input.required<ReplaceableComponents.ReplaceableTemplateDirectiveInput<any, any>>({ alias: "abpReplaceableTemplate" });

  providedData = {
    inputs: {},
    outputs: {},
  } as ReplaceableComponents.ReplaceableTemplateData<any, any>;

  context = {} as any;

  externalComponent!: Type<any>;

  defaultComponentRef: any;

  defaultComponentSubscriptions = {} as ABP.Dictionary<Subscription>;

  initialized = false;

  constructor() {
    this.context = {
      initTemplate: (ref: any) => {
        this.resetDefaultComponent();
        this.defaultComponentRef = ref;
        this.setDefaultComponentInputs();
      },
    };

    effect(() => {
      const data = this.data();
      if (data?.inputs && this.defaultComponentRef) {
        this.setDefaultComponentInputs();
      }
    });
  }

  ngOnInit() {
    const component$ = this.replaceableComponents
      .get$(this.data().componentKey)
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
          const ref = this.vcRef.createComponent(res.component, {
            index: 0,
            injector: customInjector,
          });
        } else {
          this.vcRef.createEmbeddedView(this.templateRef, this.context);
        }

        this.initialized = true;
      },
    );
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes?.data?.currentValue?.inputs && this.defaultComponentRef) {
      this.setDefaultComponentInputs();
    }
  }

  setDefaultComponentInputs() {
    const data = this.data();
    if (!this.defaultComponentRef || (!data.inputs && !data.outputs)) return;

    if (data.inputs) {
      for (const key in data.inputs) {
        if (Object.prototype.hasOwnProperty.call(data.inputs, key)) {
          if (!compare(this.defaultComponentRef[key], data.inputs[key].value)) {
            this.defaultComponentRef[key] = data.inputs[key].value;
          }
        }
      }
    }

    if (data.outputs) {
      for (const key in data.outputs) {
        if (Object.prototype.hasOwnProperty.call(data.outputs, key)) {
          if (!this.defaultComponentSubscriptions[key]) {
            this.defaultComponentSubscriptions[key] = this.defaultComponentRef[key].subscribe(
              (value: any) => {
                this.data().outputs?.[key](value);
              },
            );
          }
        }
      }
    }
  }

  setProvidedData() {
    this.providedData = { outputs: {}, ...this.data(), inputs: {} };

    const data = this.data();
    if (!data.inputs) return;
    Object.defineProperties(this.providedData.inputs, {
      ...Object.keys(data.inputs).reduce(
        (acc, key) => ({
          ...acc,
          [key]: {
            enumerable: true,
            configurable: true,
            get: () => this.data().inputs?.[key]?.value,
            ...(this.data().inputs?.[key]?.twoWay && {
              set: (newValue: any) => {
                const dataValue = this.data();
                if (dataValue.inputs?.[key]) {
                  dataValue.inputs[key].value = newValue;
                }
                if (dataValue.outputs?.[`${key}Change`]) {
                  dataValue.outputs[`${key}Change`](newValue);
                }
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
