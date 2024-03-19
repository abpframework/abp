import {
  ApplicationRef,
  Component,
  Injector,
  inject,
  OnInit,
  ComponentFactoryResolver,
  ElementRef,
  EmbeddedViewRef,
  Type,
  ViewChild,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';
import { LocalizationParam, SubscriptionService } from '@abp/ng.core';
import { ErrorScreenErrorCodes } from '../../models';

@Component({
  selector: 'abp-http-error-wrapper',
  templateUrl: './http-error-wrapper.component.html',
  styleUrls: ['http-error-wrapper.component.scss'],
  providers: [SubscriptionService],
})
export class HttpErrorWrapperComponent implements OnInit, AfterViewInit, OnDestroy {
  protected readonly document = inject(DOCUMENT);
  protected readonly window = this.document.defaultView;

  appRef!: ApplicationRef;

  cfRes!: ComponentFactoryResolver;

  injector!: Injector;

  status: ErrorScreenErrorCodes = 0;

  title: LocalizationParam = 'Oops!';

  details: LocalizationParam = 'Sorry, an error has occured.';

  customComponent: Type<any> | undefined = undefined;

  destroy$!: Subject<void>;

  hideCloseIcon = false;

  backgroundColor!: string;

  isHomeShow = true;

  @ViewChild('container', { static: false })
  containerRef?: ElementRef<HTMLDivElement>;

  get statusText(): string {
    return this.status ? `[${this.status}]` : '';
  }

  constructor(private subscription: SubscriptionService) {}

  ngOnInit(): void {
    this.backgroundColor =
      this.window.getComputedStyle(this.document.body)?.getPropertyValue('background-color') || '#fff';
  }

  ngAfterViewInit(): void {
    if (this.customComponent) {
      const customComponentRef = this.cfRes
        .resolveComponentFactory(this.customComponent)
        .create(this.injector);
      customComponentRef.instance.errorStatus = this.status;
      customComponentRef.instance.destroy$ = this.destroy$;
      this.appRef.attachView(customComponentRef.hostView);
      if (this.containerRef) {
        this.containerRef.nativeElement.appendChild(
          (customComponentRef.hostView as EmbeddedViewRef<any>).rootNodes[0],
        );
      }
      customComponentRef.changeDetectorRef.detectChanges();
    }

    const keyup$ = fromEvent<KeyboardEvent>(this.document, 'keyup').pipe(
      debounceTime(150),
      filter((key: KeyboardEvent) => key && key.key === 'Escape'),
    );
    this.subscription.addOne(keyup$, () => this.destroy());
  }

  ngOnDestroy(): void {
    this.destroy();
  }

  destroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
