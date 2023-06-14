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
import {
  ErrorScreenErrorCodes,
  HTTP_ERROR_CONFIG,
  HttpErrorWrapperComponent,
} from '@abp/ng.theme.shared';
import { Subject } from 'rxjs';
import { ResolveEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { RouterEvents } from '@abp/ng.core';
import { CanCreateCustomErrorService } from './can-create-custom-error.service';

@Injectable({ providedIn: 'root' })
export class CreateErrorComponentService {
  protected rendererFactory = inject(RendererFactory2);
  protected cfRes = inject(ComponentFactoryResolver);
  private routerEvents = inject(RouterEvents);
  private injector = inject(Injector);
  private canCreateCustomErrorService = inject(CanCreateCustomErrorService);
  private httpErrorConfig = inject(HTTP_ERROR_CONFIG);

  componentRef: ComponentRef<HttpErrorWrapperComponent> | null = null;

  protected getErrorHostElement() {
    return document.body;
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

    if (this.canCreateCustomErrorService.execute(instance.status as ErrorScreenErrorCodes)) {
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
