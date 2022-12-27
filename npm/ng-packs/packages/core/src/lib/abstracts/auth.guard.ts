import { CanActivate, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements IAuthGuard {
  canActivate(): Observable<boolean> | boolean | UrlTree {
    console.error('You should add @abp/ng-oauth packages or create your own auth packages.');
    return false;
  }
}
export interface IAuthGuard extends CanActivate {}
