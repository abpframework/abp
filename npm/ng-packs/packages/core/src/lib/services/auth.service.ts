import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthFlowStrategy, AUTH_FLOW_STRATEGY } from '../strategies/auth-flow.strategy';
import { EnvironmentService } from './environment.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private flow: string;
  private strategy: AuthFlowStrategy;

  get isInternalAuth() {
    return this.strategy.isInternalAuth;
  }

  constructor(private environment: EnvironmentService, private injector: Injector) {
    this.setStrategy();
    this.listenToSetEnvironment();
  }

  private setStrategy = () => {
    const flow = this.environment.getEnvironment().oAuthConfig.responseType || 'password';
    if (this.flow === flow) return;

    if (this.strategy) this.strategy.destroy();

    this.flow = flow;
    this.strategy = AUTH_FLOW_STRATEGY.Code(this.injector);
  };

  private listenToSetEnvironment() {
    this.environment.createOnUpdateStream(state => state.oAuthConfig).subscribe(this.setStrategy);
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
