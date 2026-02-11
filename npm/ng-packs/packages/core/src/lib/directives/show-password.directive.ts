import { Directive, ElementRef, Input, inject } from '@angular/core';

@Directive({
  selector: '[abpShowPassword]',
})
export class ShowPasswordDirective {
  protected readonly elementRef = inject(ElementRef);

  @Input() set abpShowPassword(visible: boolean) {
    const element = this.elementRef.nativeElement as HTMLInputElement;
    if (!element) return;

    element.type = visible ? 'text' : 'password';
  }
}
