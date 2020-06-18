import { Injectable, ComponentRef } from '@angular/core';
import { Toaster } from '../models';
import { ReplaySubject } from 'rxjs';
import { Config, PROJECTION_STRATEGY, ContentProjectionService } from '@abp/ng.core';
import snq from 'snq';
import { ToastContainerComponent } from '../components/toast-container/toast-container.component';

@Injectable({
  providedIn: 'root',
})
export class ToasterService {
  toasts$ = new ReplaySubject<Toaster.Toast[]>(1);

  private lastId = -1;

  private toasts = [] as Toaster.Toast[];

  private containerComponentRef: ComponentRef<ToastContainerComponent>;

  constructor(private contentProjectionService: ContentProjectionService) {}

  private setContainer() {
    this.containerComponentRef = this.contentProjectionService.projectContent(
      PROJECTION_STRATEGY.AppendComponentToBody(ToastContainerComponent, { toasts$: this.toasts$ }),
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
    message: Config.LocalizationParam,
    title?: Config.LocalizationParam,
    options?: Partial<Toaster.ToastOptions>,
  ): number {
    return this.show(message, title, 'info', options);
  }

  /**
   * Creates a success toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  success(
    message: Config.LocalizationParam,
    title?: Config.LocalizationParam,
    options?: Partial<Toaster.ToastOptions>,
  ): number {
    return this.show(message, title, 'success', options);
  }

  /**
   * Creates a warning toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  warn(
    message: Config.LocalizationParam,
    title?: Config.LocalizationParam,
    options?: Partial<Toaster.ToastOptions>,
  ): number {
    return this.show(message, title, 'warning', options);
  }

  /**
   * Creates an error toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  error(
    message: Config.LocalizationParam,
    title?: Config.LocalizationParam,
    options?: Partial<Toaster.ToastOptions>,
  ): number {
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
    message: Config.LocalizationParam,
    title: Config.LocalizationParam = null,
    severity: Toaster.Severity = 'neutral',
    options = {} as Partial<Toaster.ToastOptions>,
  ): number {
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
    this.toasts = this.toasts.filter(toast => snq(() => toast.options.id) !== id);
    this.toasts$.next(this.toasts);
  }

  /**
   * Removes all open toasts at once.
   */
  clear(key?: string): void {
    this.toasts = !key
      ? []
      : this.toasts.filter(toast => snq(() => toast.options.containerKey) !== key);
    this.toasts$.next(this.toasts);
  }
}
