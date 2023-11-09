import {
  ABP,
  CORE_OPTIONS,
  EnvironmentService,
  ExternalHttpClient,
  HttpErrorReporterService,
  RestService,
} from '@abp/ng.core';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MockRestService extends RestService {
  constructor(
    @Inject(CORE_OPTIONS) protected options: ABP.Root,
    protected http: HttpClient,
    protected externalhttp: ExternalHttpClient,
    protected environment: EnvironmentService,
  ) {
    super(options, http,externalhttp, environment, null as unknown as HttpErrorReporterService);
  }

  handleError(err: any): Observable<any> {
    return throwError(err);
  }
}
