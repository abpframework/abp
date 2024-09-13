import { DOCUMENT } from '@angular/common';
import {
  AfterViewInit,
  Directive,
  effect,
  ElementRef,
  HostBinding,
  inject,
  Inject,
  input,
  Input,
  OnDestroy,
  OnInit,
  Renderer2,
  ViewContainerRef,
} from '@angular/core';
import { ColumnMode, DatatableComponent, ScrollerComponent } from '@swimlane/ngx-datatable';
import { fromEvent, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { SpinnerComponent } from '../components';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'ngx-datatable[default]',
  standalone: true,
  exportAs: 'ngxDatatableDefault',
})
export class NgxDatatableDefaultDirective implements OnInit, AfterViewInit, OnDestroy {
  @Input() class = 'material bordered';
  loadingIndicator = input<boolean>(true);

  private _loading = true;
  private mutationObserver: MutationObserver | null = null;

  private subscription = new Subscription();
  private resizeDiff = 0;
  private elRef = inject(ElementRef);
  private renderer = inject(Renderer2);
  private viewContainerRef = inject(ViewContainerRef);

  @HostBinding('class')
  get classes(): string {
    return `ngx-datatable ${this.class}`;
  }

  constructor(
    private table: DatatableComponent,
    @Inject(DOCUMENT) private document: MockDocument,
  ) {
    this.table.columnMode = ColumnMode.force;
    this.table.footerHeight = 50;
    this.table.headerHeight = 50;
    this.table.rowHeight = 'auto';
    this.table.scrollbarH = true;
    this.table.virtualization = false;

    effect(() => {
      this._loading = this.loadingIndicator();
      this.updateLoadingIndicator();
    });
  }

  private fixHorizontalGap(scroller: ScrollerComponent) {
    const { body, documentElement } = this.document;

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

  private fixStyleOnWindowResize() {
    // avoided @HostListener('window:resize') in favor of performance
    const subscription = fromEvent(window, 'resize')
      .pipe(debounceTime(500))
      .subscribe(() => {
        const { scroller } = this.table.bodyComponent;

        if (!scroller) return;

        this.fixHorizontalGap(scroller);
      });

    this.subscription.add(subscription);
  }

  private observeMutations() {
    this.mutationObserver = new MutationObserver(() => {
      if (this._loading) {
        this.updateLoadingIndicator();
      }
    });

    this.mutationObserver.observe(this.elRef.nativeElement, {
      childList: true,
      subtree: true,
    });
  }

  private updateLoadingIndicator() {
    const progressElement = this.elRef.nativeElement.querySelector('datatable-progress');

    if (this._loading) {
      if (progressElement) {
        this.renderer.removeChild(progressElement.parentNode, progressElement);
        this.addSpinner(progressElement);
      }
    } else {
      this.removeSpinner();
    }
  }

  private addSpinner(placeholder: Comment) {
    this.viewContainerRef.clear();

    const spinnerRef = this.viewContainerRef.createComponent(SpinnerComponent);

    this.renderer.insertBefore(
      placeholder.parentNode,
      spinnerRef.location.nativeElement,
      placeholder,
    );
    this.renderer.removeChild(placeholder.parentNode, placeholder);
  }

  private removeSpinner() {
    this.viewContainerRef.clear();
  }

  ngOnInit() {
    this.observeMutations();
  }

  ngAfterViewInit() {
    this.fixStyleOnWindowResize();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
    if (this.mutationObserver) {
      this.mutationObserver.disconnect();
    }
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
