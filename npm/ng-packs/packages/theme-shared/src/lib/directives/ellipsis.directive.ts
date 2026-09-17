import {
  AfterViewInit,
  ChangeDetectorRef,
  computed,
  Directive,
  ElementRef,
  inject,
  input,
  signal
} from '@angular/core';

@Directive({
  selector: '[abpEllipsis]',
  host: {
    '[title]': 'effectiveTitle()',
    '[class.abp-ellipsis-inline]': 'inlineClass()',
    '[class.abp-ellipsis]': 'ellipsisClass()',
    '[style.max-width]': 'maxWidth()'
  }
})
export class EllipsisDirective implements AfterViewInit {
  private cdRef = inject(ChangeDetectorRef);
  private elRef = inject(ElementRef);

  readonly width = input<string | undefined>(undefined, { alias: 'abpEllipsis' });
  readonly title = input<string | undefined>(undefined);
  readonly enabled = input(true, { alias: 'abpEllipsisEnabled' });

  private readonly autoTitle = signal<string | undefined>(undefined);

  protected readonly effectiveTitle = computed(() => this.title() || this.autoTitle());

  protected readonly inlineClass = computed(() => this.enabled() && !!this.width());

  protected readonly ellipsisClass = computed(() => this.enabled() && !this.width());

  protected readonly maxWidth = computed(() => {
    const width = this.width();
    return this.enabled() && width ? width || '170px' : undefined;
  });

  ngAfterViewInit() {
    if (!this.title()) {
      this.autoTitle.set((this.elRef.nativeElement as HTMLElement).innerText);
      this.cdRef.detectChanges();
    }
  }
}
