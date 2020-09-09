import { Config, ContentProjectionService, PROJECTION_STRATEGY } from '@abp/ng.core';
import { ComponentRef, Injectable } from '@angular/core';
import { fromEvent, Observable, ReplaySubject, Subject } from 'rxjs';
import { debounceTime, filter, takeUntil } from 'rxjs/operators';
import { ConfirmationComponent } from '../components/confirmation/confirmation.component';
import { Confirmation } from '../models/confirmation';

@Injectable({ providedIn: 'root' })
export class ConfirmationService {
  status$: Subject<Confirmation.Status>;
  confirmation$ = new ReplaySubject<Confirmation.DialogData>(1);

  private containerComponentRef: ComponentRef<ConfirmationComponent>;

  clear = (status: Confirmation.Status = Confirmation.Status.dismiss) => {
    this.confirmation$.next();
    this.status$.next(status);
  };

  constructor(private contentProjectionService: ContentProjectionService) {}

  private setContainer() {
    this.containerComponentRef = this.contentProjectionService.projectContent(
      PROJECTION_STRATEGY.AppendComponentToBody(ConfirmationComponent, {
        confirmation$: this.confirmation$,
        clear: this.clear,
      }),
    );

    setTimeout(() => {
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
    options = {} as Partial<Confirmation.Options>,
  ): Observable<Confirmation.Status> {
    if (!this.containerComponentRef) this.setContainer();

    this.confirmation$.next({
      message,
      title,
      severity: severity || 'neutral',
      options,
    });

    this.status$ = new Subject();
    const { dismissible = true } = options;
    if (dismissible) this.listenToEscape();

    return this.status$;
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
