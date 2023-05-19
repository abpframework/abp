import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Params } from '@angular/router';
import { Observable, of } from 'rxjs';
import { LoginParams } from '../models/auth';
import { AbpAuthResponse } from './auth-response.model';

/**
 * Abstract service for Authentication.
 */
@Injectable({
  providedIn: 'root',
})
export class AuthService implements IAuthService {
  private warningMessage() {
    console.error('You should add @abp/ng-oauth packages or create your own auth packages.');
  }

  init(): Promise<any> {
    this.warningMessage();
    return Promise.resolve(undefined);
  }

  login(params: LoginParams): Observable<any> {
    this.warningMessage();
    return of(undefined);
  }

  logout(queryParams?: Params): Observable<any> {
    this.warningMessage();
    return of(undefined);
  }

  navigateToLogin(queryParams?: Params): void {}

  get isInternalAuth() {
    throw new Error('not implemented');
    return false;
  }

  get isAuthenticated(): boolean {
    this.warningMessage();
    return false;
  }

  loginUsingGrant(
    grantType: string,
    parameters: object,
    headers?: HttpHeaders,
  ): Promise<AbpAuthResponse> {
    console.log({ grantType, parameters, headers });
    return Promise.reject(new Error('not implemented'));
  }
}

export interface IAuthService {
  get isInternalAuth(): boolean;

  get isAuthenticated(): boolean;

  init(): Promise<any>;

  logout(queryParams?: Params): Observable<any>;

  navigateToLogin(queryParams?: Params): void;

  login(params: LoginParams): Observable<any>;

  loginUsingGrant(
    grantType: string,
    parameters: object,
    headers?: HttpHeaders,
  ): Promise<AbpAuthResponse>;
}
