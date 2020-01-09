import { Injectable } from '@angular/core';
import { Toaster } from '../models';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ToasterService {
  toasts$ = new ReplaySubject<Toaster.Toast[]>(1);

  private lastId = -1;

  private toasts = [] as Toaster.Toast[];

  /**
   * Creates an info toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  info(message: string, title?: string, options?: Partial<Toaster.ToastOptions>) {
    return this.show(message, title, 'info', options);
  }

  /**
   * Creates a success toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  success(message: string, title?: string, options?: Partial<Toaster.ToastOptions>) {
    return this.show(message, title, 'success', options);
  }

  /**
   * Creates a warning toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  warn(message: string, title?: string, options?: Partial<Toaster.ToastOptions>) {
    return this.show(message, title, 'warning', options);
  }

  /**
   * Creates an error toast with given parameters.
   * @param message Content of the toast
   * @param title Title of the toast
   * @param options Spesific style or structural options for individual toast
   */
  error(message: string, title?: string, options?: Partial<Toaster.ToastOptions>) {
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
    message: string,
    title: string = null,
    severity: Toaster.Severity = 'neutral',
    options: Partial<Toaster.ToastOptions> = null,
  ) {
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
  remove(id: number) {
    this.toasts = this.toasts.filter(toast => toast.options.id !== id);
    this.toasts$.next(this.toasts);
  }

  /**
   * Removes all open toasts at once.
   */
  clear(key?: string) {
    this.toasts = !key ? [] : this.toasts.filter(toast => toast.options.containerKey !== key);
    this.toasts$.next(this.toasts);
  }
}
