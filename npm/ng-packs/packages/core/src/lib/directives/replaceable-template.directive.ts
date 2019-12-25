import {
  ComponentFactoryResolver,
  Directive,
  Injector,
  Input,
  OnDestroy,
  OnInit,
  TemplateRef,
  Type,
  ViewContainerRef,
  SimpleChanges,
} from '@angular/core';
import { Store } from '@ngxs/store';
import compare from 'just-compare';
import { filter } from 'rxjs/operators';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ReplaceableComponentsState } from '../states/replaceable-components.state';
import { takeUntilDestroy } from '../utils/rxjs-utils';

@Directive({ selector: '[abpReplaceableTemplate]' })
export class ReplaceableTemplateDirective implements OnInit, OnDestroy {
  private context = {};

  @Input('abpReplaceableTemplate')
  data: { inputs: any; outputs: any; componentKey: string };

  providedData = { inputs: {}, outputs: {} } as { inputs: any; outputs: any; componentKey: string };

  externalComponent: Type<any> = null; // externalComponent must equal to null

  defaultComponentRef: any;

  constructor(
    private injector: Injector,
    private templateRef: TemplateRef<any>,
    private cfRes: ComponentFactoryResolver,
    private vcRef: ViewContainerRef,
    private store: Store,
  ) {
    this.context = {
      initTemplate: ref => {
        this.defaultComponentRef = ref;
        this.setDefaultComponentInputs();
        setTimeout(() => {
          ref.providerKey = 'admin';
          ref.visible = true;
        }, 5000);
      },
    };
  }

  ngOnInit() {
    this.store
      .select(ReplaceableComponentsState.getComponent(this.data.componentKey))
      .pipe(
        filter(
          (res = {} as ReplaceableComponents.ReplaceableComponent) =>
            !compare(res.component, this.externalComponent),
        ),
        takeUntilDestroy(this),
      )
      .subscribe((res = {} as ReplaceableComponents.ReplaceableComponent) => {
        this.vcRef.clear();
        this.externalComponent = res.component;

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
      });
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log(changes);
  }

  ngOnDestroy() {}

  setDefaultComponentInputs() {
    if (!this.defaultComponentRef) return;
  }

  setProvidedData() {
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

    this.providedData.outputs = this.data.outputs;

    console.warn(this.providedData);
  }
}
