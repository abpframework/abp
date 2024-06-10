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

  get oidc(): boolean {
    this.warningMessage();
    return false;
  }

  set oidc(value: boolean) {
    this.warningMessage();
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

  navigateToLogin(queryParams?: Params): void { }

  get isInternalAuth(): boolean {
    throw new Error('not implemented');
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

  getAccessTokenExpiration(): number {
    this.warningMessage();
    return 0;
  }

  getRefreshToken(): string {
    this.warningMessage();
    return '';
  }

  getAccessToken(): string {
    this.warningMessage();
    return '';
  }

  refreshToken(): Promise<AbpAuthResponse> {
    this.warningMessage();
    return Promise.resolve(undefined);
  }
}

export interface IAuthService {
  oidc: boolean;

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

  getAccessTokenExpiration(): number;

  getRefreshToken(): string;

  getAccessToken(): string;

  refreshToken(): Promise<AbpAuthResponse>;
}
