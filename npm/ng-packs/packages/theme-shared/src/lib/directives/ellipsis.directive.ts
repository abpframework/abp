import {
  AfterViewInit,
  ChangeDetectorRef,
  Directive,
  ElementRef,
  HostBinding,
  inject,
  input
} from '@angular/core';

@Directive({
  selector: '[abpEllipsis]',
})
export class EllipsisDirective implements AfterViewInit {
  private cdRef = inject(ChangeDetectorRef);
  private elRef = inject(ElementRef);

  readonly width = input<string>(undefined, { alias: "abpEllipsis" });

  @HostBinding('title')
  readonly title = input<string>(undefined);

  readonly enabled = input(true, { alias: "abpEllipsisEnabled" });

  @HostBinding('class.abp-ellipsis-inline')
  get inlineClass() {
    return this.enabled() && this.width();
  }

  @HostBinding('class.abp-ellipsis')
  get class() {
    return this.enabled() && !this.width();
  }

  @HostBinding('style.max-width')
  get maxWidth() {
    const width = this.width();
    return this.enabled() && width ? width || '170px' : undefined;
  }

  ngAfterViewInit() {
    this.title = this.title() || (this.elRef.nativeElement as HTMLElement).innerText;
    this.cdRef.detectChanges();
  }
}
