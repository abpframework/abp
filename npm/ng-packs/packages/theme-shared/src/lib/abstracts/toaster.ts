import { MessageService } from 'primeng/components/common/messageservice';
import { Observable, Subject } from 'rxjs';
import { Toaster } from '../models/toaster';

export class AbstractToaster<T = Toaster.Options> {
  status$: Subject<Toaster.Status>;

  key: string = 'abpToast';

  sticky: boolean = false;

  constructor(protected messageService: MessageService) {}
  info(message: string, title: string, options?: T): Observable<Toaster.Status> {
    return this.show(message, title, 'info', options);
  }

  success(message: string, title: string, options?: T): Observable<Toaster.Status> {
    return this.show(message, title, 'success', options);
  }

  warn(message: string, title: string, options?: T): Observable<Toaster.Status> {
    return this.show(message, title, 'warn', options);
  }

  error(message: string, title: string, options?: T): Observable<Toaster.Status> {
    return this.show(message, title, 'error', options);
  }

  protected show(message: string, title: string, severity: Toaster.Severity, options?: T): Observable<Toaster.Status> {
    this.messageService.clear(this.key);

    this.messageService.add({
      severity,
      detail: message,
      summary: title,
      ...options,
      key: this.key,
      ...(typeof (options || ({} as any)).sticky === 'undefined' && { sticky: this.sticky }),
    });
    this.status$ = new Subject<Toaster.Status>();
    return this.status$;
  }

  clear(status?: Toaster.Status) {
    this.messageService.clear(this.key);
    this.status$.next(status || Toaster.Status.dismiss);
    this.status$.complete();
  }
}
