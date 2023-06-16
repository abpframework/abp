import {
  ApplicationRef,
  ComponentFactoryResolver,
  ComponentRef,
  EmbeddedViewRef,
  inject,
  Injectable,
  Injector,
  RendererFactory2,
} from '@angular/core';
import { Subject } from 'rxjs';
import { ResolveEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { RouterEvents } from '@abp/ng.core';
import { HTTP_ERROR_CONFIG } from '../tokens/http-error.token';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { ErrorScreenErrorCodes } from '../models/common';

@Injectable({ providedIn: 'root' })
export class CreateErrorComponentService {
  protected rendererFactory = inject(RendererFactory2);
  protected cfRes = inject(ComponentFactoryResolver);
  private routerEvents = inject(RouterEvents);
  private injector = inject(Injector);
  private httpErrorConfig = inject(HTTP_ERROR_CONFIG);

  componentRef: ComponentRef<HttpErrorWrapperComponent> | null = null;

  private getErrorHostElement() {
    return document.body;
  }

  public canCreateCustomError(status: ErrorScreenErrorCodes) {
    return !!(
      this.httpErrorConfig?.errorScreen?.component &&
      this.httpErrorConfig?.errorScreen?.forWhichErrors &&
      this.httpErrorConfig?.errorScreen?.forWhichErrors.indexOf(status) > -1
    );
  }

  constructor() {
    this.listenToRouterDataResolved();
  }

  protected listenToRouterDataResolved() {
    this.routerEvents
      .getEvents(ResolveEnd)
      .pipe(filter(() => !!this.componentRef))
      .subscribe(() => {
        this.componentRef?.destroy();
        this.componentRef = null;
      });
  }

  private isCloseIconHidden() {
    return !!this.httpErrorConfig.errorScreen?.hideCloseIcon;
  }

  execute(instance: Partial<HttpErrorWrapperComponent>) {
    const renderer = this.rendererFactory.createRenderer(null, null);
    const hostElement = this.getErrorHostElement();
    const host = renderer.selectRootElement(hostElement, true);

    this.componentRef = this.cfRes
      .resolveComponentFactory(HttpErrorWrapperComponent)
      .create(this.injector);

    for (const key in instance) {
      /* istanbul ignore else */
      if (Object.prototype.hasOwnProperty.call(this.componentRef.instance, key)) {
        (this.componentRef.instance as any)[key] = (instance as any)[key];
      }
    }

    this.componentRef.instance.hideCloseIcon = this.isCloseIconHidden();
    const appRef = this.injector.get(ApplicationRef);

    if (this.canCreateCustomError(instance.status as ErrorScreenErrorCodes)) {
      this.componentRef.instance.cfRes = this.cfRes;
      this.componentRef.instance.appRef = appRef;
      this.componentRef.instance.injector = this.injector;
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
