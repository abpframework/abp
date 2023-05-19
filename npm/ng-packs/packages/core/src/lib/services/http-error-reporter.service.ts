import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class HttpErrorReporterService {
  private _reporter$ = new Subject<HttpErrorResponse>();
  private _errors$ = new BehaviorSubject<HttpErrorResponse[]>([]);

  get reporter$() {
    return this._reporter$.asObservable();
  }

  get errors$() {
    return this._errors$.asObservable();
  }

  get errors() {
    return this._errors$.value;
  }

  reportError = (error: HttpErrorResponse) => {
    this._reporter$.next(error);
    this._errors$.next([...this.errors, error]);
  };
}
