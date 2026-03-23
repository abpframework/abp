/* eslint-disable @angular-eslint/no-input-rename */
import {
  Directive,
  Injector,
  OnDestroy,
  TemplateRef,
  ViewContainerRef,
  effect,
  inject,
  input
} from '@angular/core';
import { PropData, PropList } from '../models/props';

type InferredRecord<L> = L extends PropList<infer R> ? R : never;

@Directive({
  exportAs: 'abpPropData',
  selector: '[abpPropData]',
})
export class PropDataDirective<L extends PropList<any>>
  extends PropData<InferredRecord<L>>
  implements OnDestroy
{
  private tempRef = inject<TemplateRef<any>>(TemplateRef);
  private vcRef = inject(ViewContainerRef);

  readonly propList = input<L | undefined>(undefined, { alias: 'abpPropDataFromList' });
  readonly record = input.required<InferredRecord<L>>({ alias: 'abpPropDataWithRecord' });
  readonly index = input<number | undefined>(undefined, { alias: 'abpPropDataAtIndex' });

  readonly getInjected: PropData<InferredRecord<L>>['getInjected'];

  constructor() {
    const injector = inject(Injector);

    super();

    this.getInjected = injector.get.bind(injector);

    // Watch for input changes and re-render
    effect(() => {
      // Read all inputs to track them
      this.record();
      this.index();
      this.propList();
      
      this.vcRef.clear();
      this.vcRef.createEmbeddedView(this.tempRef, {
        $implicit: this.data,
        index: 0,
      });
    });
  }

  ngOnDestroy() {
    this.vcRef.clear();
  }
}
