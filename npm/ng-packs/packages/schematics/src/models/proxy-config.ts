import { ApiDefinition } from './api-definition';

export interface ProxyConfig extends ApiDefinition {
  generated: string[];
}
