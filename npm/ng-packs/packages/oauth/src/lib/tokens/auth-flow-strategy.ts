import { Injector } from '@angular/core';
import { AuthCodeFlowStrategy } from '../strategies/auth-code-flow-strategy';
import { AuthPasswordFlowStrategy } from '../strategies/auth-password-flow-strategy';

export const AUTH_FLOW_STRATEGY = {
  Code(injector: Injector) {
    return new AuthCodeFlowStrategy(injector);
  },
  Password(injector: Injector) {
    return new AuthPasswordFlowStrategy(injector);
  },
};
