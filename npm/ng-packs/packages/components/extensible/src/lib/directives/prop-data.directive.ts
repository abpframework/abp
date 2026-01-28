/* eslint-disable @angular-eslint/no-input-rename */
import {
  Directive,
  Injector,
  OnChanges,
  OnDestroy,
  TemplateRef,
  ViewContainerRef,
  inject,
  input
} from '@angular/core';
import { PropData, PropList } from '../models/props';

@Directive({
  exportAs: 'abpPropData',
  selector: '[abpPropData]',
})
export class PropDataDirective<L extends PropList<any>>
  extends PropData<InferredData<L>>
  implements OnChanges, OnDestroy
{
  private tempRef = inject<TemplateRef<any>>(TemplateRef);
  private vcRef = inject(ViewContainerRef);

  readonly propList = input<L>(undefined, { alias: "abpPropDataFromList" });

  readonly record = input.required<InferredData<L>['record']>({ alias: "abpPropDataWithRecord" });

  readonly index = input<number>(undefined, { alias: "abpPropDataAtIndex" });

  readonly getInjected: InferredData<L>['getInjected'];

  constructor() {
    const injector = inject(Injector);

    super();

    this.getInjected = injector.get.bind(injector);
  }

  ngOnChanges() {
    this.vcRef.clear();

    this.vcRef.createEmbeddedView(this.tempRef, {
      $implicit: this.data,
      index: 0,
    });
  }

  ngOnDestroy() {
    this.vcRef.clear();
  }
}

type InferredData<L> = PropData<InferredRecord<L>>;
type InferredRecord<L> = L extends PropList<infer R> ? R : never;
