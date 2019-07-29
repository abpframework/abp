import { ABP } from '../models';

export class PatchRouteByName {
  static readonly type = '[Config] Patch Route By Name';
  constructor(public name: string, public newValue: Partial<ABP.Route>) {}
}

export class ConfigGetAppConfiguration {
  static readonly type = '[Config] Get App Configuration';
}
