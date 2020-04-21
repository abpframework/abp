import { Injectable, ComponentRef } from '@angular/core';
import { Confirmation } from '../models/confirmation';
import { fromEvent, Observable, Subject, ReplaySubject } from 'rxjs';
import { takeUntil, debounceTime, filter } from 'rxjs/operators';
import { Config, ContentProjectionService, PROJECTION_STRATEGY } from '@abp/ng.core';
import { ConfirmationComponent } from '../components/confirmation/confirmation.component';

@Injectable({ providedIn: 'root' })
export class ConfirmationService {
  status$: Subject<Confirmation.Status>;
  confirmation$ = new ReplaySubject<Confirmation.DialogData>(1);

  private containerComponentRef: ComponentRef<ConfirmationComponent>;

  constructor(private contentProjectionService: ContentProjectionService) {}

  private setContainer() {
    setTimeout(() => {
      this.containerComponentRef = this.contentProjectionService.projectContent(
        PROJECTION_STRATEGY.AppendComponentToBody(ConfirmationComponent),
      );

      this.containerComponentRef.changeDetectorRef.detectChanges();
    }, 0);
  }

  info(
    message: Config.LocalizationParam,
    title: Config.LocalizationParam,
    options?: Partial<Confirmation.Options>,
  ): Observable<Confirmation.Status> {
    return this.show(message, title, 'info', options);
  }

  success(
    message: Config.LocalizationParam,
    title: Config.LocalizationParam,
    options?: Partial<Confirmation.Options>,
  ): Observable<Confirmation.Status> {
    return this.show(message, title, 'success', options);
  }

  warn(
    message: Config.LocalizationParam,
    title: Config.LocalizationParam,
    options?: Partial<Confirmation.Options>,
  ): Observable<Confirmation.Status> {
    return this.show(message, title, 'warning', options);
  }

  error(
    message: Config.LocalizationParam,
    title: Config.LocalizationParam,
    options?: Partial<Confirmation.Options>,
  ): Observable<Confirmation.Status> {
    return this.show(message, title, 'error', options);
  }

  show(
    message: Config.LocalizationParam,
    title: Config.LocalizationParam,
    severity?: Confirmation.Severity,
    options?: Partial<Confirmation.Options>,
  ): Observable<Confirmation.Status> {
    if (!this.containerComponentRef) this.setContainer();

    this.confirmation$.next({
      message,
      title,
      severity: severity || 'neutral',
      options,
    });
    this.status$ = new Subject();
    this.listenToEscape();
    return this.status$;
  }

  clear(status: Confirmation.Status = Confirmation.Status.dismiss) {
    this.confirmation$.next();
    this.status$.next(status);
  }

  private listenToEscape() {
    fromEvent(document, 'keyup')
      .pipe(
        takeUntil(this.status$),
        debounceTime(150),
        filter((key: KeyboardEvent) => key && key.key === 'Escape'),
      )
      .subscribe(_ => {
        this.clear();
      });
  }
}
