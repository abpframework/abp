import { Directive, effect, inject, input, ElementRef, Renderer2 } from '@angular/core';
import type { FeatureDto } from '@abp/ng.feature-management/proxy';

export type FreeTextType = Pick<FeatureDto, 'valueType'>;

export const INPUT_TYPES: Record<string, string> = {
  numeric: 'number',
  default: 'text',
};

@Directive({
  selector: 'input[abpFeatureManagementFreeText]',
  exportAs: 'inputAbpFeatureManagementFreeText',
})
export class FreeTextInputDirective {
  private readonly elRef = inject(ElementRef);
  private readonly renderer = inject(Renderer2);

  readonly feature = input<FreeTextType | undefined>(undefined, {
    alias: 'abpFeatureManagementFreeText',
  });

  constructor() {
    effect(() => {
      const feature = this.feature();
      if (feature) {
        this.setInputType(feature);
      }
    });
  }

  private setInputType(feature: FreeTextType) {
    const validatorType = feature?.valueType?.validator?.name?.toLowerCase();
    const type = INPUT_TYPES[validatorType] ?? INPUT_TYPES['default'];
    this.renderer.setAttribute(this.elRef.nativeElement, 'type', type);
  }
}
