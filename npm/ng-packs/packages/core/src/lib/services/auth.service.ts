import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import {
  AuthCodeFlowStrategy,
  AuthFlowStrategy,
  AuthPasswordFlowStrategy,
  AUTH_FLOW_STRATEGY,
} from '../strategies/auth-flow.strategy';
import { EnvironmentService } from './environment.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _strategy: AuthFlowStrategy;

  set strategy(strategy: AuthFlowStrategy) {
    if (this.strategy) this.strategy.destroy();
    this._strategy = strategy;
  }

  get strategy() {
    return this._strategy;
  }

  get isInternalAuth() {
    return this.strategy.isInternalAuth;
  }

  async init() {
    return await this.strategy.init();
  }

  logout(): Observable<any> {
    return this.strategy.logout();
  }

  initLogin() {
    this.strategy.login();
  }
}
