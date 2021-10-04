/* eslint-disable @angular-eslint/no-input-rename */
import {
  Directive,
  Injector,
  Input,
  OnChanges,
  OnDestroy,
  TemplateRef,
  ViewContainerRef,
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
  @Input('abpPropDataFromList') readonly propList: L;

  @Input('abpPropDataWithRecord') readonly record: InferredData<L>['record'];

  @Input('abpPropDataAtIndex') readonly index: number;

  readonly getInjected: InferredData<L>['getInjected'];

  constructor(
    private tempRef: TemplateRef<any>,
    private vcRef: ViewContainerRef,
    injector: Injector,
  ) {
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
