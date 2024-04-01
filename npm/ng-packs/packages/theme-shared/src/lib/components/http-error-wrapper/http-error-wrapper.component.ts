import {
  ApplicationRef,
  Component,
  inject,
  OnInit,
  ElementRef,
  EmbeddedViewRef,
  Type,
  ViewChild,
  AfterViewInit,
  OnDestroy,
  createComponent,
  EnvironmentInjector,
  DestroyRef,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DOCUMENT } from '@angular/common';
import { Router } from '@angular/router';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';
import { LocalizationParam } from '@abp/ng.core';
import { ErrorScreenErrorCodes } from '../../models';

@Component({
  selector: 'abp-http-error-wrapper',
  templateUrl: './http-error-wrapper.component.html',
  styleUrls: ['http-error-wrapper.component.scss'],
})
export class HttpErrorWrapperComponent implements OnInit, AfterViewInit, OnDestroy {
  protected readonly destroyRef = inject(DestroyRef);
  protected readonly document = inject(DOCUMENT);
  protected readonly window = this.document.defaultView;
  protected readonly router = inject(Router);

  appRef!: ApplicationRef;

  environmentInjector!: EnvironmentInjector;

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

  ngOnInit(): void {
    const computedStyle = this.window.getComputedStyle(this.document.body);
    const backgroundColor = computedStyle?.getPropertyValue('background-color');
    this.backgroundColor = backgroundColor || '#fff';
  }

  ngAfterViewInit(): void {
    if (this.customComponent) {
      const customComponentRef = createComponent(this.customComponent, {
        environmentInjector: this.environmentInjector,
      });

      customComponentRef.instance.errorStatus = this.status;
      
      //In our custom "HttpErrorComponent", we have a "status" property.
      //We used to have "errorStatus", but it wasn't signal type. "status" variable is signal type.
      //I've checked because of backward compatibility. Developers might have their own custom HttpErrorComponent.
      //We need to deprecated and remove "errorStatus" in the future.
      if (customComponentRef.instance.status) {
        customComponentRef.instance.status.set(this.status);
      }
      
      customComponentRef.instance.destroy$ = this.destroy$;

      this.appRef.attachView(customComponentRef.hostView);

      if (this.containerRef) {
        this.containerRef.nativeElement.appendChild(
          (customComponentRef.hostView as EmbeddedViewRef<any>).rootNodes[0],
        );
      }
      customComponentRef.changeDetectorRef.detectChanges();
    }

    fromEvent<KeyboardEvent>(this.document, 'keyup')
      .pipe(
        debounceTime(150),
        filter((key: KeyboardEvent) => key && key.key === 'Escape'),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(() => this.destroy());
  }

  goHome(): void {
    this.router.navigateByUrl('/', { onSameUrlNavigation: 'reload' });
    this.destroy();
  }

  destroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnDestroy(): void {
    this.destroy();
  }
}
