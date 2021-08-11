import { Config } from '../models/config';

/**
 * @deprecated Use ConfigStateService. To be deleted in v5.0.
 */
export class GetAppConfiguration {
  static readonly type = '[Config] Get App Configuration';
}

/**
 * @deprecated Use EnvironmentService instead. To be deleted in v5.0.
 */
export class SetEnvironment {
  static readonly type = '[Config] Set Environment';
  constructor(public environment: Config.Environment) {}
}

/**
 * @deprecated Use EnvironmentService instead. To be deleted in v5.0.
 */
export class PatchConfigState {
  static readonly type = '[Config] Set State';
  constructor(public state: Config.State) {}
}
