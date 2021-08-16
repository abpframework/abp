import {
  ContentProjectionService,
  LocalizationParam,
  PROJECTION_STRATEGY,
  Strict,
} from '@abp/ng.core';
import { ComponentRef, Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { ToastContainerComponent } from '../components/toast-container/toast-container.component';
import { Toaster } from '../models';

@Injectable({
  providedIn: 'root',
})
export class ToasterService implements ToasterContract {
  private toasts$ = new ReplaySubject<Toaster.Toast[]>(1);

  private lastId = -1;

  private toasts = [] as Toaster.Toast[];

  private containerComponentRef: ComponentRef<ToastContainerComponent>;

  constructor(private contentProjectionService: ContentProjectionService) {}

  private setContainer() {
    this.containerComponentRef = this.contentProjectionService.projectContent(
      PROJECTION_STRATEGY.AppendComponentToBody(ToastContainerComponent, {
        toasts$: this.toasts$,
      }),
    );

    this.containerComponentRef.changeDetectorRef.detectChanges();
  }

  /**
   * Creates an info toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  info(
    message: LocalizationParam,
    title?: LocalizationParam,
    options?: Partial<Toaster.ToastOptions>,
  ): Toaster.ToasterId {
    return this.show(message, title, 'info', options);
  }

  /**
   * Creates a success toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  success(
    message: LocalizationParam,
    title?: LocalizationParam,
    options?: Partial<Toaster.ToastOptions>,
  ): Toaster.ToasterId {
    return this.show(message, title, 'success', options);
  }

  /**
   * Creates a warning toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  warn(
    message: LocalizationParam,
    title?: LocalizationParam,
    options?: Partial<Toaster.ToastOptions>,
  ): Toaster.ToasterId {
    return this.show(message, title, 'warning', options);
  }

  /**
   * Creates an error toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  error(
    message: LocalizationParam,
    title?: LocalizationParam,
    options?: Partial<Toaster.ToastOptions>,
  ): Toaster.ToasterId {
    return this.show(message, title, 'error', options);
  }

  /**
   * Creates a toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param severity Sets color of the toast. "success", "warning" etc.
   * @param options Spesific style or structural options for individual toast
   */

  show(
    message: LocalizationParam,
    title: LocalizationParam = null,
    severity: Toaster.Severity = 'neutral',
    options = {} as Partial<Toaster.ToastOptions>,
  ): Toaster.ToasterId {
    if (!this.containerComponentRef) this.setContainer();

    const id = ++this.lastId;
    this.toasts.push({
      message,
      title,
      severity,
      options: { closable: true, id, ...options },
    });
    this.toasts$.next(this.toasts);
    return id;
  }

  /**
   * Removes the toast with given id.
   * @param id ID of the toast to be removed.
   */
  remove(id: number): void {
    this.toasts = this.toasts.filter(toast => toast.options?.id !== id);
    this.toasts$.next(this.toasts);
  }

  /**
   * Removes all open toasts at once.
   */
  clear(containerKey?: string): void {
    this.toasts = !containerKey
      ? []
      : this.toasts.filter(toast => toast.options?.containerKey !== containerKey);
    this.toasts$.next(this.toasts);
  }
}

export type ToasterContract = Strict<ToasterService, Toaster.Service>;
