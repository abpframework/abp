import {
  ApplicationRef,
  ComponentRef,
  createComponent,
  EmbeddedViewRef,
  EnvironmentInjector,
  inject,
  Injectable,
  Injector,
  RendererFactory2,
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { ResolveEnd } from '@angular/router';
import { Subject } from 'rxjs';
import { filter } from 'rxjs/operators';
import { RouterEvents } from '@abp/ng.core';
import { HTTP_ERROR_CONFIG } from '../tokens/http-error.token';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { ErrorScreenErrorCodes } from '../models/common';

@Injectable({ providedIn: 'root' })
export class CreateErrorComponentService {
  protected readonly document = inject(DOCUMENT);
  protected readonly rendererFactory = inject(RendererFactory2);
  protected readonly routerEvents = inject(RouterEvents);
  protected readonly injector = inject(Injector);
  protected readonly envInjector = inject(EnvironmentInjector);
  protected readonly httpErrorConfig = inject(HTTP_ERROR_CONFIG);

  componentRef: ComponentRef<HttpErrorWrapperComponent> | null = null;

  constructor() {
    this.listenToRouterDataResolved();
  }

  protected listenToRouterDataResolved(): void {
    this.routerEvents
      .getEvents(ResolveEnd)
      .pipe(filter(() => !!this.componentRef))
      .subscribe(() => {
        this.componentRef?.destroy();
        this.componentRef = null;
      });
  }

  protected getErrorHostElement(): HTMLElement {
    return this.document.body;
  }

  protected isCloseIconHidden(): boolean {
    return !!this.httpErrorConfig?.errorScreen?.hideCloseIcon;
  }

  canCreateCustomError(status: ErrorScreenErrorCodes) {
    const { component, forWhichErrors } = this.httpErrorConfig?.errorScreen || {};

    if (!component || !forWhichErrors) {
      return false;
    }

    return forWhichErrors.indexOf(status) > -1;
  }

  execute(instance: Partial<HttpErrorWrapperComponent>): void {
    const renderer = this.rendererFactory.createRenderer(null, null);
    const hostElement = this.getErrorHostElement();
    const host = renderer.selectRootElement(hostElement, true);

    this.componentRef = createComponent(HttpErrorWrapperComponent, {
      environmentInjector: this.envInjector,
    });

    for (const key in instance) {
      /* istanbul ignore else */
      if (Object.prototype.hasOwnProperty.call(this.componentRef.instance, key)) {
        (this.componentRef.instance as any)[key] = (instance as any)[key];
      }
    }

    this.componentRef.instance.hideCloseIcon = this.isCloseIconHidden();
    const appRef = this.injector.get(ApplicationRef);

    if (this.canCreateCustomError(instance.status as ErrorScreenErrorCodes)) {
      this.componentRef.instance.appRef = appRef;
      this.componentRef.instance.environmentInjector = this.envInjector;
      this.componentRef.instance.customComponent = this.httpErrorConfig.errorScreen?.component;
    }

    appRef.attachView(this.componentRef.hostView);
    renderer.appendChild(host, (this.componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0]);

    const destroy$ = new Subject<void>();
    this.componentRef.instance.destroy$ = destroy$;

    destroy$.subscribe(() => {
      this.componentRef?.destroy();
      this.componentRef = null;
    });
  }
}
