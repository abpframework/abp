import { Directive, ElementRef, effect, inject, input } from '@angular/core';

@Directive({
  selector: '[abpShowPassword]',
})
export class ShowPasswordDirective {
  protected readonly elementRef = inject(ElementRef);

  readonly abpShowPassword = input(false);

  constructor() {
    effect(() => {
      const visible = this.abpShowPassword();
      const element = this.elementRef.nativeElement as HTMLInputElement;
      if (!element) return;

      element.type = visible ? 'text' : 'password';
    });
  }
}
