import { DOCUMENT, isPlatformBrowser } from '@angular/common';
import {
  AfterViewInit,
  Directive,
  HostBinding,
  OnDestroy,
  inject,
  PLATFORM_ID,
  input
} from '@angular/core';
import { ColumnMode, DatatableComponent } from '@swimlane/ngx-datatable';
import { fromEvent, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'ngx-datatable[default]',
  exportAs: 'ngxDatatableDefault',
})
export class NgxDatatableDefaultDirective implements AfterViewInit, OnDestroy {
  private table = inject(DatatableComponent);
  private document = inject<MockDocument>(DOCUMENT);
  private platformId = inject(PLATFORM_ID);

  private subscription = new Subscription();
  private resizeDiff = 0;

  readonly class = input('material bordered');

  @HostBinding('class')
  get classes(): string {
    return `ngx-datatable ${this.class()}`;
  }

  constructor() {
    this.table.columnMode = ColumnMode.force;
    this.table.footerHeight = 50;
    this.table.headerHeight = 50;
    this.table.rowHeight = 'auto';
    this.table.scrollbarH = true;
    this.table.virtualization = false;
  }

  private fixHorizontalGap(scroller: any) {
    const { body, documentElement } = this.document;
    if (isPlatformBrowser(this.platformId)) {
      if (documentElement.scrollHeight !== documentElement.clientHeight) {
        if (this.resizeDiff === 0) {
          this.resizeDiff = window.innerWidth - body.offsetWidth;
          scroller.scrollWidth -= this.resizeDiff;
        }
      } else {
        scroller.scrollWidth += this.resizeDiff;
        this.resizeDiff = 0;
      }
    }
  }

  private fixStyleOnWindowResize() {
    if (isPlatformBrowser(this.platformId)) {
      const subscription = fromEvent(window, 'resize')
        .pipe(debounceTime(500))
        .subscribe(() => {
          const { scroller } = this.table.bodyComponent;

          if (!scroller) return;

          this.fixHorizontalGap(scroller);
        });

      this.subscription.add(subscription);
    }
  }

  ngAfterViewInit() {
    this.fixStyleOnWindowResize();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}

// fix: https://github.com/angular/angular/issues/20351
interface MockDocument {
  body: MockBody;
  documentElement: MockDocumentElement;
}

interface MockBody {
  offsetWidth: number;
}

interface MockDocumentElement {
  clientHeight: number;
  scrollHeight: number;
}
