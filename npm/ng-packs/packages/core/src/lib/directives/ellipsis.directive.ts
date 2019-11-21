import { AfterContentInit, ChangeDetectorRef, Directive, ElementRef, HostBinding, Input } from '@angular/core';

@Directive({
  selector: '[abpEllipsis]',
})
export class EllipsisDirective implements AfterContentInit {
  @Input('abpEllipsis')
  width: string;

  @HostBinding('title')
  @Input()
  title: string;

  @Input('abpEllipsisEnabled')
  enabled = true;

  @HostBinding('class.abp-ellipsis-inline')
  get inlineClass() {
    return this.enabled && this.width;
  }

  @HostBinding('class.abp-ellipsis')
  get class() {
    return this.enabled && !this.width;
  }

  @HostBinding('style.max-width')
  get maxWidth() {
    return this.enabled && this.width ? this.width || '170px' : undefined;
  }

  constructor(private cdRef: ChangeDetectorRef, private elRef: ElementRef) {}

  ngAfterContentInit() {
    setTimeout(() => {
      const title = this.title;
      this.title = title || (this.elRef.nativeElement as HTMLElement).innerText;

      if (this.title !== title) {
        this.cdRef.detectChanges();
      }
    }, 0);
  }
}
