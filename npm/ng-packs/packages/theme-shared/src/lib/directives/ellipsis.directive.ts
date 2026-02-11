import { 
  AfterViewInit, 
  ChangeDetectorRef, 
  Directive, 
  ElementRef, 
  HostBinding, 
  Input, 
  inject 
} from '@angular/core';

@Directive({
  selector: '[abpEllipsis]',
})
export class EllipsisDirective implements AfterViewInit {
  private cdRef = inject(ChangeDetectorRef);
  private elRef = inject(ElementRef);

  @Input('abpEllipsis')
  width?: string;

  @HostBinding('title')
  @Input()
  title?: string;

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

  ngAfterViewInit() {
    this.title = this.title || (this.elRef.nativeElement as HTMLElement).innerText;
    this.cdRef.detectChanges();
  }
}
