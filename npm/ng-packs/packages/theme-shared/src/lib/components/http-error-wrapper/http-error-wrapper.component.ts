import { Config, takeUntilDestroy } from '@abp/ng.core';
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
import snq from 'snq';

@Component({
  selector: 'abp-http-error-wrapper',
  templateUrl: './http-error-wrapper.component.html',
  styleUrls: ['http-error-wrapper.component.scss'],
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

  @ViewChild('container', { static: false })
  containerRef: ElementRef<HTMLDivElement>;

  get statusText(): string {
    return this.status ? `[${this.status}]` : '';
  }

  ngOnInit() {
    this.backgroundColor =
      snq(() => window.getComputedStyle(document.body).getPropertyValue('background-color')) || '#fff';
  }

  ngAfterViewInit() {
    if (this.customComponent) {
      const customComponentRef = this.cfRes.resolveComponentFactory(this.customComponent).create(this.injector);
      customComponentRef.instance.errorStatus = this.status;
      customComponentRef.instance.destroy$ = this.destroy$;
      this.appRef.attachView(customComponentRef.hostView);
      this.containerRef.nativeElement.appendChild((customComponentRef.hostView as EmbeddedViewRef<any>).rootNodes[0]);
      customComponentRef.changeDetectorRef.detectChanges();
    }

    fromEvent(document, 'keyup')
      .pipe(
        takeUntilDestroy(this),
        debounceTime(150),
        filter((key: KeyboardEvent) => key && key.key === 'Escape'),
      )
      .subscribe(() => {
        this.destroy();
      });
  }

  ngOnDestroy() {}

  destroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
