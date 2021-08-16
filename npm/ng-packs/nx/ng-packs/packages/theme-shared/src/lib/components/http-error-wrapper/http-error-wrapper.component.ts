import { Config, SubscriptionService } from '@abp/ng.core';
import {
  AfterViewInit,
  ApplicationRef,
  Component,
  ComponentFactoryResolver,
  ElementRef,
  EmbeddedViewRef,
  Injector,
  OnDestroy,
  OnInit,
  Type,
  ViewChild,
} from '@angular/core';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';

@Component({
  selector: 'abp-http-error-wrapper',
  templateUrl: './http-error-wrapper.component.html',
  styleUrls: ['http-error-wrapper.component.scss'],
  providers: [SubscriptionService],
})
export class HttpErrorWrapperComponent implements AfterViewInit, OnDestroy, OnInit {
  appRef: ApplicationRef;

  cfRes: ComponentFactoryResolver;

  injector: Injector;

  status = 0;

  title: Config.LocalizationParam = 'Oops!';

  details: Config.LocalizationParam = 'Sorry, an error has occured.';

  customComponent: Type<any> = null;

  destroy$: Subject<void>;

  hideCloseIcon = false;

  backgroundColor: string;

  isHomeShow = true;

  @ViewChild('container', { static: false })
  containerRef: ElementRef<HTMLDivElement>;

  get statusText(): string {
    return this.status ? `[${this.status}]` : '';
  }

  constructor(private subscription: SubscriptionService) {}

  ngOnInit() {
    this.backgroundColor =
      window.getComputedStyle(document.body)?.getPropertyValue('background-color') || '#fff';
  }

  ngAfterViewInit() {
    if (this.customComponent) {
      const customComponentRef = this.cfRes
        .resolveComponentFactory(this.customComponent)
        .create(this.injector);
      customComponentRef.instance.errorStatus = this.status;
      customComponentRef.instance.destroy$ = this.destroy$;
      this.appRef.attachView(customComponentRef.hostView);
      this.containerRef.nativeElement.appendChild(
        (customComponentRef.hostView as EmbeddedViewRef<any>).rootNodes[0],
      );
      customComponentRef.changeDetectorRef.detectChanges();
    }

    const keyup$ = fromEvent(document, 'keyup').pipe(
      debounceTime(150),
      filter((key: KeyboardEvent) => key && key.key === 'Escape'),
    );
    this.subscription.addOne(keyup$, () => this.destroy());
  }

  ngOnDestroy() {
    this.destroy();
  }

  destroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
