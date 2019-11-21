import { Config, takeUntilDestroy } from '@abp/ng.core';
import {
  AfterViewInit,
  Component,
  ComponentFactoryResolver,
  ElementRef,
  EmbeddedViewRef,
  OnDestroy,
  Type,
  ViewChild,
  ApplicationRef,
  Injector,
} from '@angular/core';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';

@Component({
  selector: 'abp-error',
  templateUrl: './error.component.html',
  styleUrls: ['error.component.scss'],
})
export class ErrorComponent implements AfterViewInit, OnDestroy {
  appRef: ApplicationRef;

  cfRes: ComponentFactoryResolver;

  injector: Injector;

  status = 0;

  title: Config.LocalizationParam = 'Oops!';

  details: Config.LocalizationParam = 'Sorry, an error has occured.';

  customComponent: Type<any> = null;

  destroy$: Subject<void>;

  hideCloseIcon = false;

  @ViewChild('container', { static: false })
  containerRef: ElementRef<HTMLDivElement>;

  get statusText(): string {
    return this.status ? `[${this.status}]` : '';
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
